namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
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

        protected override void UpdateLayout(ApplicationSettings settings)
        {
            bool first = true;
            // Calculate maximum sizes that the labels can possible get.
            if (nameLabel.Visible)
            {
                nameLabel.Size = TextRenderer.MeasureText(new string('W', 15), nameLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            }
            if (goldLabel.Visible)
            {
                goldLabel.Size = TextRenderer.MeasureText("GOLD: 2500000", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                first = false;
            }
            if (deathsLabel.Visible)
            {
                deathsLabel.Size = TextRenderer.MeasureText("DEATHS: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                first = false;
            }
            if (lvlLabel.Visible)
            {
                lvlLabel.Size = TextRenderer.MeasureText("LVL: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                first = false;
            }

            // base stats have 3 char label (STR, VIT, ect.) and realistically a max value < 500 (lvl 99*5 + alkor quest... items can increase this tho)
            // we will assume the "longest" string is DEX: 499 (most likely dex or ene will be longest str.)
            if (panelBaseStats.Visible)
            {
                Size statSize = TextRenderer.MeasureText("DEX: 499", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelBaseStats.Size = new Size(statSize.Width, statSize.Height * panelBaseStats.RowCount);
                panelBaseStats.Margin = new Padding(panelBaseStats.Margin.Left, first ? 0 : settings.VerticalLayoutPadding, panelBaseStats.Margin.Right, panelBaseStats.Margin.Bottom);
                first = false;
            }

            // advanced stats have 3 char label (FCR, FRW, etc.) and realistically a max value < 100
            // we will assume the "longest" string is FRW: 99
            if (panelAdvancedStats.Visible)
            {
                Size advancedStatSize = TextRenderer.MeasureText("FRW: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelAdvancedStats.Size = new Size(advancedStatSize.Width, advancedStatSize.Height * panelAdvancedStats.RowCount);
                panelAdvancedStats.Margin = new Padding(panelAdvancedStats.Margin.Left, first ? 0 : settings.VerticalLayoutPadding, panelAdvancedStats.Margin.Right, panelAdvancedStats.Margin.Bottom);
                first = false;
            }

            // Panel size for resistances can be negative, so max number of chars are 10 (LABL: -VAL)
            // resistances never go below -100 (longest possible string for the label) and never go above 95
            // we will assume the "longest" string is COLD: -100
            if (panelResistances.Visible)
            {
                Size resStatSize = TextRenderer.MeasureText("COLD: -100", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelResistances.Size = new Size(resStatSize.Width, resStatSize.Height * panelResistances.RowCount);
                panelResistances.Margin = new Padding(panelResistances.Margin.Left, first ? 0 : settings.VerticalLayoutPadding, panelResistances.Margin.Right, panelResistances.Margin.Bottom);
                first = false;
            }

            // we will assume the "longest" string is NORM: 100%
            if (panelDiffPercentages.Visible)
            {
                Size diffPercStatSize = TextRenderer.MeasureText("NORM: 100%", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelDiffPercentages.Size = new Size(diffPercStatSize.Width, diffPercStatSize.Height * panelDiffPercentages.RowCount);
                panelDiffPercentages.Margin = new Padding(panelDiffPercentages.Margin.Left, first ? 0 : settings.VerticalLayoutPadding, panelDiffPercentages.Margin.Right, panelDiffPercentages.Margin.Bottom);
                first = false;
            }
        }

        protected override void ApplyRuneSettings(ApplicationSettings settings)
        {
            if (!settings.DisplayRunes)
                panelRuneDisplay2.Hide();
        }

        protected override void ApplyLabelSettings(ApplicationSettings settings)
        {
            realFrwIas = settings.DisplayRealFrwIas;

            BackColor = settings.ColorBackground;

            nameLabel.Font = new Font(settings.FontName, settings.FontSizeTitle);
            var infoFont = new Font(settings.FontName, settings.FontSize);
            foreach (Label label in InfoLabels)
                label.Font = infoFont;

            // Hide/show labels wanted labels.
            nameLabel.Visible = settings.DisplayName;
            goldLabel.Visible = settings.DisplayGold;
            deathsLabel.Visible = settings.DisplayDeathCounter;
            lvlLabel.Visible = settings.DisplayLevel;
            playersXLabel.Visible = settings.DisplayPlayersX;

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
