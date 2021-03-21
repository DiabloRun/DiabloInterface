namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Lib;

    class HorizontalLayout : AbstractLayout
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private FlowLayoutPanel activeRuneLayoutPanel;
        private FlowLayoutPanel outerLeftRightPanel;
        private FlowLayoutPanel flowLayoutPanel1;
        protected override Panel RuneLayoutPanel => activeRuneLayoutPanel;
        private TableLayoutPanel panelAdvancedStats;
        private TableLayoutPanel panelResistances;
        private TableLayoutPanel panelDiffPercentages;
        private FlowLayoutPanel panelStats;
        private TableLayoutPanel panelDiffPercentages2;
        private FlowLayoutPanel panelRuneDisplayHorizontal;
        private FlowLayoutPanel panelRuneDisplayVertical;
        private TableLayoutPanel panelBaseStats;

        protected void InitializeComponent()
        {
            Add("name", "WWW_WWWWWWWWW", (ApplicationConfig s) => Tuple.Create(s.DisplayName, s.ColorName, s.FontSizeTitle), "{}");
            Add("life", "9999/9999", (ApplicationConfig s) => Tuple.Create(s.DisplayLife, s.ColorLife, s.FontSize), "LIFE: {}/{}");
            Add("mana", "9999/9999", (ApplicationConfig s) => Tuple.Create(s.DisplayMana, s.ColorMana, s.FontSize), "MANA: {}/{}");
            Add("hc_sc", "HARDCORE", (ApplicationConfig s) => Tuple.Create(s.DisplayHardcoreSoftcore, s.ColorHardcoreSoftcore, s.FontSize), "{}");
            Add("exp_classic", "EXPANSION", (ApplicationConfig s) => Tuple.Create(s.DisplayExpansionClassic, s.ColorExpansionClassic, s.FontSize), "{}");
            Add("playersx", "8", (ApplicationConfig s) => Tuple.Create(s.DisplayPlayersX, s.ColorPlayersX, s.FontSize), "/players {}");
            Add("seed", "4294967295", (ApplicationConfig s) => Tuple.Create(s.DisplaySeed, s.ColorSeed, s.FontSize), "SEED: {}");
            Add("deaths", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayDeathCounter, s.ColorDeaths, s.FontSize), "DEATHS: {}");
            Add("runs", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayGameCounter, s.ColorGameCounter, s.FontSize), "RUNS: {}");
            Add("chars", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayCharCounter, s.ColorCharCounter, s.FontSize), "CHARS: {}");
            Add("gold", "2500000", (ApplicationConfig s) => Tuple.Create(s.DisplayGold, s.ColorGold, s.FontSize), "GOLD: {}");
            Add("mf", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayMagicFind, s.ColorMagicFind, s.FontSize), "MF: {}");
            Add("monstergold", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayMonsterGold, s.ColorMonsterGold, s.FontSize), "EMG: {}");
            Add("atd", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAttackerSelfDamage, s.ColorAttackerSelfDamage, s.FontSize), "ATD: {}");
            Add("lvl", "99", (ApplicationConfig s) => Tuple.Create(s.DisplayLevel, s.ColorLevel, s.FontSize), "LVL: {}");
            Add("str", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "STR:", "{}");
            Add("vit", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "VIT:", "{}");
            Add("dex", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "DEX:", "{}");
            Add("ene", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "ENE:", "{}");
            Add("ias", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "IAS:", "{}");
            Add("frw", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FRW:", "{}");
            Add("fcr", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FCR:", "{}");
            Add("fhr", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FHR:", "{}");
            Add("cold", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayResistances, s.ColorColdRes, s.FontSize), "COLD:", "{}");
            Add("ligh", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayResistances, s.ColorLightningRes, s.FontSize), "LIGH:", "{}");
            Add("pois", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayResistances, s.ColorPoisonRes, s.FontSize), "POIS:", "{}");
            Add("fire", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayResistances, s.ColorFireRes, s.FontSize), "FIRE:", "{}");
            Add("norm", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NORM:", "{}%");
            Add("nm", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NM:", "{}%");
            Add("hell", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "HELL:", "{}%");
            Add("norm_inline", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NO: {}%");
            Add("nm_inline", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NM: {}%");
            Add("hell_inline", "100", (ApplicationConfig s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "HE: {}%");

            // display multiple "things" in 1 row (a table with 1 row and x columns)
            TableLayoutPanel rowthing(params string[] names)
            {
                var t = new TableLayoutPanel();
                t.SuspendLayout();
                t.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                t.AutoSize = true;
                t.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                t.ColumnCount = names.Length;
                int i = 0;
                foreach (string n in names)
                {
                    t.ColumnStyles.Add(new ColumnStyle());
                    t.Controls.Add(def[n].labels[0], i++, 0);
                }
                t.Margin = new Padding(0);
                t.RowCount = 1;
                t.RowStyles.Add(new RowStyle());
                t.ResumeLayout(false);
                return t;
            }

            // display multiple "things" in a table. where each thing is displayed in 1 row
            // and each feature of the thing is displayed in a column
            // each thing must have the same number of columns for this to work
            TableLayoutPanel tablething(params string[] names)
            {
                var t = new TableLayoutPanel();
                t.SuspendLayout();
                t.RowCount = 0;
                t.ColumnCount = def[names[0]].labels.Length;
                for (int i = 0; i < def[names[0]].labels.Length; i++)
                {
                    t.ColumnStyles.Add(new ColumnStyle());
                }
                foreach (string n in names)
                {
                    t.RowCount++;
                    t.RowStyles.Add(new RowStyle());
                    int c = 0;
                    foreach (Label l in def[n].labels)
                    {
                        t.Controls.Add(l, c, t.RowCount - 1);
                        c++;
                    }
                }
                t.Margin = new Padding(0);
                t.AutoSize = true;
                t.ResumeLayout(false);
                return t;
            }
            
            this.panelBaseStats = tablething("str", "dex", "vit", "ene");
            this.panelAdvancedStats = tablething("frw", "fhr", "fcr", "ias");
            this.panelResistances = tablething("fire", "cold", "ligh", "pois");
            this.panelDiffPercentages = tablething("norm", "nm", "hell");
            this.panelDiffPercentages2 = rowthing("norm_inline", "nm_inline", "hell_inline");

            this.panelRuneDisplayVertical = new FlowLayoutPanel();
            this.panelRuneDisplayVertical.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRuneDisplayVertical.AutoSize = true;
            this.panelRuneDisplayVertical.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panelRuneDisplayVertical.Margin = new Padding(0);
            this.panelRuneDisplayVertical.MaximumSize = new Size(28, 0);
            this.panelRuneDisplayVertical.MinimumSize = new Size(28, 28);

            this.panelRuneDisplayHorizontal = new FlowLayoutPanel();
            this.panelRuneDisplayHorizontal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRuneDisplayHorizontal.AutoSize = true;
            this.panelRuneDisplayHorizontal.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.panelStats = new FlowLayoutPanel();
            this.panelStats.SuspendLayout();
            this.panelStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelStats.AutoSize = true;
            this.panelStats.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panelStats.Controls.Add(this.panelBaseStats);
            this.panelStats.Controls.Add(this.panelAdvancedStats);
            this.panelStats.Controls.Add(this.panelResistances);
            this.panelStats.Controls.Add(this.panelDiffPercentages);
            this.panelStats.Margin = new Padding(0);

            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(def["name"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(rowthing("life", "mana"));
            this.flowLayoutPanel1.Controls.Add(rowthing("hc_sc", "exp_classic", "playersx"));
            this.flowLayoutPanel1.Controls.Add(def["seed"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(rowthing("lvl", "deaths", "runs", "chars"));
            this.flowLayoutPanel1.Controls.Add(rowthing("atd", "monstergold"));
            this.flowLayoutPanel1.Controls.Add(rowthing("gold", "mf"));
            this.flowLayoutPanel1.Controls.Add(panelStats);
            this.flowLayoutPanel1.Controls.Add(panelDiffPercentages2);
            this.flowLayoutPanel1.Controls.Add(panelRuneDisplayHorizontal);
            this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            this.flowLayoutPanel1.Margin = new Padding(0);

            this.outerLeftRightPanel = new FlowLayoutPanel();
            this.outerLeftRightPanel.SuspendLayout();
            this.outerLeftRightPanel.AutoSize = true;
            this.outerLeftRightPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.outerLeftRightPanel.Controls.Add(this.panelRuneDisplayVertical);
            this.outerLeftRightPanel.Controls.Add(this.flowLayoutPanel1);
            this.outerLeftRightPanel.Dock = DockStyle.Fill;
            this.outerLeftRightPanel.Margin = new Padding(0);
            this.outerLeftRightPanel.Padding = new Padding(0);

            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Margin = new Padding(0);
            BackColor = Color.Black;
            Controls.Add(this.outerLeftRightPanel);
            Name = "HorizontalLayout";
            outerLeftRightPanel.ResumeLayout(false);
            outerLeftRightPanel.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panelStats.ResumeLayout(false);
            panelBaseStats.ResumeLayout(false);
            panelBaseStats.PerformLayout();
            panelAdvancedStats.ResumeLayout(false);
            panelAdvancedStats.PerformLayout();
            panelResistances.ResumeLayout(false);
            panelResistances.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
            
            RunePanels = new[]
            {
                panelRuneDisplayHorizontal,
                panelRuneDisplayVertical
            };
        }

        public HorizontalLayout(IDiabloInterface di)
        {
            this.di = di;
            RegisterServiceEventHandlers();
            InitializeComponent();

            Load += (sender, e) => UpdateConfig(di.configService.CurrentConfig);

            // Clean up events when disposed because services outlive us.
            Disposed += (sender, e) => UnregisterServiceEventHandlers();

            Logger.Info("Creating horizontal layout.");

            panelRuneDisplayHorizontal.Hide();
            panelRuneDisplayVertical.Hide();
        }

        protected override void UpdateConfig(ApplicationConfig config)
        {
            realFrwIas = config.DisplayRealFrwIas;
            BackColor = config.ColorBackground;

            var margin = new Padding(2);
            foreach (KeyValuePair<string, Def> pair in def)
            {
                var x = pair.Value.settings(config);
                var enabled = x.Item1;
                pair.Value.enabled = enabled;

                if (enabled)
                {
                    var color = x.Item2;
                    var font = CreateFont(config.FontName, x.Item3);
                    foreach (Label l in pair.Value.labels)
                    {
                        l.Visible = enabled;
                        l.Margin = margin;
                        l.ForeColor = color;
                        l.Font = font;
                        var teststr = pair.Value.defaults[l].Replace("{}", pair.Value.maxString);
                        l.Size = MeasureText(teststr, l);
                    }
                } else
                {
                    foreach (Label l in pair.Value.labels)
                    {
                        l.Visible = enabled;
                    }
                }
            }

            panelResistances.Visible = config.DisplayResistances;
            panelBaseStats.Visible = config.DisplayBaseStats;
            panelAdvancedStats.Visible = config.DisplayAdvancedStats;

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            panelDiffPercentages.Visible = count < 3 && config.DisplayDifficultyPercentages;
            panelDiffPercentages2.Visible = count >= 3 && config.DisplayDifficultyPercentages;

            panelStats.Visible =
                config.DisplayResistances
                || config.DisplayBaseStats
                || config.DisplayAdvancedStats
                || config.DisplayDifficultyPercentages;

            FlowLayoutPanel nextRuneLayoutPanel = null;

            if (config.DisplayRunes)
            {
                nextRuneLayoutPanel = config.DisplayRunesHorizontal
                    ? panelRuneDisplayHorizontal
                    : panelRuneDisplayVertical;
            }

            // Only hide panels when the panels were changed.
            if (activeRuneLayoutPanel != nextRuneLayoutPanel)
            {
                activeRuneLayoutPanel?.Hide();
                activeRuneLayoutPanel = nextRuneLayoutPanel;
            }

            // TODO: automatically size the rune panel...
            int statsWidth = (panelResistances.Visible ? panelResistances.Width : 0)
                + (panelBaseStats.Visible ? panelBaseStats.Width : 0)
                + (panelAdvancedStats.Visible ? panelAdvancedStats.Width : 0)
                + ((count < 3 && panelDiffPercentages.Visible) ? panelDiffPercentages.Width : 0)
            ;
            panelRuneDisplayHorizontal.MaximumSize = new Size(Math.Max(def["name"].labels[0].Width, statsWidth), 0);
        }

        protected override void UpdateLabels(Character player, Quests quests, Game game)
        {
            UpdateLabel("name", player.Name);
            UpdateLabel("life", new string[] { "" + player.Life, "" + player.LifeMax });
            UpdateLabel("mana", new string[] { "" + player.Mana, "" + player.ManaMax });
            UpdateLabel("hc_sc", player.IsHardcore ? "HARDCORE" : "SOFTCORE");
            UpdateLabel("exp_classic", player.IsExpansion ? "EXPANSION" : "CLASSIC");
            UpdateLabel("playersx", game.PlayersX);
            UpdateLabel("seed", game.Seed, game.SeedIsArg);
            UpdateLabel("deaths", player.Deaths);
            UpdateLabel("runs", (int) game.GameCount);
            UpdateLabel("chars", (int) game.CharCount);
            UpdateLabel("gold", player.Gold + player.GoldStash);
            UpdateLabel("mf", player.MagicFind);
            UpdateLabel("monstergold", player.MonsterGold);
            UpdateLabel("atd", player.AttackerSelfDamage);
            UpdateLabel("lvl", player.Level);
            UpdateLabel("str", player.Strength);
            UpdateLabel("vit", player.Vitality);
            UpdateLabel("dex", player.Dexterity);
            UpdateLabel("ene", player.Energy);
            UpdateLabel("ias", realFrwIas ? player.RealIAS() : player.IncreasedAttackSpeed);
            UpdateLabel("frw", realFrwIas ? player.RealFRW() : player.FasterRunWalk);
            UpdateLabel("fcr", player.FasterCastRate);
            UpdateLabel("fhr", player.FasterHitRecovery);
            UpdateLabel("cold", player.ColdResist);
            UpdateLabel("ligh", player.LightningResist);
            UpdateLabel("pois", player.PoisonResist);
            UpdateLabel("fire", player.FireResist);
            UpdateLabel("norm", $@"{quests.ProgressByDifficulty(GameDifficulty.Normal) * 100:0}");
            UpdateLabel("nm", $@"{quests.ProgressByDifficulty(GameDifficulty.Nightmare) * 100:0}");
            UpdateLabel("hell", $@"{quests.ProgressByDifficulty(GameDifficulty.Hell) * 100:0}");
            UpdateLabel("norm_inline", $@"{quests.ProgressByDifficulty(GameDifficulty.Normal) * 100:0}");
            UpdateLabel("nm_inline", $@"{quests.ProgressByDifficulty(GameDifficulty.Nightmare) * 100:0}");
            UpdateLabel("hell_inline", $@"{quests.ProgressByDifficulty(GameDifficulty.Hell) * 100:0}");
        }
    }
}
