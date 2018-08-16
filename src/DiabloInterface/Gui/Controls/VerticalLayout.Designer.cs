namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    partial class VerticalLayout
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        new protected void InitializeComponent()
        {
            base.InitializeComponent();

            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRuneDisplay2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();

            this.SuspendLayout();
            // 
            // outerLeftRightPanel
            // 
            this.outerLeftRightPanel.AutoSize = true;
            this.outerLeftRightPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.outerLeftRightPanel.Controls.Add(this.panelRuneDisplay2);
            this.outerLeftRightPanel.Controls.Add(this.flowLayoutPanel2);
            this.outerLeftRightPanel.Location = new System.Drawing.Point(0, 30);
            this.outerLeftRightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.outerLeftRightPanel.Name = "outerLeftRightPanel";
            this.outerLeftRightPanel.Size = new System.Drawing.Size(137, 326);
            this.outerLeftRightPanel.TabIndex = 22;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.deathsLabel);
            this.flowLayoutPanel2.Controls.Add(this.lvlLabel);
            this.flowLayoutPanel2.Controls.Add(this.goldLabel);
            this.flowLayoutPanel2.Controls.Add(this.panelBaseStats);
            this.flowLayoutPanel2.Controls.Add(this.panelAdvancedStats);
            this.flowLayoutPanel2.Controls.Add(this.panelResistances);
            this.flowLayoutPanel2.Controls.Add(this.panelDiffPercentages);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(34, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(100, 320);
            this.flowLayoutPanel2.TabIndex = 21;
            // 
            // deathsLabel
            // 
            this.deathsLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deathsLabel.ForeColor = System.Drawing.Color.Snow;
            this.deathsLabel.Location = new System.Drawing.Point(0, 0);
            this.deathsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.deathsLabel.Name = "deathsLabel";
            this.deathsLabel.Size = new System.Drawing.Size(80, 16);
            this.deathsLabel.TabIndex = 17;
            this.deathsLabel.Text = "DEATHS: -";
            // 
            // lvlLabel
            // 
            this.lvlLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lvlLabel.ForeColor = System.Drawing.Color.White;
            this.lvlLabel.Location = new System.Drawing.Point(0, 16);
            this.lvlLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lvlLabel.Name = "lvlLabel";
            this.lvlLabel.Size = new System.Drawing.Size(56, 16);
            this.lvlLabel.TabIndex = 2;
            this.lvlLabel.Text = "LVL: -";
            // 
            // goldLabel
            // 
            this.goldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.goldLabel.ForeColor = System.Drawing.Color.Gold;
            this.goldLabel.Location = new System.Drawing.Point(0, 32);
            this.goldLabel.Margin = new System.Windows.Forms.Padding(0);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(64, 18);
            this.goldLabel.TabIndex = 16;
            this.goldLabel.Text = "GOLD: -";
            // 
            // panelBaseStats
            // 
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
            this.panelBaseStats.Location = new System.Drawing.Point(0, 50);
            this.panelBaseStats.Margin = new System.Windows.Forms.Padding(0);
            this.panelBaseStats.Name = "panelBaseStats";
            this.panelBaseStats.RowCount = 4;
            this.panelBaseStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelBaseStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelBaseStats.Size = new System.Drawing.Size(85, 72);
            this.panelBaseStats.TabIndex = 28;
            // 
            // vitLabel
            // 
            this.vitLabel.AutoSize = true;
            this.vitLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vitLabel.ForeColor = System.Drawing.Color.Coral;
            this.vitLabel.Location = new System.Drawing.Point(0, 36);
            this.vitLabel.Margin = new System.Windows.Forms.Padding(0);
            this.vitLabel.Name = "vitLabel";
            this.vitLabel.Size = new System.Drawing.Size(40, 16);
            this.vitLabel.TabIndex = 10;
            this.vitLabel.Text = "VIT:";
            this.vitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dexLabel
            // 
            this.dexLabel.AutoSize = true;
            this.dexLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dexLabel.ForeColor = System.Drawing.Color.Coral;
            this.dexLabel.Location = new System.Drawing.Point(0, 18);
            this.dexLabel.Margin = new System.Windows.Forms.Padding(0);
            this.dexLabel.Name = "dexLabel";
            this.dexLabel.Size = new System.Drawing.Size(40, 16);
            this.dexLabel.TabIndex = 8;
            this.dexLabel.Text = "DEX:";
            this.dexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // strLabel
            // 
            this.strLabel.AutoSize = true;
            this.strLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.strLabel.ForeColor = System.Drawing.Color.Coral;
            this.strLabel.Location = new System.Drawing.Point(0, 0);
            this.strLabel.Margin = new System.Windows.Forms.Padding(0);
            this.strLabel.Name = "strLabel";
            this.strLabel.Size = new System.Drawing.Size(40, 16);
            this.strLabel.TabIndex = 7;
            this.strLabel.Text = "STR:";
            this.strLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // eneLabel
            // 
            this.eneLabel.AutoSize = true;
            this.eneLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.eneLabel.ForeColor = System.Drawing.Color.Coral;
            this.eneLabel.Location = new System.Drawing.Point(0, 54);
            this.eneLabel.Margin = new System.Windows.Forms.Padding(0);
            this.eneLabel.Name = "eneLabel";
            this.eneLabel.Size = new System.Drawing.Size(40, 16);
            this.eneLabel.TabIndex = 11;
            this.eneLabel.Text = "ENE:";
            this.eneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStrVal
            // 
            this.labelStrVal.AutoSize = true;
            this.labelStrVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelStrVal.ForeColor = System.Drawing.Color.Coral;
            this.labelStrVal.Location = new System.Drawing.Point(40, 0);
            this.labelStrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelStrVal.Name = "labelStrVal";
            this.labelStrVal.Size = new System.Drawing.Size(16, 16);
            this.labelStrVal.TabIndex = 12;
            this.labelStrVal.Text = "-";
            this.labelStrVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelDexVal
            // 
            this.labelDexVal.AutoSize = true;
            this.labelDexVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelDexVal.ForeColor = System.Drawing.Color.Coral;
            this.labelDexVal.Location = new System.Drawing.Point(40, 18);
            this.labelDexVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelDexVal.Name = "labelDexVal";
            this.labelDexVal.Size = new System.Drawing.Size(16, 16);
            this.labelDexVal.TabIndex = 12;
            this.labelDexVal.Text = "-";
            this.labelDexVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelVitVal
            // 
            this.labelVitVal.AutoSize = true;
            this.labelVitVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelVitVal.ForeColor = System.Drawing.Color.Coral;
            this.labelVitVal.Location = new System.Drawing.Point(40, 36);
            this.labelVitVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelVitVal.Name = "labelVitVal";
            this.labelVitVal.Size = new System.Drawing.Size(16, 16);
            this.labelVitVal.TabIndex = 12;
            this.labelVitVal.Text = "-";
            this.labelVitVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelEneVal
            // 
            this.labelEneVal.AutoSize = true;
            this.labelEneVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelEneVal.ForeColor = System.Drawing.Color.Coral;
            this.labelEneVal.Location = new System.Drawing.Point(40, 54);
            this.labelEneVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelEneVal.Name = "labelEneVal";
            this.labelEneVal.Size = new System.Drawing.Size(16, 16);
            this.labelEneVal.TabIndex = 12;
            this.labelEneVal.Text = "-";
            this.labelEneVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelAdvancedStats
            // 
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
            this.panelAdvancedStats.Location = new System.Drawing.Point(0, 122);
            this.panelAdvancedStats.Margin = new System.Windows.Forms.Padding(0);
            this.panelAdvancedStats.Name = "panelAdvancedStats";
            this.panelAdvancedStats.RowCount = 4;
            this.panelAdvancedStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelAdvancedStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelAdvancedStats.Size = new System.Drawing.Size(85, 72);
            this.panelAdvancedStats.TabIndex = 28;
            // 
            // iasLabel
            // 
            this.iasLabel.AutoSize = true;
            this.iasLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iasLabel.ForeColor = System.Drawing.Color.Coral;
            this.iasLabel.Location = new System.Drawing.Point(0, 54);
            this.iasLabel.Margin = new System.Windows.Forms.Padding(0);
            this.iasLabel.Name = "iasLabel";
            this.iasLabel.Size = new System.Drawing.Size(40, 16);
            this.iasLabel.TabIndex = 11;
            this.iasLabel.Text = "IAS:";
            // 
            // frwLabel
            // 
            this.frwLabel.AutoSize = true;
            this.frwLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.frwLabel.ForeColor = System.Drawing.Color.Coral;
            this.frwLabel.Location = new System.Drawing.Point(0, 0);
            this.frwLabel.Margin = new System.Windows.Forms.Padding(0);
            this.frwLabel.Name = "frwLabel";
            this.frwLabel.Size = new System.Drawing.Size(40, 16);
            this.frwLabel.TabIndex = 7;
            this.frwLabel.Text = "FRW:";
            // 
            // fcrLabel
            // 
            this.fcrLabel.AutoSize = true;
            this.fcrLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fcrLabel.ForeColor = System.Drawing.Color.Coral;
            this.fcrLabel.Location = new System.Drawing.Point(0, 36);
            this.fcrLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fcrLabel.Name = "fcrLabel";
            this.fcrLabel.Size = new System.Drawing.Size(40, 16);
            this.fcrLabel.TabIndex = 8;
            this.fcrLabel.Text = "FCR:";
            // 
            // fhrLabel
            // 
            this.fhrLabel.AutoSize = true;
            this.fhrLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fhrLabel.ForeColor = System.Drawing.Color.Coral;
            this.fhrLabel.Location = new System.Drawing.Point(0, 18);
            this.fhrLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fhrLabel.Name = "fhrLabel";
            this.fhrLabel.Size = new System.Drawing.Size(40, 16);
            this.fhrLabel.TabIndex = 10;
            this.fhrLabel.Text = "FHR:";
            // 
            // labelFrwVal
            // 
            this.labelFrwVal.AutoSize = true;
            this.labelFrwVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelFrwVal.ForeColor = System.Drawing.Color.Coral;
            this.labelFrwVal.Location = new System.Drawing.Point(40, 0);
            this.labelFrwVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFrwVal.Name = "labelFrwVal";
            this.labelFrwVal.Size = new System.Drawing.Size(16, 16);
            this.labelFrwVal.TabIndex = 12;
            this.labelFrwVal.Text = "-";
            this.labelFrwVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelFhrVal
            // 
            this.labelFhrVal.AutoSize = true;
            this.labelFhrVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelFhrVal.ForeColor = System.Drawing.Color.Coral;
            this.labelFhrVal.Location = new System.Drawing.Point(40, 18);
            this.labelFhrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFhrVal.Name = "labelFhrVal";
            this.labelFhrVal.Size = new System.Drawing.Size(16, 16);
            this.labelFhrVal.TabIndex = 12;
            this.labelFhrVal.Text = "-";
            this.labelFhrVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelFcrVal
            // 
            this.labelFcrVal.AutoSize = true;
            this.labelFcrVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelFcrVal.ForeColor = System.Drawing.Color.Coral;
            this.labelFcrVal.Location = new System.Drawing.Point(40, 36);
            this.labelFcrVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFcrVal.Name = "labelFcrVal";
            this.labelFcrVal.Size = new System.Drawing.Size(16, 16);
            this.labelFcrVal.TabIndex = 12;
            this.labelFcrVal.Text = "-";
            this.labelFcrVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIasVal
            // 
            this.labelIasVal.AutoSize = true;
            this.labelIasVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelIasVal.ForeColor = System.Drawing.Color.Coral;
            this.labelIasVal.Location = new System.Drawing.Point(40, 54);
            this.labelIasVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelIasVal.Name = "labelIasVal";
            this.labelIasVal.Size = new System.Drawing.Size(16, 16);
            this.labelIasVal.TabIndex = 12;
            this.labelIasVal.Text = "-";
            this.labelIasVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelResistances
            // 
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
            this.panelResistances.Location = new System.Drawing.Point(0, 194);
            this.panelResistances.Margin = new System.Windows.Forms.Padding(0);
            this.panelResistances.Name = "panelResistances";
            this.panelResistances.RowCount = 4;
            this.panelResistances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResistances.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResistances.Size = new System.Drawing.Size(100, 72);
            this.panelResistances.TabIndex = 28;
            // 
            // coldLabel
            // 
            this.coldLabel.AutoSize = true;
            this.coldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.coldLabel.ForeColor = System.Drawing.Color.DodgerBlue;
            this.coldLabel.Location = new System.Drawing.Point(0, 18);
            this.coldLabel.Margin = new System.Windows.Forms.Padding(0);
            this.coldLabel.Name = "coldLabel";
            this.coldLabel.Size = new System.Drawing.Size(48, 16);
            this.coldLabel.TabIndex = 13;
            this.coldLabel.Text = "COLD:";
            // 
            // lighLabel
            // 
            this.lighLabel.AutoSize = true;
            this.lighLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lighLabel.ForeColor = System.Drawing.Color.Yellow;
            this.lighLabel.Location = new System.Drawing.Point(0, 36);
            this.lighLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lighLabel.Name = "lighLabel";
            this.lighLabel.Size = new System.Drawing.Size(48, 16);
            this.lighLabel.TabIndex = 14;
            this.lighLabel.Text = "LIGH:";
            // 
            // poisLabel
            // 
            this.poisLabel.AutoSize = true;
            this.poisLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.poisLabel.ForeColor = System.Drawing.Color.YellowGreen;
            this.poisLabel.Location = new System.Drawing.Point(0, 54);
            this.poisLabel.Margin = new System.Windows.Forms.Padding(0);
            this.poisLabel.Name = "poisLabel";
            this.poisLabel.Size = new System.Drawing.Size(48, 16);
            this.poisLabel.TabIndex = 15;
            this.poisLabel.Text = "POIS:";
            // 
            // fireLabel
            // 
            this.fireLabel.AutoSize = true;
            this.fireLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fireLabel.ForeColor = System.Drawing.Color.Red;
            this.fireLabel.Location = new System.Drawing.Point(0, 0);
            this.fireLabel.Margin = new System.Windows.Forms.Padding(0);
            this.fireLabel.Name = "fireLabel";
            this.fireLabel.Size = new System.Drawing.Size(48, 16);
            this.fireLabel.TabIndex = 12;
            this.fireLabel.Text = "FIRE:";
            // 
            // labelFireResVal
            // 
            this.labelFireResVal.AutoSize = true;
            this.labelFireResVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelFireResVal.ForeColor = System.Drawing.Color.Red;
            this.labelFireResVal.Location = new System.Drawing.Point(48, 0);
            this.labelFireResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelFireResVal.Name = "labelFireResVal";
            this.labelFireResVal.Size = new System.Drawing.Size(16, 16);
            this.labelFireResVal.TabIndex = 12;
            this.labelFireResVal.Text = "-";
            this.labelFireResVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelLightResVal
            // 
            this.labelLightResVal.AutoSize = true;
            this.labelLightResVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelLightResVal.ForeColor = System.Drawing.Color.Yellow;
            this.labelLightResVal.Location = new System.Drawing.Point(48, 36);
            this.labelLightResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightResVal.Name = "labelLightResVal";
            this.labelLightResVal.Size = new System.Drawing.Size(16, 16);
            this.labelLightResVal.TabIndex = 12;
            this.labelLightResVal.Text = "-";
            this.labelLightResVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPoisonResVal
            // 
            this.labelPoisonResVal.AutoSize = true;
            this.labelPoisonResVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelPoisonResVal.ForeColor = System.Drawing.Color.YellowGreen;
            this.labelPoisonResVal.Location = new System.Drawing.Point(48, 54);
            this.labelPoisonResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelPoisonResVal.Name = "labelPoisonResVal";
            this.labelPoisonResVal.Size = new System.Drawing.Size(16, 16);
            this.labelPoisonResVal.TabIndex = 12;
            this.labelPoisonResVal.Text = "-";
            this.labelPoisonResVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelColdResVal
            // 
            this.labelColdResVal.AutoSize = true;
            this.labelColdResVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.labelColdResVal.ForeColor = System.Drawing.Color.DodgerBlue;
            this.labelColdResVal.Location = new System.Drawing.Point(48, 18);
            this.labelColdResVal.Margin = new System.Windows.Forms.Padding(0);
            this.labelColdResVal.Name = "labelColdResVal";
            this.labelColdResVal.Size = new System.Drawing.Size(16, 16);
            this.labelColdResVal.TabIndex = 12;
            this.labelColdResVal.Text = "-";
            this.labelColdResVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelDiffPercentages
            // 
            this.panelDiffPercentages.ColumnCount = 2;
            this.panelDiffPercentages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelDiffPercentages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelDiffPercentages.Controls.Add(this.normLabel, 0, 0);
            this.panelDiffPercentages.Controls.Add(this.nmLabel, 0, 1);
            this.panelDiffPercentages.Controls.Add(this.hellLabel, 0, 2);
            this.panelDiffPercentages.Controls.Add(this.normLabelVal, 1, 0);
            this.panelDiffPercentages.Controls.Add(this.nmLabelVal, 1, 1);
            this.panelDiffPercentages.Controls.Add(this.hellLabelVal, 1, 2);
            this.panelDiffPercentages.Location = new System.Drawing.Point(0, 266);
            this.panelDiffPercentages.Margin = new System.Windows.Forms.Padding(0);
            this.panelDiffPercentages.Name = "panelDiffPercentages";
            this.panelDiffPercentages.RowCount = 3;
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.Size = new System.Drawing.Size(97, 54);
            this.panelDiffPercentages.TabIndex = 29;
            // 
            // normLabel
            // 
            this.normLabel.AutoSize = true;
            this.normLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.normLabel.ForeColor = System.Drawing.Color.White;
            this.normLabel.Location = new System.Drawing.Point(0, 0);
            this.normLabel.Margin = new System.Windows.Forms.Padding(0);
            this.normLabel.Name = "normLabel";
            this.normLabel.Size = new System.Drawing.Size(48, 16);
            this.normLabel.TabIndex = 2;
            this.normLabel.Text = "NORM:";
            // 
            // nmLabel
            // 
            this.nmLabel.AutoSize = true;
            this.nmLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nmLabel.ForeColor = System.Drawing.Color.White;
            this.nmLabel.Location = new System.Drawing.Point(0, 18);
            this.nmLabel.Margin = new System.Windows.Forms.Padding(0);
            this.nmLabel.Name = "nmLabel";
            this.nmLabel.Size = new System.Drawing.Size(32, 16);
            this.nmLabel.TabIndex = 2;
            this.nmLabel.Text = "NM:";
            // 
            // hellLabel
            // 
            this.hellLabel.AutoSize = true;
            this.hellLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hellLabel.ForeColor = System.Drawing.Color.White;
            this.hellLabel.Location = new System.Drawing.Point(0, 36);
            this.hellLabel.Margin = new System.Windows.Forms.Padding(0);
            this.hellLabel.Name = "hellLabel";
            this.hellLabel.Size = new System.Drawing.Size(48, 16);
            this.hellLabel.TabIndex = 2;
            this.hellLabel.Text = "HELL:";
            // 
            // normLabelVal
            // 
            this.normLabelVal.AutoSize = true;
            this.normLabelVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.normLabelVal.ForeColor = System.Drawing.Color.White;
            this.normLabelVal.Location = new System.Drawing.Point(48, 0);
            this.normLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.normLabelVal.Name = "normLabelVal";
            this.normLabelVal.Size = new System.Drawing.Size(16, 16);
            this.normLabelVal.TabIndex = 12;
            this.normLabelVal.Text = "-";
            this.normLabelVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nmLabelVal
            // 
            this.nmLabelVal.AutoSize = true;
            this.nmLabelVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.nmLabelVal.ForeColor = System.Drawing.Color.White;
            this.nmLabelVal.Location = new System.Drawing.Point(48, 18);
            this.nmLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.nmLabelVal.Name = "nmLabelVal";
            this.nmLabelVal.Size = new System.Drawing.Size(16, 16);
            this.nmLabelVal.TabIndex = 12;
            this.nmLabelVal.Text = "-";
            this.nmLabelVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // hellLabelVal
            // 
            this.hellLabelVal.AutoSize = true;
            this.hellLabelVal.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.hellLabelVal.ForeColor = System.Drawing.Color.White;
            this.hellLabelVal.Location = new System.Drawing.Point(48, 36);
            this.hellLabelVal.Margin = new System.Windows.Forms.Padding(0);
            this.hellLabelVal.Name = "hellLabelVal";
            this.hellLabelVal.Size = new System.Drawing.Size(16, 16);
            this.hellLabelVal.TabIndex = 12;
            this.hellLabelVal.Text = "-";
            this.hellLabelVal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(68, 27);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Name";
            // 
            // playersXLabel
            // 
            this.playersXLabel.AutoSize = true;
            this.playersXLabel.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.playersXLabel.ForeColor = System.Drawing.Color.White;
            this.playersXLabel.Location = new System.Drawing.Point(3, 0);
            this.playersXLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.playersXLabel.Name = "playersXLabel";
            this.playersXLabel.Size = new System.Drawing.Size(68, 27);
            this.playersXLabel.TabIndex = 6;
            this.playersXLabel.Text = "/players X";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.nameLabel);
            this.flowLayoutPanel1.Controls.Add(this.playersXLabel);
            this.flowLayoutPanel1.Controls.Add(this.outerLeftRightPanel);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(137, 356);
            this.flowLayoutPanel1.TabIndex = 23;
            // 
            // panelRuneDisplay2
            // 
            this.panelRuneDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRuneDisplay2.AutoSize = true;
            this.panelRuneDisplay2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRuneDisplay2.Location = new System.Drawing.Point(3, 0);
            this.panelRuneDisplay2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelRuneDisplay2.MaximumSize = new System.Drawing.Size(28, 0);
            this.panelRuneDisplay2.MinimumSize = new System.Drawing.Size(28, 28);
            this.panelRuneDisplay2.Name = "panelRuneDisplay2";
            this.panelRuneDisplay2.Size = new System.Drawing.Size(28, 28);
            this.panelRuneDisplay2.TabIndex = 22;
            // 
            // VerticalLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "VerticalLayout";
            this.Size = new System.Drawing.Size(137, 356);
            this.outerLeftRightPanel.ResumeLayout(false);
            this.outerLeftRightPanel.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panelBaseStats.ResumeLayout(false);
            this.panelBaseStats.PerformLayout();
            this.panelAdvancedStats.ResumeLayout(false);
            this.panelAdvancedStats.PerformLayout();
            this.panelResistances.ResumeLayout(false);
            this.panelResistances.PerformLayout();
            this.panelDiffPercentages.ResumeLayout(false);
            this.panelDiffPercentages.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel panelRuneDisplay2;
    }
}
