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

    public class VerticalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);
        
        private FlowLayoutPanel panelRuneDisplay;
        private TableLayoutPanel table;

        public VerticalLayout(ISettingsService settingsService, IGameService gameService)
            : base(settingsService, gameService)
        {
            Logger.Info("Creating vertical layout.");

            InitializeComponent();
            InitializeElements();
        }

        protected override Panel RuneLayoutPanel => panelRuneDisplay;

        new protected void InitializeComponent()
        {
            base.InitializeComponent();
            
            panelRuneDisplay = new FlowLayoutPanel();
            table = new TableLayoutPanel();
            table.SuspendLayout();

            SuspendLayout();

            outerLeftRightPanel.AutoSize = true;
            outerLeftRightPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            outerLeftRightPanel.Controls.Add(panelRuneDisplay);
            outerLeftRightPanel.Controls.Add(table);

            table.AutoSize = true;
            table.ColumnCount = 2;
            table.ColumnStyles.Add(new ColumnStyle());
            table.ColumnStyles.Add(new ColumnStyle());
            table.RowCount = 0;

            void add(params Label[] controls)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                for (int i = 0; i < controls.Length; i++)
                {
                    controls[i].AutoSize = true;
                    table.Controls.Add(controls[i], i, table.RowCount - 1);
                    if (i > 0)
                    {
                        controls[i].TextAlign = ContentAlignment.TopRight;
                    }
                }
                if (controls.Length == 1)
                {
                    table.SetColumnSpan(controls[0], table.ColumnCount);
                }
            }

            nameLabel.Text = "Name";
            playersXLabel.Text = "/players X";
            deathsLabel.Text = "DEATHS:";
            deathsLabelVal.Text = "-";
            lvlLabel.Text = "LVL:";
            lvlLabelVal.Text = "-";
            goldLabel.Text = "GOLD:";
            goldLabelVal.Text = "-";
            strLabel.Text = "STR:";
            labelStrVal.Text = "-";
            vitLabel.Text = "VIT:";
            labelVitVal.Text = "-";
            dexLabel.Text = "DEX:";
            labelDexVal.Text = "-";
            eneLabel.Text = "ENE:";
            labelEneVal.Text = "-";
            iasLabel.Text = "IAS:";
            labelIasVal.Text = "-";
            frwLabel.Text = "FRW:";
            labelFrwVal.Text = "-";
            fcrLabel.Text = "FCR:";
            labelFcrVal.Text = "-";
            fhrLabel.Text = "FHR:";
            labelFhrVal.Text = "-";
            coldLabel.Text = "COLD:";
            labelColdResVal.Text = "-";
            lighLabel.Text = "LIGH:";
            labelLightResVal.Text = "-";
            poisLabel.Text = "POIS:";
            labelPoisonResVal.Text = "-";
            fireLabel.Text = "FIRE:";
            labelFireResVal.Text = "-";
            normLabel.Text = "NORM:";
            normLabelVal.Text = "-";
            nmLabel.Text = "NM:";
            nmLabelVal.Text = "-";
            hellLabel.Text = "HELL:";
            hellLabelVal.Text = "-";
            gameCounterLabel.Text = "RUNS:";
            gameCounterLabelVal.Text = "1";
            magicFindLabel.Text = "MF:";
            magicFindLabelVal.Text = "-";
            monsterGoldLabel.Text = "EMG:";
            monsterGoldLabelVal.Text = "-";
            attackerSelfDamageLabel.Text = "ATD:";
            attackerSelfDamageLabelVal.Text = "-";

            add(nameLabel);
            add(playersXLabel);

            add(gameCounterLabel, gameCounterLabelVal);
            add(deathsLabel, deathsLabelVal);
            add(lvlLabel, lvlLabelVal);
            add(goldLabel, goldLabelVal);
            add(magicFindLabel, magicFindLabelVal);
            add(monsterGoldLabel, monsterGoldLabelVal);
            add(attackerSelfDamageLabel, attackerSelfDamageLabelVal);
            add(strLabel, labelStrVal);
            add(dexLabel, labelDexVal);
            add(vitLabel, labelVitVal);
            add(eneLabel, labelEneVal);
            add(frwLabel, labelFrwVal);
            add(fhrLabel, labelFhrVal);
            add(fcrLabel, labelFcrVal);
            add(iasLabel, labelIasVal);
            add(fireLabel, labelFireResVal);
            add(coldLabel, labelColdResVal);
            add(lighLabel, labelLightResVal);
            add(poisLabel, labelPoisonResVal);
            add(normLabel, normLabelVal);
            add(nmLabel, nmLabelVal);
            add(hellLabel, hellLabelVal);

            table.ResumeLayout(false);
            table.PerformLayout();

            panelRuneDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelRuneDisplay.AutoSize = true;
            panelRuneDisplay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelRuneDisplay.MaximumSize = new Size(28, 0);
            panelRuneDisplay.MinimumSize = new Size(28, 28);

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode =AutoSizeMode.GrowAndShrink;
            BackColor = Color.Black;
            Controls.Add(this.outerLeftRightPanel);
            outerLeftRightPanel.ResumeLayout(false);
            outerLeftRightPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        void InitializeElements()
        {
            InfoLabels = new[]
            {
                playersXLabel,
                deathsLabel, deathsLabelVal,
                gameCounterLabel, gameCounterLabelVal,
                monsterGoldLabel, monsterGoldLabelVal,
                magicFindLabel, magicFindLabelVal,
                attackerSelfDamageLabel, attackerSelfDamageLabelVal,
                goldLabel, goldLabelVal,
                lvlLabel, lvlLabelVal,
                strLabel, labelStrVal,
                dexLabel, labelDexVal,
                vitLabel, labelVitVal,
                eneLabel, labelEneVal,
                frwLabel, labelFhrVal,
                fhrLabel, labelFrwVal,
                fcrLabel, labelFcrVal,
                iasLabel, labelIasVal,
                fireLabel, labelFireResVal,
                coldLabel, labelColdResVal,
                lighLabel, labelLightResVal,
                poisLabel, labelPoisonResVal,
                normLabel, normLabelVal,
                nmLabel, nmLabelVal,
                hellLabel, hellLabelVal,
                labelNormPerc, labelNmPerc, labelHellPerc,
            };

            ResistanceLabels = new[]
            {
                fireLabel, labelFireResVal,
                coldLabel, labelColdResVal,
                lighLabel, labelLightResVal,
                poisLabel, labelPoisonResVal,
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
                panelRuneDisplay
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
            // todo: Vertical padding..
            int padding = settings.VerticalLayoutPadding;
        }

        protected override void ApplyRuneSettings(ApplicationSettings settings)
        {
            if (!settings.DisplayRunes)
                panelRuneDisplay.Hide();
        }

        protected override void ApplyLabelSettings(ApplicationSettings settings)
        {
            base.ApplyLabelSettings(settings);

            var noMargin = new Padding(0);
            var topLeft = new Point(0, 0);
            foreach (Label label in InfoLabels)
            {
                label.Margin = noMargin;
                label.Location = topLeft;
            }

            foreach (Label label in ResistanceLabels)
            {
                label.Visible = settings.DisplayResistances;
            }

            foreach (Label label in BaseStatLabels)
            {
                label.Visible = settings.DisplayBaseStats;
            }

            foreach (Label label in AdvancedStatLabels)
            {
                label.Visible = settings.DisplayAdvancedStats;
            }

            foreach (Label label in DifficultyLabels)
            {
                label.Visible = settings.DisplayDifficultyPercentages;
            }

            UpdateLabelColors(settings);
        }
    }
}
