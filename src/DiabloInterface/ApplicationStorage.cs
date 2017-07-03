namespace Zutatensuppe.DiabloInterface
{
    using System.Reflection;

    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    internal class ApplicationStorage : IApplicationStorage
    {
        const string DefaultSettingsFile = @".\Settings\DefaultSettings.conf";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public string CurrentSettingsPath
        {
            get
            {
                var filename = Properties.Settings.Default.SettingsFile;
                if (!string.IsNullOrEmpty(filename))
                {
                    return filename;
                }

                filename = DefaultSettingsFile;
                Logger.Info($"Assigning default settings file name: \"{filename}\"");

                return filename;
            }

            set
            {
                Properties.Settings.Default.SettingsFile = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
