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
    
    class VerticalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private FlowLayoutPanel outerLeftRightPanel;
        private FlowLayoutPanel panelRuneDisplay;
        private TableLayoutPanel table;

        public VerticalLayout(ISettingsService settingsService, IGameService gameService)
        {
            this.settingsService = settingsService;
            this.gameService = gameService;

            RegisterServiceEventHandlers();
            InitializeComponent();

            Load += (sender, e) => UpdateSettings(settingsService.CurrentSettings);

            // Clean up events when disposed because services outlive us.
            Disposed += (sender, e) => UnregisterServiceEventHandlers();
            Logger.Info("Creating vertical layout.");
        }

        protected override Panel RuneLayoutPanel => panelRuneDisplay;

        protected void InitializeComponent()
        {
            Add("name", new string('W', 15), (ApplicationSettings s) => Tuple.Create(s.DisplayName, s.ColorName, s.FontSizeTitle), "{}");
            Add("hc_sc", "HARDCORE", (ApplicationSettings s) => Tuple.Create(s.DisplayHardcoreSoftcore, s.ColorHardcoreSoftcore, s.FontSize), "{}");
            Add("exp_classic", "EXPANSION", (ApplicationSettings s) => Tuple.Create(s.DisplayExpansionClassic, s.ColorExpansionClassic, s.FontSize), "{}");
            Add("playersx", "8", (ApplicationSettings s) => Tuple.Create(s.DisplayPlayersX, s.ColorPlayersX, s.FontSize), "/players {}");
            Add("deaths", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayDeathCounter, s.ColorDeaths, s.FontSize), "DEATHS: {}");
            Add("runs", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayGameCounter, s.ColorGameCounter, s.FontSize), "RUNS: {}");
            Add("gold", "2500000", (ApplicationSettings s) => Tuple.Create(s.DisplayGold, s.ColorGold, s.FontSize), "GOLD: {}");
            Add("mf", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMagicFind, s.ColorMagicFind, s.FontSize), "MF:", "{}");
            Add("monstergold", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMonsterGold, s.ColorMonsterGold, s.FontSize), "EMG:", "{}");
            Add("atd", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAttackerSelfDamage, s.ColorAttackerSelfDamage, s.FontSize), "ATD:", "{}");
            Add("lvl", "99", (ApplicationSettings s) => Tuple.Create(s.DisplayLevel, s.ColorLevel, s.FontSize), "LVL:", "{}");
            Add("str", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "STR:", "{}");
            Add("vit", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "VIT:", "{}");
            Add("dex", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "DEX:", "{}");
            Add("ene", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayBaseStats, s.ColorBaseStats, s.FontSize), "ENE:", "{}");
            Add("ias", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "IAS:", "{}");
            Add("frw", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FRW:", "{}");
            Add("fcr", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FCR:", "{}");
            Add("fhr", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAdvancedStats, s.ColorAdvancedStats, s.FontSize), "FHR:", "{}");
            Add("cold", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorColdRes, s.FontSize), "COLD:", "{}");
            Add("ligh", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorLightningRes, s.FontSize), "LIGH:", "{}");
            Add("pois", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorPoisonRes, s.FontSize), "POIS:", "{}");
            Add("fire", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayResistances, s.ColorFireRes, s.FontSize), "FIRE:", "{}");
            Add("norm", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NORM:", "{}%");
            Add("nm", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "NM:", "{}%");
            Add("hell", "100", (ApplicationSettings s) => Tuple.Create(s.DisplayDifficultyPercentages, s.ColorDifficultyPercentages, s.FontSize), "HELL:", "{}%");
            
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

        protected override void UpdateLabels(Character player, IList<QuestCollection> quests, int currentPlayersX, uint gameIndex)
        {
            UpdateLabel("name", player.Name);
            UpdateLabel("hc_sc", player.IsHardcore ? "HARDCORE" : "SOFTCORE");
            UpdateLabel("exp_classic", player.IsExpansion ? "EXPANSION" : "CLASSIC");
            UpdateLabel("playersx", currentPlayersX);
            UpdateLabel("deaths", player.Deaths);
            UpdateLabel("runs", (int) gameIndex);
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

            IList<float> completions = quests.Select(q => q.CompletionProgress).ToList();
            UpdateLabel("norm", $@"{completions[0]*100:0}");
            UpdateLabel("nm", $@"{completions[1]*100:0}");
            UpdateLabel("hell", $@"{completions[2]*100:0}");
        }

        protected override void UpdateSettings(ApplicationSettings s)
        {
            var padding = new Padding(0, 0, 0, s.VerticalLayoutPadding);
            realFrwIas = s.DisplayRealFrwIas;
            BackColor = s.ColorBackground;

            int w_full = 0;
            int w_left = 0;
            int w_right = 0;

            void upd(Label[] ls, bool v, Font f, Color c, string mstr, Dictionary<Label, string> defaults)
            {
                int i = 0;
                foreach (var l in ls)
                {
                    l.Visible = v;
                    if (v)
                    {
                        l.Font = f;
                        l.ForeColor = c;
                        var teststr = defaults[l].Replace("{}", mstr);
                        l.Size = MeasureText(teststr, l);
                        l.Margin = padding;
                        if (ls.Length == 1)
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
                Tuple<bool, Color, int> t = pair.Value.settings(s);
                upd(pair.Value.labels, t.Item1, new Font(s.FontName, t.Item3), t.Item2, pair.Value.maxString, pair.Value.defaults);
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

            if (!s.DisplayRunes)
                panelRuneDisplay.Hide();
        }
    }
}
