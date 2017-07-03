namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Zutatensuppe.DiabloInterface.Core.Logging;

    internal static class SettingsResolver
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public static ApplicationSettings Load(string path)
        {
            ILegacySettingsResolver resolver = new DefaultLegacySettingsResolver();
            foreach (Func<ISettingsReader> factory in ReaderFactoryEnumeration(resolver, path))
            {
                ISettingsReader settingsReader = null;

                try
                {
                    settingsReader = factory();

                    // Try to read settings.
                    var settings = settingsReader.Read();
                    if (settings != null)
                    {
                        Logger.Info($"Loaded settings from \"{path}\"");

                        return settings;
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Logger.Info($"Failed to read \"{path}\": Folder does not exist.");
                    return null;
                }
                catch (FileNotFoundException)
                {
                    // Stop iterating, file doesn't exist.
                    break;
                }
                catch (IOException e)
                {
                    // Log other IO exceptions.
                    Logger.Warn("Failed to read settings", e);
                }
                finally
                {
                    settingsReader?.Dispose();
                }
            }

            // Failed to load.
            return null;
        }

        static IEnumerable<Func<ISettingsReader>> ReaderFactoryEnumeration(ILegacySettingsResolver resolver, string filename)
        {
            yield return () => new JsonSettingsReader(resolver, filename);
            yield return () => new LegacySettingsReader(resolver, filename);
        }
    }
}
