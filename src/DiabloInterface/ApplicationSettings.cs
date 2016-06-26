using System.Collections.Generic;
using System.Windows.Forms;

namespace DiabloInterface
{
    public class ApplicationSettings
    {
        public string FileFolder { get; set; } = "Files";
        public string FontName { get; set; } = "Courier New";
        public string D2Version { get; set; }
        public int FontSize { get; set; } = 10;
        public int FontSizeTitle { get; set; } = 18;
        public bool CreateFiles { get; set; }
        public bool DoAutosplit { get; set; }
        public bool CheckUpdates { get; set; } = true;
        public Keys AutosplitHotkey { get; set; } = Keys.None;
        public List<AutoSplit> Autosplits { get; set; } = new List<AutoSplit>();
        public List<int> Runes { get; set; } = new List<int>();
        public bool DisplayName { get; set; } = true;
        public bool DisplayLevel { get; set; } = true;
        public bool DisplayDeathCounter { get; set; } = true;
        public bool DisplayGold { get; set; } = true;
        public bool DisplayResistances { get; set; } = true;
        public bool DisplayBaseStats { get; set; } = true;
        public bool DisplayRunes { get; set; }
        public bool DisplayRunesHorizontal { get; set; } = true;
        public bool DisplayRunesHighContrast { get; set; } = false;
        public bool DisplayAdvancedStats { get; set; }
        public bool DisplayDifficultyPercentages { get; set; } = false;
    }
}