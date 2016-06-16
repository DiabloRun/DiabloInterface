using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;

namespace DiabloInterface
{
    public class SettingsHolder
    {
        # region default values
        private const string DEFAULT_FONT_NAME = "Courier New";
        private const string DEFAULT_D2_VERSION = "";
        private const int DEFAULT_FONT_SIZE = 10;
        private const int DEFAULT_FONT_SIZE_TITLE = 18;
        private const bool DEFAULT_CREATE_FILES = false;
        private const bool DEFAULT_DO_AUTOSPLIT = false;
        private const bool DEFAULT_CHECK_UPDATES = false;
        private const string DEFAULT_TRIGGER_KEYS = "";
        private const bool DEFAULT_DISPLAY_NAME = true;
        private const bool DEFAULT_DISPLAY_LEVEL = true;
        private const bool DEFAULT_DISPLAY_DEATH_COUNTER = true;
        private const bool DEFAULT_DISPLAY_GOLD = true;
        private const bool DEFAULT_DISPLAY_RESISTANCES = true;
        private const bool DEFAULT_DISPLAY_BASE_STATS = true;
        private const bool DEFAULT_DISPLAY_RUNES = true;
        private const bool DEFAULT_DISPLAY_ADVANCED_STATS = false;
        #endregion

        public const string DefaultSettingsFile = "settings.conf";
        public string FileFolder = "txt";

        public string FontName;
        public string D2Version;
        public int FontSize;
        public int FontSizeTitle;
        public bool CreateFiles;
        public bool DoAutosplit;
        public bool CheckUpdates;
        public Keys AutosplitHotkey { get; set; } = Keys.None;
        public List<AutoSplit> Autosplits;
        public List<int> Runes;
        public bool DisplayName;
        public bool DisplayLevel;
        public bool DisplayDeathCounter;
        public bool DisplayGold;
        public bool DisplayResistances;
        public bool DisplayBaseStats;
        public bool DisplayRunes;
        public bool DisplayAdvancedStats;

        public SettingsHolder ()
        {
            // init members with default values
            init();
        }

        private void init()
        {
            FontName = DEFAULT_FONT_NAME;
            D2Version = DEFAULT_D2_VERSION;
            FontSize = DEFAULT_FONT_SIZE;
            FontSizeTitle = DEFAULT_FONT_SIZE_TITLE;
            CreateFiles = DEFAULT_CREATE_FILES;
            DoAutosplit = DEFAULT_DO_AUTOSPLIT;
            CheckUpdates = DEFAULT_CHECK_UPDATES;
            DisplayName = DEFAULT_DISPLAY_NAME;
            DisplayLevel = DEFAULT_DISPLAY_LEVEL;
            DisplayDeathCounter = DEFAULT_DISPLAY_DEATH_COUNTER;
            DisplayGold = DEFAULT_DISPLAY_GOLD;
            DisplayResistances = DEFAULT_DISPLAY_RESISTANCES;
            DisplayBaseStats = DEFAULT_DISPLAY_BASE_STATS;
            DisplayAdvancedStats = DEFAULT_DISPLAY_ADVANCED_STATS;
            DisplayRunes = DEFAULT_DISPLAY_RUNES;
            Autosplits = new List<AutoSplit>();
            Runes = new List<int>();
        }

        public void saveAs(string file)
        {
            List<dynamic> autosplits = new List<dynamic>();
            foreach (AutoSplit autosplit in this.Autosplits)
            {
                autosplits.Add(new
                {
                    Name = autosplit.Name.Replace('|', ' '),
                    Type = (int)autosplit.Type,
                    Value = autosplit.Value,
                    Difficulty = autosplit.Difficulty,
                });
            }

            dynamic json = new
            {
                Font = FontName,
                FontSize = FontSize,
                FontSizeTitle = FontSizeTitle,
                CreateFiles = CreateFiles,
                DoAutosplit = DoAutosplit,
                CheckUpdates = CheckUpdates,
                AutosplitHotkey = AutosplitHotkey,
                D2Version = D2Version,

                DisplayName = DisplayName,
                DisplayLevel = DisplayLevel,
                DisplayDeathCounter = DisplayDeathCounter,
                DisplayGold = DisplayGold,
                DisplayResistances = DisplayResistances,
                DisplayBaseStats = DisplayResistances,
                DisplayAdvancedStats = DisplayAdvancedStats,
                DisplayRunes = DisplayRunes,

                AutoSplits = autosplits,
                Runes = Runes,
            };
            string jsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
            File.WriteAllText(file, jsonString);

            Properties.Settings.Default.SettingsFile = file;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// old config file reader
        /// </summary>
        private void loadFromLegacy(string[] conf)
        {
            string triggerKeys = null;

            foreach (string line in conf)
            {
                string[] parts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                switch (parts[0])
                {
                    case "Font": FontName = parts[1]; break;
                    case "CheckUpdates": CheckUpdates = (parts[1] == "1"); break;
                    case "CreateFiles": CreateFiles = (parts[1] == "1"); break;
                    case "D2Version": D2Version = parts[1]; break;
                    case "TriggerKeys": triggerKeys = parts[1]; break;
                    case "DoAutosplit": DoAutosplit = (parts[1] == "1"); break;
                    case "Rune": Runes.Add(Convert.ToInt32(parts[1])); break;
                    case "FontSize":
                        try
                        {
                            FontSize = Int32.Parse(parts[1]);
                            if (FontSize == 0) { FontSize = 10; }
                        }
                        catch { FontSize = 10; }
                        break;
                    case "FontSizeTitle":
                        try
                        {
                            FontSizeTitle = Int32.Parse(parts[1]);
                            if (FontSizeTitle == 0) { FontSizeTitle = 10; }
                        }
                        catch { FontSizeTitle = 10; }
                        break;
                    case "AutoSplit":
                        string[] parts2 = parts[1].Split(new string[] { "|" }, 4, StringSplitOptions.None);
                        if (parts2.Length == 3)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                (AutoSplit.SplitType)Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                (short)0
                            );
                            Autosplits.Add(autosplit);
                        }
                        else if (parts2.Length == 4)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                (AutoSplit.SplitType)Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                Convert.ToInt16(parts2[3])
                            );
                            Autosplits.Add(autosplit);
                        }
                        break;
                }
            }

            // Handle legacy hotkeys.
            AutosplitHotkey = KeyManager.LegacyStringToKey(triggerKeys);
        }

        public void loadFrom(string file)
        {
            // init members with default values
            init();

            Properties.Settings.Default.SettingsFile = file;
            Properties.Settings.Default.Save();

            if (!File.Exists(file))
            {
                return;
            }
            dynamic json = null;
            try
            {
                json = JsonConvert.DeserializeObject(File.ReadAllText(file));
            } catch (JsonReaderException)
            {
                // try to load old config file
                loadFromLegacy(File.ReadAllLines(file));
                return;
            }

            try { FontName = (string)json.Font; } catch (RuntimeBinderException) { }
            try { FontSize = (int)json.FontSize; } catch (RuntimeBinderException) { }
            try { FontSizeTitle = (int)json.FontSizeTitle; } catch (RuntimeBinderException) { }
            try { CreateFiles = (bool)json.CreateFiles; } catch (RuntimeBinderException) { }
            try { DoAutosplit = (bool)json.DoAutosplit; } catch (RuntimeBinderException) { }
            try { CheckUpdates = (bool)json.CheckUpdates; } catch (RuntimeBinderException) { }
            try { D2Version = (string)json.D2Version; } catch (RuntimeBinderException) { }

            // First try loading hotkeys normally.
            try { AutosplitHotkey = json.AutosplitHotkey; }
            catch (RuntimeBinderException)
            {
                // Revert to trying legacy hotkeys.
                try { AutosplitHotkey = KeyManager.LegacyStringToKey((string)json.TriggerKeys); }
                // If all else fails, assume there is no hotkey.
                catch (RuntimeBinderException) { AutosplitHotkey = Keys.None; }
            }

            try { DisplayName = (bool)json.DisplayName; } catch (RuntimeBinderException) { }
            try { DisplayLevel = (bool)json.DisplayLevel; } catch (RuntimeBinderException) { }
            try { DisplayDeathCounter = (bool)json.DisplayDeathCounter; } catch (RuntimeBinderException) { }
            try { DisplayGold = (bool)json.DisplayGold; } catch (RuntimeBinderException) { }
            try { DisplayResistances = (bool)json.DisplayResistances; } catch (RuntimeBinderException) { }
            try { DisplayBaseStats = (bool)json.DisplayBaseStats; } catch (RuntimeBinderException) { }
            try { DisplayAdvancedStats = (bool)json.DisplayAdvancedStats; } catch (RuntimeBinderException) { }
            try { DisplayRunes = (bool)json.DisplayRunes; } catch (RuntimeBinderException) { }

            try
            {
                foreach (dynamic autosplit in json.AutoSplits)
                {
                    string n = "";
                    AutoSplit.SplitType t = AutoSplit.SplitType.None;
                    short v = 0;
                    short d = 0;
                    try { n = (string)autosplit.Name; } catch (RuntimeBinderException) {}
                    try { t = (AutoSplit.SplitType)autosplit.Type; } catch (RuntimeBinderException) {}
                    try { v = (short)autosplit.Value; } catch (RuntimeBinderException) {}
                    try { d = (short)autosplit.Difficulty; } catch (RuntimeBinderException) {}
                    Autosplits.Add(new AutoSplit(n, t, v, d));
                }
            } catch (RuntimeBinderException) { }

            try
            {
                foreach (int rune in json.Runes)
                {
                    Runes.Add(rune);
                }
            }
            catch (RuntimeBinderException) { }
        }

        private string getSettingsFileName()
        {
            string settingsFile = Properties.Settings.Default.SettingsFile;
            if (settingsFile == null || settingsFile == "")
            {
                settingsFile = DefaultSettingsFile;
            }
            return settingsFile;
        }

        public void save()
        {
            saveAs(getSettingsFileName());
        }

        public void load()
        {
            loadFrom(getSettingsFileName());
        }
    }

}