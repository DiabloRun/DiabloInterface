using System;
using System.Collections.Generic;
using System.IO;

namespace DiabloInterface
{
    public class SettingsHolder
    {
        public string fileFolder = "txt";
        public string fontName = "Courier New";
        public int fontSize = 10;
        public int titleFontSize = 18;
        public bool createFiles = false;
        public bool doAutosplit = false;
        public bool showDebug = false;
        public string triggerKeys = "";
        public List<AutoSplit> autosplits = new List<AutoSplit>();

        public void save()
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            string confString = "";
            confString += "Font: " + fontName + "\n";
            confString += "FontSize: " + fontSize + "\n";
            confString += "FontSizeTitle: " + titleFontSize + "\n";
            confString += "CreateFiles: " + (createFiles ? "1" : "0") + "\n";
            confString += "DoAutosplit: " + (doAutosplit ? "1" : "0") + "\n";
            confString += "ShowDebug: " + (showDebug ? "1" : "0") + "\n";
            confString += "TriggerKeys: " + triggerKeys + "\n";
            foreach (AutoSplit autosplit in autosplits)
            {
                confString += "AutoSplit: " + autosplit.name.Replace('|', ' ') + "|"+ autosplit.type + "|" + autosplit.value + "|" + autosplit.difficulty + "\n";
            }
            
            File.WriteAllText("settings.conf", confString);
        }
        public void load()
        {
            if (!File.Exists("settings.conf"))
            {
                return;
            }

            List<AutoSplit> autosplitsNew = new List<AutoSplit>();
            string[] conf = File.ReadAllLines("settings.conf");
            string[] parts;
            string[] parts2;
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
                        catch (Exception e) { fontSize = 10; }
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
                        catch (Exception e) { titleFontSize = 10; }
                        break;
                    case "ShowDebug":
                        showDebug = (parts[1] == "1");
                        break;
                    case "CreateFiles":
                        createFiles = (parts[1] == "1");
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
                                Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                (short)0
                            );
                            autosplitsNew.Add(autosplit);
                        }
                        else if (parts2.Length == 4)
                        {
                            AutoSplit autosplit = new AutoSplit(
                                parts2[0],
                                Convert.ToInt16(parts2[1]),
                                Convert.ToInt16(parts2[2]),
                                Convert.ToInt16(parts2[3])
                            );
                            autosplitsNew.Add(autosplit);
                        }
                        break;
                }
            }
            autosplits = autosplitsNew;
        }
    }

}