#tool nuget:?package=vswhere
#addin nuget:?package=Cake.Compression&version=0.1.1
#addin nuget:?package=SharpZipLib&version=0.86.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", "0");
var buildDir = Directory("./artifacts");
var binDir = buildDir + Directory("bin");

var vsLatest = VSWhereLatest();
var modernMSBuildPath = vsLatest + File(@"\MSBuild\15.0\Bin\MSBuild.exe");
if (!FileExists(modernMSBuildPath)) {
    modernMSBuildPath = vsLatest + File(@"\MSBuild\Current\bin\MSBuild.exe");
}
var modernMSTestPath = vsLatest + File(@"\Common7\IDE\MSTest.exe");

Task("Build")
    .Does(() =>
{
    CleanDirectory(binDir);
    
    var solution = "./src/DiabloInterface.sln";
    NuGetRestore(solution);
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

Task("CopyBuilt")
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
});

Task("CopyBuiltPlugins")
    .IsDependentOn("Build")
    .Does(() =>
{
    var pluginFiles = GetFiles("./src/DiabloInterface.Plugin.*/bin/" + configuration + "/*.dll")
        .Where(x => !x.FullPath.Contains(".Test"));
    var pluginDir = binDir + Directory("Plugins");
    CreateDirectory(pluginDir);
    CopyFiles(pluginFiles, pluginDir);
});

Task("Package")
    .IsDependentOn("CopyBuilt")
//    .IsDependentOn("CopyBuiltPlugins")
    .Does(() =>
{
    var assemblyInfo = ParseAssemblyInfo("./src/DiabloInterface/Properties/AssemblyInfo.cs");
    var fileName = string.Format("DiabloInterface-v{0}.zip", assemblyInfo.AssemblyInformationalVersion);
    CreateDirectory(binDir + Directory("Libs"));
    CreateDirectory(binDir + Directory("Plugins"));

    MoveFiles("./artifacts/bin/DiabloInterface.Plugin.*.dll", binDir + Directory("Plugins"));
    MoveFiles("./artifacts/bin/*.dll", binDir + Directory("Libs"));
    ZipCompress(binDir, buildDir + File(fileName));
});

Task("Default")
    .IsDependentOn("Package");

Task("Test")
    .IsDependentOn("CopyBuilt")
    .Does(() =>
{
    foreach(var project in GetFiles("./src/*.Test/*.csproj"))
    {
        Information("Solution {0}", project);
        var filename = project.GetFilenameWithoutExtension().ToString();
        var testDir = Directory(project.GetDirectory().ToString() + "/bin/" + configuration + "/");
        CleanDirectory(testDir);

        CopyDirectory(binDir, testDir);

        MSBuild(project, settings =>
        {
            if (FileExists(modernMSBuildPath))
                settings.ToolPath = modernMSBuildPath;

            settings.SetConfiguration(configuration);
            settings.SetVerbosity(Verbosity.Minimal);
        });

        var s = new MSTestSettings();
        s.ResultsFile = testDir + File(@"TestResults.trx");
        if (FileExists(modernMSTestPath))
            s.ToolPath = modernMSTestPath;

        MSTest(testDir + File(filename + ".dll"), s);
    }
});

RunTarget(target);
