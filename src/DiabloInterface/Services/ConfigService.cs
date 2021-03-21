namespace Zutatensuppe.DiabloInterface.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    
    using Newtonsoft.Json;
    using Zutatensuppe.DiabloInterface.Lib.Services;

    public class ConfigService : IConfigService
    {
        const string ConfDirPath = @".\Settings";
        const string ConfExtPattern = "*.conf";

        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string appPropertyConfigPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        readonly ApplicationStorage appStorage;

        FileSystemWatcher collectionWatcher;

        public ConfigService(ApplicationStorage appStorage)
        {
            Logger.Info("Initializing config service.");

            this.appStorage = appStorage;

            LoadSettingsFileCollection();
            InitializeCollectionWatcher();
        }

        public event EventHandler<ApplicationConfigEventArgs> Changed;

        public event EventHandler<ConfigCollectionEventArgs> CollectionChanged;

        public ApplicationConfig CurrentConfig { get; private set; } = ApplicationConfig.Default;

        public IEnumerable<FileInfo> ConfigFileCollection { get; private set; }

        public string CurrentConfigFile => appStorage.CurrentConfigPath;

        public void Dispose()
        {
            collectionWatcher?.Dispose();
        }

        public void Initialize()
        {
            Logger.Info($"App Property Config Path is \"{appPropertyConfigPath}\".");

            // TODO: LoadSettingsInternal should throw a custom Exception.
            var path = appStorage.CurrentConfigPath;

            Logger.Info($"Using previous config from: \"{path}\".");

            if (!Load(path))
            {
                Logger.Info("No previous config found. Using default.");
                UpdateConfig(ApplicationConfig.Default);
            }
        }

        public bool Load(string path)
        {
            var config = LoadInternal(path);
            if (config == null)
                return false;

            appStorage.CurrentConfigPath = path;
            UpdateConfig(config);
            return true;
        }

        public void Save(string path, ApplicationConfig config)
        {
            SaveConfig(path, config);
            Load(path);
        }

        public void Rename(string oldPath, string newPath)
        {
            // TODO: can this not be replaced with File.Move?
            File.Copy(oldPath, newPath);
            File.Delete(oldPath);
        }

        public void Clone(string oldPath, string newPath)
        {
            File.Copy(oldPath, newPath);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        void UpdateConfig(ApplicationConfig config)
        {
            CurrentConfig = config;
            Changed?.Invoke(this, new ApplicationConfigEventArgs(config));
        }

        void SaveConfig(string path, ApplicationConfig config)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var directory = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!directory.Exists)
            {
                Logger.Info($"Created directory at: \"{directory.FullName}\".");
                directory.Create();
            }

            Logger.Info($"Saving config at: \"{path}\".");

            Write(path, config);
        }

        string ToJson(ApplicationConfig config)
        {
            return JsonConvert.SerializeObject(
                config,
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            );
        }

        ApplicationConfig FromJson(string config)
        {
            return JsonConvert.DeserializeObject<ApplicationConfig>(
                File.ReadAllText(config),
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            );
        }

        void Write(string filename, ApplicationConfig config)
        {
            if (config == null) return;

            using (var writer = new StreamWriter(filename))
            {
                writer.Write(ToJson(config));
                writer.Flush();
                writer.Close();
            }
        }

        ApplicationConfig LoadInternal(string path)
        {
            try
            {
                var config = FromJson(path);
                if (config != null)
                {
                    Logger.Info($"Loaded config from \"{path}\"");
                    return config;
                }
            }
            catch (DirectoryNotFoundException)
            {
                Logger.Info($"Failed to read \"{path}\": Folder does not exist.");
            }
            catch (FileNotFoundException)
            {
                Logger.Info($"Failed to read \"{path}\": File does not exist.");
            }
            catch (JsonException e)
            {
                Logger.Warn($"Failed to read JSON \"{path}\": {e.Message}");
            }
            catch (IOException e)
            {
                Logger.Warn($"Failed to read \"{path}\": {e.Message}");
            }

            return null;
        }
        
        void InitializeCollectionWatcher()
        {
            Logger.Info("Initializing config file watcher.");

            // FileSystemWatcher requires that the directory we want to watch exist,
            // so we have to create it here.
            if (!Directory.Exists(ConfDirPath))
                Directory.CreateDirectory(ConfDirPath);

            collectionWatcher = new FileSystemWatcher(ConfDirPath);
            collectionWatcher.Created += CollectionWatcherOnNotify;
            collectionWatcher.Deleted += CollectionWatcherOnNotify;
            collectionWatcher.Renamed += CollectionWatcherOnNotify;
            collectionWatcher.EnableRaisingEvents = true;
        }

        void CollectionWatcherOnNotify(object sender, FileSystemEventArgs e)
        {
            LoadSettingsFileCollection();
        }

        void LoadSettingsFileCollection()
        {
            Logger.Info("Reloading config collection.");

            var collection = new List<FileInfo>();
            var dir = new DirectoryInfo(ConfDirPath);
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles(ConfExtPattern, SearchOption.AllDirectories);
                collection.AddRange(files);
            }

            ConfigFileCollection = collection;
            CollectionChanged?.Invoke(this, new ConfigCollectionEventArgs(collection));
        }
    }
}
