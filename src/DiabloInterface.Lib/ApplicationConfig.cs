namespace Zutatensuppe.DiabloInterface
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.DiabloInterface.Lib;
    using Zutatensuppe.DiabloInterface.Lib.Plugin;

    public class ApplicationConfig
    {
        public static ApplicationConfig Default => new ApplicationConfig();

        // Plugin settings
        public Dictionary<string, IPluginConfig> Plugins { get; set; } = new Dictionary<string, IPluginConfig>();
        virtual public IPluginConfig PluginConf(string type) => Plugins.ContainsKey(type) ? Plugins[type] : null;

        // Rune settings (should be plugin?)
        public IReadOnlyList<IClassRuneSettings> ClassRunes { get; set; } = new List<IClassRuneSettings>();

        // GUI settings:
        public string FontName { get; set; } = "Courier New";

        public int FontSize { get; set; } = 10;
        public int FontSizeTitle { get; set; } = 18;

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
        public bool DisplayRealFrwIas { get; set; } = false;
        public bool DisplayPlayersX { get; set; } = false;
        public bool DisplayGameCounter { get; set; } = false;
        public bool DisplayCharCounter { get; set; } = false;
        public bool DisplayMagicFind { get; set; } = false;
        public bool DisplayMonsterGold { get; set; } = false;
        public bool DisplayAttackerSelfDamage { get; set; } = false;
        public bool DisplayHardcoreSoftcore { get; set; } = false;
        public bool DisplayExpansionClassic { get; set; } = false;
        public bool DisplaySeed { get; set; } = false;
        public bool DisplayLife { get; set; } = false;
        public bool DisplayMana { get; set; } = false;

        public int VerticalLayoutPadding { get; set; } = 5;

        public Color ColorName { get; set; } = Color.RoyalBlue;
        public Color ColorDeaths { get; set; } = Color.Snow;
        public Color ColorPlayersX { get; set; } = Color.Snow;
        public Color ColorGameCounter { get; set; } = Color.Snow;
        public Color ColorCharCounter { get; set; } = Color.Snow;
        public Color ColorLevel { get; set; } = Color.Snow;
        public Color ColorDifficultyPercentages { get; set; } = Color.Snow;
        public Color ColorGold { get; set; } = Color.Gold;
        public Color ColorBaseStats { get; set; } = Color.Coral;
        public Color ColorAdvancedStats { get; set; } = Color.Coral;
        public Color ColorFireRes { get; set; } = Color.Red;
        public Color ColorColdRes { get; set; } = Color.DodgerBlue;
        public Color ColorLightningRes { get; set; } = Color.Yellow;
        public Color ColorPoisonRes { get; set; } = Color.YellowGreen;
        public Color ColorMagicFind { get; set; } = Color.Gold;
        public Color ColorMonsterGold { get; set; } = Color.Gold;
        public Color ColorAttackerSelfDamage { get; set; } = Color.Snow;
        public Color ColorHardcoreSoftcore { get; set; } = Color.Snow;
        public Color ColorExpansionClassic { get; set; } = Color.Snow;
        public Color ColorSeed { get; set; } = Color.Snow;
        public Color ColorBackground { get; set; } = Color.Black;

        public Color ColorLife { get; set; } = Color.Snow;
        public Color ColorMana { get; set; } = Color.Snow;

        // Reader settings:
        public ProcessDescription[] ProcessDescriptions { get; set; } = new ProcessDescription[]
        {
            // Diablo 2
            // Project Diablo 2
            new ProcessDescription{
                ModuleName = "Game.exe",
                ProcessName = "game",
            },
            // D2SE
            new ProcessDescription{
                ModuleName = "D2SE.exe",
                ProcessName = "d2se",
            },
            // D2Loader-high
            new ProcessDescription
            {
                ModuleName = "D2Loader-high.exe",
                ProcessName = "D2Loader-high",
            }
        };
    }
}
