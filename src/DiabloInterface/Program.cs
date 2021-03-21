namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
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

            Logger.Initialize();

            var appInfo = new ApplicationInfo
            {
                Version = Application.ProductVersion,
                OS = Environment.OSVersion.VersionString,
                DotNet = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion)
            };

            Lib.Logging.CreateLogger(typeof(Program)).Info(appInfo);

            var pluginTypes = new List<Type>
            {
                typeof(Plugin.Autosplits.Plugin),
                typeof(Plugin.FileWriter.Plugin),
                typeof(Plugin.HttpClient.Plugin),
                typeof(Plugin.PipeServer.Plugin),
                typeof(Plugin.Updater.Plugin),
            };

            using (var di = DiabloInterface.Create(appInfo, pluginTypes))
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

            var logger = Lib.Logging.CreateLogger(typeof(Program));
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
    }
}
