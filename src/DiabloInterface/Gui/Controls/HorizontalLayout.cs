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

    public class HorizontalLayout : AbstractLayout
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        FlowLayoutPanel activeRuneLayoutPanel;

        protected FlowLayoutPanel flowLayoutPanel1;
        protected override Panel RuneLayoutPanel => activeRuneLayoutPanel;

        new protected void InitializeComponent()
        {
            base.InitializeComponent();

            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();

            this.SuspendLayout();

            this.outerLeftRightPanel.AutoSize = true;
            this.outerLeftRightPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.outerLeftRightPanel.Controls.Add(this.panelRuneDisplayVertical);
            this.outerLeftRightPanel.Controls.Add(this.flowLayoutPanel1);
            this.outerLeftRightPanel.Dock = DockStyle.Fill;
            this.outerLeftRightPanel.Location = new Point(0, 0);
            this.outerLeftRightPanel.Margin = new Padding(0);
            this.outerLeftRightPanel.Padding = new Padding(3, 5, 3, 5);

            this.panelRuneDisplayVertical.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRuneDisplayVertical.AutoSize = true;
            this.panelRuneDisplayVertical.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panelRuneDisplayVertical.Location = new Point(6, 5);
            this.panelRuneDisplayVertical.Margin = new Padding(3, 0, 0, 0);
            this.panelRuneDisplayVertical.MaximumSize = new Size(28, 0);
            this.panelRuneDisplayVertical.MinimumSize = new Size(28, 28);

            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.nameLabel);
            this.flowLayoutPanel1.Controls.Add(this.playersXLabel);
            this.flowLayoutPanel1.Controls.Add(this.gameCounterLabel);
            this.flowLayoutPanel1.Controls.Add(this.panelDeathsLvl);
            this.flowLayoutPanel1.Controls.Add(this.attackerSelfDamageLabel);
            this.flowLayoutPanel1.Controls.Add(this.magicFindLabel);
            this.flowLayoutPanel1.Controls.Add(this.monsterGoldLabel);
            this.flowLayoutPanel1.Controls.Add(this.panelSimpleStats);
            this.flowLayoutPanel1.Controls.Add(this.panelStats);
            this.flowLayoutPanel1.Controls.Add(this.panelDiffPercentages2);
            this.flowLayoutPanel1.Controls.Add(this.panelRuneDisplayHorizontal);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(34, 5);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);

            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.nameLabel.Text = "Name";

            this.gameCounterLabel.AutoSize = true;
            this.gameCounterLabel.Location = new System.Drawing.Point(3, 0);
            this.gameCounterLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.gameCounterLabel.Text = "Run count: 1";

            this.magicFindLabel.AutoSize = true;
            this.magicFindLabel.Location = new System.Drawing.Point(3, 0);
            this.magicFindLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.magicFindLabel.Text = "MF: -";

            this.monsterGoldLabel.AutoSize = true;
            this.monsterGoldLabel.Location = new System.Drawing.Point(3, 0);
            this.monsterGoldLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.monsterGoldLabel.Text = "EMG: -";

            this.attackerSelfDamageLabel.AutoSize = true;
            this.attackerSelfDamageLabel.Location = new System.Drawing.Point(3, 0);
            this.attackerSelfDamageLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.attackerSelfDamageLabel.Text = "ATD: -";

            this.playersXLabel.AutoSize = true;
            this.playersXLabel.Location = new System.Drawing.Point(3, 0);
            this.playersXLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.playersXLabel.Text = "/players X";

            this.panelDeathsLvl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelDeathsLvl.AutoSize = true;
            this.panelDeathsLvl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDeathsLvl.ColumnCount = 2;
            this.panelDeathsLvl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.panelDeathsLvl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.panelDeathsLvl.Controls.Add(this.deathsLabel, 0, 0);
            this.panelDeathsLvl.Controls.Add(this.lvlLabel, 1, 0);
            this.panelDeathsLvl.Location = new System.Drawing.Point(0, 30);
            this.panelDeathsLvl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelDeathsLvl.RowCount = 1;
            this.panelDeathsLvl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.panelDeathsLvl.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));

            this.deathsLabel.AutoSize = true;
            this.deathsLabel.Location = new System.Drawing.Point(3, 0);
            this.deathsLabel.Text = "DEATHS: -";

            this.lvlLabel.AutoSize = true;
            this.lvlLabel.Location = new System.Drawing.Point(189, 0);
            this.lvlLabel.Text = "LVL: -";

            this.panelSimpleStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelSimpleStats.AutoSize = true;
            this.panelSimpleStats.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSimpleStats.ColumnCount = 1;
            this.panelSimpleStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            this.panelSimpleStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.panelSimpleStats.Controls.Add(this.goldLabel, 0, 0);
            this.panelSimpleStats.Location = new System.Drawing.Point(0, 49);
            this.panelSimpleStats.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelSimpleStats.RowCount = 1;
            this.panelSimpleStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            this.goldLabel.AutoSize = true;
            this.goldLabel.Location = new System.Drawing.Point(3, 0);
            this.goldLabel.Text = "GOLD: -";

            this.panelStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelStats.AutoSize = true;
            this.panelStats.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelStats.Controls.Add(this.panelBaseStats);
            this.panelStats.Controls.Add(this.panelAdvancedStats);
            this.panelStats.Controls.Add(this.panelResistances);
            this.panelStats.Controls.Add(this.panelDiffPercentages);
            this.panelStats.Location = new System.Drawing.Point(3, 68);
            this.panelStats.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);

            this.panelBaseStats.ColumnCount = 2;
            this.panelBaseStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelBaseStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelBaseStats.Controls.Add(this.vitLabel, 0, 2);
            this.panelBaseStats.Controls.Add(this.dexLabel, 0, 1);
            this.panelBaseStats.Controls.Add(this.strLabel, 0, 0);
            this.panelBaseStats.Controls.Add(this.eneLabel, 0, 3);
            this.panelBaseStats.Controls.Add(this.labelStrVal, 1, 0);
            this.panelBaseStats.Controls.Add(this.labelDexVal, 1, 1);
            this.panelBaseStats.Controls.Add(this.labelVitVal, 1, 2);
            this.panelBaseStats.Controls.Add(this.labelEneVal, 1, 3);
            this.panelBaseStats.Location = new System.Drawing.Point(0, 0);
            this.panelBaseStats.Margin = new System.Windows.Forms.Padding(0);
            this.panelBaseStats.RowCount = 4;
            this.panelBaseStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelBaseStats.Size = new System.Drawing.Size(85, 72);

            this.vitLabel.AutoSize = true;
            this.vitLabel.Location = new System.Drawing.Point(0, 36);
            this.vitLabel.Margin = new System.Windows.Forms.Padding(0);
            this.vitLabel.Text = "VIT:";
            this.vitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.dexLabel.AutoSize = true;
            this.dexLabel.Location = new System.Drawing.Point(0, 18);
            this.dexLabel.Margin = new System.Windows.Forms.Padding(0);
            this.dexLabel.Text = "DEX:";
            this.dexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.strLabel.AutoSize = true;
            this.strLabel.Location = new System.Drawing.Point(0, 0);
            this.strLabel.Margin = new System.Windows.Forms.Padding(0);
            this.strLabel.Text = "STR:";
            this.strLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.eneLabel.AutoSize = true;
            this.eneLabel.Location = new System.Drawing.Point(0, 54);
            this.eneLabel.Margin = new System.Windows.Forms.Padding(0);
            this.eneLabel.Text = "ENE:";
            this.eneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.labelStrVal.AutoSize = true;
            this.labelStrVal.Location = new System.Drawing.Point(40, 0);
            this.labelStrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelStrVal.Text = "-";
            this.labelStrVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelDexVal.AutoSize = true;
            this.labelDexVal.Location = new System.Drawing.Point(40, 18);
            this.labelDexVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelDexVal.Text = "-";
            this.labelDexVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelVitVal.AutoSize = true;
            this.labelVitVal.Location = new System.Drawing.Point(40, 36);
            this.labelVitVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelVitVal.Text = "-";
            this.labelVitVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelEneVal.AutoSize = true;
            this.labelEneVal.Location = new System.Drawing.Point(40, 54);
            this.labelEneVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelEneVal.Text = "-";
            this.labelEneVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.panelAdvancedStats.ColumnCount = 2;
            this.panelAdvancedStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelAdvancedStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelAdvancedStats.Controls.Add(this.iasLabel, 0, 3);
            this.panelAdvancedStats.Controls.Add(this.frwLabel, 0, 0);
            this.panelAdvancedStats.Controls.Add(this.fcrLabel, 0, 2);
            this.panelAdvancedStats.Controls.Add(this.fhrLabel, 0, 1);
            this.panelAdvancedStats.Controls.Add(this.labelFrwVal, 1, 0);
            this.panelAdvancedStats.Controls.Add(this.labelFhrVal, 1, 1);
            this.panelAdvancedStats.Controls.Add(this.labelFcrVal, 1, 2);
            this.panelAdvancedStats.Controls.Add(this.labelIasVal, 1, 3);
            this.panelAdvancedStats.Location = new Point(85, 0);
            this.panelAdvancedStats.Margin = new Padding(0);
            this.panelAdvancedStats.RowCount = 4;
            this.panelAdvancedStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelAdvancedStats.Size = new Size(85, 72);

            this.iasLabel.AutoSize = true;
            this.iasLabel.Location = new Point(0, 54);
            this.iasLabel.Margin = new Padding(0);
            this.iasLabel.Text = "IAS:";

            this.frwLabel.AutoSize = true;
            this.frwLabel.Location = new System.Drawing.Point(0, 0);
            this.frwLabel.Margin = new System.Windows.Forms.Padding(0);
            this.frwLabel.Text = "FRW:";

            this.fcrLabel.AutoSize = true;
            this.fcrLabel.Location = new System.Drawing.Point(0, 36);
            this.fcrLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fcrLabel.Text = "FCR:";

            this.fhrLabel.AutoSize = true;
            this.fhrLabel.Location = new System.Drawing.Point(0, 18);
            this.fhrLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fhrLabel.Text = "FHR:";

            this.labelFrwVal.AutoSize = true;
            this.labelFrwVal.Location = new System.Drawing.Point(40, 0);
            this.labelFrwVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFrwVal.Text = "-";
            this.labelFrwVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelFhrVal.AutoSize = true;
            this.labelFhrVal.Location = new System.Drawing.Point(40, 18);
            this.labelFhrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFhrVal.Text = "-";
            this.labelFhrVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelFcrVal.AutoSize = true;
            this.labelFcrVal.Location = new System.Drawing.Point(40, 36);
            this.labelFcrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFcrVal.Text = "-";
            this.labelFcrVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelIasVal.AutoSize = true;
            this.labelIasVal.Location = new System.Drawing.Point(40, 54);
            this.labelIasVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelIasVal.Text = "-";
            this.labelIasVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.panelResistances.ColumnCount = 2;
            this.panelResistances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelResistances.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelResistances.Controls.Add(this.coldLabel, 0, 1);
            this.panelResistances.Controls.Add(this.lighLabel, 0, 2);
            this.panelResistances.Controls.Add(this.poisLabel, 0, 3);
            this.panelResistances.Controls.Add(this.fireLabel, 0, 0);
            this.panelResistances.Controls.Add(this.labelFireResVal, 1, 0);
            this.panelResistances.Controls.Add(this.labelLightResVal, 1, 2);
            this.panelResistances.Controls.Add(this.labelPoisonResVal, 1, 3);
            this.panelResistances.Controls.Add(this.labelColdResVal, 1, 1);
            this.panelResistances.Location = new System.Drawing.Point(170, 0);
            this.panelResistances.Margin = new System.Windows.Forms.Padding(0);
            this.panelResistances.RowCount = 4;
            this.panelResistances.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelResistances.Size = new System.Drawing.Size(100, 72);

            this.coldLabel.AutoSize = true;
            this.coldLabel.Location = new System.Drawing.Point(0, 18);
            this.coldLabel.Margin = new System.Windows.Forms.Padding(0);
            this.coldLabel.Text = "COLD:";

            this.lighLabel.AutoSize = true;
            this.lighLabel.Location = new System.Drawing.Point(0, 36);
            this.lighLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lighLabel.Text = "LIGH:";

            this.poisLabel.AutoSize = true;
            this.poisLabel.Location = new System.Drawing.Point(0, 54);
            this.poisLabel.Margin = new System.Windows.Forms.Padding(0);
            this.poisLabel.Text = "POIS:";

            this.fireLabel.AutoSize = true;
            this.fireLabel.Location = new System.Drawing.Point(0, 0);
            this.fireLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fireLabel.Text = "FIRE:";

            this.labelFireResVal.AutoSize = true;
            this.labelFireResVal.Location = new System.Drawing.Point(48, 0);
            this.labelFireResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFireResVal.Text = "-";
            this.labelFireResVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelLightResVal.AutoSize = true;
            this.labelLightResVal.Location = new System.Drawing.Point(48, 36);
            this.labelLightResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightResVal.Text = "-";
            this.labelLightResVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelPoisonResVal.AutoSize = true;
            this.labelPoisonResVal.Location = new System.Drawing.Point(48, 54);
            this.labelPoisonResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelPoisonResVal.Text = "-";
            this.labelPoisonResVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.labelColdResVal.AutoSize = true;
            this.labelColdResVal.Location = new System.Drawing.Point(48, 18);
            this.labelColdResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelColdResVal.Text = "-";
            this.labelColdResVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.panelDiffPercentages.ColumnCount = 2;
            this.panelDiffPercentages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelDiffPercentages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelDiffPercentages.Controls.Add(this.normLabel, 0, 0);
            this.panelDiffPercentages.Controls.Add(this.nmLabel, 0, 1);
            this.panelDiffPercentages.Controls.Add(this.hellLabel, 0, 2);
            this.panelDiffPercentages.Controls.Add(this.normLabelVal, 1, 0);
            this.panelDiffPercentages.Controls.Add(this.nmLabelVal, 1, 1);
            this.panelDiffPercentages.Controls.Add(this.hellLabelVal, 1, 2);
            this.panelDiffPercentages.Location = new System.Drawing.Point(270, 0);
            this.panelDiffPercentages.Margin = new System.Windows.Forms.Padding(0);
            this.panelDiffPercentages.RowCount = 4;
            this.panelDiffPercentages.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.panelDiffPercentages.Size = new System.Drawing.Size(97, 72);

            this.normLabel.AutoSize = true;
            this.normLabel.Location = new System.Drawing.Point(0, 0);
            this.normLabel.Margin = new System.Windows.Forms.Padding(0);
            this.normLabel.Text = "NORM:";

            this.nmLabel.AutoSize = true;
            this.nmLabel.Location = new System.Drawing.Point(0, 18);
            this.nmLabel.Margin = new System.Windows.Forms.Padding(0);
            this.nmLabel.Text = "NM:";

            this.hellLabel.AutoSize = true;
            this.hellLabel.Location = new System.Drawing.Point(0, 36);
            this.hellLabel.Margin = new System.Windows.Forms.Padding(0);
            this.hellLabel.Text = "HELL:";

            this.normLabelVal.AutoSize = true;
            this.normLabelVal.Location = new System.Drawing.Point(48, 0);
            this.normLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.normLabelVal.Text = "-";
            this.normLabelVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.nmLabelVal.AutoSize = true;
            this.nmLabelVal.Location = new System.Drawing.Point(48, 18);
            this.nmLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.nmLabelVal.Text = "-";
            this.nmLabelVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.hellLabelVal.AutoSize = true;
            this.hellLabelVal.Location = new System.Drawing.Point(48, 36);
            this.hellLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.hellLabelVal.Text = "-";
            this.hellLabelVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.panelDiffPercentages2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelDiffPercentages2.AutoSize = true;
            this.panelDiffPercentages2.Controls.Add(this.labelNormPerc);
            this.panelDiffPercentages2.Controls.Add(this.labelNmPerc);
            this.panelDiffPercentages2.Controls.Add(this.labelHellPerc);
            this.panelDiffPercentages2.Location = new System.Drawing.Point(3, 146);

            this.labelNormPerc.AutoSize = true;
            this.labelNormPerc.Location = new System.Drawing.Point(0, 0);
            this.labelNormPerc.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormPerc.Text = "NO: -";

            this.labelNmPerc.AutoSize = true;
            this.labelNmPerc.Location = new System.Drawing.Point(53, 0);
            this.labelNmPerc.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelNmPerc.Text = "NM: -";

            this.labelHellPerc.AutoSize = true;
            this.labelHellPerc.Location = new System.Drawing.Point(106, 0);
            this.labelHellPerc.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelHellPerc.Text = "HE: -";

            this.panelRuneDisplayHorizontal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRuneDisplayHorizontal.AutoSize = true;
            this.panelRuneDisplayHorizontal.Location = new System.Drawing.Point(3, 168);
            this.panelRuneDisplayHorizontal.MinimumSize = new System.Drawing.Size(200, 28);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.outerLeftRightPanel);
            this.Name = "HorizontalLayout";
            this.outerLeftRightPanel.ResumeLayout(false);
            this.outerLeftRightPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelDeathsLvl.ResumeLayout(false);
            this.panelDeathsLvl.PerformLayout();
            this.panelSimpleStats.ResumeLayout(false);
            this.panelSimpleStats.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelBaseStats.ResumeLayout(false);
            this.panelBaseStats.PerformLayout();
            this.panelAdvancedStats.ResumeLayout(false);
            this.panelAdvancedStats.PerformLayout();
            this.panelResistances.ResumeLayout(false);
            this.panelResistances.PerformLayout();
            this.panelDiffPercentages.ResumeLayout(false);
            this.panelDiffPercentages.PerformLayout();
            this.panelDiffPercentages2.ResumeLayout(false);
            this.panelDiffPercentages2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public HorizontalLayout(ISettingsService settingsService, IGameService gameService)
            : base(settingsService, gameService)
        {
            Logger.Info("Creating horizontal layout.");

            InitializeComponent();
            InitializeElements();

            panelRuneDisplayHorizontal.Hide();
            panelRuneDisplayVertical.Hide();
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

            FireLabels = new[] { fireLabel, labelFireResVal };
            ColdLabels = new[] { coldLabel, labelColdResVal };
            LighLabels = new[] { lighLabel, labelLightResVal };
            PoisLabels = new[] { poisLabel, labelPoisonResVal };

            BaseStatLabels = new[] {
                strLabel, labelStrVal,
                vitLabel, labelVitVal,
                dexLabel, labelDexVal,
                eneLabel, labelEneVal
            };

            AdvancedStatLabels = new [] {
                fcrLabel, labelFcrVal,
                fhrLabel, labelFhrVal,
                iasLabel, labelIasVal,
                frwLabel, labelFrwVal
            };

            DifficultyLabels = new [] {
                normLabel, normLabelVal,
                nmLabel, nmLabelVal,
                hellLabel, hellLabelVal,
                labelNormPerc,
                labelNmPerc,
                labelHellPerc
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

            UpdateLabelColors(settings);
        }
    }
}
