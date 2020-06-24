namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Framework;
    using Zutatensuppe.DiabloInterface.Gui;
    using Zutatensuppe.DiabloInterface.Plugin;
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

            Log4NetLogger.Initialize();

            LogApplicationInfo();

            var pluginTypes = new List<Type>
            {
                typeof(Plugin.Autosplits.Plugin),
                typeof(Plugin.FileWriter.Plugin),
                typeof(Plugin.HttpClient.Plugin),
                typeof(Plugin.PipeServer.Plugin),
                typeof(Plugin.Updater.Plugin),
            };

            using (var di = DiabloInterface.Create(pluginTypes))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow(di));
            }
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
            if (IsFrameworkVersionSupported(NetFrameworkVersion.Version_4_6_1))
                return false;

            var versionName = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion);
            var message = "It seems that you do not have the .NET Framework 4.6.1 or later installed.\n" +
                "Without the proper .NET Framework support the application is likely to crash.\n" +
                $"Your .NET Framework version is: {versionName}\n\n" +
                "Do you wish to try running the application anyway?";
            var result = MessageBox.Show(message, @".NET Framework Error", MessageBoxButtons.YesNo);
            return result == DialogResult.No;
        }
        
        static void LogApplicationInfo()
        {
            var logger = LogServiceLocator.Get(typeof(Program));
            logger.Info($"Diablo Interface Version {Application.ProductVersion}");
            logger.Info($"Operating system: {Environment.OSVersion}");
            var versionName = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion);
            logger.Info($".NET Framework: {versionName}");
        }
    }
}
