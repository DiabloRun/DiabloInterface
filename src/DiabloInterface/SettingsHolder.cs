using System;
using System.Collections.Generic;
using System.IO;

namespace DiabloInterface
{
    public class SettingsHolder
    {
        const string defaultSettingsFile = "settings.conf";

        public string fileFolder;
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
            fileFolder = "txt";
            fontName = "Courier New";
            d2Version = "";
            fontSize = 10;
            titleFontSize = 18;
            createFiles = false;
            doAutosplit = false;
            showDebug = false;
            checkUpdates = false;
            triggerKeys = "";
            autosplits = new List<AutoSplit>();
            runes = new List<int>();
        }
        
        public void saveAs(string file)
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            string confString = "";
            confString += "Font: " + fontName + "\n";
            confString += "FontSize: " + fontSize + "\n";
            confString += "FontSizeTitle: " + titleFontSize + "\n";
            confString += "CreateFiles: " + (createFiles ? "1" : "0") + "\n";
            confString += "DoAutosplit: " + (doAutosplit ? "1" : "0") + "\n";
            confString += "ShowDebug: " + (showDebug ? "1" : "0") + "\n";
            confString += "CheckUpdates: " + (checkUpdates ? "1" : "0") + "\n";
            confString += "TriggerKeys: " + triggerKeys + "\n";
            confString += "D2Version: " + d2Version + "\n";
            foreach (AutoSplit autosplit in autosplits)
            {
                confString += "AutoSplit: " + autosplit.name.Replace('|', ' ') + "|" + (int)autosplit.type + "|" + autosplit.value + "|" + autosplit.difficulty + "\n";
            }
            foreach (int rune in runes)
            {
                confString += "Rune: " + rune + "\n";
            }

            Properties.Settings.Default.SettingsFile = file;
            Properties.Settings.Default.Save();

            File.WriteAllText(file, confString);
        }

        public void loadFrom(string file)
        {
            Properties.Settings.Default.SettingsFile = file;
            Properties.Settings.Default.Save();

            if (!File.Exists(file))
            {
                return;
            }

            // init members with default values
            init();

            List<AutoSplit> autosplitsNew = new List<AutoSplit>();
            string[] conf = File.ReadAllLines(file);
            string[] parts;
            string[] parts2;

            runes = new List<int>();
            foreach (string line in conf)
            {
                parts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                switch (parts[0])
                {
                    case "Font":
                        fontName = parts[1];
                        break;
                    case "FontSize":
                        try
                        {
                            fontSize = Int32.Parse(parts[1]);
                            if (fontSize == 0)
                            {
                                fontSize = 10;
                            }
                        }
                        catch { fontSize = 10; }
                        break;
                    case "FontSizeTitle":
                        try
                        {
                            titleFontSize = Int32.Parse(parts[1]);
                            if (titleFontSize == 0)
                            {
                                titleFontSize = 10;
                            }
                        }
                        catch { titleFontSize = 10; }
                        break;
                    case "ShowDebug":
                        showDebug = (parts[1] == "1");
                        break;
                    case "CheckUpdates":
                        checkUpdates = (parts[1] == "1");
                        break;
                    case "CreateFiles":
                        createFiles = (parts[1] == "1");
                        break;
                    case "D2Version":
                        d2Version = parts[1];
                        break;
                    case "TriggerKeys":
                        triggerKeys = parts[1];
                        break;
                    case "DoAutosplit":
                        doAutosplit = (parts[1] == "1");
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
                            autosplitsNew.Add(autosplit);
                        }
                        else if (parts2.Length == 4)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                (AutoSplit.Type)Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                Convert.ToInt16(parts2[3])
                            );
                            autosplitsNew.Add(autosplit);
                        }
                        break;
                    case "Rune":
                        runes.Add(Convert.ToInt32(parts[1]));
                        break;
                }
            }
            autosplits = autosplitsNew;
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