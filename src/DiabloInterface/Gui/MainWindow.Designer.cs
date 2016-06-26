namespace DiabloInterface.Gui
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.lvlLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.strLabel = new System.Windows.Forms.Label();
            this.dexLabel = new System.Windows.Forms.Label();
            this.vitLabel = new System.Windows.Forms.Label();
            this.eneLabel = new System.Windows.Forms.Label();
            this.fireLabel = new System.Windows.Forms.Label();
            this.coldLabel = new System.Windows.Forms.Label();
            this.lighLabel = new System.Windows.Forms.Label();
            this.poisLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.deathsLabel = new System.Windows.Forms.Label();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frwLabel = new System.Windows.Forms.Label();
            this.fcrLabel = new System.Windows.Forms.Label();
            this.fhrLabel = new System.Windows.Forms.Label();
            this.iasLabel = new System.Windows.Forms.Label();
            this.panelRuneDisplay = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelDeathsLvl = new System.Windows.Forms.TableLayoutPanel();
            this.panelSimpleStats = new System.Windows.Forms.TableLayoutPanel();
            this.panelStats = new System.Windows.Forms.FlowLayoutPanel();
            this.panelBaseStats = new System.Windows.Forms.TableLayoutPanel();
            this.labelStrVal = new System.Windows.Forms.Label();
            this.labelDexVal = new System.Windows.Forms.Label();
            this.labelVitVal = new System.Windows.Forms.Label();
            this.labelEneVal = new System.Windows.Forms.Label();
            this.panelAdvancedStats = new System.Windows.Forms.TableLayoutPanel();
            this.labelFrwVal = new System.Windows.Forms.Label();
            this.labelFhrVal = new System.Windows.Forms.Label();
            this.labelFcrVal = new System.Windows.Forms.Label();
            this.labelIasVal = new System.Windows.Forms.Label();
            this.panelResistances = new System.Windows.Forms.TableLayoutPanel();
            this.labelFireResVal = new System.Windows.Forms.Label();
            this.labelColdResVal = new System.Windows.Forms.Label();
            this.labelLightResVal = new System.Windows.Forms.Label();
            this.labelPoisonResVal = new System.Windows.Forms.Label();
            this.labelNormPerc = new System.Windows.Forms.Label();
            this.labelNmPerc = new System.Windows.Forms.Label();
            this.labelHellPerc = new System.Windows.Forms.Label();
            this.panelDiffPercentages2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelDiffPercentages = new System.Windows.Forms.TableLayoutPanel();
            this.normLabel = new System.Windows.Forms.Label();
            this.nmLabel = new System.Windows.Forms.Label();
            this.hellLabel = new System.Windows.Forms.Label();
            this.normLabelVal = new System.Windows.Forms.Label();
            this.nmLabelVal = new System.Windows.Forms.Label();
            this.hellLabelVal = new System.Windows.Forms.Label();
            this.outerLeftRightPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRuneDisplay2 = new System.Windows.Forms.Panel();
            this.contextMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelDeathsLvl.SuspendLayout();
            this.panelSimpleStats.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelBaseStats.SuspendLayout();
            this.panelAdvancedStats.SuspendLayout();
            this.panelResistances.SuspendLayout();
            this.panelDiffPercentages2.SuspendLayout();
            this.panelDiffPercentages.SuspendLayout();
            this.outerLeftRightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvlLabel
            // 
            this.lvlLabel.AutoSize = true;
            this.lvlLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lvlLabel.ForeColor = System.Drawing.Color.White;
            this.lvlLabel.Location = new System.Drawing.Point(186, 0);
            this.lvlLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lvlLabel.Name = "lvlLabel";
            this.lvlLabel.Size = new System.Drawing.Size(56, 16);
            this.lvlLabel.TabIndex = 2;
            this.lvlLabel.Text = "LVL: -";
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
            // goldLabel
            // 
            this.goldLabel.AutoSize = true;
            this.goldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.goldLabel.ForeColor = System.Drawing.Color.Gold;
            this.goldLabel.Location = new System.Drawing.Point(3, 0);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(64, 16);
            this.goldLabel.TabIndex = 16;
            this.goldLabel.Text = "GOLD: -";
            // 
            // deathsLabel
            // 
            this.deathsLabel.AutoSize = true;
            this.deathsLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deathsLabel.ForeColor = System.Drawing.Color.Snow;
            this.deathsLabel.Location = new System.Drawing.Point(3, 0);
            this.deathsLabel.Name = "deathsLabel";
            this.deathsLabel.Size = new System.Drawing.Size(80, 16);
            this.deathsLabel.TabIndex = 17;
            this.deathsLabel.Text = "DEATHS: -";
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.debugMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(161, 92);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.wrench_orange;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.arrow_refresh;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetMenuItem_Click);
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("debugMenuItem.Image")));
            this.debugMenuItem.Name = "debugMenuItem";
            this.debugMenuItem.Size = new System.Drawing.Size(160, 22);
            this.debugMenuItem.Text = "Debug";
            this.debugMenuItem.Click += new System.EventHandler(this.debugMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.cross;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
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
            // panelRuneDisplay
            // 
            this.panelRuneDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRuneDisplay.AutoSize = true;
            this.panelRuneDisplay.Location = new System.Drawing.Point(3, 168);
            this.panelRuneDisplay.MinimumSize = new System.Drawing.Size(200, 28);
            this.panelRuneDisplay.Name = "panelRuneDisplay";
            this.panelRuneDisplay.Size = new System.Drawing.Size(367, 28);
            this.panelRuneDisplay.TabIndex = 18;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.nameLabel);
            this.flowLayoutPanel1.Controls.Add(this.panelDeathsLvl);
            this.flowLayoutPanel1.Controls.Add(this.panelSimpleStats);
            this.flowLayoutPanel1.Controls.Add(this.panelStats);
            this.flowLayoutPanel1.Controls.Add(this.panelDiffPercentages2);
            this.flowLayoutPanel1.Controls.Add(this.panelRuneDisplay);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(34, 5);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(373, 199);
            this.flowLayoutPanel1.TabIndex = 20;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // panelDeathsLvl
            // 
            this.panelDeathsLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDeathsLvl.AutoSize = true;
            this.panelDeathsLvl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDeathsLvl.ColumnCount = 2;
            this.panelDeathsLvl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelDeathsLvl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelDeathsLvl.Controls.Add(this.deathsLabel, 0, 0);
            this.panelDeathsLvl.Controls.Add(this.lvlLabel, 1, 0);
            this.panelDeathsLvl.Location = new System.Drawing.Point(0, 30);
            this.panelDeathsLvl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelDeathsLvl.Name = "panelDeathsLvl";
            this.panelDeathsLvl.RowCount = 1;
            this.panelDeathsLvl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelDeathsLvl.Size = new System.Drawing.Size(373, 16);
            this.panelDeathsLvl.TabIndex = 29;
            // 
            // panelSimpleStats
            // 
            this.panelSimpleStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSimpleStats.AutoSize = true;
            this.panelSimpleStats.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSimpleStats.ColumnCount = 1;
            this.panelSimpleStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.panelSimpleStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.panelSimpleStats.Controls.Add(this.goldLabel, 0, 0);
            this.panelSimpleStats.Location = new System.Drawing.Point(0, 49);
            this.panelSimpleStats.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelSimpleStats.Name = "panelSimpleStats";
            this.panelSimpleStats.RowCount = 1;
            this.panelSimpleStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSimpleStats.Size = new System.Drawing.Size(373, 16);
            this.panelSimpleStats.TabIndex = 28;
            // 
            // panelStats
            // 
            this.panelStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStats.AutoSize = true;
            this.panelStats.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelStats.Controls.Add(this.panelBaseStats);
            this.panelStats.Controls.Add(this.panelAdvancedStats);
            this.panelStats.Controls.Add(this.panelResistances);
            this.panelStats.Controls.Add(this.panelDiffPercentages);
            this.panelStats.Location = new System.Drawing.Point(3, 68);
            this.panelStats.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(367, 72);
            this.panelStats.TabIndex = 27;
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
            this.panelBaseStats.Location = new System.Drawing.Point(0, 0);
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
            this.panelAdvancedStats.Location = new System.Drawing.Point(85, 0);
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
            this.panelResistances.Location = new System.Drawing.Point(170, 0);
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
            // labelNormPerc
            // 
            this.labelNormPerc.AutoSize = true;
            this.labelNormPerc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelNormPerc.ForeColor = System.Drawing.Color.White;
            this.labelNormPerc.Location = new System.Drawing.Point(0, 0);
            this.labelNormPerc.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormPerc.Name = "labelNormPerc";
            this.labelNormPerc.Size = new System.Drawing.Size(48, 16);
            this.labelNormPerc.TabIndex = 2;
            this.labelNormPerc.Text = "NO: -";
            // 
            // labelNmPerc
            // 
            this.labelNmPerc.AutoSize = true;
            this.labelNmPerc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelNmPerc.ForeColor = System.Drawing.Color.White;
            this.labelNmPerc.Location = new System.Drawing.Point(53, 0);
            this.labelNmPerc.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelNmPerc.Name = "labelNmPerc";
            this.labelNmPerc.Size = new System.Drawing.Size(48, 16);
            this.labelNmPerc.TabIndex = 2;
            this.labelNmPerc.Text = "NM: -";
            // 
            // labelHellPerc
            // 
            this.labelHellPerc.AutoSize = true;
            this.labelHellPerc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelHellPerc.ForeColor = System.Drawing.Color.White;
            this.labelHellPerc.Location = new System.Drawing.Point(106, 0);
            this.labelHellPerc.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelHellPerc.Name = "labelHellPerc";
            this.labelHellPerc.Size = new System.Drawing.Size(48, 16);
            this.labelHellPerc.TabIndex = 2;
            this.labelHellPerc.Text = "HE: -";
            // 
            // panelDiffPercentages2
            // 
            this.panelDiffPercentages2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDiffPercentages2.AutoSize = true;
            this.panelDiffPercentages2.Controls.Add(this.labelNormPerc);
            this.panelDiffPercentages2.Controls.Add(this.labelNmPerc);
            this.panelDiffPercentages2.Controls.Add(this.labelHellPerc);
            this.panelDiffPercentages2.Location = new System.Drawing.Point(3, 146);
            this.panelDiffPercentages2.Name = "panelDiffPercentages2";
            this.panelDiffPercentages2.Size = new System.Drawing.Size(367, 16);
            this.panelDiffPercentages2.TabIndex = 33;
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
            this.panelDiffPercentages.Location = new System.Drawing.Point(270, 0);
            this.panelDiffPercentages.Margin = new System.Windows.Forms.Padding(0);
            this.panelDiffPercentages.Name = "panelDiffPercentages";
            this.panelDiffPercentages.RowCount = 4;
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelDiffPercentages.Size = new System.Drawing.Size(97, 72);
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
            // outerLeftRightPanel
            // 
            this.outerLeftRightPanel.AutoSize = true;
            this.outerLeftRightPanel.Controls.Add(this.panelRuneDisplay2);
            this.outerLeftRightPanel.Controls.Add(this.flowLayoutPanel1);
            this.outerLeftRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerLeftRightPanel.Location = new System.Drawing.Point(0, 0);
            this.outerLeftRightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.outerLeftRightPanel.Name = "outerLeftRightPanel";
            this.outerLeftRightPanel.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.outerLeftRightPanel.Size = new System.Drawing.Size(425, 222);
            this.outerLeftRightPanel.TabIndex = 21;
            // 
            // panelRuneDisplay2
            // 
            this.panelRuneDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRuneDisplay2.AutoSize = true;
            this.panelRuneDisplay2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRuneDisplay2.Location = new System.Drawing.Point(6, 5);
            this.panelRuneDisplay2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelRuneDisplay2.MaximumSize = new System.Drawing.Size(28, 0);
            this.panelRuneDisplay2.MinimumSize = new System.Drawing.Size(28, 28);
            this.panelRuneDisplay2.Name = "panelRuneDisplay2";
            this.panelRuneDisplay2.Size = new System.Drawing.Size(28, 28);
            this.panelRuneDisplay2.TabIndex = 18;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(425, 222);
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.outerLeftRightPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DiabloInterface";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.contextMenu.ResumeLayout(false);
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
            this.panelDiffPercentages2.ResumeLayout(false);
            this.panelDiffPercentages2.PerformLayout();
            this.panelDiffPercentages.ResumeLayout(false);
            this.panelDiffPercentages.PerformLayout();
            this.outerLeftRightPanel.ResumeLayout(false);
            this.outerLeftRightPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lvlLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label strLabel;
        private System.Windows.Forms.Label dexLabel;
        private System.Windows.Forms.Label vitLabel;
        private System.Windows.Forms.Label eneLabel;
        private System.Windows.Forms.Label fireLabel;
        private System.Windows.Forms.Label coldLabel;
        private System.Windows.Forms.Label lighLabel;
        private System.Windows.Forms.Label poisLabel;
        private System.Windows.Forms.Label goldLabel;
        private System.Windows.Forms.Label deathsLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label frwLabel;
        private System.Windows.Forms.Label fcrLabel;
        private System.Windows.Forms.Label fhrLabel;
        private System.Windows.Forms.Label iasLabel;
        private System.Windows.Forms.Panel panelRuneDisplay;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel panelStats;
        private System.Windows.Forms.TableLayoutPanel panelBaseStats;
        private System.Windows.Forms.TableLayoutPanel panelAdvancedStats;
        private System.Windows.Forms.TableLayoutPanel panelResistances;
        private System.Windows.Forms.TableLayoutPanel panelSimpleStats;
        private System.Windows.Forms.TableLayoutPanel panelDeathsLvl;
        private System.Windows.Forms.Label labelStrVal;
        private System.Windows.Forms.Label labelDexVal;
        private System.Windows.Forms.Label labelVitVal;
        private System.Windows.Forms.Label labelEneVal;
        private System.Windows.Forms.Label labelFrwVal;
        private System.Windows.Forms.Label labelFhrVal;
        private System.Windows.Forms.Label labelFcrVal;
        private System.Windows.Forms.Label labelIasVal;
        private System.Windows.Forms.Label labelFireResVal;
        private System.Windows.Forms.Label labelColdResVal;
        private System.Windows.Forms.Label labelLightResVal;
        private System.Windows.Forms.Label labelPoisonResVal;
        private System.Windows.Forms.Label labelNormPerc;
        private System.Windows.Forms.Label labelNmPerc;
        private System.Windows.Forms.Label labelHellPerc;
        private System.Windows.Forms.FlowLayoutPanel panelDiffPercentages2;
        private System.Windows.Forms.TableLayoutPanel panelDiffPercentages;
        private System.Windows.Forms.Label normLabel;
        private System.Windows.Forms.Label nmLabel;
        private System.Windows.Forms.Label hellLabel;
        private System.Windows.Forms.Label normLabelVal;
        private System.Windows.Forms.Label nmLabelVal;
        private System.Windows.Forms.Label hellLabelVal;
        private System.Windows.Forms.FlowLayoutPanel outerLeftRightPanel;
        private System.Windows.Forms.Panel panelRuneDisplay2;
    }
}

