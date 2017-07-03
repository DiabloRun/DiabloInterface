using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using System;

namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using Zutatensuppe.DiabloInterface.Business.Settings;

    public partial class VerticalLayout : AbstractLayout
    {

        public VerticalLayout()
        {
            InitializeComponent();
            InitializeElements();
        }

        override protected void InitializeElements()
        {
            infoLabels = new[]
            {
                deathsLabel,
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

            runePanels = new[] 
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
            defaultTexts = new Dictionary<Control, string>();
            foreach (Control c in l)
            {
                defaultTexts.Add(c, c.Text);
            }
        }

        public void MakeInactive()
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => MakeInactive()));
                return;
            }
            Hide();
        }

        public void MakeActive(ApplicationSettings Settings)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => MakeActive(Settings)));
                return;
            }

            ApplyLabelSettings(Settings);
            ApplyRuneSettings(Settings);
            Show();
            UpdateLayout(Settings);
        }

        public void UpdateLabels(Character player, Dictionary<int, int> itemClassMap)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateLabels(player, itemClassMap)));
                return;
            }

            nameLabel.Text = player.Name;
            lvlLabel.Text = "LVL: " + player.Level;
            goldLabel.Text = "GOLD: " + (player.Gold + player.GoldStash);
            deathsLabel.Text = "DEATHS: " + player.Deaths;

            labelStrVal.Text = "" + player.Strength;
            labelDexVal.Text = "" + player.Dexterity;
            labelVitVal.Text = "" + player.Vitality;
            labelEneVal.Text = "" + player.Energy;
            UpdateMinWidth(new Label[] { labelStrVal, labelDexVal, labelVitVal, labelEneVal });

            labelFrwVal.Text = "" + (RealFrwIas ? player.RealFRW() : player.FasterRunWalk);
            labelFcrVal.Text = "" + player.FasterCastRate;
            labelFhrVal.Text = "" + player.FasterHitRecovery;
            labelIasVal.Text = "" + (RealFrwIas ? player.RealIAS() : player.IncreasedAttackSpeed);
            UpdateMinWidth(new Label[] { labelFrwVal, labelFcrVal, labelFhrVal, labelIasVal });

            labelFireResVal.Text = "" + player.FireResist;
            labelColdResVal.Text = "" + player.ColdResist;
            labelLightResVal.Text = "" + player.LightningResist;
            labelPoisonResVal.Text = "" + player.PoisonResist;
            UpdateMinWidth(new Label[] { labelFireResVal, labelColdResVal, labelLightResVal, labelPoisonResVal });

            int perc0 = (int)(100.0 * player.CompletedQuestCounts[0] / (float)D2QuestHelper.Quests.Count + .5);
            int perc1 = (int)(100.0 * player.CompletedQuestCounts[1] / (float)D2QuestHelper.Quests.Count + .5);
            int perc2 = (int)(100.0 * player.CompletedQuestCounts[2] / (float)D2QuestHelper.Quests.Count + .5);

            normLabelVal.Text = perc0 + "%";
            nmLabelVal.Text = perc1 + "%";
            hellLabelVal.Text = perc2 + "%";
            UpdateMinWidth(new Label[] { normLabelVal, nmLabelVal, hellLabelVal });

            UpdateRuneDisplay(itemClassMap);
        }

        private void UpdateLayout(ApplicationSettings Settings)
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
                panelBaseStats.Margin = new Padding(panelBaseStats.Margin.Left, first ? 0 : Settings.VerticalLayoutPadding, panelBaseStats.Margin.Right, panelBaseStats.Margin.Bottom);
                first = false;
            }

            // advanced stats have 3 char label (FCR, FRW, etc.) and realistically a max value < 100
            // we will assume the "longest" string is FRW: 99
            if (panelAdvancedStats.Visible)
            {
                Size advancedStatSize = TextRenderer.MeasureText("FRW: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelAdvancedStats.Size = new Size(advancedStatSize.Width, advancedStatSize.Height * panelAdvancedStats.RowCount);
                panelAdvancedStats.Margin = new Padding(panelAdvancedStats.Margin.Left, first ? 0 : Settings.VerticalLayoutPadding, panelAdvancedStats.Margin.Right, panelAdvancedStats.Margin.Bottom);
                first = false;
            }

            // Panel size for resistances can be negative, so max number of chars are 10 (LABL: -VAL)
            // resistances never go below -100 (longest possible string for the label) and never go above 95
            // we will assume the "longest" string is COLD: -100
            if (panelResistances.Visible)
            {
                Size resStatSize = TextRenderer.MeasureText("COLD: -100", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelResistances.Size = new Size(resStatSize.Width, resStatSize.Height * panelResistances.RowCount);
                panelResistances.Margin = new Padding(panelResistances.Margin.Left, first ? 0 : Settings.VerticalLayoutPadding, panelResistances.Margin.Right, panelResistances.Margin.Bottom);
                first = false;
            }

            // we will assume the "longest" string is NORM: 100%
            if (panelDiffPercentages.Visible)
            {
                Size diffPercStatSize = TextRenderer.MeasureText("NORM: 100%", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
                panelDiffPercentages.Size = new Size(diffPercStatSize.Width, diffPercStatSize.Height * panelDiffPercentages.RowCount);
                panelDiffPercentages.Margin = new Padding(panelDiffPercentages.Margin.Left, first ? 0 : Settings.VerticalLayoutPadding, panelDiffPercentages.Margin.Right, panelDiffPercentages.Margin.Bottom);
                first = false;
            }

        }

        private void ApplyRuneSettings(ApplicationSettings Settings)
        {
            if (Settings.DisplayRunes && Settings.Runes.Count > 0)
            {
                panelRuneDisplay2.Controls.Clear();
                Settings.Runes.ForEach(r => { panelRuneDisplay2.Controls.Add(new RuneDisplayElement((Rune)r, Settings.DisplayRunesHighContrast, false, false)); });

                ChangeVisibility(panelRuneDisplay2, true);
            } else
            {
                ChangeVisibility(panelRuneDisplay2, false);
            }
        }

        private void ApplyLabelSettings(ApplicationSettings Settings)
        {
            BackColor = Settings.ColorBackground;

            nameLabel.Font = new Font(Settings.FontName, Settings.FontSizeTitle);
            Font infoFont = new Font(Settings.FontName, Settings.FontSize);
            foreach (Label label in infoLabels)
                label.Font = infoFont;

            // Hide/show labels wanted labels.
            ChangeVisibility(nameLabel, Settings.DisplayName);
            ChangeVisibility(goldLabel, Settings.DisplayGold);
            ChangeVisibility(deathsLabel, Settings.DisplayDeathCounter);
            ChangeVisibility(lvlLabel, Settings.DisplayLevel);
            ChangeVisibility(panelResistances, Settings.DisplayResistances);
            ChangeVisibility(panelBaseStats, Settings.DisplayBaseStats);
            ChangeVisibility(panelAdvancedStats, Settings.DisplayAdvancedStats);
            ChangeVisibility(panelDiffPercentages, Settings.DisplayDifficultyPercentages);

            RealFrwIas = Settings.DisplayRealFrwIas;

            nameLabel.ForeColor = Settings.ColorName;
            goldLabel.ForeColor = Settings.ColorGold;
            deathsLabel.ForeColor = Settings.ColorDeaths;
            lvlLabel.ForeColor = Settings.ColorLevel;

            fireLabel.ForeColor = Settings.ColorFireRes;
            labelFireResVal.ForeColor = Settings.ColorFireRes;
            coldLabel.ForeColor = Settings.ColorColdRes;
            labelColdResVal.ForeColor = Settings.ColorColdRes;
            lighLabel.ForeColor = Settings.ColorLightningRes;
            labelLightResVal.ForeColor = Settings.ColorLightningRes;
            poisLabel.ForeColor = Settings.ColorPoisonRes;
            labelPoisonResVal.ForeColor = Settings.ColorPoisonRes;

            strLabel.ForeColor = Settings.ColorBaseStats;
            labelStrVal.ForeColor = Settings.ColorBaseStats;
            vitLabel.ForeColor = Settings.ColorBaseStats;
            labelVitVal.ForeColor = Settings.ColorBaseStats;
            dexLabel.ForeColor = Settings.ColorBaseStats;
            labelDexVal.ForeColor = Settings.ColorBaseStats;
            eneLabel.ForeColor = Settings.ColorBaseStats;
            labelEneVal.ForeColor = Settings.ColorBaseStats;

            fcrLabel.ForeColor = Settings.ColorAdvancedStats;
            labelFcrVal.ForeColor = Settings.ColorAdvancedStats;
            fhrLabel.ForeColor = Settings.ColorAdvancedStats;
            labelFhrVal.ForeColor = Settings.ColorAdvancedStats;
            iasLabel.ForeColor = Settings.ColorAdvancedStats;
            labelIasVal.ForeColor = Settings.ColorAdvancedStats;
            frwLabel.ForeColor = Settings.ColorAdvancedStats;
            labelFrwVal.ForeColor = Settings.ColorAdvancedStats;
            
            normLabel.ForeColor = Settings.ColorDifficultyPercentages;            
            normLabelVal.ForeColor = Settings.ColorDifficultyPercentages;
            nmLabel.ForeColor = Settings.ColorDifficultyPercentages;
            nmLabelVal.ForeColor = Settings.ColorDifficultyPercentages;
            hellLabel.ForeColor = Settings.ColorDifficultyPercentages;
            hellLabelVal.ForeColor = Settings.ColorDifficultyPercentages;
        }
        
    }
}
