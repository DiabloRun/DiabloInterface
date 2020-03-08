#tool nuget:?package=vswhere
#addin nuget:?package=Cake.Compression&version=0.1.1
#addin nuget:?package=SharpZipLib&version=0.86.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", "0");
var buildDir = Directory("./artifacts");
var binDir = buildDir + Directory("bin");
var solution = "./src/DiabloInterface.sln";

var testSolution = "./src/DiabloInterface.test";
var testDir = Directory(testSolution + "/bin/" + configuration + "/");
var testFile = testDir + File(@"DiabloInterface.test.dll");
var testResultFile = testDir + File(@"TestResults.trx");

var vsLatest = VSWhereLatest();
var modernMSBuildPath = vsLatest + File(@"\MSBuild\15.0\Bin\MSBuild.exe");
if (!FileExists(modernMSBuildPath)) {
    modernMSBuildPath = vsLatest + File(@"\MSBuild\Current\bin\MSBuild.exe");
}
var modernMSTestPath = vsLatest + File(@"\Common7\IDE\MSTest.exe");

Task("Clean")
    .Does(() =>
{
    CleanDirectory(binDir);
});

Task("RestorePackages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    MSBuild(solution, settings =>
    {
        if (FileExists(modernMSBuildPath))
        {
            settings.ToolPath = modernMSBuildPath;
        }

        settings.SetConfiguration(configuration);
        settings.SetVerbosity(Verbosity.Minimal);
    });
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var path = "./src/DiabloInterface/bin/" + configuration + "/";
    var allFiles =
        GetFiles(path + "*.dll") +
        GetFiles(path + "*.exe") +
        GetFiles(path + "*.config");
    var files = allFiles.Where(x => !x.GetFilename().ToString().Contains(".vshost.exe"));

    Information("Copying from {0}", path);
    CopyFiles(files, binDir);

    var assemblyInfo = ParseAssemblyInfo("./src/DiabloInterface/Properties/AssemblyInfo.cs");
    var fileName = string.Format("DiabloInterface-v{0}.zip", assemblyInfo.AssemblyInformationalVersion);
    ZipCompress(binDir, buildDir + File(fileName));
});

Task("Default")
    .IsDependentOn("Package");


Task("CleanTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    CleanDirectory(testDir);
});

Task("BuildTest")
    .IsDependentOn("CleanTest")
    .Does(() =>
{
    MSBuild(testSolution, settings =>
    {
        if (FileExists(modernMSBuildPath))
        {
            settings.ToolPath = modernMSBuildPath;
        }

        settings.SetConfiguration(configuration);
        settings.SetVerbosity(Verbosity.Minimal);
    });
});

Task("Test")
    .IsDependentOn("BuildTest")
    .Does(() =>
{
    if (FileExists(modernMSTestPath)) {
        MSTest(testFile, new MSTestSettings() {
            ToolPath = modernMSTestPath,
            ResultsFile = testResultFile
        });
    } else {
        MSTest(testFile, new MSTestSettings() {
            ResultsFile = testResultFile
        });
    }
});

RunTarget(target);
