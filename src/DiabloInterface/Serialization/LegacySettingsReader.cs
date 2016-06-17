using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiabloInterface.Serialization
{
    public class LegacySettingsReader : ISettingsReader
    {
        ILegacySettingsResolver resolver;
        StreamReader reader;

        public LegacySettingsReader(ILegacySettingsResolver resolver, string filename)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");

            this.resolver = resolver;
            reader = new StreamReader(filename);
        }

        public LegacySettingsReader(ILegacySettingsResolver resolver, string filename, Encoding encoding)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");

            this.resolver = resolver;
            reader = new StreamReader(filename, encoding);
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                reader.Close();
            }
        }

        public ApplicationSettings Read()
        {
            var settings = new ApplicationSettings();
            Dictionary<string, object> data = ReadToDict();

            // Helper methods for assigning fields.
            Func<string, bool, bool> getBoolDefault = (field, default_value) =>
                data.ContainsKey(field) ? (string)data[field] == "1" : default_value;
            Func<string, string, string> getStringDefault = (field, default_value) =>
                data.ContainsKey(field) ? (string)data[field] : default_value;
            Func<string, int, int> getIntDefault = (field, default_value) => {
                int value;
                object obj;
                if (!data.TryGetValue(field, out obj))
                    return default_value;
                return int.TryParse((string)obj, out value) ? value : default_value;
            };

            // Assign fields from settings.
            settings.FontName = getStringDefault("Font", settings.FontName);
            settings.FontSize = getIntDefault("FontSize", settings.FontSize);
            settings.FontSizeTitle = getIntDefault("FontSizeTitle", settings.FontSizeTitle);
            settings.CheckUpdates = getBoolDefault("CheckUpdates", settings.CheckUpdates);
            settings.CreateFiles = getBoolDefault("CreateFiles", settings.CreateFiles);
            settings.D2Version = getStringDefault("D2Version", settings.D2Version);
            settings.DoAutosplit = getBoolDefault("DoAutosplit", settings.DoAutosplit);
            settings.Runes = data.ContainsKey("Runes") ? (List<int>)data["Runes"] : settings.Runes;

            // Handle auto splits.
            if (data.ContainsKey("AutoSplits"))
            {
                settings.Autosplits = new List<AutoSplit>();
                var splits = (List<string>)data["AutoSplits"];
                foreach (var autoSplitData in splits)
                {
                    string[] splitParts = autoSplitData.Split(new string[] { "|" }, 4, StringSplitOptions.None);
                    if (splitParts.Length < 3) continue;

                    string splitName = splitParts[0];
                    short splitType;
                    short splitValue;
                    short splitDifficulty = 0;

                    if (!short.TryParse(splitParts[1], out splitType))
                        continue;
                    if (!short.TryParse(splitParts[2], out splitValue))
                        continue;

                    if (splitParts.Length == 4)
                    {
                        if (!short.TryParse(splitParts[3], out splitDifficulty))
                            continue;
                    }

                    var autoSplit = new AutoSplit(splitName, (AutoSplit.SplitType)splitType, splitValue, splitDifficulty);
                    settings.Autosplits.Add(autoSplit);
                }
            }

            var legacyObject = new LegacySettingsObject(data);
            return resolver.ResolveSettings(settings, legacyObject);
        }

        Dictionary<string, object> ReadToDict()
        {
            var data = new Dictionary<string, object>();
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;

                string[] parts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                if (parts.Length < 2) continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                // Condense rune elements into an array.
                if (key == "Rune")
                {
                    // Get or create rune array.
                    object runesObj;
                    if (!data.TryGetValue("Runes", out runesObj))
                        runesObj = new List<int>();
                    List<int> runes = (List<int>)runesObj;

                    int rune;
                    if (int.TryParse(value, out rune))
                    {
                        runes.Add(rune);
                    }

                    data["Runes"] = runes;
                }

                // Condense auto splits into an array.
                else if (key == "AutoSplit")
                {
                    // Get or create autosplit array.
                    object splitsObj;
                    if (!data.TryGetValue("AutoSplits", out splitsObj))
                        splitsObj = new List<string>();
                    List<string> splits = (List<string>)splitsObj;

                    splits.Add(value);
                    data["AutoSplits"] = splits;
                }

                // Otherwise, just add the string value.
                else data[key] = value;
            }

            return data;
        }
    }
}
