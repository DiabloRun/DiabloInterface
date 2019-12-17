using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Business.Settings;

namespace Zutatensuppe.DiabloInterface.Gui
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
            this.fontComboBox = new Zutatensuppe.DiabloInterface.Gui.Controls.FontComboBox();
            this.titleFontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.fontSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.FontSizeLabel = new System.Windows.Forms.Label();
            this.TitleFontSizeLabel = new System.Windows.Forms.Label();
            this.AutoSplitLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AutoSplitToolbar = new System.Windows.Forms.Panel();
            this.autoSplitHotkeyControl = new Zutatensuppe.DiabloInterface.Gui.Controls.HotkeyControl();
            this.AutoSplitHotkeyLabel = new System.Windows.Forms.Label();
            this.AutoSplitTestHotkeyButton = new System.Windows.Forms.Button();
            this.EnableAutosplitCheckBox = new System.Windows.Forms.CheckBox();
            this.AddAutoSplitButton = new System.Windows.Forms.Button();
            this.VerticalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.grpConfigFiles = new System.Windows.Forms.GroupBox();
            this.lstConfigFiles = new System.Windows.Forms.ListBox();
            this.ctxConfigFileList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClone = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.HorizontalSplitContainer = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSettingsLayout = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPaddingInVerticalLayout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxRunesOrientation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxLayout = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetPlayersXColor = new System.Windows.Forms.Button();
            this.btnSetGameCounterColor = new System.Windows.Forms.Button();
            this.chkShowPlayersX = new System.Windows.Forms.CheckBox();
            this.chkShowGameCounter = new System.Windows.Forms.CheckBox();
            this.chkShowRealValues = new System.Windows.Forms.CheckBox();
            this.chkHighContrastRunes = new System.Windows.Forms.CheckBox();
            this.chkDisplayRunes = new System.Windows.Forms.CheckBox();
            this.btnSetBackgroundColor = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSetLevelColor = new System.Windows.Forms.Button();
            this.btnSetDifficultyColor = new System.Windows.Forms.Button();
            this.btnSetPoisonResColor = new System.Windows.Forms.Button();
            this.btnSetLightningResColor = new System.Windows.Forms.Button();
            this.btnSetColdResColor = new System.Windows.Forms.Button();
            this.btnSetFireResColor = new System.Windows.Forms.Button();
            this.btnSetDeathsColor = new System.Windows.Forms.Button();
            this.btnSetAdvancedStatsColor = new System.Windows.Forms.Button();
            this.btnSetBaseStatsColor = new System.Windows.Forms.Button();
            this.btnSetGoldColor = new System.Windows.Forms.Button();
            this.btnSetNameColor = new System.Windows.Forms.Button();
            this.chkDisplayDifficultyPercents = new System.Windows.Forms.CheckBox();
            this.chkDisplayAdvancedStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayLevel = new System.Windows.Forms.CheckBox();
            this.chkDisplayGold = new System.Windows.Forms.CheckBox();
            this.chkDisplayResistances = new System.Windows.Forms.CheckBox();
            this.chkDisplayBaseStats = new System.Windows.Forms.CheckBox();
            this.chkDisplayDeathCounter = new System.Windows.Forms.CheckBox();
            this.chkDisplayName = new System.Windows.Forms.CheckBox();
            this.tabPageSettingsRunes = new System.Windows.Forms.TabPage();
            this.runeSettingsPage = new Zutatensuppe.DiabloInterface.Gui.Controls.RuneSettingsPage();
            this.tabPageSettingsAutosplit = new System.Windows.Forms.TabPage();
            this.tabPageSettingsMisc = new System.Windows.Forms.TabPage();
            this.groupBoxPipeServer = new System.Windows.Forms.GroupBox();
            this.textBoxPipeName = new System.Windows.Forms.TextBox();
            this.labelPipeName = new System.Windows.Forms.Label();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.CreateFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.UpdateGroup = new System.Windows.Forms.GroupBox();
            this.CheckUpdatesButton = new System.Windows.Forms.Button();
            this.CheckUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.FontGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumeric)).BeginInit();
            this.AutoSplitLayout.SuspendLayout();
            this.AutoSplitToolbar.SuspendLayout();
            this.VerticalSplitContainer.SuspendLayout();
            this.grpConfigFiles.SuspendLayout();
            this.ctxConfigFileList.SuspendLayout();
            this.HorizontalSplitContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSettingsLayout.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingInVerticalLayout)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPageSettingsRunes.SuspendLayout();
            this.tabPageSettingsAutosplit.SuspendLayout();
            this.tabPageSettingsMisc.SuspendLayout();
            this.groupBoxPipeServer.SuspendLayout();
            this.DataGroup.SuspendLayout();
            this.UpdateGroup.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(8, 30);
            this.FontLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(40, 17);
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
            this.FontGroup.Location = new System.Drawing.Point(12, 7);
            this.FontGroup.Margin = new System.Windows.Forms.Padding(0);
            this.FontGroup.Name = "FontGroup";
            this.FontGroup.Padding = new System.Windows.Forms.Padding(4);
            this.FontGroup.Size = new System.Drawing.Size(308, 128);
            this.FontGroup.TabIndex = 5;
            this.FontGroup.TabStop = false;
            this.FontGroup.Text = "Font";
            // 
            // fontComboBox
            // 
            this.fontComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontComboBox.DropDownWidth = 250;
            this.fontComboBox.FormattingEnabled = true;
            this.fontComboBox.Location = new System.Drawing.Point(140, 26);
            this.fontComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.Size = new System.Drawing.Size(155, 23);
            this.fontComboBox.Sorted = true;
            this.fontComboBox.TabIndex = 12;
            // 
            // titleFontSizeNumeric
            // 
            this.titleFontSizeNumeric.Location = new System.Drawing.Point(140, 91);
            this.titleFontSizeNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.titleFontSizeNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.titleFontSizeNumeric.Name = "titleFontSizeNumeric";
            this.titleFontSizeNumeric.Size = new System.Drawing.Size(156, 22);
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
            this.fontSizeNumeric.Location = new System.Drawing.Point(140, 59);
            this.fontSizeNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.fontSizeNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.fontSizeNumeric.Name = "fontSizeNumeric";
            this.fontSizeNumeric.Size = new System.Drawing.Size(156, 22);
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
            this.FontSizeLabel.Location = new System.Drawing.Point(8, 63);
            this.FontSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FontSizeLabel.Name = "FontSizeLabel";
            this.FontSizeLabel.Size = new System.Drawing.Size(69, 17);
            this.FontSizeLabel.TabIndex = 7;
            this.FontSizeLabel.Text = "Font size:";
            // 
            // TitleFontSizeLabel
            // 
            this.TitleFontSizeLabel.AutoSize = true;
            this.TitleFontSizeLabel.Location = new System.Drawing.Point(8, 95);
            this.TitleFontSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TitleFontSizeLabel.Name = "TitleFontSizeLabel";
            this.TitleFontSizeLabel.Size = new System.Drawing.Size(106, 17);
            this.TitleFontSizeLabel.TabIndex = 6;
            this.TitleFontSizeLabel.Text = "Name font size:";
            // 
            // AutoSplitLayout
            // 
            this.AutoSplitLayout.ColumnCount = 1;
            this.AutoSplitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutoSplitLayout.Controls.Add(this.AutoSplitToolbar, 0, 1);
            this.AutoSplitLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoSplitLayout.Location = new System.Drawing.Point(0, 0);
            this.AutoSplitLayout.Margin = new System.Windows.Forms.Padding(0);
            this.AutoSplitLayout.Name = "AutoSplitLayout";
            this.AutoSplitLayout.RowCount = 2;
            this.AutoSplitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutoSplitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AutoSplitLayout.Size = new System.Drawing.Size(666, 589);
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
            this.AutoSplitToolbar.Location = new System.Drawing.Point(0, 551);
            this.AutoSplitToolbar.Margin = new System.Windows.Forms.Padding(0);
            this.AutoSplitToolbar.Name = "AutoSplitToolbar";
            this.AutoSplitToolbar.Size = new System.Drawing.Size(666, 38);
            this.AutoSplitToolbar.TabIndex = 20;
            // 
            // autoSplitHotkeyControl
            // 
            this.autoSplitHotkeyControl.Hotkey = System.Windows.Forms.Keys.None;
            this.autoSplitHotkeyControl.Location = new System.Drawing.Point(107, 9);
            this.autoSplitHotkeyControl.Margin = new System.Windows.Forms.Padding(4);
            this.autoSplitHotkeyControl.Name = "autoSplitHotkeyControl";
            this.autoSplitHotkeyControl.Size = new System.Drawing.Size(108, 22);
            this.autoSplitHotkeyControl.TabIndex = 3;
            this.autoSplitHotkeyControl.Text = "None";
            this.autoSplitHotkeyControl.UseKeyWhitelist = true;
            this.autoSplitHotkeyControl.HotkeyChanged += new System.EventHandler<System.Windows.Forms.Keys>(this.AutoSplitHotkeyControlOnHotkeyChanged);
            // 
            // AutoSplitHotkeyLabel
            // 
            this.AutoSplitHotkeyLabel.AutoSize = true;
            this.AutoSplitHotkeyLabel.Location = new System.Drawing.Point(4, 12);
            this.AutoSplitHotkeyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AutoSplitHotkeyLabel.Name = "AutoSplitHotkeyLabel";
            this.AutoSplitHotkeyLabel.Size = new System.Drawing.Size(88, 17);
            this.AutoSplitHotkeyLabel.TabIndex = 13;
            this.AutoSplitHotkeyLabel.Text = "Split-Hotkey:";
            // 
            // AutoSplitTestHotkeyButton
            // 
            this.AutoSplitTestHotkeyButton.Location = new System.Drawing.Point(224, 6);
            this.AutoSplitTestHotkeyButton.Margin = new System.Windows.Forms.Padding(4);
            this.AutoSplitTestHotkeyButton.Name = "AutoSplitTestHotkeyButton";
            this.AutoSplitTestHotkeyButton.Size = new System.Drawing.Size(100, 28);
            this.AutoSplitTestHotkeyButton.TabIndex = 18;
            this.AutoSplitTestHotkeyButton.Text = "Test Hotkey";
            this.AutoSplitTestHotkeyButton.UseVisualStyleBackColor = true;
            this.AutoSplitTestHotkeyButton.Click += new System.EventHandler(this.AutoSplitTestHotkey_Click);
            // 
            // EnableAutosplitCheckBox
            // 
            this.EnableAutosplitCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnableAutosplitCheckBox.AutoSize = true;
            this.EnableAutosplitCheckBox.Location = new System.Drawing.Point(475, 11);
            this.EnableAutosplitCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.EnableAutosplitCheckBox.Name = "EnableAutosplitCheckBox";
            this.EnableAutosplitCheckBox.Size = new System.Drawing.Size(74, 21);
            this.EnableAutosplitCheckBox.TabIndex = 1;
            this.EnableAutosplitCheckBox.Text = "Enable";
            this.EnableAutosplitCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAutoSplitButton
            // 
            this.AddAutoSplitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAutoSplitButton.Location = new System.Drawing.Point(556, 6);
            this.AddAutoSplitButton.Margin = new System.Windows.Forms.Padding(4);
            this.AddAutoSplitButton.Name = "AddAutoSplitButton";
            this.AddAutoSplitButton.Size = new System.Drawing.Size(105, 28);
            this.AddAutoSplitButton.TabIndex = 11;
            this.AddAutoSplitButton.Text = "Add Split";
            this.AddAutoSplitButton.UseVisualStyleBackColor = true;
            this.AddAutoSplitButton.Click += new System.EventHandler(this.AddAutoSplitButton_Clicked);
            // 
            // VerticalSplitContainer
            // 
            this.VerticalSplitContainer.ColumnCount = 2;
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233F));
            this.VerticalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.Controls.Add(this.grpConfigFiles, 0, 0);
            this.VerticalSplitContainer.Controls.Add(this.HorizontalSplitContainer, 1, 0);
            this.VerticalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerticalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.VerticalSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.VerticalSplitContainer.Name = "VerticalSplitContainer";
            this.VerticalSplitContainer.RowCount = 1;
            this.VerticalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VerticalSplitContainer.Size = new System.Drawing.Size(922, 626);
            this.VerticalSplitContainer.TabIndex = 20;
            // 
            // grpConfigFiles
            // 
            this.grpConfigFiles.Controls.Add(this.lstConfigFiles);
            this.grpConfigFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpConfigFiles.Location = new System.Drawing.Point(4, 4);
            this.grpConfigFiles.Margin = new System.Windows.Forms.Padding(4);
            this.grpConfigFiles.Name = "grpConfigFiles";
            this.grpConfigFiles.Padding = new System.Windows.Forms.Padding(4);
            this.grpConfigFiles.Size = new System.Drawing.Size(225, 618);
            this.grpConfigFiles.TabIndex = 3;
            this.grpConfigFiles.TabStop = false;
            this.grpConfigFiles.Text = "Config Files";
            // 
            // lstConfigFiles
            // 
            this.lstConfigFiles.ContextMenuStrip = this.ctxConfigFileList;
            this.lstConfigFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConfigFiles.FormattingEnabled = true;
            this.lstConfigFiles.ItemHeight = 16;
            this.lstConfigFiles.Location = new System.Drawing.Point(4, 19);
            this.lstConfigFiles.Margin = new System.Windows.Forms.Padding(4);
            this.lstConfigFiles.Name = "lstConfigFiles";
            this.lstConfigFiles.Size = new System.Drawing.Size(217, 595);
            this.lstConfigFiles.TabIndex = 2;
            this.lstConfigFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstConfigFiles_MouseDoubleClick);
            this.lstConfigFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstConfigFiles_MouseUp);
            // 
            // ctxConfigFileList
            // 
            this.ctxConfigFileList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxConfigFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuLoad,
            this.renameToolStripMenuItem,
            this.menuClone,
            this.menuDelete});
            this.ctxConfigFileList.Name = "ctxConfigFileList";
            this.ctxConfigFileList.Size = new System.Drawing.Size(131, 114);
            // 
            // menuNew
            // 
            this.menuNew.Name = "menuNew";
            this.menuNew.Size = new System.Drawing.Size(130, 22);
            this.menuNew.Text = "New";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuLoad
            // 
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.Size = new System.Drawing.Size(130, 22);
            this.menuLoad.Text = "Load";
            this.menuLoad.Click += new System.EventHandler(this.menuLoad_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // menuClone
            // 
            this.menuClone.Name = "menuClone";
            this.menuClone.Size = new System.Drawing.Size(130, 22);
            this.menuClone.Text = "Clone";
            this.menuClone.Click += new System.EventHandler(this.menuClone_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(130, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // HorizontalSplitContainer
            // 
            this.HorizontalSplitContainer.ColumnCount = 1;
            this.HorizontalSplitContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.Controls.Add(this.tabControl1, 0, 0);
            this.HorizontalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HorizontalSplitContainer.Location = new System.Drawing.Point(233, 0);
            this.HorizontalSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.HorizontalSplitContainer.Name = "HorizontalSplitContainer";
            this.HorizontalSplitContainer.RowCount = 2;
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HorizontalSplitContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.HorizontalSplitContainer.Size = new System.Drawing.Size(689, 626);
            this.HorizontalSplitContainer.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSettingsLayout);
            this.tabControl1.Controls.Add(this.tabPageSettingsRunes);
            this.tabControl1.Controls.Add(this.tabPageSettingsAutosplit);
            this.tabControl1.Controls.Add(this.tabPageSettingsMisc);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(681, 618);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPageSettingsLayout
            // 
            this.tabPageSettingsLayout.Controls.Add(this.groupBox2);
            this.tabPageSettingsLayout.Controls.Add(this.FontGroup);
            this.tabPageSettingsLayout.Controls.Add(this.groupBox1);
            this.tabPageSettingsLayout.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettingsLayout.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsLayout.Name = "tabPageSettingsLayout";
            this.tabPageSettingsLayout.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsLayout.Size = new System.Drawing.Size(673, 589);
            this.tabPageSettingsLayout.TabIndex = 1;
            this.tabPageSettingsLayout.Text = "Layout";
            this.tabPageSettingsLayout.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownPaddingInVerticalLayout);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxRunesOrientation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxLayout);
            this.groupBox2.Location = new System.Drawing.Point(332, 7);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(321, 128);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layout";
            // 
            // numericUpDownPaddingInVerticalLayout
            // 
            this.numericUpDownPaddingInVerticalLayout.Location = new System.Drawing.Point(193, 92);
            this.numericUpDownPaddingInVerticalLayout.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownPaddingInVerticalLayout.Name = "numericUpDownPaddingInVerticalLayout";
            this.numericUpDownPaddingInVerticalLayout.Size = new System.Drawing.Size(107, 22);
            this.numericUpDownPaddingInVerticalLayout.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "Runes orientation:";
            // 
            // comboBoxRunesOrientation
            // 
            this.comboBoxRunesOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRunesOrientation.FormattingEnabled = true;
            this.comboBoxRunesOrientation.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.comboBoxRunesOrientation.Location = new System.Drawing.Point(193, 59);
            this.comboBoxRunesOrientation.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxRunesOrientation.Name = "comboBoxRunesOrientation";
            this.comboBoxRunesOrientation.Size = new System.Drawing.Size(105, 24);
            this.comboBoxRunesOrientation.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "Orientation:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 95);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Padding in vertical layout:";
            // 
            // comboBoxLayout
            // 
            this.comboBoxLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayout.FormattingEnabled = true;
            this.comboBoxLayout.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.comboBoxLayout.Location = new System.Drawing.Point(193, 26);
            this.comboBoxLayout.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLayout.Name = "comboBoxLayout";
            this.comboBoxLayout.Size = new System.Drawing.Size(105, 24);
            this.comboBoxLayout.TabIndex = 22;
            this.comboBoxLayout.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayout_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetPlayersXColor);
            this.groupBox1.Controls.Add(this.btnSetGameCounterColor);
            this.groupBox1.Controls.Add(this.chkShowPlayersX);
            this.groupBox1.Controls.Add(this.chkShowGameCounter);
            this.groupBox1.Controls.Add(this.chkShowRealValues);
            this.groupBox1.Controls.Add(this.chkHighContrastRunes);
            this.groupBox1.Controls.Add(this.chkDisplayRunes);
            this.groupBox1.Controls.Add(this.btnSetBackgroundColor);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnSetLevelColor);
            this.groupBox1.Controls.Add(this.btnSetDifficultyColor);
            this.groupBox1.Controls.Add(this.btnSetPoisonResColor);
            this.groupBox1.Controls.Add(this.btnSetLightningResColor);
            this.groupBox1.Controls.Add(this.btnSetColdResColor);
            this.groupBox1.Controls.Add(this.btnSetFireResColor);
            this.groupBox1.Controls.Add(this.btnSetDeathsColor);
            this.groupBox1.Controls.Add(this.btnSetAdvancedStatsColor);
            this.groupBox1.Controls.Add(this.btnSetBaseStatsColor);
            this.groupBox1.Controls.Add(this.btnSetGoldColor);
            this.groupBox1.Controls.Add(this.btnSetNameColor);
            this.groupBox1.Controls.Add(this.chkDisplayDifficultyPercents);
            this.groupBox1.Controls.Add(this.chkDisplayAdvancedStats);
            this.groupBox1.Controls.Add(this.chkDisplayLevel);
            this.groupBox1.Controls.Add(this.chkDisplayGold);
            this.groupBox1.Controls.Add(this.chkDisplayResistances);
            this.groupBox1.Controls.Add(this.chkDisplayBaseStats);
            this.groupBox1.Controls.Add(this.chkDisplayDeathCounter);
            this.groupBox1.Controls.Add(this.chkDisplayName);
            this.groupBox1.Location = new System.Drawing.Point(12, 135);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(641, 450);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display";
            // 
            // btnSetPlayersXColor
            // 
            this.btnSetPlayersXColor.Location = new System.Drawing.Point(169, 335);
            this.btnSetPlayersXColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetPlayersXColor.Name = "btnSetPlayersXColor";
            this.btnSetPlayersXColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetPlayersXColor.TabIndex = 29;
            this.btnSetPlayersXColor.Text = "Color";
            this.btnSetPlayersXColor.UseVisualStyleBackColor = true;
            this.btnSetPlayersXColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetGameCounterColor
            // 
            this.btnSetGameCounterColor.Location = new System.Drawing.Point(169, 376);
            this.btnSetGameCounterColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetGameCounterColor.Name = "btnSetGameCounterColor";
            this.btnSetGameCounterColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetGameCounterColor.TabIndex = 30;
            this.btnSetGameCounterColor.Text = "Color";
            this.btnSetGameCounterColor.UseVisualStyleBackColor = true;
            this.btnSetGameCounterColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // chkShowPlayersX
            // 
            this.chkShowPlayersX.AutoSize = true;
            this.chkShowPlayersX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkShowPlayersX.Location = new System.Drawing.Point(19, 341);
            this.chkShowPlayersX.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowPlayersX.Name = "chkShowPlayersX";
            this.chkShowPlayersX.Size = new System.Drawing.Size(93, 21);
            this.chkShowPlayersX.TabIndex = 31;
            this.chkShowPlayersX.Text = "/players X";
            this.chkShowPlayersX.UseVisualStyleBackColor = true;
            // 
            // chkShowGameCounter
            // 
            this.chkShowGameCounter.AutoSize = true;
            this.chkShowGameCounter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkShowGameCounter.Location = new System.Drawing.Point(19, 382);
            this.chkShowGameCounter.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowGameCounter.Name = "chkShowGameCounter";
            this.chkShowGameCounter.Size = new System.Drawing.Size(95, 21);
            this.chkShowGameCounter.TabIndex = 28;
            this.chkShowGameCounter.Text = "Run count";
            this.chkShowGameCounter.UseVisualStyleBackColor = true;
            // 
            // chkShowRealValues
            // 
            this.chkShowRealValues.AutoSize = true;
            this.chkShowRealValues.Location = new System.Drawing.Point(276, 148);
            this.chkShowRealValues.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowRealValues.Name = "chkShowRealValues";
            this.chkShowRealValues.Size = new System.Drawing.Size(216, 21);
            this.chkShowRealValues.TabIndex = 27;
            this.chkShowRealValues.Text = "show real values for FRW/IAS";
            this.chkShowRealValues.UseVisualStyleBackColor = true;
            // 
            // chkHighContrastRunes
            // 
            this.chkHighContrastRunes.AutoSize = true;
            this.chkHighContrastRunes.Location = new System.Drawing.Point(169, 420);
            this.chkHighContrastRunes.Margin = new System.Windows.Forms.Padding(4);
            this.chkHighContrastRunes.Name = "chkHighContrastRunes";
            this.chkHighContrastRunes.Size = new System.Drawing.Size(114, 21);
            this.chkHighContrastRunes.TabIndex = 25;
            this.chkHighContrastRunes.Text = "High contrast";
            this.chkHighContrastRunes.UseVisualStyleBackColor = true;
            // 
            // chkDisplayRunes
            // 
            this.chkDisplayRunes.AutoSize = true;
            this.chkDisplayRunes.Location = new System.Drawing.Point(19, 420);
            this.chkDisplayRunes.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayRunes.Name = "chkDisplayRunes";
            this.chkDisplayRunes.Size = new System.Drawing.Size(71, 21);
            this.chkDisplayRunes.TabIndex = 26;
            this.chkDisplayRunes.Text = "Runes";
            this.chkDisplayRunes.UseVisualStyleBackColor = true;
            // 
            // btnSetBackgroundColor
            // 
            this.btnSetBackgroundColor.Location = new System.Drawing.Point(468, 334);
            this.btnSetBackgroundColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetBackgroundColor.Name = "btnSetBackgroundColor";
            this.btnSetBackgroundColor.Size = new System.Drawing.Size(150, 28);
            this.btnSetBackgroundColor.TabIndex = 23;
            this.btnSetBackgroundColor.Text = "Background color";
            this.btnSetBackgroundColor.UseVisualStyleBackColor = true;
            this.btnSetBackgroundColor.Click += new System.EventHandler(this.backgroundColorButtonClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(468, 379);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "Reset to default colors";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.resetColorsButton);
            // 
            // btnSetLevelColor
            // 
            this.btnSetLevelColor.Location = new System.Drawing.Point(169, 218);
            this.btnSetLevelColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetLevelColor.Name = "btnSetLevelColor";
            this.btnSetLevelColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetLevelColor.TabIndex = 7;
            this.btnSetLevelColor.Text = "Color";
            this.btnSetLevelColor.UseVisualStyleBackColor = true;
            this.btnSetLevelColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetDifficultyColor
            // 
            this.btnSetDifficultyColor.Location = new System.Drawing.Point(169, 294);
            this.btnSetDifficultyColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetDifficultyColor.Name = "btnSetDifficultyColor";
            this.btnSetDifficultyColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetDifficultyColor.TabIndex = 7;
            this.btnSetDifficultyColor.Text = "Color";
            this.btnSetDifficultyColor.UseVisualStyleBackColor = true;
            this.btnSetDifficultyColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetPoisonResColor
            // 
            this.btnSetPoisonResColor.Location = new System.Drawing.Point(489, 256);
            this.btnSetPoisonResColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetPoisonResColor.Name = "btnSetPoisonResColor";
            this.btnSetPoisonResColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetPoisonResColor.TabIndex = 7;
            this.btnSetPoisonResColor.Text = "Poison color";
            this.btnSetPoisonResColor.UseVisualStyleBackColor = true;
            this.btnSetPoisonResColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetLightningResColor
            // 
            this.btnSetLightningResColor.Location = new System.Drawing.Point(383, 256);
            this.btnSetLightningResColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetLightningResColor.Name = "btnSetLightningResColor";
            this.btnSetLightningResColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetLightningResColor.TabIndex = 7;
            this.btnSetLightningResColor.Text = "Light. color";
            this.btnSetLightningResColor.UseVisualStyleBackColor = true;
            this.btnSetLightningResColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetColdResColor
            // 
            this.btnSetColdResColor.Location = new System.Drawing.Point(276, 256);
            this.btnSetColdResColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetColdResColor.Name = "btnSetColdResColor";
            this.btnSetColdResColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetColdResColor.TabIndex = 7;
            this.btnSetColdResColor.Text = "Cold color";
            this.btnSetColdResColor.UseVisualStyleBackColor = true;
            this.btnSetColdResColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetFireResColor
            // 
            this.btnSetFireResColor.Location = new System.Drawing.Point(169, 256);
            this.btnSetFireResColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetFireResColor.Name = "btnSetFireResColor";
            this.btnSetFireResColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetFireResColor.TabIndex = 7;
            this.btnSetFireResColor.Text = "Fire color";
            this.btnSetFireResColor.UseVisualStyleBackColor = true;
            this.btnSetFireResColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetDeathsColor
            // 
            this.btnSetDeathsColor.Location = new System.Drawing.Point(169, 180);
            this.btnSetDeathsColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetDeathsColor.Name = "btnSetDeathsColor";
            this.btnSetDeathsColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetDeathsColor.TabIndex = 7;
            this.btnSetDeathsColor.Text = "Color";
            this.btnSetDeathsColor.UseVisualStyleBackColor = true;
            this.btnSetDeathsColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetAdvancedStatsColor
            // 
            this.btnSetAdvancedStatsColor.Location = new System.Drawing.Point(169, 142);
            this.btnSetAdvancedStatsColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetAdvancedStatsColor.Name = "btnSetAdvancedStatsColor";
            this.btnSetAdvancedStatsColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetAdvancedStatsColor.TabIndex = 7;
            this.btnSetAdvancedStatsColor.Text = "Color";
            this.btnSetAdvancedStatsColor.UseVisualStyleBackColor = true;
            this.btnSetAdvancedStatsColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetBaseStatsColor
            // 
            this.btnSetBaseStatsColor.Location = new System.Drawing.Point(169, 103);
            this.btnSetBaseStatsColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetBaseStatsColor.Name = "btnSetBaseStatsColor";
            this.btnSetBaseStatsColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetBaseStatsColor.TabIndex = 7;
            this.btnSetBaseStatsColor.Text = "Color";
            this.btnSetBaseStatsColor.UseVisualStyleBackColor = true;
            this.btnSetBaseStatsColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetGoldColor
            // 
            this.btnSetGoldColor.Location = new System.Drawing.Point(169, 64);
            this.btnSetGoldColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetGoldColor.Name = "btnSetGoldColor";
            this.btnSetGoldColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetGoldColor.TabIndex = 7;
            this.btnSetGoldColor.Text = "Color";
            this.btnSetGoldColor.UseVisualStyleBackColor = true;
            this.btnSetGoldColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // btnSetNameColor
            // 
            this.btnSetNameColor.Location = new System.Drawing.Point(169, 23);
            this.btnSetNameColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetNameColor.Name = "btnSetNameColor";
            this.btnSetNameColor.Size = new System.Drawing.Size(99, 31);
            this.btnSetNameColor.TabIndex = 7;
            this.btnSetNameColor.Text = "Color";
            this.btnSetNameColor.UseVisualStyleBackColor = true;
            this.btnSetNameColor.Click += new System.EventHandler(this.btnSelectColor);
            // 
            // chkDisplayDifficultyPercents
            // 
            this.chkDisplayDifficultyPercents.AutoSize = true;
            this.chkDisplayDifficultyPercents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDisplayDifficultyPercents.Location = new System.Drawing.Point(19, 300);
            this.chkDisplayDifficultyPercents.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayDifficultyPercents.Name = "chkDisplayDifficultyPercents";
            this.chkDisplayDifficultyPercents.Size = new System.Drawing.Size(99, 21);
            this.chkDisplayDifficultyPercents.TabIndex = 5;
            this.chkDisplayDifficultyPercents.Text = "Difficulty %";
            this.chkDisplayDifficultyPercents.UseVisualStyleBackColor = true;
            // 
            // chkDisplayAdvancedStats
            // 
            this.chkDisplayAdvancedStats.AutoSize = true;
            this.chkDisplayAdvancedStats.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDisplayAdvancedStats.Location = new System.Drawing.Point(19, 148);
            this.chkDisplayAdvancedStats.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayAdvancedStats.Name = "chkDisplayAdvancedStats";
            this.chkDisplayAdvancedStats.Size = new System.Drawing.Size(135, 21);
            this.chkDisplayAdvancedStats.TabIndex = 5;
            this.chkDisplayAdvancedStats.Text = "Fcr, Frw, Fhr, Ias";
            this.chkDisplayAdvancedStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayLevel
            // 
            this.chkDisplayLevel.AutoSize = true;
            this.chkDisplayLevel.Location = new System.Drawing.Point(19, 224);
            this.chkDisplayLevel.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayLevel.Name = "chkDisplayLevel";
            this.chkDisplayLevel.Size = new System.Drawing.Size(130, 21);
            this.chkDisplayLevel.TabIndex = 4;
            this.chkDisplayLevel.Text = "Character Level";
            this.chkDisplayLevel.UseVisualStyleBackColor = true;
            // 
            // chkDisplayGold
            // 
            this.chkDisplayGold.AutoSize = true;
            this.chkDisplayGold.Location = new System.Drawing.Point(19, 70);
            this.chkDisplayGold.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayGold.Name = "chkDisplayGold";
            this.chkDisplayGold.Size = new System.Drawing.Size(60, 21);
            this.chkDisplayGold.TabIndex = 4;
            this.chkDisplayGold.Text = "Gold";
            this.chkDisplayGold.UseVisualStyleBackColor = true;
            // 
            // chkDisplayResistances
            // 
            this.chkDisplayResistances.AutoSize = true;
            this.chkDisplayResistances.Location = new System.Drawing.Point(19, 262);
            this.chkDisplayResistances.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayResistances.Name = "chkDisplayResistances";
            this.chkDisplayResistances.Size = new System.Drawing.Size(107, 21);
            this.chkDisplayResistances.TabIndex = 3;
            this.chkDisplayResistances.Text = "Resistances";
            this.chkDisplayResistances.UseVisualStyleBackColor = true;
            // 
            // chkDisplayBaseStats
            // 
            this.chkDisplayBaseStats.AutoSize = true;
            this.chkDisplayBaseStats.Location = new System.Drawing.Point(19, 110);
            this.chkDisplayBaseStats.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayBaseStats.Name = "chkDisplayBaseStats";
            this.chkDisplayBaseStats.Size = new System.Drawing.Size(137, 21);
            this.chkDisplayBaseStats.TabIndex = 2;
            this.chkDisplayBaseStats.Text = "Str, Dex, Vit, Ene";
            this.chkDisplayBaseStats.UseVisualStyleBackColor = true;
            // 
            // chkDisplayDeathCounter
            // 
            this.chkDisplayDeathCounter.AutoSize = true;
            this.chkDisplayDeathCounter.Location = new System.Drawing.Point(19, 186);
            this.chkDisplayDeathCounter.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayDeathCounter.Name = "chkDisplayDeathCounter";
            this.chkDisplayDeathCounter.Size = new System.Drawing.Size(75, 21);
            this.chkDisplayDeathCounter.TabIndex = 1;
            this.chkDisplayDeathCounter.Text = "Deaths";
            this.chkDisplayDeathCounter.UseVisualStyleBackColor = true;
            // 
            // chkDisplayName
            // 
            this.chkDisplayName.AutoSize = true;
            this.chkDisplayName.Location = new System.Drawing.Point(19, 30);
            this.chkDisplayName.Margin = new System.Windows.Forms.Padding(4);
            this.chkDisplayName.Name = "chkDisplayName";
            this.chkDisplayName.Size = new System.Drawing.Size(67, 21);
            this.chkDisplayName.TabIndex = 0;
            this.chkDisplayName.Text = "Name";
            this.chkDisplayName.UseVisualStyleBackColor = true;
            // 
            // tabPageSettingsRunes
            // 
            this.tabPageSettingsRunes.Controls.Add(this.runeSettingsPage);
            this.tabPageSettingsRunes.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettingsRunes.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsRunes.Name = "tabPageSettingsRunes";
            this.tabPageSettingsRunes.Size = new System.Drawing.Size(666, 589);
            this.tabPageSettingsRunes.TabIndex = 2;
            this.tabPageSettingsRunes.Text = "Runes";
            this.tabPageSettingsRunes.UseVisualStyleBackColor = true;
            // 
            // runeSettingsPage
            // 
            this.runeSettingsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runeSettingsPage.Location = new System.Drawing.Point(0, 0);
            this.runeSettingsPage.Margin = new System.Windows.Forms.Padding(5);
            this.runeSettingsPage.Name = "runeSettingsPage";
            this.runeSettingsPage.SettingsList = ((System.Collections.Generic.IReadOnlyList<Zutatensuppe.DiabloInterface.Business.Settings.ClassRuneSettings>)(resources.GetObject("runeSettingsPage.SettingsList")));
            this.runeSettingsPage.Size = new System.Drawing.Size(666, 589);
            this.runeSettingsPage.TabIndex = 0;
            // 
            // tabPageSettingsAutosplit
            // 
            this.tabPageSettingsAutosplit.Controls.Add(this.AutoSplitLayout);
            this.tabPageSettingsAutosplit.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettingsAutosplit.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsAutosplit.Name = "tabPageSettingsAutosplit";
            this.tabPageSettingsAutosplit.Size = new System.Drawing.Size(666, 589);
            this.tabPageSettingsAutosplit.TabIndex = 0;
            this.tabPageSettingsAutosplit.Text = "Auto-Split";
            this.tabPageSettingsAutosplit.UseVisualStyleBackColor = true;
            // 
            // tabPageSettingsMisc
            // 
            this.tabPageSettingsMisc.Controls.Add(this.groupBoxPipeServer);
            this.tabPageSettingsMisc.Controls.Add(this.DataGroup);
            this.tabPageSettingsMisc.Controls.Add(this.UpdateGroup);
            this.tabPageSettingsMisc.Location = new System.Drawing.Point(4, 25);
            this.tabPageSettingsMisc.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsMisc.Name = "tabPageSettingsMisc";
            this.tabPageSettingsMisc.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSettingsMisc.Size = new System.Drawing.Size(666, 589);
            this.tabPageSettingsMisc.TabIndex = 3;
            this.tabPageSettingsMisc.Text = "Misc";
            this.tabPageSettingsMisc.UseVisualStyleBackColor = true;
            // 
            // groupBoxPipeServer
            // 
            this.groupBoxPipeServer.Controls.Add(this.textBoxPipeName);
            this.groupBoxPipeServer.Controls.Add(this.labelPipeName);
            this.groupBoxPipeServer.Location = new System.Drawing.Point(7, 72);
            this.groupBoxPipeServer.Name = "groupBoxPipeServer";
            this.groupBoxPipeServer.Size = new System.Drawing.Size(366, 65);
            this.groupBoxPipeServer.TabIndex = 19;
            this.groupBoxPipeServer.TabStop = false;
            this.groupBoxPipeServer.Text = "Pipe Server";
            // 
            // textBoxPipeName
            // 
            this.textBoxPipeName.Location = new System.Drawing.Point(91, 30);
            this.textBoxPipeName.Name = "textBoxPipeName";
            this.textBoxPipeName.Size = new System.Drawing.Size(257, 22);
            this.textBoxPipeName.TabIndex = 1;
            // 
            // labelPipeName
            // 
            this.labelPipeName.AutoSize = true;
            this.labelPipeName.Location = new System.Drawing.Point(4, 30);
            this.labelPipeName.Name = "labelPipeName";
            this.labelPipeName.Size = new System.Drawing.Size(81, 17);
            this.labelPipeName.TabIndex = 0;
            this.labelPipeName.Text = "Pipe Name:";
            // 
            // DataGroup
            // 
            this.DataGroup.Controls.Add(this.CreateFilesCheckBox);
            this.DataGroup.Location = new System.Drawing.Point(4, 4);
            this.DataGroup.Margin = new System.Windows.Forms.Padding(0);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Padding = new System.Windows.Forms.Padding(4);
            this.DataGroup.Size = new System.Drawing.Size(369, 53);
            this.DataGroup.TabIndex = 17;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "Files";
            // 
            // CreateFilesCheckBox
            // 
            this.CreateFilesCheckBox.AutoSize = true;
            this.CreateFilesCheckBox.Location = new System.Drawing.Point(13, 23);
            this.CreateFilesCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.CreateFilesCheckBox.Name = "CreateFilesCheckBox";
            this.CreateFilesCheckBox.Size = new System.Drawing.Size(101, 21);
            this.CreateFilesCheckBox.TabIndex = 8;
            this.CreateFilesCheckBox.Text = "Create files";
            this.CreateFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // UpdateGroup
            // 
            this.UpdateGroup.Controls.Add(this.CheckUpdatesButton);
            this.UpdateGroup.Controls.Add(this.CheckUpdatesCheckBox);
            this.UpdateGroup.Location = new System.Drawing.Point(4, 140);
            this.UpdateGroup.Margin = new System.Windows.Forms.Padding(0);
            this.UpdateGroup.Name = "UpdateGroup";
            this.UpdateGroup.Padding = new System.Windows.Forms.Padding(4);
            this.UpdateGroup.Size = new System.Drawing.Size(369, 86);
            this.UpdateGroup.TabIndex = 18;
            this.UpdateGroup.TabStop = false;
            this.UpdateGroup.Text = "Updates";
            // 
            // CheckUpdatesButton
            // 
            this.CheckUpdatesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckUpdatesButton.Location = new System.Drawing.Point(8, 52);
            this.CheckUpdatesButton.Margin = new System.Windows.Forms.Padding(4);
            this.CheckUpdatesButton.Name = "CheckUpdatesButton";
            this.CheckUpdatesButton.Size = new System.Drawing.Size(353, 28);
            this.CheckUpdatesButton.TabIndex = 2;
            this.CheckUpdatesButton.Text = "Check for updates now";
            this.CheckUpdatesButton.UseVisualStyleBackColor = true;
            this.CheckUpdatesButton.Click += new System.EventHandler(this.CheckUpdatesButton_Click);
            // 
            // CheckUpdatesCheckBox
            // 
            this.CheckUpdatesCheckBox.AutoSize = true;
            this.CheckUpdatesCheckBox.Location = new System.Drawing.Point(13, 23);
            this.CheckUpdatesCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.CheckUpdatesCheckBox.Name = "CheckUpdatesCheckBox";
            this.CheckUpdatesCheckBox.Size = new System.Drawing.Size(193, 21);
            this.CheckUpdatesCheckBox.TabIndex = 1;
            this.CheckUpdatesCheckBox.Text = "Check for updates at start";
            this.CheckUpdatesCheckBox.UseVisualStyleBackColor = true;
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
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.mainPanel.Size = new System.Drawing.Size(922, 665);
            this.mainPanel.TabIndex = 23;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.btnUndo);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 626);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(922, 39);
            this.panel1.TabIndex = 21;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(690, 7);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(100, 28);
            this.btnSaveAs.TabIndex = 1;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.SaveSettingsAsMenuItem_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Location = new System.Drawing.Point(798, 7);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(120, 28);
            this.btnUndo.TabIndex = 0;
            this.btnUndo.Text = "Undo Changes";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(582, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 665);
            this.Controls.Add(this.mainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(930, 656);
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindowOnFormClosing);
            this.FontGroup.ResumeLayout(false);
            this.FontGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumeric)).EndInit();
            this.AutoSplitLayout.ResumeLayout(false);
            this.AutoSplitLayout.PerformLayout();
            this.AutoSplitToolbar.ResumeLayout(false);
            this.AutoSplitToolbar.PerformLayout();
            this.VerticalSplitContainer.ResumeLayout(false);
            this.grpConfigFiles.ResumeLayout(false);
            this.ctxConfigFileList.ResumeLayout(false);
            this.HorizontalSplitContainer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSettingsLayout.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingInVerticalLayout)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageSettingsRunes.ResumeLayout(false);
            this.tabPageSettingsAutosplit.ResumeLayout(false);
            this.tabPageSettingsMisc.ResumeLayout(false);
            this.groupBoxPipeServer.ResumeLayout(false);
            this.groupBoxPipeServer.PerformLayout();
            this.DataGroup.ResumeLayout(false);
            this.DataGroup.PerformLayout();
            this.UpdateGroup.ResumeLayout(false);
            this.UpdateGroup.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label FontLabel;
        private System.Windows.Forms.GroupBox FontGroup;
        private System.Windows.Forms.Label TitleFontSizeLabel;
        private System.Windows.Forms.Label FontSizeLabel;
        private System.Windows.Forms.Button AddAutoSplitButton;
        private System.Windows.Forms.CheckBox EnableAutosplitCheckBox;
        private System.Windows.Forms.Label AutoSplitHotkeyLabel;
        private System.Windows.Forms.Button AutoSplitTestHotkeyButton;
        private System.Windows.Forms.TableLayoutPanel VerticalSplitContainer;
        private System.Windows.Forms.TableLayoutPanel HorizontalSplitContainer;
        private System.Windows.Forms.TableLayoutPanel AutoSplitLayout;
        private System.Windows.Forms.Panel AutoSplitToolbar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDisplayName;
        private System.Windows.Forms.CheckBox chkDisplayDeathCounter;
        private System.Windows.Forms.CheckBox chkDisplayAdvancedStats;
        private System.Windows.Forms.CheckBox chkDisplayGold;
        private System.Windows.Forms.CheckBox chkDisplayResistances;
        private System.Windows.Forms.CheckBox chkDisplayBaseStats;
        private System.Windows.Forms.CheckBox chkDisplayLevel;
        private System.Windows.Forms.NumericUpDown fontSizeNumeric;
        private System.Windows.Forms.NumericUpDown titleFontSizeNumeric;
        private Gui.Controls.FontComboBox fontComboBox;
        private Controls.HotkeyControl autoSplitHotkeyControl;
        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkDisplayDifficultyPercents;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.GroupBox grpConfigFiles;
        private System.Windows.Forms.ListBox lstConfigFiles;
        private System.Windows.Forms.ContextMenuStrip ctxConfigFileList;
        private System.Windows.Forms.ToolStripMenuItem menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuClone;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSettingsLayout;
        private System.Windows.Forms.TabPage tabPageSettingsAutosplit;
        private System.Windows.Forms.TabPage tabPageSettingsRunes;
        private System.Windows.Forms.TabPage tabPageSettingsMisc;
        private System.Windows.Forms.GroupBox DataGroup;
        private System.Windows.Forms.CheckBox CreateFilesCheckBox;
        private System.Windows.Forms.GroupBox UpdateGroup;
        private System.Windows.Forms.Button CheckUpdatesButton;
        private System.Windows.Forms.CheckBox CheckUpdatesCheckBox;
        private System.Windows.Forms.Button btnSetLevelColor;
        private System.Windows.Forms.Button btnSetFireResColor;
        private System.Windows.Forms.Button btnSetDeathsColor;
        private System.Windows.Forms.Button btnSetAdvancedStatsColor;
        private System.Windows.Forms.Button btnSetBaseStatsColor;
        private System.Windows.Forms.Button btnSetGoldColor;
        private System.Windows.Forms.Button btnSetNameColor;
        private System.Windows.Forms.ComboBox comboBoxLayout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetDifficultyColor;
        private System.Windows.Forms.Button btnSetPoisonResColor;
        private System.Windows.Forms.Button btnSetLightningResColor;
        private System.Windows.Forms.Button btnSetColdResColor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSetBackgroundColor;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingInVerticalLayout;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxRunesOrientation;
        private System.Windows.Forms.CheckBox chkHighContrastRunes;
        private System.Windows.Forms.CheckBox chkDisplayRunes;
        private System.Windows.Forms.CheckBox chkShowRealValues;
        private Controls.RuneSettingsPage runeSettingsPage;
        private System.Windows.Forms.Button btnSetPlayersXColor;
        private System.Windows.Forms.Button btnSetGameCounterColor;
        private System.Windows.Forms.CheckBox chkShowPlayersX;
        private System.Windows.Forms.CheckBox chkShowGameCounter;
        private System.Windows.Forms.GroupBox groupBoxPipeServer;
        private System.Windows.Forms.TextBox textBoxPipeName;
        private System.Windows.Forms.Label labelPipeName;
    }
}