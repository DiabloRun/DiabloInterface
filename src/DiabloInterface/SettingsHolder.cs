using DiabloInterface.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface
{
    public class SettingsHolder
    {

        public string fontName;
        public int fontSize;
        public int titleFontSize;
        public bool createFiles;
        public bool doAutosplit;
        public string triggerKeys;
        public List<AutoSplit.AutoSplit> autosplits = new List<AutoSplit.AutoSplit>();

        public void save()
        {
            Settings.Default["Font"] = fontName;
            Settings.Default["FontSize"] = fontSize.ToString();
            Settings.Default["FontSizeTitle"] = titleFontSize.ToString();
            Settings.Default["CreateFiles"] = createFiles;
            Settings.Default["Autosplit"] = doAutosplit;
            Settings.Default["AutoSplits"] = getAutosplitSettingsString(autosplits);
            Settings.Default["TriggerKeys"] = triggerKeys;
            Settings.Default.Save();
        }
        public void load()
        {

            fontName = Settings.Default["Font"].ToString();
            try
            {
                fontSize = Int32.Parse(Settings.Default["FontSize"].ToString());
            }
            catch (Exception e) { fontSize = 10; }
            try
            {
                titleFontSize = Int32.Parse(Settings.Default["FontSizeTitle"].ToString());
            }
            catch (Exception e) { titleFontSize = 18; }
            createFiles = Settings.Default["CreateFiles"].Equals(true);
            doAutosplit = Settings.Default["Autosplit"].Equals(true);
            autosplits = getAutosplitsByString(Settings.Default["AutoSplits"].ToString());
            triggerKeys = Settings.Default["TriggerKeys"].ToString();
        }

        private List<AutoSplit.AutoSplit> getAutosplitsByString(string autosplitString)
        {
            List<AutoSplit.AutoSplit> autosplits = new List<AutoSplit.AutoSplit>();
            if (autosplitString == "")
            {
                return autosplits;
            }
            string[] strArray = autosplitString.Split(new char[] { ';' });
            string[] typeVal;
            foreach (string str in strArray)
            {
                typeVal = str.Split(new char[] { ',' });

                if (typeVal.Length != 3)
                {
                    continue;
                }
                AutoSplit.AutoSplit autosplit = new AutoSplit.AutoSplit(typeVal[0], Int32.Parse(typeVal[1]), Int32.Parse(typeVal[2]));
                autosplits.Add(autosplit);
            }
            return autosplits;
        }

        public string getAutosplitSettingsString(List<AutoSplit.AutoSplit> autosplits)
        {
            String settings = "";
            foreach (AutoSplit.AutoSplit autosplit in autosplits)
            {
                settings += autosplit.name + "," + autosplit.type + "," + autosplit.value + ";";
            }
            return settings;
        }
    }

}