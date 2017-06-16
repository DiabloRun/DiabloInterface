using System;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Framework;
using Zutatensuppe.DiabloInterface.Gui;
using static Zutatensuppe.DiabloInterface.Framework.NetFrameworkVersionComparator;

namespace Zutatensuppe.DiabloInterface
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            RegisterAppDomainExceptionLogging();
            if (ShouldQuitWithoutProperDotNetFramework())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        static bool ShouldQuitWithoutProperDotNetFramework()
        {
            if (IsFrameworkVersionSupported(NetFrameworkVersion.Version_4_5_2))
                return false;

            var versionName = NetFrameworkVersionExtension.FriendlyName(NewestFrameworkVersion);
            var message = "It seems that you do not have the .NET Framework 4.5.2 or later installed.\n" +
                          "Without the proper .NET Framework support the application is likely to crash.\n" +
                          $"Your .NET Framework version is: {versionName}\n\n" +
                          "Do you wish to try running the application anyway?";
            var result = MessageBox.Show(message, @".NET Framework Error", MessageBoxButtons.YesNo);
            return result == DialogResult.No;
        }

        static void RegisterAppDomainExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
        }

        static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (Logger.Instance != null)
            {
                Logger.Instance.WriteLine($"[ERROR]: Unhandled Exception: {e.ExceptionObject}");
                Logger.Instance.WriteLine("Terminating with error.");
            }
        }
    }
}
