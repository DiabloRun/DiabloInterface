using System.Collections.Generic;
using System.Drawing;
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
        public bool DisplayLayoutHorizontal { get; set; } = true;

        public int VerticalLayoutPadding { get; set; } = 5;

        public Color ColorName { get; set; } = Color.RoyalBlue;
        public Color ColorDeaths { get; set; } = Color.Snow;
        public Color ColorLevel { get; set; } = Color.Snow;
        public Color ColorDifficultyPercentages { get; set; } = Color.Snow;
        public Color ColorGold { get; set; } = Color.Gold;
        public Color ColorBaseStats { get; set; } = Color.Coral;
        public Color ColorAdvancedStats { get; set; } = Color.Coral;
        public Color ColorFireRes { get; set; } = Color.Red;
        public Color ColorColdRes { get; set; } = Color.DodgerBlue;
        public Color ColorLightningRes { get; set; } = Color.Yellow;
        public Color ColorPoisonRes { get; set; } = Color.YellowGreen;

        public Color ColorBackground { get; set; } = Color.Black;
    }
}