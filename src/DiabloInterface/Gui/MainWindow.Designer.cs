namespace DiabloInterface
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
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frwLabel = new System.Windows.Forms.Label();
            this.fcrLabel = new System.Windows.Forms.Label();
            this.fhrLabel = new System.Windows.Forms.Label();
            this.iasLabel = new System.Windows.Forms.Label();
            this.runeDisplayPanel = new System.Windows.Forms.Panel();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvlLabel
            // 
            this.lvlLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lvlLabel.ForeColor = System.Drawing.Color.White;
            this.lvlLabel.Location = new System.Drawing.Point(196, 63);
            this.lvlLabel.Name = "lvlLabel";
            this.lvlLabel.Size = new System.Drawing.Size(80, 16);
            this.lvlLabel.TabIndex = 2;
            this.lvlLabel.Text = "LVL: -";
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.nameLabel.Location = new System.Drawing.Point(12, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(260, 33);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Char Name";
            // 
            // strLabel
            // 
            this.strLabel.AutoSize = true;
            this.strLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.strLabel.ForeColor = System.Drawing.Color.Coral;
            this.strLabel.Location = new System.Drawing.Point(15, 84);
            this.strLabel.Name = "strLabel";
            this.strLabel.Size = new System.Drawing.Size(56, 16);
            this.strLabel.TabIndex = 7;
            this.strLabel.Text = "STR: -";
            // 
            // dexLabel
            // 
            this.dexLabel.AutoSize = true;
            this.dexLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dexLabel.ForeColor = System.Drawing.Color.Coral;
            this.dexLabel.Location = new System.Drawing.Point(15, 100);
            this.dexLabel.Name = "dexLabel";
            this.dexLabel.Size = new System.Drawing.Size(56, 16);
            this.dexLabel.TabIndex = 8;
            this.dexLabel.Text = "DEX: -";
            // 
            // vitLabel
            // 
            this.vitLabel.AutoSize = true;
            this.vitLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vitLabel.ForeColor = System.Drawing.Color.Coral;
            this.vitLabel.Location = new System.Drawing.Point(15, 116);
            this.vitLabel.Name = "vitLabel";
            this.vitLabel.Size = new System.Drawing.Size(56, 16);
            this.vitLabel.TabIndex = 10;
            this.vitLabel.Text = "VIT: -";
            // 
            // eneLabel
            // 
            this.eneLabel.AutoSize = true;
            this.eneLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.eneLabel.ForeColor = System.Drawing.Color.Coral;
            this.eneLabel.Location = new System.Drawing.Point(15, 132);
            this.eneLabel.Name = "eneLabel";
            this.eneLabel.Size = new System.Drawing.Size(56, 16);
            this.eneLabel.TabIndex = 11;
            this.eneLabel.Text = "ENE: -";
            // 
            // fireLabel
            // 
            this.fireLabel.AutoSize = true;
            this.fireLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fireLabel.ForeColor = System.Drawing.Color.Red;
            this.fireLabel.Location = new System.Drawing.Point(196, 84);
            this.fireLabel.Name = "fireLabel";
            this.fireLabel.Size = new System.Drawing.Size(64, 16);
            this.fireLabel.TabIndex = 12;
            this.fireLabel.Text = "FIRE: -";
            // 
            // coldLabel
            // 
            this.coldLabel.AutoSize = true;
            this.coldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.coldLabel.ForeColor = System.Drawing.Color.DodgerBlue;
            this.coldLabel.Location = new System.Drawing.Point(196, 100);
            this.coldLabel.Name = "coldLabel";
            this.coldLabel.Size = new System.Drawing.Size(64, 16);
            this.coldLabel.TabIndex = 13;
            this.coldLabel.Text = "COLD: -";
            // 
            // lighLabel
            // 
            this.lighLabel.AutoSize = true;
            this.lighLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lighLabel.ForeColor = System.Drawing.Color.Yellow;
            this.lighLabel.Location = new System.Drawing.Point(196, 116);
            this.lighLabel.Name = "lighLabel";
            this.lighLabel.Size = new System.Drawing.Size(64, 16);
            this.lighLabel.TabIndex = 14;
            this.lighLabel.Text = "LIGH: -";
            // 
            // poisLabel
            // 
            this.poisLabel.AutoSize = true;
            this.poisLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.poisLabel.ForeColor = System.Drawing.Color.YellowGreen;
            this.poisLabel.Location = new System.Drawing.Point(196, 132);
            this.poisLabel.Name = "poisLabel";
            this.poisLabel.Size = new System.Drawing.Size(64, 16);
            this.poisLabel.TabIndex = 15;
            this.poisLabel.Text = "POIS: -";
            // 
            // goldLabel
            // 
            this.goldLabel.AutoSize = true;
            this.goldLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.goldLabel.ForeColor = System.Drawing.Color.Gold;
            this.goldLabel.Location = new System.Drawing.Point(15, 63);
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
            this.deathsLabel.Location = new System.Drawing.Point(15, 42);
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
            this.contextMenu.Size = new System.Drawing.Size(172, 114);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.wrench_orange;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.button3_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.arrow_refresh;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.button2_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::DiabloInterface.Properties.Resources.cross;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // frwLabel
            // 
            this.frwLabel.AutoSize = true;
            this.frwLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.frwLabel.ForeColor = System.Drawing.Color.Coral;
            this.frwLabel.Location = new System.Drawing.Point(107, 84);
            this.frwLabel.Name = "frwLabel";
            this.frwLabel.Size = new System.Drawing.Size(56, 16);
            this.frwLabel.TabIndex = 7;
            this.frwLabel.Text = "FRW: -";
            // 
            // fcrLabel
            // 
            this.fcrLabel.AutoSize = true;
            this.fcrLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fcrLabel.ForeColor = System.Drawing.Color.Coral;
            this.fcrLabel.Location = new System.Drawing.Point(107, 100);
            this.fcrLabel.Name = "fcrLabel";
            this.fcrLabel.Size = new System.Drawing.Size(56, 16);
            this.fcrLabel.TabIndex = 8;
            this.fcrLabel.Text = "FCR: -";
            // 
            // fhrLabel
            // 
            this.fhrLabel.AutoSize = true;
            this.fhrLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fhrLabel.ForeColor = System.Drawing.Color.Coral;
            this.fhrLabel.Location = new System.Drawing.Point(107, 116);
            this.fhrLabel.Name = "fhrLabel";
            this.fhrLabel.Size = new System.Drawing.Size(56, 16);
            this.fhrLabel.TabIndex = 10;
            this.fhrLabel.Text = "FHR: -";
            // 
            // iasLabel
            // 
            this.iasLabel.AutoSize = true;
            this.iasLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iasLabel.ForeColor = System.Drawing.Color.Coral;
            this.iasLabel.Location = new System.Drawing.Point(107, 132);
            this.iasLabel.Name = "iasLabel";
            this.iasLabel.Size = new System.Drawing.Size(56, 16);
            this.iasLabel.TabIndex = 11;
            this.iasLabel.Text = "IAS: -";
            // 
            // runeDisplayPanel
            // 
            this.runeDisplayPanel.Location = new System.Drawing.Point(18, 158);
            this.runeDisplayPanel.Name = "runeDisplayPanel";
            this.runeDisplayPanel.Size = new System.Drawing.Size(248, 41);
            this.runeDisplayPanel.TabIndex = 18;
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Name = "debugMenuItem";
            this.debugMenuItem.Size = new System.Drawing.Size(171, 22);
            this.debugMenuItem.Text = "Debug";
            this.debugMenuItem.Click += new System.EventHandler(this.debugMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.runeDisplayPanel);
            this.Controls.Add(this.deathsLabel);
            this.Controls.Add(this.goldLabel);
            this.Controls.Add(this.poisLabel);
            this.Controls.Add(this.lighLabel);
            this.Controls.Add(this.coldLabel);
            this.Controls.Add(this.fireLabel);
            this.Controls.Add(this.iasLabel);
            this.Controls.Add(this.eneLabel);
            this.Controls.Add(this.fhrLabel);
            this.Controls.Add(this.vitLabel);
            this.Controls.Add(this.fcrLabel);
            this.Controls.Add(this.dexLabel);
            this.Controls.Add(this.frwLabel);
            this.Controls.Add(this.strLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.lvlLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DiabloInterface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.contextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Panel runeDisplayPanel;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
    }
}

