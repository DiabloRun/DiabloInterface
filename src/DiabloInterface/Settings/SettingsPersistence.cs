using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace Zutatensuppe.DiabloInterface.Settings
{
    class SettingsPersistence
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        //todo:remove the other filetype options, only Di should be writing these so they should all have the right extension
        const string DefaultSettingsFile = @".\Settings\DefaultSettings.conf";
        public const string FileFilter = "Config Files|*.conf;*.json|All Files|*.*";

        readonly string appPropertySettingsPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

        public string CurrentSettingsFile
        {
            get
            {
                var filename = Properties.Settings.Default.SettingsFile;
                Logger.Info($"Current settings file is \"{filename}\"");
                if (string.IsNullOrEmpty(filename))
                {
                    filename = DefaultSettingsFile;
                    Logger.Info($"Using Default Config File instead: \"{filename}\"");
                }

                return filename;
            }
            private set
            {
                Properties.Settings.Default.SettingsFile = value;
                Properties.Settings.Default.Save();
            }
        }

        IEnumerable<Func<ISettingsReader>> ReaderFactoryEnumeration(ILegacySettingsResolver resolver, string filename)
        {
            yield return () => new JsonSettingsReader(resolver, filename);
            yield return () => new LegacySettingsReader(resolver, filename);
        }

        public ApplicationSettings Load()
        {
            Logger.Info($"App Property Settings Path is \"{appPropertySettingsPath}\"");
            return Load(CurrentSettingsFile);
        }

        public ApplicationSettings Load(string filename)
        {
            ILegacySettingsResolver resolver = new DefaultLegacySettingsResolver();
            foreach (Func<ISettingsReader> factory in ReaderFactoryEnumeration(resolver, filename))
            {
                ISettingsReader reader = null;

                try
                {
                    // Create reader from factory.
                    reader = factory();

                    // Try to read settings.
                    ApplicationSettings settings = reader.Read();
                    if (settings != null)
                    {
                        Logger.Info($"Loaded settings from \"{filename}\"");

                        // Update current settings file.
                        if (CurrentSettingsFile != filename)
                        {
                            CurrentSettingsFile = filename;
                        }

                        return settings;
                    }
                }
                // Stop iterating, file doesn't exist.
                catch (FileNotFoundException)
                {
                    break;
                }
                // Log other IO exceptions.
                catch (IOException e)
                {
                    Logger.Warn("Failed to read settings", e);
                }
                finally
                {
                    reader?.Dispose();
                }
            }

            // Failed to load.
            return null;
        }

        public void Save(ApplicationSettings settings)
        {
            Save(settings, CurrentSettingsFile);
        }

        public void Save(ApplicationSettings settings, string filename)
        {
            // check if directory exists first
            string directory = new FileInfo(filename).Directory.FullName;
            if (!Directory.Exists(directory))
            {
                SaveFileDialog d = new SaveFileDialog();
                d.Filter = FileFilter;
                if (d.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(d.FileName))
                {
                    filename = d.FileName;
                } else
                {
                    return;
                }
            }

            Logger.Info($"Saving settings as \"{filename}\".");

            using (var writer = new JsonSettingsWriter(filename))
            {
                writer.Write(settings);
            }

            if (CurrentSettingsFile != filename)
            {
                CurrentSettingsFile = filename;
            }
        }
    }
}
