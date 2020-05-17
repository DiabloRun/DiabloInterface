namespace Zutatensuppe.DiabloInterface
{
    using System.Reflection;

    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class ApplicationStorage
    {
        const string DefaultConfigFile = @".\Settings\DefaultSettings.conf";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public string CurrentConfigPath
        {
            get
            {
                var filename = Properties.Settings.Default.SettingsFile;
                if (!string.IsNullOrEmpty(filename))
                {
                    return filename;
                }

                filename = DefaultConfigFile;
                Logger.Info($"Assigning default config file name: \"{filename}\"");

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
