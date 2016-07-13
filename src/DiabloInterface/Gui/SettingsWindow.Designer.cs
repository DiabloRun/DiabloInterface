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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.FontLabel = new System.Windows.Forms.Label();
            this.FontGroup = new System.Windows.Forms.GroupBox();
            this.titleFontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.fontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.FontSizeLabel = new System.Windows.Forms.Label();
            this.TitleFontSizeLabel = new System.Windows.Forms.Label();
            this.CreateFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.AutoSplitGroup = new System.Windows.Forms.GroupBox();
            this.AutoSplitLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AutoSplitToolbar = new System.Windows.Forms.Panel();
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
            this.btnAddRuneWord = new System.Windows.Forms.Button();
            this.cbRuneWord = new System.Windows.Forms.ComboBox();
            this.RuneDisplayPanel = new System.Windows.Forms.Panel();
            this.AddRuneButton = new System.Windows.Forms.Button();
            this.RuneComboBox = new System.Windows.Forms.ComboBox();
            this.VerticalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.HorizontalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.RightPanelLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDisplayRunes = new System.Windows.Forms.CheckBox();
            this.chkDisplayAdvancedStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayLevel = new System.Windows.Forms.CheckBox();
            this.chkDisplayGold = new System.Windows.Forms.CheckBox();
            this.chkDisplayResistances = new System.Windows.Forms.CheckBox();
            this.chkDisplayBaseStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayDeathCounter = new System.Windows.Forms.CheckBox();
            this.chkDisplayName = new System.Windows.Forms.CheckBox();
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lstConfigFiles = new System.Windows.Forms.ListBox();
            this.grpConfigFiles = new System.Windows.Forms.GroupBox();
            this.ctxConfigFileList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClone = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSplitHotkeyControl = new DiabloInterface.Gui.Controls.HotkeyControl();
            this.fontComboBox = new DiabloInterface.Gui.Controls.FontComboBox();
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
            this.VerticalSplitContainer.SuspendLayout();
            this.HorizontalSplitContainer.SuspendLayout();
            this.RightPanelLayout.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpConfigFiles.SuspendLayout();
            this.ctxConfigFileList.SuspendLayout();
            this.SuspendLayout();
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(6, 24);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(31, 13);
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
            this.FontSizeLabel.Size = new System.Drawing.Size(54, 13);
            this.FontSizeLabel.TabIndex = 7;
            this.FontSizeLabel.Text = "Font Size:";
            // 
            // TitleFontSizeLabel
            // 
            this.TitleFontSizeLabel.AutoSize = true;
            this.TitleFontSizeLabel.Location = new System.Drawing.Point(6, 77);
            this.TitleFontSizeLabel.Name = "TitleFontSizeLabel";
            this.TitleFontSizeLabel.Size = new System.Drawing.Size(77, 13);
            this.TitleFontSizeLabel.TabIndex = 6;
            this.TitleFontSizeLabel.Text = "Title Font Size:";
            // 
            // CreateFilesCheckBox
            // 
            this.CreateFilesCheckBox.AutoSize = true;
            this.CreateFilesCheckBox.Location = new System.Drawing.Point(10, 19);
            this.CreateFilesCheckBox.Name = "CreateFilesCheckBox";
            this.CreateFilesCheckBox.Size = new System.Drawing.Size(78, 17);
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
            this.AutoSplitGroup.Size = new System.Drawing.Size(636, 357);
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
            this.AutoSplitLayout.Size = new System.Drawing.Size(630, 338);
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
            this.AutoSplitToolbar.Location = new System.Drawing.Point(3, 304);
            this.AutoSplitToolbar.Name = "AutoSplitToolbar";
            this.AutoSplitToolbar.Size = new System.Drawing.Size(624, 31);
            this.AutoSplitToolbar.TabIndex = 20;
            // 
            // AutoSplitHotkeyLabel
            // 
            this.AutoSplitHotkeyLabel.AutoSize = true;
            this.AutoSplitHotkeyLabel.Location = new System.Drawing.Point(3, 10);
            this.AutoSplitHotkeyLabel.Name = "AutoSplitHotkeyLabel";
            this.AutoSplitHotkeyLabel.Size = new System.Drawing.Size(67, 13);
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
            this.EnableAutosplitCheckBox.Location = new System.Drawing.Point(477, 9);
            this.EnableAutosplitCheckBox.Name = "EnableAutosplitCheckBox";
            this.EnableAutosplitCheckBox.Size = new System.Drawing.Size(59, 17);
            this.EnableAutosplitCheckBox.TabIndex = 1;
            this.EnableAutosplitCheckBox.Text = "Enable";
            this.EnableAutosplitCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAutoSplitButton
            // 
            this.AddAutoSplitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAutoSplitButton.Location = new System.Drawing.Point(542, 5);
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
            this.CheckUpdatesCheckBox.Size = new System.Drawing.Size(148, 17);
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
            this.VersionLabel.Size = new System.Drawing.Size(45, 13);
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
            this.RuneDisplayGroup.Controls.Add(this.btnAddRuneWord);
            this.RuneDisplayGroup.Controls.Add(this.cbRuneWord);
            this.RuneDisplayGroup.Controls.Add(this.RuneDisplayPanel);
            this.RuneDisplayGroup.Controls.Add(this.AddRuneButton);
            this.RuneDisplayGroup.Controls.Add(this.RuneComboBox);
            this.RuneDisplayGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RuneDisplayGroup.Location = new System.Drawing.Point(3, 366);
            this.RuneDisplayGroup.Name = "RuneDisplayGroup";
            this.RuneDisplayGroup.Size = new System.Drawing.Size(636, 110);
            this.RuneDisplayGroup.TabIndex = 19;
            this.RuneDisplayGroup.TabStop = false;
            this.RuneDisplayGroup.Text = "Rune Display";
            // 
            // btnAddRuneWord
            // 
            this.btnAddRuneWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRuneWord.Location = new System.Drawing.Point(531, 82);
            this.btnAddRuneWord.Name = "btnAddRuneWord";
            this.btnAddRuneWord.Size = new System.Drawing.Size(96, 23);
            this.btnAddRuneWord.TabIndex = 4;
            this.btnAddRuneWord.Text = "Add Runeword";
            this.btnAddRuneWord.UseVisualStyleBackColor = true;
            this.btnAddRuneWord.Click += new System.EventHandler(this.btnAddRuneWord_Click);
            // 
            // cbRuneWord
            // 
            this.cbRuneWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRuneWord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRuneWord.FormattingEnabled = true;
            this.cbRuneWord.Location = new System.Drawing.Point(425, 84);
            this.cbRuneWord.Name = "cbRuneWord";
            this.cbRuneWord.Size = new System.Drawing.Size(100, 21);
            this.cbRuneWord.TabIndex = 3;
            // 
            // RuneDisplayPanel
            // 
            this.RuneDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RuneDisplayPanel.AutoScroll = true;
            this.RuneDisplayPanel.Location = new System.Drawing.Point(6, 19);
            this.RuneDisplayPanel.Name = "RuneDisplayPanel";
            this.RuneDisplayPanel.Size = new System.Drawing.Size(621, 57);
            this.RuneDisplayPanel.TabIndex = 2;
            // 
            // AddRuneButton
            // 
            this.AddRuneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddRuneButton.Location = new System.Drawing.Point(340, 83);
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
            this.RuneComboBox.Location = new System.Drawing.Point(275, 84);
            this.RuneComboBox.Name = "RuneComboBox";
            this.RuneComboBox.Size = new System.Drawing.Size(59, 21);
            this.RuneComboBox.TabIndex = 0;
            this.RuneComboBox.SelectedIndexChanged += new System.EventHandler(this.RuneComboBox_SelectedIndexChanged);
            // 
            // VerticalSplitContainer
            // 
            this.VerticalSplitContainer.ColumnCount = 3;
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.VerticalSplitContainer.Controls.Add(this.HorizontalSplitContainer, 1, 0);
            this.VerticalSplitContainer.Controls.Add(this.RightPanelLayout, 2, 0);
            this.VerticalSplitContainer.Controls.Add(this.grpConfigFiles, 0, 0);
            this.VerticalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerticalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.VerticalSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.VerticalSplitContainer.Name = "VerticalSplitContainer";
            this.VerticalSplitContainer.RowCount = 1;
            this.VerticalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.Size = new System.Drawing.Size(1063, 485);
            this.VerticalSplitContainer.TabIndex = 20;
            // 
            // HorizontalSplitContainer
            // 
            this.HorizontalSplitContainer.ColumnCount = 1;
            this.HorizontalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.Controls.Add(this.AutoSplitGroup, 0, 0);
            this.HorizontalSplitContainer.Controls.Add(this.RuneDisplayGroup, 0, 1);
            this.HorizontalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HorizontalSplitContainer.Location = new System.Drawing.Point(178, 3);
            this.HorizontalSplitContainer.Name = "HorizontalSplitContainer";
            this.HorizontalSplitContainer.RowCount = 2;
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.HorizontalSplitContainer.Size = new System.Drawing.Size(642, 479);
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
            this.RightPanelLayout.Location = new System.Drawing.Point(826, 3);
            this.RightPanelLayout.Name = "RightPanelLayout";
            this.RightPanelLayout.Size = new System.Drawing.Size(234, 479);
            this.RightPanelLayout.TabIndex = 1;
            // 
            // groupBox1
            // 
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
            this.groupBox1.Size = new System.Drawing.Size(231, 109);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display";
            // 
            // chkDisplayRunes
            // 
            this.chkDisplayRunes.AutoSize = true;
            this.chkDisplayRunes.Location = new System.Drawing.Point(141, 88);
            this.chkDisplayRunes.Name = "chkDisplayRunes";
            this.chkDisplayRunes.Size = new System.Drawing.Size(57, 17);
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
            this.chkDisplayAdvancedStats.Size = new System.Drawing.Size(105, 17);
            this.chkDisplayAdvancedStats.TabIndex = 5;
            this.chkDisplayAdvancedStats.Text = "Fcr, Frw, Fhr, Ias";
            this.chkDisplayAdvancedStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayLevel
            // 
            this.chkDisplayLevel.AutoSize = true;
            this.chkDisplayLevel.Location = new System.Drawing.Point(141, 42);
            this.chkDisplayLevel.Name = "chkDisplayLevel";
            this.chkDisplayLevel.Size = new System.Drawing.Size(52, 17);
            this.chkDisplayLevel.TabIndex = 4;
            this.chkDisplayLevel.Text = "Level";
            this.chkDisplayLevel.UseVisualStyleBackColor = true;
            // 
            // chkDisplayGold
            // 
            this.chkDisplayGold.AutoSize = true;
            this.chkDisplayGold.Location = new System.Drawing.Point(10, 42);
            this.chkDisplayGold.Name = "chkDisplayGold";
            this.chkDisplayGold.Size = new System.Drawing.Size(48, 17);
            this.chkDisplayGold.TabIndex = 4;
            this.chkDisplayGold.Text = "Gold";
            this.chkDisplayGold.UseVisualStyleBackColor = true;
            // 
            // chkDisplayResistances
            // 
            this.chkDisplayResistances.AutoSize = true;
            this.chkDisplayResistances.Location = new System.Drawing.Point(141, 65);
            this.chkDisplayResistances.Name = "chkDisplayResistances";
            this.chkDisplayResistances.Size = new System.Drawing.Size(84, 17);
            this.chkDisplayResistances.TabIndex = 3;
            this.chkDisplayResistances.Text = "Resistances";
            this.chkDisplayResistances.UseVisualStyleBackColor = true;
            // 
            // chkDisplayBaseStats
            // 
            this.chkDisplayBaseStats.AutoSize = true;
            this.chkDisplayBaseStats.Location = new System.Drawing.Point(10, 65);
            this.chkDisplayBaseStats.Name = "chkDisplayBaseStats";
            this.chkDisplayBaseStats.Size = new System.Drawing.Size(107, 17);
            this.chkDisplayBaseStats.TabIndex = 2;
            this.chkDisplayBaseStats.Text = "Str, Dex, Vit, Ene";
            this.chkDisplayBaseStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayDeathCounter
            // 
            this.chkDisplayDeathCounter.AutoSize = true;
            this.chkDisplayDeathCounter.Location = new System.Drawing.Point(141, 19);
            this.chkDisplayDeathCounter.Name = "chkDisplayDeathCounter";
            this.chkDisplayDeathCounter.Size = new System.Drawing.Size(60, 17);
            this.chkDisplayDeathCounter.TabIndex = 1;
            this.chkDisplayDeathCounter.Text = "Deaths";
            this.chkDisplayDeathCounter.UseVisualStyleBackColor = true;
            // 
            // chkDisplayName
            // 
            this.chkDisplayName.AutoSize = true;
            this.chkDisplayName.Location = new System.Drawing.Point(10, 19);
            this.chkDisplayName.Name = "chkDisplayName";
            this.chkDisplayName.Size = new System.Drawing.Size(54, 17);
            this.chkDisplayName.TabIndex = 0;
            this.chkDisplayName.Text = "Name";
            this.chkDisplayName.UseVisualStyleBackColor = true;
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 1;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.Controls.Add(this.VerticalSplitContainer, 0, 0);
            this.mainPanel.Controls.Add(this.panel1, 0, 1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 2;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainPanel.Size = new System.Drawing.Size(1063, 517);
            this.mainPanel.TabIndex = 23;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.btnUndo);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 485);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1063, 32);
            this.panel1.TabIndex = 21;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(893, 6);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.TabIndex = 1;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.SaveSettingsAsMenuItem_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Location = new System.Drawing.Point(970, 6);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(90, 23);
            this.btnUndo.TabIndex = 0;
            this.btnUndo.Text = "Undo Changes";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(815, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lstConfigFiles
            // 
            this.lstConfigFiles.ContextMenuStrip = this.ctxConfigFileList;
            this.lstConfigFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConfigFiles.FormattingEnabled = true;
            this.lstConfigFiles.Location = new System.Drawing.Point(3, 16);
            this.lstConfigFiles.Name = "lstConfigFiles";
            this.lstConfigFiles.Size = new System.Drawing.Size(163, 460);
            this.lstConfigFiles.TabIndex = 2;
            this.lstConfigFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstConfigFiles_MouseDoubleClick);
            this.lstConfigFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstConfigFiles_MouseUp);
            // 
            // grpConfigFiles
            // 
            this.grpConfigFiles.Controls.Add(this.lstConfigFiles);
            this.grpConfigFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpConfigFiles.Location = new System.Drawing.Point(3, 3);
            this.grpConfigFiles.Name = "grpConfigFiles";
            this.grpConfigFiles.Size = new System.Drawing.Size(169, 479);
            this.grpConfigFiles.TabIndex = 3;
            this.grpConfigFiles.TabStop = false;
            this.grpConfigFiles.Text = "Config Files";
            // 
            // ctxConfigFileList
            // 
            this.ctxConfigFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuLoad,
            this.menuClone,
            this.menuDelete});
            this.ctxConfigFileList.Name = "ctxConfigFileList";
            this.ctxConfigFileList.Size = new System.Drawing.Size(108, 92);
            // 
            // menuLoad
            // 
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.Size = new System.Drawing.Size(107, 22);
            this.menuLoad.Text = "Load";
            this.menuLoad.Click += new System.EventHandler(this.menuLoad_Click);
            // 
            // menuClone
            // 
            this.menuClone.Name = "menuClone";
            this.menuClone.Size = new System.Drawing.Size(107, 22);
            this.menuClone.Text = "Clone";
            this.menuClone.Click += new System.EventHandler(this.menuClone_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(107, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // menuNew
            // 
            this.menuNew.Name = "menuNew";
            this.menuNew.Size = new System.Drawing.Size(107, 22);
            this.menuNew.Text = "New";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
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
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 517);
            this.Controls.Add(this.mainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(794, 555);
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
            this.VerticalSplitContainer.ResumeLayout(false);
            this.HorizontalSplitContainer.ResumeLayout(false);
            this.RightPanelLayout.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpConfigFiles.ResumeLayout(false);
            this.ctxConfigFileList.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.NumericUpDown fontSizeNumeric;
        private System.Windows.Forms.NumericUpDown titleFontSizeNumeric;
        private Gui.Controls.FontComboBox fontComboBox;
        private Controls.HotkeyControl autoSplitHotkeyControl;
        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddRuneWord;
        private System.Windows.Forms.ComboBox cbRuneWord;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.GroupBox grpConfigFiles;
        private System.Windows.Forms.ListBox lstConfigFiles;
        private System.Windows.Forms.ContextMenuStrip ctxConfigFileList;
        private System.Windows.Forms.ToolStripMenuItem menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuClone;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
    }
}