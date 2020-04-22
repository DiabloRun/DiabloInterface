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
            var settingsReader = new JsonSettingsReader(path);
            try
            {
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
                Logger.Info($"Failed to read \"{path}\": File does not exist.");
                return null;
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

            return null;
        }
    }
}
