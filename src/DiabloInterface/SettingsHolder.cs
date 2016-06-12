using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiabloInterface
{
    public class SettingsHolder
    {

        private const string DEFAULT_FONT_NAME = "Courier New";
        private const string DEFAULT_D2_VERSION = "";
        private const int DEFAULT_FONT_SIZE = 10;
        private const int DEFAULT_TITLE_FONT_SIZE = 18;
        private const bool DEFAULT_CREATE_FILES = false;
        private const bool DEFAULT_DO_AUTOSPLIT = false;
        private const bool DEFAULT_SHOW_DEBUG = false;
        private const bool DEFAULT_CHECK_UPDATES = false;
        private const string DEFAULT_TRIGGER_KEYS = "";

        const string defaultSettingsFile = "settings.conf";
        public string fileFolder = "txt";

        public string fontName;
        public string d2Version;
        public int fontSize;
        public int titleFontSize;
        public bool createFiles;
        public bool doAutosplit;
        public bool showDebug;
        public bool checkUpdates;
        public string triggerKeys;
        public List<AutoSplit> autosplits;
        public List<int> runes;

        public SettingsHolder ()
        {
            // init members with default values
            init();
        }

        private void init()
        {
            fontName = DEFAULT_FONT_NAME;
            d2Version = DEFAULT_D2_VERSION;
            fontSize = DEFAULT_FONT_SIZE;
            titleFontSize = DEFAULT_TITLE_FONT_SIZE;
            createFiles = DEFAULT_CREATE_FILES;
            doAutosplit = DEFAULT_DO_AUTOSPLIT;
            showDebug = DEFAULT_SHOW_DEBUG;
            checkUpdates = DEFAULT_CHECK_UPDATES;
            triggerKeys = DEFAULT_TRIGGER_KEYS;
            autosplits = new List<AutoSplit>();
            runes = new List<int>();
        }
        
        public void saveAs(string file)
        {
            List<dynamic> autosplits = new List<dynamic>();
            foreach (AutoSplit autosplit in this.autosplits)
            {
                autosplits.Add(new
                {
                    Name = autosplit.name.Replace('|', ' '),
                    Type = (int)autosplit.type,
                    Value = autosplit.value,
                    Difficulty = autosplit.difficulty,
                });
            }

            dynamic json = new
            {
                Font = fontName,
                FontSize = fontSize,
                FontSizeTitle = titleFontSize,
                CreateFiles = createFiles,
                DoAutosplit = doAutosplit,
                ShowDebug = showDebug,
                CheckUpdates = checkUpdates,
                TriggerKeys = triggerKeys,
                D2Version = d2Version,
                AutoSplits = autosplits,
                Runes = runes,
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
                    case "Font": fontName = parts[1]; break;
                    case "ShowDebug": showDebug = (parts[1] == "1"); break;
                    case "CheckUpdates": checkUpdates = (parts[1] == "1"); break;
                    case "CreateFiles": createFiles = (parts[1] == "1"); break;
                    case "D2Version": d2Version = parts[1]; break;
                    case "TriggerKeys": triggerKeys = parts[1]; break;
                    case "DoAutosplit": doAutosplit = (parts[1] == "1"); break;
                    case "Rune": runes.Add(Convert.ToInt32(parts[1])); break;
                    case "FontSize":
                        try
                        {
                            fontSize = Int32.Parse(parts[1]);
                            if (fontSize == 0) { fontSize = 10; }
                        }
                        catch { fontSize = 10; }
                        break;
                    case "FontSizeTitle":
                        try
                        {
                            titleFontSize = Int32.Parse(parts[1]);
                            if (titleFontSize == 0) { titleFontSize = 10; }
                        }
                        catch { titleFontSize = 10; }
                        break;
                    case "AutoSplit":
                        parts2 = parts[1].Split(new string[] { "|" }, 4, StringSplitOptions.None);
                        if (parts2.Length == 3)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                (AutoSplit.Type)Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                (short)0
                            );
                            autosplits.Add(autosplit);
                        }
                        else if (parts2.Length == 4)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                (AutoSplit.Type)Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                Convert.ToInt16(parts2[3])
                            );
                            autosplits.Add(autosplit);
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

            if (propExists(json, "Font")) fontName = (string)json.Font;
            if (propExists(json, "FontSize")) fontSize = (int)json.FontSize;
            if (propExists(json, "FontSizeTitle")) titleFontSize = (int)json.FontSizeTitle;
            if (propExists(json, "CreateFiles")) createFiles = (bool)json.CreateFiles;
            if (propExists(json, "DoAutosplit")) doAutosplit = (bool)json.DoAutosplit;
            if (propExists(json, "ShowDebug")) showDebug = (bool)json.ShowDebug;
            if (propExists(json, "CheckUpdates")) checkUpdates = (bool)json.CheckUpdates;
            if (propExists(json, "TriggerKeys")) triggerKeys = (string)json.TriggerKeys;
            if (propExists(json, "D2Version")) d2Version = (string)json.D2Version;

            if (propExists(json, "AutoSplits"))
            {
                foreach (dynamic autosplit in json.AutoSplits)
                {
                    autosplits.Add(new AutoSplit(
                        propExists(json, "Name") ? (string)autosplit.Name : "",
                        propExists(json, "Type") ? (AutoSplit.Type)autosplit.Type : AutoSplit.Type.None,
                        propExists(json, "Value") ? (short)autosplit.Value : (short)0,
                        propExists(json, "Difficulty") ? (short)autosplit.Difficulty : (short)0
                    ));
                }
            }
            if (propExists(json, "Runes"))
            {
                foreach (int rune in json.Runes)
                {
                    runes.Add(rune);
                }
            }
        }

        private string getSettingsFileName()
        {
            string settingsFile = Properties.Settings.Default.SettingsFile;
            if (settingsFile == null || settingsFile == "")
            {
                settingsFile = defaultSettingsFile;
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