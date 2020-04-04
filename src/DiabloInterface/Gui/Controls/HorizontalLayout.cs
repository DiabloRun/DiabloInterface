namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    class HorizontalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private FlowLayoutPanel activeRuneLayoutPanel;
        private FlowLayoutPanel outerLeftRightPanel;
        private FlowLayoutPanel flowLayoutPanel1;
        protected override Panel RuneLayoutPanel => activeRuneLayoutPanel;
        private TableLayoutPanel panelAdvancedStats;
        private TableLayoutPanel panelResistances;
        private TableLayoutPanel panelDiffPercentages;
        private TableLayoutPanel panelDeathsLvl;
        private TableLayoutPanel panelSimpleStats;
        private FlowLayoutPanel panelStats;
        private TableLayoutPanel panelDiffPercentages2;
        private FlowLayoutPanel panelRuneDisplayHorizontal;
        private FlowLayoutPanel panelRuneDisplayVertical;
        private TableLayoutPanel panelBaseStats;

        protected void InitializeComponent()
        {
            add("name", "WWW_WWWWWWWWW", (ApplicationSettings s) => Tuple.Create(s.DisplayName, s.ColorName, s.FontSizeTitle), "{}");
            add("playersx", "8", (ApplicationSettings s) => Tuple.Create(s.DisplayPlayersX, s.ColorPlayersX, s.FontSize), "/players {}");
            add("deaths", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayDeathCounter, s.ColorDeaths, s.FontSize), "DEATHS: {}");
            add("runs", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayGameCounter, s.ColorGameCounter, s.FontSize), "RUNS: {}");
            add("gold", "2500000", (ApplicationSettings s) => Tuple.Create(s.DisplayGold, s.ColorGold, s.FontSize), "GOLD: {}");
            add("mf", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMagicFind, s.ColorMagicFind, s.FontSize), "MF: {}");
            add("monstergold", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMonsterGold, s.ColorMonsterGold, s.FontSize), "EMG: {}");
            add("atd", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAttackerSelfDamage, s.ColorAttackerSelfDamage, s.FontSize), "ATD: {}");
            add("lvl", "99", (ApplicationSettings s) => Tuple.Create(s.DisplayLevel, s.ColorLevel, s.FontSize), "LVL: {}");
            add("str", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "STR:", "{}");
            add("vit", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "VIT:", "{}");
            add("dex", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "DEX:", "{}");
            add("ene", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "ENE:", "{}");
            add("ias", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "IAS:", "{}");
            add("frw", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FRW:", "{}");
            add("fcr", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FCR:", "{}");
            add("fhr", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FHR:", "{}");
            add("cold", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorColdRes, s.FontSize), "COLD:", "{}");
            add("ligh", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorLightningRes, s.FontSize), "LIGH:", "{}");
            add("pois", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorPoisonRes, s.FontSize), "POIS:", "{}");
            add("fire", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorFireRes, s.FontSize), "FIRE:", "{}");
            add("norm", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NORM:", "{}%");
            add("nm", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NM:", "{}%");
            add("hell", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "HELL:", "{}%");
            add("norm_inline", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NO: {}%");
            add("nm_inline", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NM: {}%");
            add("hell_inline", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "HE: {}%");

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

            this.panelSimpleStats = rowthing("gold", "mf");
            this.panelDeathsLvl = rowthing("deaths", "lvl");
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
            this.flowLayoutPanel1.Controls.Add(def["playersx"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(def["runs"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(this.panelDeathsLvl);
            this.flowLayoutPanel1.Controls.Add(def["atd"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(def["monstergold"].labels[0]);
            this.flowLayoutPanel1.Controls.Add(this.panelSimpleStats);
            this.flowLayoutPanel1.Controls.Add(this.panelStats);
            this.flowLayoutPanel1.Controls.Add(this.panelDiffPercentages2);
            this.flowLayoutPanel1.Controls.Add(this.panelRuneDisplayHorizontal);
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

        public HorizontalLayout(ISettingsService settingsService, IGameService gameService)
        {
            this.settingsService = settingsService;
            this.gameService = gameService;

            RegisterServiceEventHandlers();
            InitializeComponent();

            Load += (sender, e) => UpdateSettings(settingsService.CurrentSettings);

            // Clean up events when disposed because services outlive us.
            Disposed += (sender, e) => UnregisterServiceEventHandlers();

            Logger.Info("Creating horizontal layout.");

            panelRuneDisplayHorizontal.Hide();
            panelRuneDisplayVertical.Hide();
        }

        protected override void UpdateSettings(ApplicationSettings settings)
        {
            realFrwIas = settings.DisplayRealFrwIas;
            BackColor = settings.ColorBackground;

            var margin = new Padding(2);
            foreach (KeyValuePair<string, Def> pair in def)
            {
                var x = pair.Value.settings(settings);
                var visible = x.Item1;
                if (visible)
                {
                    var color = x.Item2;
                    var font = new Font(settings.FontName, x.Item3);
                    foreach (Label l in pair.Value.labels)
                    {
                        l.Visible = visible;
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
                        l.Visible = visible;
                    }
                }
            }

            panelResistances.Visible = settings.DisplayResistances;
            panelBaseStats.Visible = settings.DisplayBaseStats;
            panelAdvancedStats.Visible = settings.DisplayAdvancedStats;

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            panelDiffPercentages.Visible = count < 3 && settings.DisplayDifficultyPercentages;
            panelDiffPercentages2.Visible = count >= 3 && settings.DisplayDifficultyPercentages;

            panelStats.Visible =
                settings.DisplayResistances
                || settings.DisplayBaseStats
                || settings.DisplayAdvancedStats
                || settings.DisplayDifficultyPercentages;

            FlowLayoutPanel nextRuneLayoutPanel = null;

            if (settings.DisplayRunes)
            {
                nextRuneLayoutPanel = settings.DisplayRunesHorizontal
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

        protected override void UpdateLabels(Character player, IList<QuestCollection> quests, int currentPlayersX, uint gameIndex)
        {
            updateLabel("name", player.Name);
            updateLabel("playersx", currentPlayersX);
            updateLabel("deaths", player.Deaths);
            updateLabel("runs", (int)gameIndex);
            updateLabel("gold", player.Gold + player.GoldStash);
            updateLabel("mf", player.MagicFind);
            updateLabel("monstergold", player.MonsterGold);
            updateLabel("atd", player.AttackerSelfDamage);
            updateLabel("lvl", player.Level);
            updateLabel("str", player.Strength);
            updateLabel("vit", player.Vitality);
            updateLabel("dex", player.Dexterity);
            updateLabel("ene", player.Energy);
            updateLabel("ias", realFrwIas ? player.RealIAS() : player.IncreasedAttackSpeed);
            updateLabel("frw", realFrwIas ? player.RealFRW() : player.FasterRunWalk);
            updateLabel("fcr", player.FasterCastRate);
            updateLabel("fhr", player.FasterHitRecovery);
            updateLabel("cold", player.ColdResist);
            updateLabel("ligh", player.LightningResist);
            updateLabel("pois", player.PoisonResist);
            updateLabel("fire", player.FireResist);

            IList<float> completions = quests.Select(q => q.CompletionProgress).ToList();
            updateLabel("norm", $@"{completions[0] * 100:0}");
            updateLabel("nm", $@"{completions[1] * 100:0}");
            updateLabel("hell", $@"{completions[2] * 100:0}");
            updateLabel("norm_inline", $@"{completions[0] * 100:0}");
            updateLabel("nm_inline", $@"{completions[1] * 100:0}");
            updateLabel("hell_inline", $@"{completions[2] * 100:0}");
        }
    }
}
