namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Framework;
    using Zutatensuppe.DiabloInterface.Gui;
    using Zutatensuppe.DiabloInterface.Server;
    using Zutatensuppe.DiabloInterface.Server.Handlers;

    using static Framework.NetFrameworkVersionComparator;

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            RegisterAppDomainExceptionLogging();
            if (ShouldQuitWithoutProperDotNetFramework())
            {
                return;
            }

            InitializeLogger();

            RunApplication();
        }

        static void RegisterAppDomainExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
        }

        static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            var logger = LogServiceLocator.Get(typeof(Program));
            logger?.Fatal("Unhandled Exception", (Exception)e.ExceptionObject);
        }

        static bool ShouldQuitWithoutProperDotNetFramework()
        {
            if (IsFrameworkVersionSupported(NetFrameworkVersion.Version_4_5_2))
            {
                return false;
            }

            var versionName = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion);
            var message = "It seems that you do not have the .NET Framework 4.5.2 or later installed.\n" +
                          "Without the proper .NET Framework support the application is likely to crash.\n" +
                          $"Your .NET Framework version is: {versionName}\n\n" +
                          "Do you wish to try running the application anyway?";
            var result = MessageBox.Show(message, @".NET Framework Error", MessageBoxButtons.YesNo);
            return result == DialogResult.No;
        }

        static void InitializeLogger()
        {
            Log4NetLogger.Initialize();

            LogApplicationInfo();
        }

        static void LogApplicationInfo()
        {
            var logger = LogServiceLocator.Get(typeof(Program));
            logger.Info($"Diablo Interface Version {Application.ProductVersion}");
            logger.Info($"Operating system: {Environment.OSVersion}");

            var versionName = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion);
            logger.Info($".NET Framework: {versionName}");
        }

        static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var settingsService = CreateSettingsService())
            using (var gameService = new GameService(settingsService))
            {
                CheckForApplicationUpdates(settingsService);

                new CharacterStatFileWriterService(settingsService, gameService);
                var autoSplitService = new AutoSplitService(settingsService, gameService);
                var pipeServer = CreatePipeServer(gameService, settingsService);
                var mainWindow = new MainWindow(settingsService, gameService, autoSplitService);
                Application.Run(mainWindow);

                pipeServer.Stop();
            }
        }

        static void CheckForApplicationUpdates(ISettingsService settingsService)
        {
            if (settingsService.CurrentSettings.CheckUpdates)
            {
                VersionChecker.AutomaticallyCheckForUpdate();
            }
        }

        static SettingsService CreateSettingsService()
        {
            var appStorage = new ApplicationStorage();
            var service = new SettingsService(appStorage);

            service.LoadSettingsFromPreviousSession();

            return service;
        }

        static DiabloInterfaceServer CreatePipeServer(GameService gameService, SettingsService settingsService)
        {
            var logger = LogServiceLocator.Get(typeof(Program));
            logger.Info("Initializing pipe server.");
            
            var pipeServer = new DiabloInterfaceServer(settingsService.CurrentSettings.PipeName);

            var dataReader = gameService.DataReader;
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(dataReader));

            return pipeServer;
        }
    }
}
