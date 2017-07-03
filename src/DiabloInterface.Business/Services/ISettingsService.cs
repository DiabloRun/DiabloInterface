namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;

    using Zutatensuppe.DiabloInterface.Business.Settings;

    public interface ISettingsService
    {
        /// <summary>
        ///     Occurs whenever any setting have changed.
        /// </summary>
        event EventHandler<ApplicationSettingsEventArgs> SettingsChanged;

        /// <summary>
        ///     Gets the currently loaded settings.
        /// </summary>
        ApplicationSettings CurrentSettings { get; }

        /// <summary>
        ///     Gets the path of the current settings file.
        /// </summary>
        string CurrentSettingsFile { get; }

        /// <summary>
        ///     Loads the default settings of the application.
        /// </summary>
        void LoadDefaultSettings();

        /// <summary>
        ///     Loads the settings that were used the last time the application was run.
        /// </summary>
        void LoadSettingsFromPreviousSession();

        /// <summary>
        ///     Loads settings from a file.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <returns>Returns true if file was successfully loaded.</returns>
        bool LoadSettings(string path);

        /// <summary>
        ///     Saves a settings object by writing or rewriting a file.
        ///     This does NOT load or set the current settings.
        /// </summary>
        /// <param name="path">Path to the file to write.</param>
        /// <param name="settings">The settings object to save.</param>
        void SaveSettings(string path, ApplicationSettings settings);
    }

    public class ApplicationSettingsEventArgs : EventArgs
    {
        public ApplicationSettingsEventArgs(ApplicationSettings settings)
        {
            Settings = settings;
        }

        public ApplicationSettings Settings { get; }
    }
}
