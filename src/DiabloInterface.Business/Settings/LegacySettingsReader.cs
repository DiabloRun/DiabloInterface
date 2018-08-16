namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;

    public class LegacySettingsReader : ISettingsReader
    {
        readonly ILegacySettingsResolver resolver;
        readonly StreamReader reader;

        public LegacySettingsReader(ILegacySettingsResolver resolver, string filename)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            reader = new StreamReader(filename);
        }

        public LegacySettingsReader(ILegacySettingsResolver resolver, string filename, Encoding encoding)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
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
            bool GetBoolDefault(string field, bool defaultValue) =>
                data.ContainsKey(field) ? (string)data[field] == "1" : defaultValue;

            string GetStringDefault(string field, string defaultValue) =>
                data.ContainsKey(field) ? (string)data[field] : defaultValue;

            int GetIntDefault(string field, int defaultValue)
            {
                if (!data.TryGetValue(field, out var obj))
                    return defaultValue;
                return int.TryParse((string)obj, out int value) ? value : defaultValue;
            }

            // Assign fields from settings.
            settings.FontName = GetStringDefault("Font", settings.FontName);
            settings.FontSize = GetIntDefault("FontSize", settings.FontSize);
            settings.FontSizeTitle = GetIntDefault("FontSizeTitle", settings.FontSizeTitle);
            settings.CheckUpdates = GetBoolDefault("CheckUpdates", settings.CheckUpdates);
            settings.CreateFiles = GetBoolDefault("CreateFiles", settings.CreateFiles);
            settings.DoAutosplit = GetBoolDefault("DoAutosplit", settings.DoAutosplit);

            ConvertRuneData(settings, data);

            // Handle auto splits.
            if (data.ContainsKey("AutoSplits"))
            {
                settings.Autosplits = new List<AutoSplit>();
                var splits = (List<string>)data["AutoSplits"];
                foreach (var autoSplitData in splits)
                {
                    string[] splitParts = autoSplitData.Split(new[] { "|" }, 4, StringSplitOptions.None);
                    if (splitParts.Length < 3)
                        continue;

                    if (!short.TryParse(splitParts[1], out short splitType))
                        continue;

                    if (!short.TryParse(splitParts[2], out short splitValue))
                        continue;

                    short splitDifficulty = 0;
                    if (splitParts.Length == 4)
                    {
                        if (!short.TryParse(splitParts[3], out splitDifficulty))
                            continue;
                    }


                    string splitName = splitParts[0];
                    var autoSplit = new AutoSplit(splitName, (AutoSplit.SplitType)splitType, splitValue, splitDifficulty);
                    settings.Autosplits.Add(autoSplit);
                }
            }

            var legacyObject = new LegacySettingsObject(data);
            return resolver.ResolveSettings(settings, legacyObject);
        }

        void ConvertRuneData(ApplicationSettings settings, IReadOnlyDictionary<string, object> data)
        {
            List<int> runes = data.ContainsKey("Runes")
                ? (List<int>)data["Runes"]
                : new List<int>();

            settings.ClassRunes = new List<ClassRuneSettings>
            {
                new ClassRuneSettings()
                {
                    Class = null,
                    Difficulty = null,
                    Runes = runes.Select(runeId => (Rune)runeId).ToList()
                }
            };
        }

        Dictionary<string, object> ReadToDict()
        {
            var data = new Dictionary<string, object>();
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                string[] parts = line.Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (parts.Length < 2)
                {
                    continue;
                }

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                // Condense rune elements into an array.
                if (key == "Rune")
                {
                    data["Runes"] = GetRunes(data, value);
                }

                // Condense auto splits into an array.
                else if (key == "AutoSplit")
                {
                    data["AutoSplits"] = GetAutosplits(data, value);
                }

                // Otherwise, just add the string value.
                else data[key] = value;
            }

            return data;
        }

        private static List<string> GetAutosplits(Dictionary<string, object> data, string value)
        {

            // Get or create autosplit array.
            if (!data.TryGetValue("AutoSplits", out object splitsObj))
            {
                splitsObj = new List<string>();
            }
            List<string> splits = (List<string>)splitsObj;

            splits.Add(value);
            return splits;
        }

        private static List<int> GetRunes(Dictionary<string, object> data, string value)
        {
            // Get or create rune array.
            if (!data.TryGetValue("Runes", out object runesObj))
            {
                runesObj = new List<int>();
            }
            List<int> runes = (List<int>)runesObj;

            if (int.TryParse(value, out int rune))
            {
                runes.Add(rune);
            }

            return runes;
        }
    }
}
