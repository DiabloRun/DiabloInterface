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
            add("name", new string('W', 15), (ApplicationSettings s) => Tuple.Create(s.DisplayName, s.ColorName, s.FontSizeTitle), "{}");
            add("playersx", "8", (ApplicationSettings s) => Tuple.Create(s.DisplayPlayersX, s.ColorPlayersX, s.FontSize), "/players {}");
            add("deaths", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayDeathCounter, s.ColorDeaths, s.FontSize), "DEATHS: {}");
            add("runs", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayGameCounter, s.ColorGameCounter, s.FontSize), "RUNS: {}");
            add("gold", "2500000", (ApplicationSettings s) => Tuple.Create(s.DisplayGold, s.ColorGold, s.FontSize), "GOLD: {}");
            add("mf", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMagicFind, s.ColorMagicFind, s.FontSize), "MF:", "{}");
            add("monstergold", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayMonsterGold, s.ColorMonsterGold, s.FontSize), "EMG:", "{}");
            add("atd", "999", (ApplicationSettings s) => Tuple.Create(s.DisplayAttackerSelfDamage, s.ColorAttackerSelfDamage, s.FontSize), "ATD:", "{}");
            add("lvl", "99", (ApplicationSettings s) => Tuple.Create(s.DisplayLevel, s.ColorLevel, s.FontSize), "LVL:", "{}");
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
            updateLabel("name", player.Name);
            updateLabel("playersx", currentPlayersX);
            updateLabel("deaths", player.Deaths);
            updateLabel("runs", (int) gameIndex);
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
            updateLabel("norm", $@"{completions[0]*100:0}");
            updateLabel("nm", $@"{completions[1]*100:0}");
            updateLabel("hell", $@"{completions[2]*100:0}");
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
