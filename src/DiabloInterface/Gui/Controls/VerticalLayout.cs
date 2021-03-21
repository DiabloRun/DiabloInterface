namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Lib;

    class VerticalLayout : AbstractLayout
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private FlowLayoutPanel outerLeftRightPanel;
        private FlowLayoutPanel panelRuneDisplay;
        private TableLayoutPanel table;

        public VerticalLayout(IDiabloInterface di)
        {
            this.di = di;
            RegisterServiceEventHandlers();
            InitializeComponent();

            Load += (sender, e) => UpdateConfig(di.configService.CurrentConfig);

            // Clean up events when disposed because services outlive us.
            Disposed += (sender, e) => UnregisterServiceEventHandlers();
            Logger.Info("Creating vertical layout.");
        }

        protected override Panel RuneLayoutPanel => panelRuneDisplay;

        protected void InitializeComponent()
        {
            Add("name", new string('W', 15), (ApplicationConfig s) => Tuple.Create(s.DisplayName, s.ColorName, s.FontSizeTitle), "{}");
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
            Add("mf", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayMagicFind, s.ColorMagicFind, s.FontSize), "MF:", "{}");
            Add("monstergold", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayMonsterGold, s.ColorMonsterGold, s.FontSize), "EMG:", "{}");
            Add("atd", "999", (ApplicationConfig s) => Tuple.Create(s.DisplayAttackerSelfDamage, s.ColorAttackerSelfDamage, s.FontSize), "ATD:", "{}");
            Add("lvl", "99", (ApplicationConfig s) => Tuple.Create(s.DisplayLevel, s.ColorLevel, s.FontSize), "LVL:", "{}");
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
            
            table = new TableLayoutPanel();
            table.SuspendLayout();
            table.AutoSize = true;
            table.ColumnCount = 2;
            table.ColumnStyles.Add(new ColumnStyle());
            table.ColumnStyles.Add(new ColumnStyle());
            table.RowCount = 0;
            
            foreach (KeyValuePair<string, Def> pair in def)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                for (int i = 0; i < pair.Value.labels.Length; i++)
                {
                    table.Controls.Add(pair.Value.labels[i], i, table.RowCount - 1);
                    if (i > 0)
                    {
                        pair.Value.labels[i].TextAlign = ContentAlignment.TopRight;
                    }
                }
                if (pair.Value.labels.Length == 1)
                {
                    table.SetColumnSpan(pair.Value.labels[0], table.ColumnCount);
                }
            }

            table.ResumeLayout(false);
            table.PerformLayout();

            panelRuneDisplay = new FlowLayoutPanel();
            panelRuneDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelRuneDisplay.AutoSize = true;
            panelRuneDisplay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelRuneDisplay.MaximumSize = new Size(28, 0);
            panelRuneDisplay.MinimumSize = new Size(28, 28);

            outerLeftRightPanel = new FlowLayoutPanel();
            outerLeftRightPanel.SuspendLayout();
            outerLeftRightPanel.AutoSize = true;
            outerLeftRightPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            outerLeftRightPanel.Controls.Add(panelRuneDisplay);
            outerLeftRightPanel.Controls.Add(table);

            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Margin = new Padding(0);
            BackColor = Color.Black;
            Controls.Add(this.outerLeftRightPanel);
            outerLeftRightPanel.ResumeLayout(false);
            outerLeftRightPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

            RunePanels = new[]
            {
                panelRuneDisplay
            };
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
        }

        protected override void UpdateConfig(ApplicationConfig config)
        {
            var padding = new Padding(0, 0, 0, config.VerticalLayoutPadding);
            realFrwIas = config.DisplayRealFrwIas;
            BackColor = config.ColorBackground;

            int w_full = 0;
            int w_left = 0;
            int w_right = 0;

            foreach (KeyValuePair<string, Def> pair in def)
            {
                Tuple<bool, Color, int> t = pair.Value.settings(config);
                var enabled = t.Item1;
                Font font = CreateFont(config.FontName, t.Item3);
                var labels = pair.Value.labels;
                var color = t.Item2;
                var mstr = pair.Value.maxString;
                var defaults = pair.Value.defaults;

                pair.Value.enabled = enabled;

                int i = 0;
                foreach (var l in labels)
                {
                    l.Visible = enabled;
                    if (enabled)
                    {
                        l.Font = font;
                        l.ForeColor = color;
                        var teststr = defaults[l].Replace("{}", mstr);
                        l.Size = MeasureText(teststr, l);
                        l.Margin = padding;
                        if (labels.Length == 1)
                            w_full = Math.Max(l.Size.Width, w_full);
                        else if (i == 0)
                            w_left = Math.Max(l.Size.Width, w_left);
                        else
                            w_right = Math.Max(l.Size.Width, w_right);
                        i++;
                    }
                }
            }

            foreach (KeyValuePair<string, Def> pair in def)
            {
                int i = 0;
                foreach (var l in pair.Value.labels)
                {
                    if (pair.Value.labels.Length == 1)
                        l.Size = new Size(w_full, l.Size.Height);
                    else if (i == 0)
                        l.Size = new Size(w_left, l.Size.Height);
                    else
                        l.Size = new Size(w_right, l.Size.Height);
                    i++;
                }
            }

            if (!config.DisplayRunes)
                panelRuneDisplay.Hide();
        }
    }
}
