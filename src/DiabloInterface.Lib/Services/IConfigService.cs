namespace Zutatensuppe.DiabloInterface.Lib.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IConfigService: IDisposable
    {
        /// <summary>
        ///     Occurs whenever any setting have changed.
        /// </summary>
        event EventHandler<ApplicationConfigEventArgs> Changed;

        /// <summary>
        ///     Occurs whenever a file is added or removed from the settings directory.
        /// </summary>
        event EventHandler<ConfigCollectionEventArgs> CollectionChanged;

        /// <summary>
        ///     Gets the currently loaded settings.
        /// </summary>
        ApplicationConfig CurrentConfig { get; }

        /// <summary>
        ///     Gets the settings file collection.
        /// </summary>
        IEnumerable<FileInfo> ConfigFileCollection { get; }

        /// <summary>
        ///     Gets the path of the current settings file.
        /// </summary>
        string CurrentConfigFile { get; }

        /// <summary>
        ///     Loads the settings that were used the last time the application was run.
        /// </summary>
        void Initialize();

        bool Load(string path);
        void Save(string path, ApplicationConfig config);
        void Rename(string oldPath, string newPath);
        void Clone(string oldPath, string newPath);
        void Delete(string path);
    }

    public class ApplicationConfigEventArgs : EventArgs
    {
        public ApplicationConfigEventArgs(ApplicationConfig config)
        {
            Config = config;
        }

        public ApplicationConfig Config { get; }
    }

    public class ConfigCollectionEventArgs : EventArgs
    {
        public ConfigCollectionEventArgs(IEnumerable<FileInfo> collection)
        {
            Collection = collection;
        }

        public IEnumerable<FileInfo> Collection { get; }
    }
}
