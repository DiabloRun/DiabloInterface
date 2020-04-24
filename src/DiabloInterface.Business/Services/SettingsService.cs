namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class SettingsService : ISettingsService, IDisposable
    {
        const string SettingsDirectoryPath = @".\Settings";
        const string SettingsFileExtensionPattern = "*.conf";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string appPropertySettingsPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        readonly IApplicationStorage appStorage;

        FileSystemWatcher collectionWatcher;

        public SettingsService(IApplicationStorage appStorage)
        {
            Logger.Info("Initializing settings service.");

            this.appStorage = appStorage;

            LoadSettingsFileCollection();
            InitializeCollectionWatcher();
        }

        public event EventHandler<ApplicationSettingsEventArgs> SettingsChanged;

        public event EventHandler<SettingsCollectionEventArgs> SettingsCollectionChanged;

        public ApplicationSettings CurrentSettings { get; private set; } = ApplicationSettings.Default;

        public IEnumerable<FileInfo> SettingsFileCollection { get; private set; }

        public string CurrentSettingsFile => appStorage.CurrentSettingsPath;

        public void Dispose()
        {
            collectionWatcher?.Dispose();
        }

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

        void InitializeCollectionWatcher()
        {
            Logger.Info("Initializing settings file watcher.");

            // FileSystemWatcher requires that the directory we want to watch exist,
            // so we have to create it here.
            EnsureSettingsDirectoryExists();

            collectionWatcher = new FileSystemWatcher(SettingsDirectoryPath);
            collectionWatcher.Created += CollectionWatcherOnNotify;
            collectionWatcher.Deleted += CollectionWatcherOnNotify;
            collectionWatcher.Renamed += CollectionWatcherOnNotify;

            collectionWatcher.EnableRaisingEvents = true;
        }

        static void EnsureSettingsDirectoryExists()
        {
            if (!Directory.Exists(SettingsDirectoryPath))
            {
                Directory.CreateDirectory(SettingsDirectoryPath);
            }
        }

        void UpdateSettings(ApplicationSettings newSettings)
        {
            CurrentSettings = newSettings;
            OnSettingsChanged(new ApplicationSettingsEventArgs(newSettings));
        }

        void CollectionWatcherOnNotify(object sender, FileSystemEventArgs e)
        {
            LoadSettingsFileCollection();
        }

        void LoadSettingsFileCollection()
        {
            Logger.Info("Reloading settings collection.");

            var collection = new List<FileInfo>();
            var directory = new DirectoryInfo(SettingsDirectoryPath);
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles(SettingsFileExtensionPattern, SearchOption.AllDirectories);
                collection.AddRange(files);
            }

            SettingsFileCollection = collection;
            var eventArgs = new SettingsCollectionEventArgs(collection);
            OnSettingsCollectionChanged(eventArgs);
        }

        void OnSettingsChanged(ApplicationSettingsEventArgs e) =>
            SettingsChanged?.Invoke(this, e);

        void OnSettingsCollectionChanged(SettingsCollectionEventArgs e) =>
            SettingsCollectionChanged?.Invoke(this, e);

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
