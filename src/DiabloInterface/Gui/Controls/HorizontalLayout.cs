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

    public partial class HorizontalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        bool realFrwIas;
        FlowLayoutPanel activeRuneLayoutPanel;

        public HorizontalLayout(ISettingsService settingsService, IGameService gameService)
            : base(settingsService, gameService)
        {
            Logger.Info("Creating horizontal layout.");

            InitializeComponent();
            InitializeElements();

            panelRuneDisplayHorizontal.Hide();
            panelRuneDisplayVertical.Hide();
        }

        protected override Panel RuneLayoutPanel => activeRuneLayoutPanel;

        void InitializeElements()
        {
            InfoLabels = new[]
            {
                deathsLabel,
                goldLabel, lvlLabel,
                strLabel, dexLabel, vitLabel, eneLabel,
                frwLabel, fhrLabel, fcrLabel, iasLabel,
                fireLabel, coldLabel, lighLabel, poisLabel,
                labelPoisonResVal, labelFireResVal, labelLightResVal, labelColdResVal,
                labelFhrVal, labelFcrVal, labelFrwVal, labelIasVal,
                labelStrVal, labelDexVal, labelVitVal, labelEneVal,
                labelNormPerc, labelNmPerc, labelHellPerc,
                normLabel, nmLabel, hellLabel,
                normLabelVal, nmLabelVal, hellLabelVal,
            };

            RunePanels = new[]
            {
                panelRuneDisplayHorizontal,
                panelRuneDisplayVertical
            };

            IEnumerable<Control> l = new[]
            {
                nameLabel, lvlLabel, goldLabel, deathsLabel,
                labelStrVal, labelDexVal, labelVitVal, labelEneVal,
                labelFrwVal, labelFcrVal, labelFhrVal, labelIasVal,
                labelFireResVal, labelColdResVal, labelLightResVal, labelPoisonResVal,
                normLabelVal, nmLabelVal, hellLabelVal,
                labelNormPerc, labelNmPerc, labelHellPerc,
            };
            DefaultTexts = new Dictionary<Control, string>();
            foreach (Control c in l )
            {
                DefaultTexts.Add(c, c.Text);
            }
        }

        protected override void UpdateSettings(ApplicationSettings settings)
        {
            ApplyLabelSettings(settings);
            ApplyRuneSettings(settings);
            UpdateLayout();
        }

        protected override void UpdateLabels(Character player, IList<QuestCollection> quests)
        {
            nameLabel.Text = player.Name;
            lvlLabel.Text = "LVL: " + player.Level;
            goldLabel.Text = "GOLD: " + (player.Gold + player.GoldStash);
            deathsLabel.Text = "DEATHS: " + player.Deaths;

            labelStrVal.Text = "" + player.Strength;
            labelDexVal.Text = "" + player.Dexterity;
            labelVitVal.Text = "" + player.Vitality;
            labelEneVal.Text = "" + player.Energy;
            UpdateLabelWidthAlignment(labelStrVal, labelDexVal, labelVitVal, labelEneVal);

            labelFrwVal.Text = "" + (realFrwIas ? player.RealFRW() : player.FasterRunWalk);
            labelFcrVal.Text = "" + player.FasterCastRate;
            labelFhrVal.Text = "" + player.FasterHitRecovery;
            labelIasVal.Text = "" + (realFrwIas ? player.RealIAS() : player.IncreasedAttackSpeed);
            UpdateLabelWidthAlignment(labelFrwVal, labelFcrVal, labelFhrVal, labelIasVal);

            labelFireResVal.Text = "" + player.FireResist;
            labelColdResVal.Text = "" + player.ColdResist;
            labelLightResVal.Text = "" + player.LightningResist;
            labelPoisonResVal.Text = "" + player.PoisonResist;
            UpdateLabelWidthAlignment(labelFireResVal, labelColdResVal, labelLightResVal, labelPoisonResVal);

            IList<float> completions = quests.Select(q => q.CompletionProgress).ToList();

            normLabelVal.Text = $@"{completions[0]:0%}";
            nmLabelVal.Text = $@"{completions[1]:0%}";
            hellLabelVal.Text = $@"{completions[2]:0%}";
            UpdateLabelWidthAlignment(normLabelVal, nmLabelVal, hellLabelVal);

            labelNormPerc.Text = $@"NO: {completions[0]:0%}";
            labelNmPerc.Text = $@"NM: {completions[1]:0%}";
            labelHellPerc.Text = $@"HE: {completions[2]:0%}";
        }

        void UpdateLayout()
        {
            // Calculate maximum sizes that the labels can possible get.
            Size nameSize = TextRenderer.MeasureText(new string('W', 15), nameLabel.Font, Size.Empty, TextFormatFlags.SingleLine);

            // base stats have 3 char label (STR, VIT, ect.) and realistically a max value < 500 (lvl 99*5 + alkor quest... items can increase this tho)
            // we will assume the "longest" string is DEX: 499 (most likely dex or ene will be longest str.)
            var padding = (panelAdvancedStats.Visible || panelResistances.Visible) ? 8 : 0;
            Size statSize = TextRenderer.MeasureText("DEX: 499", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size basePanelSize = new Size(statSize.Width + padding, statSize.Height * panelBaseStats.RowCount);

            // advanced stats have 3 char label (FCR, FRW, etc.) and realistically a max value < 100
            // we will assume the "longest" string is FRW: 99
            padding = panelResistances.Visible ? 8 : 0;
            Size advancedStatSize = TextRenderer.MeasureText("FRW: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size advancedStatPanelSize = new Size(advancedStatSize.Width + padding, advancedStatSize.Height * panelAdvancedStats.RowCount);

            // Panel size for resistances can be negative, so max number of chars are 10 (LABL: -VAL)
            // resistances never go below -100 (longest possible string for the label) and never go above 95
            // we will assume the "longest" string is COLD: -100
            Size resStatSize = TextRenderer.MeasureText("COLD: -100", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size resPanelSize = new Size(resStatSize.Width, resStatSize.Height * panelResistances.RowCount);

            // we will assume the "longest" string is NORM: 100%
            Size diffPercStatSize = TextRenderer.MeasureText("NORM: 100%", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size diffPercPanelSize = new Size(diffPercStatSize.Width, diffPercStatSize.Height * panelDiffPercentages.RowCount);

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            // Recalculate panel size if the title is wider than all panels combined.
            int statsWidth = (panelResistances.Visible ? resPanelSize.Width : 0)
                + (panelBaseStats.Visible ? basePanelSize.Width : 0)
                + (panelAdvancedStats.Visible ? advancedStatPanelSize.Width : 0)
                + ((count < 3 && panelDiffPercentages.Visible) ? panelDiffPercentages.Width : 0)
            ;

            float ratio = nameSize.Width / (float)statsWidth;

            if (ratio > 1.0f)
            {
                resPanelSize.Width = (int)(resPanelSize.Width * ratio + .5f);
                basePanelSize.Width = (int)(basePanelSize.Width * ratio + .5f);
                advancedStatPanelSize.Width = (int)(advancedStatPanelSize.Width * ratio + .5f);
                if (count < 3 && panelDiffPercentages.Visible)
                {
                    panelDiffPercentages.Width = (int)(diffPercPanelSize.Width * ratio + .5f);
                }
            }

            nameLabel.Size = nameSize;
            panelBaseStats.Size = basePanelSize;
            panelAdvancedStats.Size = advancedStatPanelSize;
            panelResistances.Size = resPanelSize;
            panelDiffPercentages.Size = diffPercPanelSize;
            panelRuneDisplayHorizontal.MaximumSize = new Size(Math.Max(nameSize.Width, statsWidth), 0);
        }
        
        void ApplyRuneSettings(ApplicationSettings settings)
        {
            FlowLayoutPanel nextRuneLayoutPanel = null;

            if (settings.DisplayRunes && settings.DisplayRunesHorizontal)
                nextRuneLayoutPanel = panelRuneDisplayHorizontal;
            else if (settings.DisplayRunes && !settings.DisplayLayoutHorizontal)
                nextRuneLayoutPanel = panelRuneDisplayVertical;

            // Only hide panels when the panels were changed.
            if (activeRuneLayoutPanel == nextRuneLayoutPanel) return;

            activeRuneLayoutPanel?.Hide();
            activeRuneLayoutPanel = nextRuneLayoutPanel;
        }

        void ApplyLabelSettings(ApplicationSettings settings)
        {
            realFrwIas = settings.DisplayRealFrwIas;

            BackColor = settings.ColorBackground;

            nameLabel.Font = new Font(settings.FontName, settings.FontSizeTitle);
            Font infoFont = new Font(settings.FontName, settings.FontSize);
            foreach (Label label in InfoLabels)
                label.Font = infoFont;

            // Hide/show labels wanted labels.
            nameLabel.Visible = settings.DisplayName;

            goldLabel.Visible =  settings.DisplayGold;
            panelSimpleStats.Visible = settings.DisplayGold;

            deathsLabel.Visible = settings.DisplayDeathCounter;
            lvlLabel.Visible = settings.DisplayLevel;
            panelDeathsLvl.Visible = settings.DisplayDeathCounter || settings.DisplayLevel;

            // If only death XOR lvl is to be displayed, set the col width of the non displayed col to 0
            if (settings.DisplayDeathCounter ^ settings.DisplayLevel)
            {
                panelDeathsLvl.ColumnStyles[settings.DisplayDeathCounter ? 1 : 0].SizeType = SizeType.Absolute;
                panelDeathsLvl.ColumnStyles[settings.DisplayDeathCounter ? 1 : 0].Width = 0;
            }
            else
            {
                // otherwise restore columns to equal width:
                panelDeathsLvl.ColumnStyles[0].SizeType = SizeType.Percent;
                panelDeathsLvl.ColumnStyles[0].Width = 50;
                panelDeathsLvl.ColumnStyles[1].SizeType = SizeType.Percent;
                panelDeathsLvl.ColumnStyles[1].Width = 50;
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

            nameLabel.ForeColor = settings.ColorName;
            goldLabel.ForeColor = settings.ColorGold;
            deathsLabel.ForeColor = settings.ColorDeaths;
            lvlLabel.ForeColor = settings.ColorLevel;

            fireLabel.ForeColor = settings.ColorFireRes;
            labelFireResVal.ForeColor = settings.ColorFireRes;
            coldLabel.ForeColor = settings.ColorColdRes;
            labelColdResVal.ForeColor = settings.ColorColdRes;
            lighLabel.ForeColor = settings.ColorLightningRes;
            labelLightResVal.ForeColor = settings.ColorLightningRes;
            poisLabel.ForeColor = settings.ColorPoisonRes;
            labelPoisonResVal.ForeColor = settings.ColorPoisonRes;

            strLabel.ForeColor = settings.ColorBaseStats;
            labelStrVal.ForeColor = settings.ColorBaseStats;
            vitLabel.ForeColor = settings.ColorBaseStats;
            labelVitVal.ForeColor = settings.ColorBaseStats;
            dexLabel.ForeColor = settings.ColorBaseStats;
            labelDexVal.ForeColor = settings.ColorBaseStats;
            eneLabel.ForeColor = settings.ColorBaseStats;
            labelEneVal.ForeColor = settings.ColorBaseStats;

            fcrLabel.ForeColor = settings.ColorAdvancedStats;
            labelFcrVal.ForeColor = settings.ColorAdvancedStats;
            fhrLabel.ForeColor = settings.ColorAdvancedStats;
            labelFhrVal.ForeColor = settings.ColorAdvancedStats;
            iasLabel.ForeColor = settings.ColorAdvancedStats;
            labelIasVal.ForeColor = settings.ColorAdvancedStats;
            frwLabel.ForeColor = settings.ColorAdvancedStats;
            labelFrwVal.ForeColor = settings.ColorAdvancedStats;

            normLabel.ForeColor = settings.ColorDifficultyPercentages;
            normLabelVal.ForeColor = settings.ColorDifficultyPercentages;
            nmLabel.ForeColor = settings.ColorDifficultyPercentages;
            nmLabelVal.ForeColor = settings.ColorDifficultyPercentages;
            hellLabel.ForeColor = settings.ColorDifficultyPercentages;
            hellLabelVal.ForeColor = settings.ColorDifficultyPercentages;

            labelNormPerc.ForeColor = settings.ColorDifficultyPercentages;
            labelNmPerc.ForeColor = settings.ColorDifficultyPercentages;
            labelHellPerc.ForeColor = settings.ColorDifficultyPercentages;
        }
    }
}
