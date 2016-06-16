namespace DiabloInterface.Gui
{
    partial class SettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.FontLabel = new System.Windows.Forms.Label();
            this.FontGroup = new System.Windows.Forms.GroupBox();
            this.fontComboBox = new DiabloInterface.Gui.Controls.FontComboBox();
            this.titleFontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.fontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.FontSizeLabel = new System.Windows.Forms.Label();
            this.TitleFontSizeLabel = new System.Windows.Forms.Label();
            this.CreateFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.AutoSplitGroup = new System.Windows.Forms.GroupBox();
            this.AutoSplitLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AutoSplitToolbar = new System.Windows.Forms.Panel();
            this.autoSplitHotkeyControl = new DiabloInterface.Gui.Controls.HotkeyControl();
            this.AutoSplitHotkeyLabel = new System.Windows.Forms.Label();
            this.AutoSplitTestHotkeyButton = new System.Windows.Forms.Button();
            this.EnableAutosplitCheckBox = new System.Windows.Forms.CheckBox();
            this.AddAutoSplitButton = new System.Windows.Forms.Button();
            this.UpdateGroup = new System.Windows.Forms.GroupBox();
            this.CheckUpdatesButton = new System.Windows.Forms.Button();
            this.CheckUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.VersionGroup = new System.Windows.Forms.GroupBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.VersionComboBox = new System.Windows.Forms.ComboBox();
            this.RuneDisplayGroup = new System.Windows.Forms.GroupBox();
            this.RuneDisplayPanel = new System.Windows.Forms.Panel();
            this.AddRuneButton = new System.Windows.Forms.Button();
            this.RuneComboBox = new System.Windows.Forms.ComboBox();
            this.SettingsMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSettingsAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CloseSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.VerticalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.HorizontalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.RightPanelLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkDisplayRunes = new System.Windows.Forms.CheckBox();
            this.chkDisplayAdvancedStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayLevel = new System.Windows.Forms.CheckBox();
            this.chkDisplayGold = new System.Windows.Forms.CheckBox();
            this.chkDisplayResistances = new System.Windows.Forms.CheckBox();
            this.chkDisplayBaseStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayDeathCounter = new System.Windows.Forms.CheckBox();
            this.chkDisplayName = new System.Windows.Forms.CheckBox();
            this.FontGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumeric)).BeginInit();
            this.DataGroup.SuspendLayout();
            this.AutoSplitGroup.SuspendLayout();
            this.AutoSplitLayout.SuspendLayout();
            this.AutoSplitToolbar.SuspendLayout();
            this.UpdateGroup.SuspendLayout();
            this.VersionGroup.SuspendLayout();
            this.RuneDisplayGroup.SuspendLayout();
            this.SettingsMenuStrip.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.VerticalSplitContainer.SuspendLayout();
            this.HorizontalSplitContainer.SuspendLayout();
            this.RightPanelLayout.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(6, 24);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(32, 13);
            this.FontLabel.TabIndex = 2;
            this.FontLabel.Text = "Font:";
            // 
            // FontGroup
            // 
            this.FontGroup.Controls.Add(this.fontComboBox);
            this.FontGroup.Controls.Add(this.titleFontSizeNumeric);
            this.FontGroup.Controls.Add(this.fontSizeNumeric);
            this.FontGroup.Controls.Add(this.FontSizeLabel);
            this.FontGroup.Controls.Add(this.TitleFontSizeLabel);
            this.FontGroup.Controls.Add(this.FontLabel);
            this.FontGroup.Location = new System.Drawing.Point(3, 3);
            this.FontGroup.Name = "FontGroup";
            this.FontGroup.Size = new System.Drawing.Size(231, 104);
            this.FontGroup.TabIndex = 5;
            this.FontGroup.TabStop = false;
            this.FontGroup.Text = "Font";
            // 
            // fontComboBox
            // 
            this.fontComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontComboBox.DropDownWidth = 250;
            this.fontComboBox.FormattingEnabled = true;
            this.fontComboBox.Location = new System.Drawing.Point(105, 21);
            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.Size = new System.Drawing.Size(117, 21);
            this.fontComboBox.Sorted = true;
            this.fontComboBox.TabIndex = 12;
            // 
            // titleFontSizeNumeric
            // 
            this.titleFontSizeNumeric.Location = new System.Drawing.Point(105, 74);
            this.titleFontSizeNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.titleFontSizeNumeric.Name = "titleFontSizeNumeric";
            this.titleFontSizeNumeric.Size = new System.Drawing.Size(117, 20);
            this.titleFontSizeNumeric.TabIndex = 11;
            this.titleFontSizeNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // fontSizeNumeric
            // 
            this.fontSizeNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontSizeNumeric.Location = new System.Drawing.Point(105, 48);
            this.fontSizeNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.fontSizeNumeric.Name = "fontSizeNumeric";
            this.fontSizeNumeric.Size = new System.Drawing.Size(117, 20);
            this.fontSizeNumeric.TabIndex = 10;
            this.fontSizeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // FontSizeLabel
            // 
            this.FontSizeLabel.AutoSize = true;
            this.FontSizeLabel.Location = new System.Drawing.Point(6, 51);
            this.FontSizeLabel.Name = "FontSizeLabel";
            this.FontSizeLabel.Size = new System.Drawing.Size(57, 13);
            this.FontSizeLabel.TabIndex = 7;
            this.FontSizeLabel.Text = "Font Size:";
            // 
            // TitleFontSizeLabel
            // 
            this.TitleFontSizeLabel.AutoSize = true;
            this.TitleFontSizeLabel.Location = new System.Drawing.Point(6, 77);
            this.TitleFontSizeLabel.Name = "TitleFontSizeLabel";
            this.TitleFontSizeLabel.Size = new System.Drawing.Size(82, 13);
            this.TitleFontSizeLabel.TabIndex = 6;
            this.TitleFontSizeLabel.Text = "Title Font Size:";
            // 
            // CreateFilesCheckBox
            // 
            this.CreateFilesCheckBox.AutoSize = true;
            this.CreateFilesCheckBox.Location = new System.Drawing.Point(10, 19);
            this.CreateFilesCheckBox.Name = "CreateFilesCheckBox";
            this.CreateFilesCheckBox.Size = new System.Drawing.Size(83, 17);
            this.CreateFilesCheckBox.TabIndex = 8;
            this.CreateFilesCheckBox.Text = "Create files";
            this.CreateFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // DataGroup
            // 
            this.DataGroup.Controls.Add(this.CreateFilesCheckBox);
            this.DataGroup.Location = new System.Drawing.Point(3, 113);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Size = new System.Drawing.Size(231, 45);
            this.DataGroup.TabIndex = 9;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "Data";
            // 
            // AutoSplitGroup
            // 
            this.AutoSplitGroup.Controls.Add(this.AutoSplitLayout);
            this.AutoSplitGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoSplitGroup.Location = new System.Drawing.Point(3, 3);
            this.AutoSplitGroup.Name = "AutoSplitGroup";
            this.AutoSplitGroup.Size = new System.Drawing.Size(526, 335);
            this.AutoSplitGroup.TabIndex = 10;
            this.AutoSplitGroup.TabStop = false;
            this.AutoSplitGroup.Text = "Auto-Split";
            // 
            // AutoSplitLayout
            // 
            this.AutoSplitLayout.ColumnCount = 1;
            this.AutoSplitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutoSplitLayout.Controls.Add(this.AutoSplitToolbar, 0, 1);
            this.AutoSplitLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoSplitLayout.Location = new System.Drawing.Point(3, 16);
            this.AutoSplitLayout.Name = "AutoSplitLayout";
            this.AutoSplitLayout.RowCount = 2;
            this.AutoSplitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutoSplitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AutoSplitLayout.Size = new System.Drawing.Size(520, 316);
            this.AutoSplitLayout.TabIndex = 21;
            // 
            // AutoSplitToolbar
            // 
            this.AutoSplitToolbar.AutoSize = true;
            this.AutoSplitToolbar.Controls.Add(this.autoSplitHotkeyControl);
            this.AutoSplitToolbar.Controls.Add(this.AutoSplitHotkeyLabel);
            this.AutoSplitToolbar.Controls.Add(this.AutoSplitTestHotkeyButton);
            this.AutoSplitToolbar.Controls.Add(this.EnableAutosplitCheckBox);
            this.AutoSplitToolbar.Controls.Add(this.AddAutoSplitButton);
            this.AutoSplitToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoSplitToolbar.Location = new System.Drawing.Point(3, 282);
            this.AutoSplitToolbar.Name = "AutoSplitToolbar";
            this.AutoSplitToolbar.Size = new System.Drawing.Size(514, 31);
            this.AutoSplitToolbar.TabIndex = 20;
            // 
            // autoSplitHotkeyControl
            // 
            this.autoSplitHotkeyControl.Hotkey = System.Windows.Forms.Keys.None;
            this.autoSplitHotkeyControl.Location = new System.Drawing.Point(80, 7);
            this.autoSplitHotkeyControl.Name = "autoSplitHotkeyControl";
            this.autoSplitHotkeyControl.Size = new System.Drawing.Size(82, 20);
            this.autoSplitHotkeyControl.TabIndex = 3;
            this.autoSplitHotkeyControl.Text = "None";
            this.autoSplitHotkeyControl.UseKeyWhitelist = true;
            // 
            // AutoSplitHotkeyLabel
            // 
            this.AutoSplitHotkeyLabel.AutoSize = true;
            this.AutoSplitHotkeyLabel.Location = new System.Drawing.Point(3, 10);
            this.AutoSplitHotkeyLabel.Name = "AutoSplitHotkeyLabel";
            this.AutoSplitHotkeyLabel.Size = new System.Drawing.Size(71, 13);
            this.AutoSplitHotkeyLabel.TabIndex = 13;
            this.AutoSplitHotkeyLabel.Text = "Split-Hotkey:";
            // 
            // AutoSplitTestHotkeyButton
            // 
            this.AutoSplitTestHotkeyButton.Location = new System.Drawing.Point(168, 5);
            this.AutoSplitTestHotkeyButton.Name = "AutoSplitTestHotkeyButton";
            this.AutoSplitTestHotkeyButton.Size = new System.Drawing.Size(75, 23);
            this.AutoSplitTestHotkeyButton.TabIndex = 18;
            this.AutoSplitTestHotkeyButton.Text = "Test Hotkey";
            this.AutoSplitTestHotkeyButton.UseVisualStyleBackColor = true;
            this.AutoSplitTestHotkeyButton.Click += new System.EventHandler(this.AutoSplitTestHotkey_Click);
            // 
            // EnableAutosplitCheckBox
            // 
            this.EnableAutosplitCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnableAutosplitCheckBox.AutoSize = true;
            this.EnableAutosplitCheckBox.Location = new System.Drawing.Point(366, 9);
            this.EnableAutosplitCheckBox.Name = "EnableAutosplitCheckBox";
            this.EnableAutosplitCheckBox.Size = new System.Drawing.Size(60, 17);
            this.EnableAutosplitCheckBox.TabIndex = 1;
            this.EnableAutosplitCheckBox.Text = "Enable";
            this.EnableAutosplitCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAutoSplitButton
            // 
            this.AddAutoSplitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAutoSplitButton.Location = new System.Drawing.Point(432, 5);
            this.AddAutoSplitButton.Name = "AddAutoSplitButton";
            this.AddAutoSplitButton.Size = new System.Drawing.Size(79, 23);
            this.AddAutoSplitButton.TabIndex = 11;
            this.AddAutoSplitButton.Text = "Add Split";
            this.AddAutoSplitButton.UseVisualStyleBackColor = true;
            this.AddAutoSplitButton.Click += new System.EventHandler(this.AddAutoSplitButton_Clicked);
            // 
            // UpdateGroup
            // 
            this.UpdateGroup.Controls.Add(this.CheckUpdatesButton);
            this.UpdateGroup.Controls.Add(this.CheckUpdatesCheckBox);
            this.UpdateGroup.Location = new System.Drawing.Point(3, 164);
            this.UpdateGroup.Name = "UpdateGroup";
            this.UpdateGroup.Size = new System.Drawing.Size(231, 70);
            this.UpdateGroup.TabIndex = 16;
            this.UpdateGroup.TabStop = false;
            this.UpdateGroup.Text = "Updates";
            // 
            // CheckUpdatesButton
            // 
            this.CheckUpdatesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckUpdatesButton.Location = new System.Drawing.Point(6, 42);
            this.CheckUpdatesButton.Name = "CheckUpdatesButton";
            this.CheckUpdatesButton.Size = new System.Drawing.Size(219, 23);
            this.CheckUpdatesButton.TabIndex = 2;
            this.CheckUpdatesButton.Text = "Check for updates now";
            this.CheckUpdatesButton.UseVisualStyleBackColor = true;
            this.CheckUpdatesButton.Click += new System.EventHandler(this.CheckUpdatesButton_Click);
            // 
            // CheckUpdatesCheckBox
            // 
            this.CheckUpdatesCheckBox.AutoSize = true;
            this.CheckUpdatesCheckBox.Location = new System.Drawing.Point(10, 19);
            this.CheckUpdatesCheckBox.Name = "CheckUpdatesCheckBox";
            this.CheckUpdatesCheckBox.Size = new System.Drawing.Size(153, 17);
            this.CheckUpdatesCheckBox.TabIndex = 1;
            this.CheckUpdatesCheckBox.Text = "Check for updates at start";
            this.CheckUpdatesCheckBox.UseVisualStyleBackColor = true;
            // 
            // VersionGroup
            // 
            this.VersionGroup.Controls.Add(this.VersionLabel);
            this.VersionGroup.Controls.Add(this.VersionComboBox);
            this.VersionGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.VersionGroup.Location = new System.Drawing.Point(3, 240);
            this.VersionGroup.Name = "VersionGroup";
            this.VersionGroup.Size = new System.Drawing.Size(231, 49);
            this.VersionGroup.TabIndex = 17;
            this.VersionGroup.TabStop = false;
            this.VersionGroup.Text = "Diablo 2";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(6, 20);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(48, 13);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Version:";
            // 
            // VersionComboBox
            // 
            this.VersionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionComboBox.FormattingEnabled = true;
            this.VersionComboBox.Items.AddRange(new object[] {
            "1.14d",
            "1.14c"});
            this.VersionComboBox.Location = new System.Drawing.Point(105, 17);
            this.VersionComboBox.Name = "VersionComboBox";
            this.VersionComboBox.Size = new System.Drawing.Size(117, 21);
            this.VersionComboBox.TabIndex = 0;
            // 
            // RuneDisplayGroup
            // 
            this.RuneDisplayGroup.Controls.Add(this.RuneDisplayPanel);
            this.RuneDisplayGroup.Controls.Add(this.AddRuneButton);
            this.RuneDisplayGroup.Controls.Add(this.RuneComboBox);
            this.RuneDisplayGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RuneDisplayGroup.Location = new System.Drawing.Point(3, 344);
            this.RuneDisplayGroup.Name = "RuneDisplayGroup";
            this.RuneDisplayGroup.Size = new System.Drawing.Size(526, 110);
            this.RuneDisplayGroup.TabIndex = 19;
            this.RuneDisplayGroup.TabStop = false;
            this.RuneDisplayGroup.Text = "Rune Display";
            // 
            // RuneDisplayPanel
            // 
            this.RuneDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RuneDisplayPanel.AutoScroll = true;
            this.RuneDisplayPanel.Location = new System.Drawing.Point(6, 19);
            this.RuneDisplayPanel.Name = "RuneDisplayPanel";
            this.RuneDisplayPanel.Size = new System.Drawing.Size(511, 57);
            this.RuneDisplayPanel.TabIndex = 2;
            // 
            // AddRuneButton
            // 
            this.AddRuneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddRuneButton.Location = new System.Drawing.Point(438, 81);
            this.AddRuneButton.Name = "AddRuneButton";
            this.AddRuneButton.Size = new System.Drawing.Size(79, 23);
            this.AddRuneButton.TabIndex = 1;
            this.AddRuneButton.Text = "Add Rune";
            this.AddRuneButton.UseVisualStyleBackColor = true;
            this.AddRuneButton.Click += new System.EventHandler(this.AddRuneButton_Click);
            // 
            // RuneComboBox
            // 
            this.RuneComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RuneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RuneComboBox.FormattingEnabled = true;
            this.RuneComboBox.Items.AddRange(new object[] {
            "El",
            "Eld",
            "Tir",
            "Nef",
            "Eth",
            "Ith",
            "Tal",
            "Ral",
            "Ort",
            "Thul",
            "Amn",
            "Sol",
            "Shael",
            "Dol",
            "Hel",
            "Io",
            "Lum",
            "Ko",
            "Fal",
            "Lem",
            "Pul",
            "Um",
            "Mal",
            "Ist",
            "Gul",
            "Vex",
            "Ohm",
            "Lo",
            "Sur",
            "Ber",
            "Jah",
            "Cham",
            "Zod"});
            this.RuneComboBox.Location = new System.Drawing.Point(373, 82);
            this.RuneComboBox.Name = "RuneComboBox";
            this.RuneComboBox.Size = new System.Drawing.Size(59, 21);
            this.RuneComboBox.TabIndex = 0;
            // 
            // SettingsMenuStrip
            // 
            this.SettingsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.SettingsMenuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.SettingsMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.SettingsMenuStrip.Name = "SettingsMenuStrip";
            this.SettingsMenuStrip.Size = new System.Drawing.Size(778, 24);
            this.SettingsMenuStrip.TabIndex = 21;
            this.SettingsMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadSettingsMenuItem,
            this.SaveSettingsAsMenuItem,
            this.SaveSettingsMenuItem,
            this.toolStripSeparator1,
            this.CloseSettingsMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // LoadSettingsMenuItem
            // 
            this.LoadSettingsMenuItem.Image = global::DiabloInterface.Properties.Resources.folder_explore;
            this.LoadSettingsMenuItem.Name = "LoadSettingsMenuItem";
            this.LoadSettingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LoadSettingsMenuItem.Text = "Load Config";
            this.LoadSettingsMenuItem.Click += new System.EventHandler(this.LoadSettingsMenuItem_Click);
            // 
            // SaveSettingsAsMenuItem
            // 
            this.SaveSettingsAsMenuItem.Image = global::DiabloInterface.Properties.Resources.disk;
            this.SaveSettingsAsMenuItem.Name = "SaveSettingsAsMenuItem";
            this.SaveSettingsAsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SaveSettingsAsMenuItem.Text = "Save As";
            this.SaveSettingsAsMenuItem.Click += new System.EventHandler(this.SaveSettingsAsMenuItem_Click);
            // 
            // SaveSettingsMenuItem
            // 
            this.SaveSettingsMenuItem.Image = global::DiabloInterface.Properties.Resources.disk;
            this.SaveSettingsMenuItem.Name = "SaveSettingsMenuItem";
            this.SaveSettingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SaveSettingsMenuItem.Text = "Save";
            this.SaveSettingsMenuItem.Click += new System.EventHandler(this.SaveSettingsMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // CloseSettingsMenuItem
            // 
            this.CloseSettingsMenuItem.Image = global::DiabloInterface.Properties.Resources.cross;
            this.CloseSettingsMenuItem.Name = "CloseSettingsMenuItem";
            this.CloseSettingsMenuItem.ShowShortcutKeys = false;
            this.CloseSettingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.CloseSettingsMenuItem.Text = "Close Settings";
            this.CloseSettingsMenuItem.Click += new System.EventHandler(this.CloseSettingsMenuItem_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.VerticalSplitContainer);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 24);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(778, 463);
            this.mainPanel.TabIndex = 22;
            // 
            // VerticalSplitContainer
            // 
            this.VerticalSplitContainer.ColumnCount = 2;
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.VerticalSplitContainer.Controls.Add(this.HorizontalSplitContainer, 0, 0);
            this.VerticalSplitContainer.Controls.Add(this.RightPanelLayout, 1, 0);
            this.VerticalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerticalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.VerticalSplitContainer.Name = "VerticalSplitContainer";
            this.VerticalSplitContainer.RowCount = 1;
            this.VerticalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.Size = new System.Drawing.Size(778, 463);
            this.VerticalSplitContainer.TabIndex = 20;
            // 
            // HorizontalSplitContainer
            // 
            this.HorizontalSplitContainer.ColumnCount = 1;
            this.HorizontalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.Controls.Add(this.AutoSplitGroup, 0, 0);
            this.HorizontalSplitContainer.Controls.Add(this.RuneDisplayGroup, 0, 1);
            this.HorizontalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HorizontalSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.HorizontalSplitContainer.Name = "HorizontalSplitContainer";
            this.HorizontalSplitContainer.RowCount = 2;
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HorizontalSplitContainer.Size = new System.Drawing.Size(532, 457);
            this.HorizontalSplitContainer.TabIndex = 0;
            // 
            // RightPanelLayout
            // 
            this.RightPanelLayout.Controls.Add(this.FontGroup);
            this.RightPanelLayout.Controls.Add(this.DataGroup);
            this.RightPanelLayout.Controls.Add(this.UpdateGroup);
            this.RightPanelLayout.Controls.Add(this.VersionGroup);
            this.RightPanelLayout.Controls.Add(this.groupBox1);
            this.RightPanelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanelLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.RightPanelLayout.Location = new System.Drawing.Point(541, 3);
            this.RightPanelLayout.Name = "RightPanelLayout";
            this.RightPanelLayout.Size = new System.Drawing.Size(234, 457);
            this.RightPanelLayout.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.chkDisplayRunes);
            this.groupBox1.Controls.Add(this.chkDisplayAdvancedStats);
            this.groupBox1.Controls.Add(this.chkDisplayLevel);
            this.groupBox1.Controls.Add(this.chkDisplayGold);
            this.groupBox1.Controls.Add(this.chkDisplayResistances);
            this.groupBox1.Controls.Add(this.chkDisplayBaseStats);
            this.groupBox1.Controls.Add(this.chkDisplayDeathCounter);
            this.groupBox1.Controls.Add(this.chkDisplayName);
            this.groupBox1.Location = new System.Drawing.Point(3, 295);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 155);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.DarkRed;
            this.textBox1.Location = new System.Drawing.Point(9, 111);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 44);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "(!) Enabling this may result in your run getting rejected by speedrun sites. Use " +
    "for fun runs only.";
            // 
            // chkDisplayRunes
            // 
            this.chkDisplayRunes.AutoSize = true;
            this.chkDisplayRunes.Location = new System.Drawing.Point(141, 88);
            this.chkDisplayRunes.Name = "chkDisplayRunes";
            this.chkDisplayRunes.Size = new System.Drawing.Size(58, 17);
            this.chkDisplayRunes.TabIndex = 6;
            this.chkDisplayRunes.Text = "Runes";
            this.chkDisplayRunes.UseVisualStyleBackColor = true;
            // 
            // chkDisplayAdvancedStats
            // 
            this.chkDisplayAdvancedStats.AutoSize = true;
            this.chkDisplayAdvancedStats.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDisplayAdvancedStats.Location = new System.Drawing.Point(10, 88);
            this.chkDisplayAdvancedStats.Name = "chkDisplayAdvancedStats";
            this.chkDisplayAdvancedStats.Size = new System.Drawing.Size(126, 17);
            this.chkDisplayAdvancedStats.TabIndex = 5;
            this.chkDisplayAdvancedStats.Text = "(!) Fcr, Frw, Fhr, Ias";
            this.chkDisplayAdvancedStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayLevel
            // 
            this.chkDisplayLevel.AutoSize = true;
            this.chkDisplayLevel.Location = new System.Drawing.Point(141, 42);
            this.chkDisplayLevel.Name = "chkDisplayLevel";
            this.chkDisplayLevel.Size = new System.Drawing.Size(53, 17);
            this.chkDisplayLevel.TabIndex = 4;
            this.chkDisplayLevel.Text = "Level";
            this.chkDisplayLevel.UseVisualStyleBackColor = true;
            // 
            // chkDisplayGold
            // 
            this.chkDisplayGold.AutoSize = true;
            this.chkDisplayGold.Location = new System.Drawing.Point(10, 42);
            this.chkDisplayGold.Name = "chkDisplayGold";
            this.chkDisplayGold.Size = new System.Drawing.Size(50, 17);
            this.chkDisplayGold.TabIndex = 4;
            this.chkDisplayGold.Text = "Gold";
            this.chkDisplayGold.UseVisualStyleBackColor = true;
            // 
            // chkDisplayResistances
            // 
            this.chkDisplayResistances.AutoSize = true;
            this.chkDisplayResistances.Location = new System.Drawing.Point(141, 65);
            this.chkDisplayResistances.Name = "chkDisplayResistances";
            this.chkDisplayResistances.Size = new System.Drawing.Size(88, 17);
            this.chkDisplayResistances.TabIndex = 3;
            this.chkDisplayResistances.Text = "Resistances";
            this.chkDisplayResistances.UseVisualStyleBackColor = true;
            // 
            // chkDisplayBaseStats
            // 
            this.chkDisplayBaseStats.AutoSize = true;
            this.chkDisplayBaseStats.Location = new System.Drawing.Point(10, 65);
            this.chkDisplayBaseStats.Name = "chkDisplayBaseStats";
            this.chkDisplayBaseStats.Size = new System.Drawing.Size(110, 17);
            this.chkDisplayBaseStats.TabIndex = 2;
            this.chkDisplayBaseStats.Text = "Str, Dex, Vit, Ene";
            this.chkDisplayBaseStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayDeathCounter
            // 
            this.chkDisplayDeathCounter.AutoSize = true;
            this.chkDisplayDeathCounter.Location = new System.Drawing.Point(141, 19);
            this.chkDisplayDeathCounter.Name = "chkDisplayDeathCounter";
            this.chkDisplayDeathCounter.Size = new System.Drawing.Size(61, 17);
            this.chkDisplayDeathCounter.TabIndex = 1;
            this.chkDisplayDeathCounter.Text = "Deaths";
            this.chkDisplayDeathCounter.UseVisualStyleBackColor = true;
            // 
            // chkDisplayName
            // 
            this.chkDisplayName.AutoSize = true;
            this.chkDisplayName.Location = new System.Drawing.Point(10, 19);
            this.chkDisplayName.Name = "chkDisplayName";
            this.chkDisplayName.Size = new System.Drawing.Size(55, 17);
            this.chkDisplayName.TabIndex = 0;
            this.chkDisplayName.Text = "Name";
            this.chkDisplayName.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 487);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.SettingsMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.SettingsMenuStrip;
            this.MinimumSize = new System.Drawing.Size(794, 525);
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsWindow_Shown);
            this.FontGroup.ResumeLayout(false);
            this.FontGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumeric)).EndInit();
            this.DataGroup.ResumeLayout(false);
            this.DataGroup.PerformLayout();
            this.AutoSplitGroup.ResumeLayout(false);
            this.AutoSplitLayout.ResumeLayout(false);
            this.AutoSplitLayout.PerformLayout();
            this.AutoSplitToolbar.ResumeLayout(false);
            this.AutoSplitToolbar.PerformLayout();
            this.UpdateGroup.ResumeLayout(false);
            this.UpdateGroup.PerformLayout();
            this.VersionGroup.ResumeLayout(false);
            this.VersionGroup.PerformLayout();
            this.RuneDisplayGroup.ResumeLayout(false);
            this.SettingsMenuStrip.ResumeLayout(false);
            this.SettingsMenuStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.VerticalSplitContainer.ResumeLayout(false);
            this.HorizontalSplitContainer.ResumeLayout(false);
            this.RightPanelLayout.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label FontLabel;
        private System.Windows.Forms.GroupBox FontGroup;
        private System.Windows.Forms.Label TitleFontSizeLabel;
        private System.Windows.Forms.Label FontSizeLabel;
        private System.Windows.Forms.CheckBox CreateFilesCheckBox;
        private System.Windows.Forms.GroupBox DataGroup;
        private System.Windows.Forms.GroupBox AutoSplitGroup;
        private System.Windows.Forms.Button AddAutoSplitButton;
        private System.Windows.Forms.CheckBox EnableAutosplitCheckBox;
        private System.Windows.Forms.Label AutoSplitHotkeyLabel;
        private System.Windows.Forms.GroupBox UpdateGroup;
        private System.Windows.Forms.GroupBox VersionGroup;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ComboBox VersionComboBox;
        private System.Windows.Forms.Button AutoSplitTestHotkeyButton;
        private System.Windows.Forms.GroupBox RuneDisplayGroup;
        private System.Windows.Forms.Button AddRuneButton;
        private System.Windows.Forms.ComboBox RuneComboBox;
        private System.Windows.Forms.Panel RuneDisplayPanel;
        private System.Windows.Forms.MenuStrip SettingsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadSettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveSettingsAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveSettingsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem CloseSettingsMenuItem;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.CheckBox CheckUpdatesCheckBox;
        private System.Windows.Forms.Button CheckUpdatesButton;
        private System.Windows.Forms.TableLayoutPanel VerticalSplitContainer;
        private System.Windows.Forms.TableLayoutPanel HorizontalSplitContainer;
        private System.Windows.Forms.TableLayoutPanel AutoSplitLayout;
        private System.Windows.Forms.Panel AutoSplitToolbar;
        private System.Windows.Forms.FlowLayoutPanel RightPanelLayout;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDisplayName;
        private System.Windows.Forms.CheckBox chkDisplayDeathCounter;
        private System.Windows.Forms.CheckBox chkDisplayAdvancedStats;
        private System.Windows.Forms.CheckBox chkDisplayGold;
        private System.Windows.Forms.CheckBox chkDisplayResistances;
        private System.Windows.Forms.CheckBox chkDisplayBaseStats;
        private System.Windows.Forms.CheckBox chkDisplayLevel;
        private System.Windows.Forms.CheckBox chkDisplayRunes;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown fontSizeNumeric;
        private System.Windows.Forms.NumericUpDown titleFontSizeNumeric;
        private Gui.Controls.FontComboBox fontComboBox;
        private Controls.HotkeyControl autoSplitHotkeyControl;
    }
}