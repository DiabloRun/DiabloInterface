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

    public partial class VerticalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public VerticalLayout(ISettingsService settingsService, IGameService gameService)
            : base(settingsService, gameService)
        {
            Logger.Info("Creating vertical layout.");

            InitializeComponent();
            InitializeElements();
        }

        protected override Panel RuneLayoutPanel => panelRuneDisplay2;

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

            AdvancedStatLabels = new[] {
                fcrLabel, labelFcrVal, fhrLabel, labelFhrVal,
                iasLabel, labelIasVal, frwLabel, labelFrwVal
            };

            DifficultyLabels = new[] {
                normLabel, normLabelVal, nmLabel, nmLabelVal,
                hellLabel, hellLabelVal,
            };

            RunePanels = new[]
            {
                panelRuneDisplay2
            };

            IEnumerable<Control> l = new[]
            {
                nameLabel, lvlLabel, goldLabel, deathsLabel,
                labelStrVal, labelDexVal, labelVitVal, labelEneVal,
                labelFrwVal, labelFcrVal, labelFhrVal, labelIasVal,
                labelFireResVal, labelColdResVal, labelLightResVal, labelPoisonResVal,
                normLabelVal, nmLabelVal, hellLabelVal,
            };
            DefaultTexts = new Dictionary<Control, string>();
            foreach (Control c in l)
            {
                DefaultTexts.Add(c, c.Text);
            }
        }

        private void UpdatePanel(TableLayoutPanel panel, Size size, int padding)
        {
            panel.Size = new Size(size.Width, size.Height * panel.RowCount);
            panel.Margin = new Padding(panel.Margin.Left, padding, panel.Margin.Right, panel.Margin.Bottom);
        }

        private bool MaybeUpdatePanel(TableLayoutPanel panel, Func<Size> measurer, int padding)
        {
            if (!panel.Visible)
                return false;

            UpdatePanel(panel, measurer(), padding);
            return true;
        }

        private bool MaybeUpdateLabel(Label label, Func<Size> measurer)
        {
            if (!label.Visible)
                return false;

            label.Size = measurer();
            return true;
        }

        protected override void UpdateLayout(ApplicationSettings settings)
        {
            int padding = settings.VerticalLayoutPadding;

            bool hasPre = false;

            MaybeUpdateLabel(nameLabel, MeasureNameSize);

            hasPre |= MaybeUpdateLabel(goldLabel, MeasureGoldSize);
            hasPre |= MaybeUpdateLabel(deathsLabel, MeasureDeathsSize);
            hasPre |= MaybeUpdateLabel(lvlLabel, MeasureLvlSize);

            hasPre |= MaybeUpdatePanel(panelBaseStats, MeasureBaseStatsSize, hasPre ? padding : 0);
            hasPre |= MaybeUpdatePanel(panelAdvancedStats, MeasureAdvancedStatsSize, hasPre ? padding : 0);
            hasPre |= MaybeUpdatePanel(panelResistances, MeasureResistancesSize, hasPre ? padding : 0);
            hasPre |= MaybeUpdatePanel(panelDiffPercentages, MeasureDifficultyPercentageSize, hasPre ? padding : 0);
        }

        protected override void ApplyRuneSettings(ApplicationSettings settings)
        {
            if (!settings.DisplayRunes)
                panelRuneDisplay2.Hide();
        }

        protected override void ApplyLabelSettings(ApplicationSettings settings)
        {
            base.ApplyLabelSettings(settings);

            panelResistances.Visible = settings.DisplayResistances;
            panelBaseStats.Visible = settings.DisplayBaseStats;
            panelAdvancedStats.Visible = settings.DisplayAdvancedStats;
            panelDiffPercentages.Visible = settings.DisplayDifficultyPercentages;

            nameLabel.ForeColor = settings.ColorName;
            goldLabel.ForeColor = settings.ColorGold;
            deathsLabel.ForeColor = settings.ColorDeaths;
            lvlLabel.ForeColor = settings.ColorLevel;
            playersXLabel.ForeColor = settings.ColorPlayersX;

            UpdateLabelColors(settings);
        }
    }
}
