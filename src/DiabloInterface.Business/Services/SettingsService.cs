namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class SettingsService : ISettingsService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string appPropertySettingsPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        readonly IApplicationStorage appStorage;

        public SettingsService(IApplicationStorage appStorage)
        {
            Logger.Info("Initializing settings service.");

            this.appStorage = appStorage;
        }

        public event EventHandler<ApplicationSettingsEventArgs> SettingsChanged;

        public ApplicationSettings CurrentSettings { get; private set; } = ApplicationSettings.Default;

        public string CurrentSettingsFile => appStorage.CurrentSettingsPath;

        public void LoadDefaultSettings()
        {
            UpdateSettings(ApplicationSettings.Default);
        }

        public void LoadSettingsFromPreviousSession()
        {
            Logger.Info($"App Property Settings Path is \"{appPropertySettingsPath}\".");

            // TODO: LoadSettingsInternal should throw a custom Exception.
            var settingsPath = appStorage.CurrentSettingsPath;
            Logger.Info($"Using previous settings from: \"{settingsPath}\".");

            var settings = LoadSettingsInternal(settingsPath);
            if (settings == null)
            {
                Logger.Info("No previous settings found. Using default.");
                settings = ApplicationSettings.Default;
            }

            UpdateSettings(settings);
        }

        public bool LoadSettings(string path)
        {
            var settings = LoadSettingsInternal(path);
            if (settings == null) return false;

            UpdateSettings(settings);
            return true;
        }

        public void SaveSettings(string path, ApplicationSettings settings)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var directory = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!directory.Exists)
            {
                Logger.Info($"Created directory at: \"{directory.FullName}\".");

                directory.Create();
            }

            Logger.Info($"Saving settings at: \"{path}\".");

            using (var writer = new JsonSettingsWriter(path))
            {
                writer.Write(settings);
            }
        }

        void UpdateSettings(ApplicationSettings newSettings)
        {
            CurrentSettings = newSettings;
            OnSettingsChanged(new ApplicationSettingsEventArgs(newSettings));
        }

        void OnSettingsChanged(ApplicationSettingsEventArgs e)
        {
            SettingsChanged?.Invoke(this, e);
        }

        ApplicationSettings LoadSettingsInternal(string path)
        {
            var settings = SettingsResolver.Load(path);
            if (settings != null)
            {
                appStorage.CurrentSettingsPath = path;
            }

            return settings;
        }
    }
}
