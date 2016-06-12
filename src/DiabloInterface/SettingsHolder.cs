using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiabloInterface
{
    public class SettingsHolder
    {
        # region default values
        private const string DEFAULT_FONT_NAME = "Courier New";
        private const string DEFAULT_D2_VERSION = "";
        private const int DEFAULT_FONT_SIZE = 10;
        private const int DEFAULT_TITLE_FONT_SIZE = 18;
        private const bool DEFAULT_CREATE_FILES = false;
        private const bool DEFAULT_DO_AUTOSPLIT = false;
        private const bool DEFAULT_SHOW_DEBUG = false;
        private const bool DEFAULT_CHECK_UPDATES = false;
        private const string DEFAULT_TRIGGER_KEYS = "";
        #endregion

        public const string DefaultSettingsFile = "settings.conf";
        public string FileFolder = "txt";

        public string FontName;
        public string D2Version;
        public int FontSize;
        public int TitleFontSize;
        public bool CreateFiles;
        public bool DoAutosplit;
        public bool ShowDebug;
        public bool CheckUpdates;
        public string TriggerKeys;
        public List<AutoSplit> Autosplits;
        public List<int> Runes;

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
            TitleFontSize = DEFAULT_TITLE_FONT_SIZE;
            CreateFiles = DEFAULT_CREATE_FILES;
            DoAutosplit = DEFAULT_DO_AUTOSPLIT;
            ShowDebug = DEFAULT_SHOW_DEBUG;
            CheckUpdates = DEFAULT_CHECK_UPDATES;
            TriggerKeys = DEFAULT_TRIGGER_KEYS;
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
                FontSizeTitle = TitleFontSize,
                CreateFiles = CreateFiles,
                DoAutosplit = DoAutosplit,
                ShowDebug = ShowDebug,
                CheckUpdates = CheckUpdates,
                TriggerKeys = TriggerKeys,
                D2Version = D2Version,
                AutoSplits = autosplits,
                Runes = Runes,
            };
            string jsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
            File.WriteAllText(file, jsonString);

            Properties.Settings.Default.SettingsFile = file;
            Properties.Settings.Default.Save();
        }

        private bool propExists(dynamic dyn, string prop)
        {
            try
            {
                var x = dyn[prop];
                return true;
            } catch (RuntimeBinderException)
            {
                return false;
            }
        }

        /// <summary>
        /// old config file reader
        /// </summary>
        private void loadFromLegacy(string[] conf)
        {
            string[] parts;
            string[] parts2;
            
            foreach (string line in conf)
            {
                parts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                switch (parts[0])
                {
                    case "Font": FontName = parts[1]; break;
                    case "ShowDebug": ShowDebug = (parts[1] == "1"); break;
                    case "CheckUpdates": CheckUpdates = (parts[1] == "1"); break;
                    case "CreateFiles": CreateFiles = (parts[1] == "1"); break;
                    case "D2Version": D2Version = parts[1]; break;
                    case "TriggerKeys": TriggerKeys = parts[1]; break;
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
                            TitleFontSize = Int32.Parse(parts[1]);
                            if (TitleFontSize == 0) { TitleFontSize = 10; }
                        }
                        catch { TitleFontSize = 10; }
                        break;
                    case "AutoSplit":
                        parts2 = parts[1].Split(new string[] { "|" }, 4, StringSplitOptions.None);
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

            if (propExists(json, "Font")) FontName = (string)json.Font;
            if (propExists(json, "FontSize")) FontSize = (int)json.FontSize;
            if (propExists(json, "FontSizeTitle")) TitleFontSize = (int)json.FontSizeTitle;
            if (propExists(json, "CreateFiles")) CreateFiles = (bool)json.CreateFiles;
            if (propExists(json, "DoAutosplit")) DoAutosplit = (bool)json.DoAutosplit;
            if (propExists(json, "ShowDebug")) ShowDebug = (bool)json.ShowDebug;
            if (propExists(json, "CheckUpdates")) CheckUpdates = (bool)json.CheckUpdates;
            if (propExists(json, "TriggerKeys")) TriggerKeys = (string)json.TriggerKeys;
            if (propExists(json, "D2Version")) D2Version = (string)json.D2Version;

            if (propExists(json, "AutoSplits"))
            {
                foreach (dynamic autosplit in json.AutoSplits)
                {
                    Autosplits.Add(new AutoSplit(
                        propExists(json, "Name") ? (string)autosplit.Name : "",
                        propExists(json, "Type") ? (AutoSplit.SplitType)autosplit.Type : AutoSplit.SplitType.None,
                        propExists(json, "Value") ? (short)autosplit.Value : (short)0,
                        propExists(json, "Difficulty") ? (short)autosplit.Difficulty : (short)0
                    ));
                }
            }
            if (propExists(json, "Runes"))
            {
                foreach (int rune in json.Runes)
                {
                    Runes.Add(rune);
                }
            }
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