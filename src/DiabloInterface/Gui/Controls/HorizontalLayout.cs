namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public partial class HorizontalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

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
                playersXLabel,
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

            FireLabels = new[] { fireLabel, labelFireResVal };
            ColdLabels = new[] { coldLabel, labelColdResVal };
            LighLabels = new[] { lighLabel, labelLightResVal };
            PoisLabels = new[] { poisLabel, labelPoisonResVal };

            BaseStatLabels = new[] {
                strLabel,labelStrVal,vitLabel,labelVitVal,
                dexLabel, labelDexVal, eneLabel, labelEneVal
            };

            AdvancedStatLabels = new [] {
                fcrLabel, labelFcrVal, fhrLabel, labelFhrVal,
                iasLabel, labelIasVal, frwLabel, labelFrwVal
            };

            DifficultyLabels = new [] {
                normLabel, normLabelVal, nmLabel, nmLabelVal,
                hellLabel, hellLabelVal, labelNormPerc, labelNmPerc, labelHellPerc
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

        protected override void UpdateLayout(ApplicationSettings settings)
        {
            // Calculate maximum sizes that the labels can possible get.
            Size nameSize = MeasureNameSize();
            
            var padding = (panelAdvancedStats.Visible || panelResistances.Visible) ? 8 : 0;
            Size statSize = MeasureBaseStatsSize();
            Size basePanelSize = new Size(statSize.Width + padding, statSize.Height * panelBaseStats.RowCount);

            padding = panelResistances.Visible ? 8 : 0;
            Size advancedStatSize = MeasureAdvancedStatsSize();
            Size advancedStatPanelSize = new Size(advancedStatSize.Width + padding, advancedStatSize.Height * panelAdvancedStats.RowCount);

            Size resStatSize = MeasureResistancesSize();
            Size resPanelSize = new Size(resStatSize.Width, resStatSize.Height * panelResistances.RowCount);

            Size diffPercStatSize = MeasureDifficultyPercentageSize();
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
        
        protected override void ApplyRuneSettings(ApplicationSettings settings)
        {
            FlowLayoutPanel nextRuneLayoutPanel = null;

            if (settings.DisplayRunes && settings.DisplayRunesHorizontal)
                nextRuneLayoutPanel = panelRuneDisplayHorizontal;
            else if (settings.DisplayRunes && !settings.DisplayRunesHorizontal)
                nextRuneLayoutPanel = panelRuneDisplayVertical;

            // Only hide panels when the panels were changed.
            if (activeRuneLayoutPanel == nextRuneLayoutPanel) return;

            activeRuneLayoutPanel?.Hide();
            activeRuneLayoutPanel = nextRuneLayoutPanel;
        }

        protected override void ApplyLabelSettings(ApplicationSettings settings)
        {
            base.ApplyLabelSettings(settings);

            panelSimpleStats.Visible = settings.DisplayGold;
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
            playersXLabel.ForeColor = settings.ColorPlayersX;

            UpdateLabelColors(settings);
        }
    }
}
