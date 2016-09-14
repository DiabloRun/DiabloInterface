using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    public partial class HorizontalLayout : AbstractLayout
    {

        public HorizontalLayout()
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
                labelNormPerc, labelNmPerc, labelHellPerc,
                normLabel, nmLabel, hellLabel,
                normLabelVal, nmLabelVal, hellLabelVal,
            };

            runePanels = new[] 
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
            defaultTexts = new Dictionary<Control, string>();
            foreach ( Control c in l )
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

            labelNormPerc.Text = "NO: " + perc0 + "%";
            labelNmPerc.Text = "NM: " + perc1 + "%";
            labelHellPerc.Text = "HE: " + perc2 + "%";

            UpdateRuneDisplay(itemClassMap);
        }

        private void UpdateLayout(ApplicationSettings Settings)
        {
            int padding = 0;
            // Calculate maximum sizes that the labels can possible get.
            Size nameSize = TextRenderer.MeasureText(new string('W', 15), nameLabel.Font, Size.Empty, TextFormatFlags.SingleLine);

            // base stats have 3 char label (STR, VIT, ect.) and realistically a max value < 500 (lvl 99*5 + alkor quest... items can increase this tho)
            // we will assume the "longest" string is DEX: 499 (most likely dex or ene will be longest str.)
            padding = (panelAdvancedStats.Visible || panelResistances.Visible) ? 8 : 0;
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

            float ratio = (float)nameSize.Width / (float)statsWidth;

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
        
        private void ApplyRuneSettings(ApplicationSettings Settings)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => ApplyRuneSettings(Settings)));
                return;
            }

            if (Settings.DisplayRunes && Settings.DisplayRunesHorizontal && Settings.Runes.Count > 0)
            {
                panelRuneDisplayHorizontal.Controls.Clear();
                Settings.Runes.ForEach(r => { panelRuneDisplayHorizontal.Controls.Add(new RuneDisplayElement((Rune)r, Settings.DisplayRunesHighContrast, false, false)); });
                ChangeVisibility(panelRuneDisplayHorizontal, true);
            } else
            {
                ChangeVisibility(panelRuneDisplayHorizontal, false);
            }
            

            if (Settings.DisplayRunes && !Settings.DisplayRunesHorizontal && Settings.Runes.Count > 0)
            {
                panelRuneDisplayVertical.Controls.Clear();
                Settings.Runes.ForEach(r => { panelRuneDisplayVertical.Controls.Add(new RuneDisplayElement((Rune)r, Settings.DisplayRunesHighContrast, false, false)); });
                ChangeVisibility(panelRuneDisplayVertical, true);
            } else
            {
                ChangeVisibility(panelRuneDisplayVertical, false);
            }

        }

        private void ApplyLabelSettings(ApplicationSettings Settings)
        {

            RealFrwIas = Settings.DisplayRealFrwIas;

            this.BackColor = Settings.ColorBackground;

            nameLabel.Font = new Font(Settings.FontName, Settings.FontSizeTitle);
            Font infoFont = new Font(Settings.FontName, Settings.FontSize);
            foreach (Label label in infoLabels)
                label.Font = infoFont;

            // Hide/show labels wanted labels.
            ChangeVisibility(nameLabel, Settings.DisplayName);

            ChangeVisibility(goldLabel, Settings.DisplayGold);
            ChangeVisibility(panelSimpleStats, Settings.DisplayGold);

            ChangeVisibility(deathsLabel, Settings.DisplayDeathCounter);
            ChangeVisibility(lvlLabel, Settings.DisplayLevel);
            ChangeVisibility(panelDeathsLvl, Settings.DisplayDeathCounter || Settings.DisplayLevel);

            // If only death XOR lvl is to be displayed, set the col width of the non displayed col to 0
            if ( Settings.DisplayDeathCounter ^ Settings.DisplayLevel )
            {
                panelDeathsLvl.ColumnStyles[Settings.DisplayDeathCounter ? 1 : 0].SizeType = SizeType.Absolute;
                panelDeathsLvl.ColumnStyles[Settings.DisplayDeathCounter ? 1 : 0].Width = 0;
            } else
            {
                // otherwise restore columns to equal width:
                panelDeathsLvl.ColumnStyles[0].SizeType = SizeType.Percent;
                panelDeathsLvl.ColumnStyles[0].Width = 50;
                panelDeathsLvl.ColumnStyles[1].SizeType = SizeType.Percent;
                panelDeathsLvl.ColumnStyles[1].Width = 50;
            }

            ChangeVisibility(panelResistances, Settings.DisplayResistances);
            ChangeVisibility(panelBaseStats, Settings.DisplayBaseStats);
            ChangeVisibility(panelAdvancedStats, Settings.DisplayAdvancedStats);

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            ChangeVisibility(panelDiffPercentages, count < 3 && Settings.DisplayDifficultyPercentages);
            ChangeVisibility(panelDiffPercentages2, count >= 3 && Settings.DisplayDifficultyPercentages);

            ChangeVisibility(panelStats,
                Settings.DisplayResistances
                || Settings.DisplayBaseStats
                || Settings.DisplayAdvancedStats
                || Settings.DisplayDifficultyPercentages
            );

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

            labelNormPerc.ForeColor = Settings.ColorDifficultyPercentages;
            labelNmPerc.ForeColor = Settings.ColorDifficultyPercentages;
            labelHellPerc.ForeColor = Settings.ColorDifficultyPercentages;
        }
    }
}
