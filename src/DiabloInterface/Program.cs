namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using Core.Logging;
    using Framework;
    using Gui;
    using Server;
    using Server.Handlers;
    using Business.Services;
    using System.Runtime.InteropServices;

    using static Framework.NetFrameworkVersionComparator;

    internal static class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        static void Main()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                ShowWindow(handle, SW_SHOW);
            }

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
                var pipeServer = CreatePipeServer(gameService);
                var mainWindow = new MainWindow(settingsService, gameService);
                Application.Run(mainWindow);

                pipeServer.Stop();
            }
        }

        static SettingsService CreateSettingsService()
        {
            var appStorage = new ApplicationStorage();
            var service = new SettingsService(appStorage);

            service.LoadSettingsFromPreviousSession();

            return service;
        }

        static DiabloInterfaceServer CreatePipeServer(GameService gameService)
        {
            const string PipeName = "DiabloInterfacePipe";

            var logger = LogServiceLocator.Get(typeof(Program));
            logger.Info("Initializing pipe server.");

            var pipeServer = new DiabloInterfaceServer(PipeName);

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
