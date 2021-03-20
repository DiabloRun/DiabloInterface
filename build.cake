#tool nuget:?package=vswhere
#addin nuget:?package=Cake.Compression&version=0.1.1
#addin nuget:?package=SharpZipLib&version=0.86.0
    
ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = 437;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var platformTarget = Argument("platformTarget", "Any CPU");
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
        if (platformTarget == "x64") {
          settings.SetPlatformTarget(PlatformTarget.x64);
        } else if (platformTarget == "x86") {
          settings.SetPlatformTarget(PlatformTarget.x86);
        }
    });

    var platformPart = "";
    if (platformTarget == "x64" || platformTarget == "x86") {
      platformPart = platformTarget + "/";
    }
    var path = "./src/DiabloInterface/bin/" + platformPart + configuration + "/";
    var allFiles =
        GetFiles(path + "*.dll") +
        GetFiles(path + "*.exe") +
        GetFiles(path + "*.config");
    var files = allFiles.Where(x => !x.GetFilename().ToString().Contains(".vshost.exe"));

    Information("Copying from {0}", path);
    CopyFiles(files, binDir);
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var platformPart = "";
    if (platformTarget == "x64" || platformTarget == "x86") {
      platformPart = "." + platformTarget;
    }
    var assemblyInfo = ParseAssemblyInfo("./src/DiabloInterface/Properties/AssemblyInfo.cs");
    var fileName = string.Format(
      "DiabloInterface-v{0}{1}.zip", 
      assemblyInfo.AssemblyInformationalVersion,
      platformPart
    );
    CreateDirectory(binDir + Directory("Libs"));
    CreateDirectory(binDir + Directory("Plugins"));

    MoveFiles("./artifacts/bin/DiabloInterface.Plugin.*.dll", binDir + Directory("Plugins"));
    MoveFiles("./artifacts/bin/*.dll", binDir + Directory("Libs"));
    ZipCompress(binDir, buildDir + File(fileName));
});

Task("Default")
    .IsDependentOn("Package");

Task("Test")
    .IsDependentOn("Build")
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
            if (platformTarget == "x64") {
              settings.SetPlatformTarget(PlatformTarget.x64);
            } else if (platformTarget == "x86") {
              settings.SetPlatformTarget(PlatformTarget.x86);
            }
        });

        var s = new MSTestSettings();
        s.ResultsFile = testDir + File(@"TestResults.trx");
        if (FileExists(modernMSTestPath))
            s.ToolPath = modernMSTestPath;

        MSTest(testDir + File(filename + ".dll"), s);
    }
});

RunTarget(target);
