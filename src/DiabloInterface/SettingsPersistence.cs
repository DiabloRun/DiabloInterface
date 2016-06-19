using DiabloInterface.Logging;
using DiabloInterface.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DiabloInterface
{
    class SettingsPersistence
    {
        const string DefaultSettingsFile = "settings.conf";
        public const string FileFilter = "Config Files|*.conf;*.json|All Files|*.*";

        public string CurrentSettingsFile
        {
            get
            {
                string filename = Properties.Settings.Default.SettingsFile;
                if (string.IsNullOrEmpty(filename))
                    filename = DefaultSettingsFile;

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
            return Load(CurrentSettingsFile);
        }

        public ApplicationSettings Load(string filename)
        {
            ILegacySettingsResolver resolver = new DefaultLegacySettingsResolver();
            foreach (var factory in ReaderFactoryEnumeration(resolver, filename))
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
                        Logger.Instance.WriteLine("Loaded settings from \"{0}\"", filename);

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
                    Logger.Instance.WriteLine("Settings Error: [{0}] {1}", e.GetType().Name, e.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Dispose();
                    }
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

            Logger.Instance.WriteLine("Saving settings as \"{0}\".", filename);

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
