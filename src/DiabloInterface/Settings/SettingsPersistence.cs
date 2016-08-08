﻿using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace Zutatensuppe.DiabloInterface.Settings
{
    class SettingsPersistence
    {
        //todo:remove the other filetype options, only Di should be writing these so they should all have the right extension
        const string DefaultSettingsFile = @".\Settings\DefaultSettings.conf";
        public const string FileFilter = "Config Files|*.conf;*.json|All Files|*.*";

        private string appPropertySettingsPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

        public string CurrentSettingsFile
        {
            get
            {
                string filename = Properties.Settings.Default.SettingsFile;
                Logger.Instance.WriteLine("Current settings file is \"{0}\"", filename);
                if (string.IsNullOrEmpty(filename))
                {
                    filename = DefaultSettingsFile;
                    Logger.Instance.WriteLine("Using Default Config File instead: \"{0}\"", filename);
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
            Logger.Instance.WriteLine("App Property Settings Path is \"{0}\"", appPropertySettingsPath);
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
