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
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalLayout1 = new Zutatensuppe.DiabloInterface.Gui.Controls.HorizontalLayout();
            this.verticalLayout2 = new Zutatensuppe.DiabloInterface.Gui.Controls.VerticalLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.loadConfigMenuItem,
            this.resetToolStripMenuItem,
            this.debugMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(161, 114);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.wrench_orange;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // loadConfigMenuItem
            // 
            this.loadConfigMenuItem.Name = "loadConfigMenuItem";
            this.loadConfigMenuItem.Size = new System.Drawing.Size(160, 22);
            this.loadConfigMenuItem.Text = "Load Config";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.arrow_refresh;
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
            this.exitToolStripMenuItem.Image = global::Zutatensuppe.DiabloInterface.Properties.Resources.cross;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // horizontalLayout1
            // 
            this.horizontalLayout1.AutoSize = true;
            this.horizontalLayout1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.horizontalLayout1.BackColor = System.Drawing.Color.Black;
            this.horizontalLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalLayout1.Location = new System.Drawing.Point(0, 0);
            this.horizontalLayout1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalLayout1.Name = "horizontalLayout1";
            this.horizontalLayout1.Size = new System.Drawing.Size(730, 514);
            this.horizontalLayout1.TabIndex = 0;
            // 
            // verticalLayout2
            // 
            this.verticalLayout2.AutoSize = true;
            this.verticalLayout2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.verticalLayout2.BackColor = System.Drawing.Color.Black;
            this.verticalLayout2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.verticalLayout2.Location = new System.Drawing.Point(0, 0);
            this.verticalLayout2.Name = "verticalLayout2";
            this.verticalLayout2.Size = new System.Drawing.Size(730, 514);
            this.verticalLayout2.TabIndex = 2;
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
            this.Controls.Add(this.verticalLayout2);
            this.Controls.Add(this.horizontalLayout1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DiabloInterface";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigMenuItem;
        
        private Controls.VerticalLayout verticalLayout2;
        private Controls.HorizontalLayout horizontalLayout1;
    }
}

