namespace Zutatensuppe.DiabloInterface.Gui
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.difficultyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nightmareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.difficultyToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.loadConfigMenuItem,
            this.resetToolStripMenuItem,
            this.debugMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 136);
            // 
            // difficultyToolStripMenuItem
            // 
            this.difficultyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.nightmareToolStripMenuItem,
            this.hellToolStripMenuItem});
            this.difficultyToolStripMenuItem.Name = "difficultyToolStripMenuItem";
            this.difficultyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.difficultyToolStripMenuItem.Text = "Difficulty";
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Checked = true;
            this.normalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.DifficultyNormalToolStripMenuItemOnClick);
            // 
            // nightmareToolStripMenuItem
            // 
            this.nightmareToolStripMenuItem.Name = "nightmareToolStripMenuItem";
            this.nightmareToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.nightmareToolStripMenuItem.Text = "Nightmare";
            this.nightmareToolStripMenuItem.Click += new System.EventHandler(this.NightmareToolStripMenuItemOnClick);
            // 
            // hellToolStripMenuItem
            // 
            this.hellToolStripMenuItem.Name = "hellToolStripMenuItem";
            this.hellToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.hellToolStripMenuItem.Text = "Hell";
            this.hellToolStripMenuItem.Click += new System.EventHandler(this.HellToolStripMenuItemOnClick);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.wrench_orange;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsMenuItemOnClick);
            // 
            // loadConfigMenuItem
            // 
            this.loadConfigMenuItem.Name = "loadConfigMenuItem";
            this.loadConfigMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadConfigMenuItem.Text = "Load Config";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.arrow_refresh;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetMenuItemOnClick);
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("debugMenuItem.Image")));
            this.debugMenuItem.Name = "debugMenuItem";
            this.debugMenuItem.Size = new System.Drawing.Size(152, 22);
            this.debugMenuItem.Text = "Debug";
            this.debugMenuItem.Click += new System.EventHandler(this.DebugMenuItemOnClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.cross;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuItemOnClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(730, 514);
            this.ContextMenuStrip = this.contextMenu;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DiabloInterface";
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigMenuItem;
        private System.Windows.Forms.ToolStripMenuItem difficultyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nightmareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hellToolStripMenuItem;
    }
}

