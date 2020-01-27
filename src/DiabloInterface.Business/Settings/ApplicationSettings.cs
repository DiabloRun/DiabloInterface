namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;

    public class ApplicationSettings
    {
        public static ApplicationSettings Default => new ApplicationSettings();

        public string FileFolder { get; set; } = "Files";
        public string FontName { get; set; } = "Courier New";
        
        public int FontSize { get; set; } = 10;
        public int FontSizeTitle { get; set; } = 18;
        public bool CreateFiles { get; set; }
        public bool DoAutosplit { get; set; }
        public bool CheckUpdates { get; set; } = true;
        public string PipeName { get; set; } = "DiabloInterfacePipe";
        public Keys AutosplitHotkey { get; set; } = Keys.None;
        public List<AutoSplit> Autosplits { get; set; } = new List<AutoSplit>();
        public IReadOnlyList<ClassRuneSettings> ClassRunes { get; set; } = new List<ClassRuneSettings>();
        
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

        public int VerticalLayoutPadding { get; set; } = 5;

        public Color ColorName { get; set; } = Color.RoyalBlue;
        public Color ColorDeaths { get; set; } = Color.Snow;
        public Color ColorPlayersX { get; set; } = Color.Snow;
        public Color ColorGameCounter { get; set; } = Color.Snow;
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

        public ProcessDescription[] ProcessDescriptions { get; set; } = new ProcessDescription[]
        {
            // Diablo 2
            new ProcessDescription{
                ModuleName = "Game.exe",
                ProcessName = "game",
                SubModules = new string[] { "D2Common.dll", "D2Launch.dll", "D2Lang.dll", "D2Net.dll", "D2Game.dll", "D2Client.dll" }
            },
            // D2SE
            new ProcessDescription{
                ModuleName = "D2SE.exe",
                ProcessName = "d2se",
                SubModules = new string[] { "D2Common.dll", "D2Launch.dll", "D2Lang.dll", "D2Net.dll", "D2Game.dll", "D2Client.dll", "Fog.dll" }
            },
        };
    }

    [Serializable]
    public class ClassRuneSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CharacterClass? Class { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GameDifficulty? Difficulty { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IReadOnlyList<Rune> Runes { get; set; } = new List<Rune>();
    }
}
