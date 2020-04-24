namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Zutatensuppe.DiabloInterface.Business.Plugin;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Framework;
    using Zutatensuppe.DiabloInterface.Gui;

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

        private static List<Assembly> LoadPlugInAssemblies()
        {
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory));
            FileInfo[] files = dInfo.GetFiles("DiabloInterface.Plugin.*.dll");
            List<Assembly> plugInAssemblyList = new List<Assembly>();

            if (null != files)
            {
                foreach (FileInfo file in files)
                {
                    plugInAssemblyList.Add(Assembly.LoadFile(file.FullName));
                }
            }

            return plugInAssemblyList;
        }

        static List<IPlugin> GetPlugIns(List<Assembly> assemblies, GameService gameService, ISettingsService settingsService)
        {
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
                availableTypes.AddRange(currentAssembly.GetTypes());

            // get a list of objects that implement the IPlugin interface
            List<Type> pluginList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                return interfaceTypes.Contains(typeof(IPlugin));
            });

            // convert the list of Objects to an instantiated list of IPlugins
            return pluginList.ConvertAll<IPlugin>(delegate (Type t) { return Activator.CreateInstance(t, gameService, settingsService) as IPlugin; });
        }

        static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var settingsService = CreateSettingsService())
            using (var gameService = new GameService(settingsService))
            {
                CheckForApplicationUpdates(settingsService);

                var plugins = GetPlugIns(LoadPlugInAssemblies(), gameService, settingsService);

                var mainWindow = new MainWindow(
                    settingsService,
                    gameService,
                    plugins
                );
                Application.Run(mainWindow);

                foreach (IPlugin p in plugins)
                    p.Dispose();
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
    }
}
