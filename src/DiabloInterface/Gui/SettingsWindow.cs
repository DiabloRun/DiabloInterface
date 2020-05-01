namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.DiabloInterface.Plugin;
    using Zutatensuppe.DiabloInterface.Services;
    using Zutatensuppe.DiabloInterface.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Gui.Forms;

    public class SettingsWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string SettingsFilePath = Application.StartupPath + @"\Settings";

        private readonly DiabloInterface di;

        bool dirty;

        private Label FontLabel;
        private GroupBox FontGroup;
        private Label TitleFontSizeLabel;
        private Label FontSizeLabel;
        private TableLayoutPanel VerticalSplitContainer;
        private TableLayoutPanel HorizontalSplitContainer;
        private GroupBox groupBox1;
        private CheckBox chkDisplayName;
        private CheckBox chkDisplayDeathCounter;
        private CheckBox chkDisplayAdvancedStats;
        private CheckBox chkDisplayGold;
        private CheckBox chkDisplayResistances;
        private CheckBox chkDisplayBaseStats;
        private CheckBox chkDisplayLevel;
        private NumericUpDown fontSizeNumeric;
        private NumericUpDown titleFontSizeNumeric;
        private Gui.Controls.FontComboBox fontComboBox;
        private TableLayoutPanel mainPanel;
        private Panel panel1;
        private Button btnUndo;
        private Button btnSave;
        private CheckBox chkDisplayDifficultyPercents;
        private Button btnSaveAs;
        private GroupBox grpConfigFiles;
        private ListBox lstConfigFiles;
        private ContextMenuStrip ctxConfigFileList;
        private ToolStripMenuItem menuLoad;
        private ToolStripMenuItem menuClone;
        private ToolStripMenuItem menuDelete;
        private ToolStripMenuItem menuNew;
        private TabControl tabControl1;
        private TabPage tabPageSettingsLayout;
        private TabPage tabPageSettingsRunes;
        private Button btnSetLevelColor;
        private Button btnSetFireResColor;
        private Button btnSetDeathsColor;
        private Button btnSetAdvancedStatsColor;
        private Button btnSetBaseStatsColor;
        private Button btnSetGoldColor;
        private Button btnSetNameColor;
        private ComboBox comboBoxLayout;
        private Label label2;
        private Label label1;
        private Button btnSetDifficultyColor;
        private Button btnSetPoisonResColor;
        private Button btnSetLightningResColor;
        private Button btnSetColdResColor;
        private Button button1;
        private Button btnSetBackgroundColor;
        private NumericUpDown numericUpDownPaddingInVerticalLayout;
        private ToolStripMenuItem renameToolStripMenuItem;
        private GroupBox groupBox2;
        private Label label3;
        private ComboBox comboBoxRunesOrientation;
        private CheckBox chkHighContrastRunes;
        private CheckBox chkDisplayRunes;
        private CheckBox chkShowRealValues;
        private Controls.RuneSettingsPage runeSettingsPage;
        private Button btnSetPlayersXColor;
        private Button btnSetGameCounterColor;
        private CheckBox chkShowPlayersX;
        private CheckBox chkShowGameCounter;
        private CheckBox checkBoxAttackerSelfDamage;
        private CheckBox checkBoxMonsterGold;
        private CheckBox checkBoxMagicFind;
        private Button btnSetAttackerSelfDamageColor;
        private Button btnSetExtraGoldColor;
        private Button btnSetMFColor;
        private CheckBox chkDisplayExpansionClassic;
        private Button btnColorExpansionClassic;
        private CheckBox chkDisplayHardcoreSoftcore;
        private Button btnColorHardcoreSoftcore;
        private Button btnColorAll;
        private Button btnColorCharCount;
        private CheckBox chkDisplayCharCount;

        public SettingsWindow(DiabloInterface di)
        {
            Logger.Info("Creating settings window.");

            this.di = di;

            RegisterServiceEventHandlers();
            InitializeComponent();
            PopulateSettingsFileList(this.di.settings.SettingsFileCollection);

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing settings window.");
                UnregisterServiceEventHandlers();
            };

            ReloadComponentsWithCurrentSettings(this.di.settings.CurrentSettings);
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            FontLabel = new Label();
            FontGroup = new GroupBox();
            fontComboBox = new Zutatensuppe.DiabloInterface.Gui.Controls.FontComboBox();
            FontSizeLabel = new Label();
            TitleFontSizeLabel = new Label();
            VerticalSplitContainer = new TableLayoutPanel();
            grpConfigFiles = new GroupBox();
            lstConfigFiles = new ListBox();
            ctxConfigFileList = new ContextMenuStrip();
            menuNew = new ToolStripMenuItem();
            menuLoad = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            menuClone = new ToolStripMenuItem();
            menuDelete = new ToolStripMenuItem();
            HorizontalSplitContainer = new TableLayoutPanel();
            tabControl1 = new TabControl();
            tabPageSettingsLayout = new TabPage();
            groupBox2 = new GroupBox();
            numericUpDownPaddingInVerticalLayout = new NumericUpDown();
            label3 = new Label();
            comboBoxRunesOrientation = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            comboBoxLayout = new ComboBox();
            groupBox1 = new GroupBox();
            btnColorCharCount = new Button();
            chkDisplayCharCount = new CheckBox();
            btnColorAll = new Button();
            chkDisplayExpansionClassic = new CheckBox();
            btnColorExpansionClassic = new Button();
            chkDisplayHardcoreSoftcore = new CheckBox();
            btnColorHardcoreSoftcore = new Button();
            checkBoxAttackerSelfDamage = new CheckBox();
            checkBoxMonsterGold = new CheckBox();
            checkBoxMagicFind = new CheckBox();
            btnSetAttackerSelfDamageColor = new Button();
            btnSetExtraGoldColor = new Button();
            btnSetMFColor = new Button();
            btnSetPlayersXColor = new Button();
            btnSetGameCounterColor = new Button();
            chkShowPlayersX = new CheckBox();
            chkShowGameCounter = new CheckBox();
            chkShowRealValues = new CheckBox();
            chkHighContrastRunes = new CheckBox();
            chkDisplayRunes = new CheckBox();
            btnSetBackgroundColor = new Button();
            button1 = new Button();
            btnSetLevelColor = new Button();
            btnSetDifficultyColor = new Button();
            btnSetPoisonResColor = new Button();
            btnSetLightningResColor = new Button();
            btnSetColdResColor = new Button();
            btnSetFireResColor = new Button();
            btnSetDeathsColor = new Button();
            btnSetAdvancedStatsColor = new Button();
            btnSetBaseStatsColor = new Button();
            btnSetGoldColor = new Button();
            btnSetNameColor = new Button();
            chkDisplayDifficultyPercents = new CheckBox();
            chkDisplayAdvancedStats = new CheckBox();
            chkDisplayLevel = new CheckBox();
            chkDisplayGold = new CheckBox();
            chkDisplayResistances = new CheckBox();
            chkDisplayBaseStats = new CheckBox();
            chkDisplayDeathCounter = new CheckBox();
            chkDisplayName = new CheckBox();
            tabPageSettingsRunes = new TabPage();
            runeSettingsPage = new Zutatensuppe.DiabloInterface.Gui.Controls.RuneSettingsPage();
            mainPanel = new TableLayoutPanel();
            panel1 = new Panel();
            btnSaveAs = new Button();
            btnUndo = new Button();
            btnSave = new Button();
            FontGroup.SuspendLayout();
            VerticalSplitContainer.SuspendLayout();
            grpConfigFiles.SuspendLayout();
            ctxConfigFileList.SuspendLayout();
            HorizontalSplitContainer.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageSettingsLayout.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingInVerticalLayout)).BeginInit();
            groupBox1.SuspendLayout();
            tabPageSettingsRunes.SuspendLayout();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();

            fontComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            fontComboBox.DropDownWidth = 250;
            fontComboBox.FormattingEnabled = true;
            fontComboBox.Location = new System.Drawing.Point(105, 21);
            fontComboBox.Size = new System.Drawing.Size(117, 21);
            fontComboBox.Sorted = true;

            titleFontSizeNumeric = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).BeginInit();
            titleFontSizeNumeric.Location = new System.Drawing.Point(105, 74);
            titleFontSizeNumeric.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            titleFontSizeNumeric.Size = new System.Drawing.Size(118, 20);
            titleFontSizeNumeric.Value = new decimal(new int[] { 20, 0, 0, 0 });

            fontSizeNumeric = new NumericUpDown();
            fontSizeNumeric.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fontSizeNumeric.Location = new System.Drawing.Point(105, 48);
            fontSizeNumeric.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            fontSizeNumeric.Size = new System.Drawing.Size(118, 20);
            fontSizeNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });

            FontSizeLabel.AutoSize = true;
            FontSizeLabel.Location = new System.Drawing.Point(6, 51);
            FontSizeLabel.Size = new System.Drawing.Size(52, 13);
            FontSizeLabel.Text = "Font size:";

            TitleFontSizeLabel.AutoSize = true;
            TitleFontSizeLabel.Location = new System.Drawing.Point(6, 77);
            TitleFontSizeLabel.Size = new System.Drawing.Size(80, 13);
            TitleFontSizeLabel.Text = "Name font size:";

            FontLabel.AutoSize = true;
            FontLabel.Location = new System.Drawing.Point(6, 24);
            FontLabel.Size = new System.Drawing.Size(31, 13);
            FontLabel.TabIndex = 2;
            FontLabel.Text = "Font:";

            FontGroup.Controls.Add(fontComboBox);
            FontGroup.Controls.Add(titleFontSizeNumeric);
            FontGroup.Controls.Add(fontSizeNumeric);
            FontGroup.Controls.Add(FontSizeLabel);
            FontGroup.Controls.Add(TitleFontSizeLabel);
            FontGroup.Controls.Add(FontLabel);
            FontGroup.Location = new System.Drawing.Point(9, 6);
            FontGroup.Margin = new Padding(0);
            FontGroup.Size = new System.Drawing.Size(231, 104);
            FontGroup.Text = "Font";

            grpConfigFiles.Controls.Add(this.lstConfigFiles);
            grpConfigFiles.Dock = DockStyle.Fill;
            grpConfigFiles.Location = new System.Drawing.Point(3, 3);
            grpConfigFiles.Size = new System.Drawing.Size(169, 502);
            grpConfigFiles.Text = "Config Files";

            tabControl1.Controls.Add(tabPageSettingsLayout);
            tabControl1.Controls.Add(tabPageSettingsRunes);

            foreach (var p in di.plugins.CreateControls<IPluginSettingsRenderer>())
            {
                var c = new TabPage();
                c.Controls.Add(p.Value);
                c.Dock = DockStyle.Fill;
                c.Text = p.Key;
                tabControl1.Controls.Add(c);
            }

            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 3);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(513, 502);
            foreach (TabPage c in tabControl1.Controls)
                c.UseVisualStyleBackColor = true;

            HorizontalSplitContainer.ColumnCount = 1;
            HorizontalSplitContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            HorizontalSplitContainer.Controls.Add(this.tabControl1, 0, 0);
            HorizontalSplitContainer.Dock = DockStyle.Fill;
            HorizontalSplitContainer.Location = new System.Drawing.Point(175, 0);
            HorizontalSplitContainer.Margin = new Padding(0);
            HorizontalSplitContainer.RowCount = 2;
            HorizontalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            HorizontalSplitContainer.RowStyles.Add(new RowStyle());
            HorizontalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            HorizontalSplitContainer.Size = new System.Drawing.Size(519, 508);

            VerticalSplitContainer.ColumnCount = 2;
            VerticalSplitContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
            VerticalSplitContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            VerticalSplitContainer.Controls.Add(this.grpConfigFiles, 0, 0);
            VerticalSplitContainer.Controls.Add(this.HorizontalSplitContainer, 1, 0);
            VerticalSplitContainer.Dock = DockStyle.Fill;
            VerticalSplitContainer.Location = new System.Drawing.Point(0, 0);
            VerticalSplitContainer.Margin = new Padding(0);
            VerticalSplitContainer.RowCount = 1;
            VerticalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            VerticalSplitContainer.Size = new System.Drawing.Size(694, 508);

            lstConfigFiles.ContextMenuStrip = this.ctxConfigFileList;
            lstConfigFiles.Dock = DockStyle.Fill;
            lstConfigFiles.FormattingEnabled = true;
            lstConfigFiles.Location = new System.Drawing.Point(3, 16);
            lstConfigFiles.Size = new System.Drawing.Size(163, 483);
            lstConfigFiles.MouseDoubleClick += new MouseEventHandler(this.lstConfigFiles_MouseDoubleClick);
            lstConfigFiles.MouseUp += new MouseEventHandler(this.lstConfigFiles_MouseUp);

            ctxConfigFileList.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxConfigFileList.Items.AddRange(new ToolStripItem[] {
            menuNew,
            menuLoad,
            renameToolStripMenuItem,
            menuClone,
            menuDelete});
            ctxConfigFileList.Size = new System.Drawing.Size(118, 114);

            menuNew.Size = new System.Drawing.Size(117, 22);
            menuNew.Text = "New";
            menuNew.Click += new System.EventHandler(this.menuNew_Click);

            menuLoad.Size = new System.Drawing.Size(117, 22);
            menuLoad.Text = "Load";
            menuLoad.Click += new System.EventHandler(this.menuLoad_Click);

            renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);

            menuClone.Size = new Size(117, 22);
            menuClone.Text = "Clone";
            menuClone.Click += new EventHandler(this.menuClone_Click);

            menuDelete.Size = new Size(117, 22);
            menuDelete.Text = "Delete";
            menuDelete.Click += new EventHandler(this.menuDelete_Click);

            tabPageSettingsLayout.Controls.Add(this.groupBox2);
            tabPageSettingsLayout.Controls.Add(this.FontGroup);
            tabPageSettingsLayout.Controls.Add(this.groupBox1);
            tabPageSettingsLayout.Dock = DockStyle.Fill;
            tabPageSettingsLayout.Text = "Layout";

            this.groupBox2.Controls.Add(this.numericUpDownPaddingInVerticalLayout);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxRunesOrientation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxLayout);
            this.groupBox2.Location = new System.Drawing.Point(249, 6);
            this.groupBox2.Size = new System.Drawing.Size(241, 104);
            this.groupBox2.Text = "Layout";

            this.numericUpDownPaddingInVerticalLayout.Location = new System.Drawing.Point(145, 75);
            this.numericUpDownPaddingInVerticalLayout.Size = new System.Drawing.Size(81, 20);
            this.numericUpDownPaddingInVerticalLayout.TabIndex = 23;

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.Text = "Runes orientation:";

            this.comboBoxRunesOrientation.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRunesOrientation.FormattingEnabled = true;
            this.comboBoxRunesOrientation.Items.AddRange(new object[] { "Horizontal", "Vertical" });
            this.comboBoxRunesOrientation.Location = new System.Drawing.Point(145, 48);
            this.comboBoxRunesOrientation.Size = new System.Drawing.Size(80, 21);

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.Text = "Orientation:";

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.Text = "Padding in vertical layout:";

            this.comboBoxLayout.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLayout.FormattingEnabled = true;
            this.comboBoxLayout.Items.AddRange(new object[] { "Horizontal", "Vertical" });
            this.comboBoxLayout.Location = new System.Drawing.Point(145, 21);
            this.comboBoxLayout.Size = new System.Drawing.Size(80, 21);
            this.comboBoxLayout.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayout_SelectedIndexChanged);

            this.btnColorCharCount.Location = new System.Drawing.Point(155, 249);
            this.btnColorCharCount.Size = new System.Drawing.Size(44, 22);
            this.btnColorCharCount.Text = "Color";
            this.btnColorCharCount.Click += new System.EventHandler(this.btnSelectColor);

            this.chkDisplayCharCount.AutoSize = true;
            this.chkDisplayCharCount.Location = new System.Drawing.Point(14, 253);
            this.chkDisplayCharCount.Size = new System.Drawing.Size(116, 17);
            this.chkDisplayCharCount.Text = "Characters created";

            this.btnColorAll.Location = new System.Drawing.Point(338, 280);
            this.btnColorAll.Size = new System.Drawing.Size(136, 22);
            this.btnColorAll.Text = "Text Color (All)";
            this.btnColorAll.Click += new System.EventHandler(this.btnColorAll_Click);

            this.chkDisplayExpansionClassic.AutoSize = true;
            this.chkDisplayExpansionClassic.Location = new System.Drawing.Point(264, 138);
            this.chkDisplayExpansionClassic.Size = new System.Drawing.Size(86, 17);
            this.chkDisplayExpansionClassic.Text = "Classic/LOD";

            this.btnColorExpansionClassic.Location = new System.Drawing.Point(397, 134);
            this.btnColorExpansionClassic.Size = new System.Drawing.Size(44, 22);
            this.btnColorExpansionClassic.Text = "Color";
            this.btnColorExpansionClassic.Click += new System.EventHandler(this.btnSelectColor);

            this.groupBox1.Controls.Add(this.btnColorCharCount);
            this.groupBox1.Controls.Add(this.chkDisplayCharCount);
            this.groupBox1.Controls.Add(this.btnColorAll);
            this.groupBox1.Controls.Add(this.chkDisplayExpansionClassic);
            this.groupBox1.Controls.Add(this.btnColorExpansionClassic);
            this.groupBox1.Controls.Add(this.chkDisplayHardcoreSoftcore);
            this.groupBox1.Controls.Add(this.btnColorHardcoreSoftcore);
            this.groupBox1.Controls.Add(this.checkBoxAttackerSelfDamage);
            this.groupBox1.Controls.Add(this.checkBoxMonsterGold);
            this.groupBox1.Controls.Add(this.checkBoxMagicFind);
            this.groupBox1.Controls.Add(this.btnSetAttackerSelfDamageColor);
            this.groupBox1.Controls.Add(this.btnSetExtraGoldColor);
            this.groupBox1.Controls.Add(this.btnSetMFColor);
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
            this.groupBox1.Location = new System.Drawing.Point(9, 110);
            this.groupBox1.Margin = new Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(481, 366);
            this.groupBox1.Text = "Display";

            this.chkDisplayHardcoreSoftcore.AutoSize = true;
            this.chkDisplayHardcoreSoftcore.Location = new System.Drawing.Point(264, 115);
            this.chkDisplayHardcoreSoftcore.Size = new System.Drawing.Size(60, 17);
            this.chkDisplayHardcoreSoftcore.Text = "HC/SC";

            this.btnColorHardcoreSoftcore.Location = new System.Drawing.Point(397, 111);
            this.btnColorHardcoreSoftcore.Size = new System.Drawing.Size(44, 22);
            this.btnColorHardcoreSoftcore.Text = "Color";
            this.btnColorHardcoreSoftcore.Click += new System.EventHandler(this.btnSelectColor);

            this.checkBoxAttackerSelfDamage.AutoSize = true;
            this.checkBoxAttackerSelfDamage.Location = new System.Drawing.Point(264, 70);
            this.checkBoxAttackerSelfDamage.Size = new System.Drawing.Size(130, 17);
            this.checkBoxAttackerSelfDamage.Text = "Attacker Self Damage";

            this.checkBoxMonsterGold.AutoSize = true;
            this.checkBoxMonsterGold.Location = new System.Drawing.Point(264, 47);
            this.checkBoxMonsterGold.Size = new System.Drawing.Size(75, 17);
            this.checkBoxMonsterGold.Text = "Extra Gold";

            this.checkBoxMagicFind.AutoSize = true;
            this.checkBoxMagicFind.Location = new System.Drawing.Point(264, 24);
            this.checkBoxMagicFind.Size = new System.Drawing.Size(78, 17);
            this.checkBoxMagicFind.Text = "Magic Find";

            this.btnSetAttackerSelfDamageColor.Location = new System.Drawing.Point(397, 66);
            this.btnSetAttackerSelfDamageColor.Margin = new Padding(0);
            this.btnSetAttackerSelfDamageColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetAttackerSelfDamageColor.Text = "Color";
            this.btnSetAttackerSelfDamageColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetExtraGoldColor.Location = new System.Drawing.Point(397, 43);
            this.btnSetExtraGoldColor.Margin = new Padding(0);
            this.btnSetExtraGoldColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetExtraGoldColor.Text = "Color";
            this.btnSetExtraGoldColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetMFColor.Location = new System.Drawing.Point(397, 20);
            this.btnSetMFColor.Margin = new Padding(0);
            this.btnSetMFColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetMFColor.Text = "Color";
            this.btnSetMFColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetPlayersXColor.Location = new System.Drawing.Point(155, 203);
            this.btnSetPlayersXColor.Margin = new Padding(0);
            this.btnSetPlayersXColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetPlayersXColor.Text = "Color";
            this.btnSetPlayersXColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetGameCounterColor.Location = new System.Drawing.Point(155, 226);
            this.btnSetGameCounterColor.Margin = new Padding(0);
            this.btnSetGameCounterColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetGameCounterColor.Text = "Color";
            this.btnSetGameCounterColor.Click += new System.EventHandler(this.btnSelectColor);

            this.chkShowPlayersX.AutoSize = true;
            this.chkShowPlayersX.Location = new System.Drawing.Point(14, 207);
            this.chkShowPlayersX.Size = new System.Drawing.Size(74, 17);
            this.chkShowPlayersX.Text = "/players X";

            this.chkShowGameCounter.AutoSize = true;
            this.chkShowGameCounter.Location = new System.Drawing.Point(14, 230);
            this.chkShowGameCounter.Size = new System.Drawing.Size(106, 17);
            this.chkShowGameCounter.Text = "Games launched";

            this.chkShowRealValues.AutoSize = true;
            this.chkShowRealValues.Location = new System.Drawing.Point(203, 92);
            this.chkShowRealValues.Size = new System.Drawing.Size(109, 17);
            this.chkShowRealValues.Text = "calculated values";

            this.chkHighContrastRunes.AutoSize = true;
            this.chkHighContrastRunes.Location = new System.Drawing.Point(82, 341);
            this.chkHighContrastRunes.Size = new System.Drawing.Size(89, 17);
            this.chkHighContrastRunes.Text = "High contrast";

            this.chkDisplayRunes.AutoSize = true;
            this.chkDisplayRunes.Location = new System.Drawing.Point(14, 341);
            this.chkDisplayRunes.Size = new System.Drawing.Size(57, 17);
            this.chkDisplayRunes.Text = "Runes";

            this.btnSetBackgroundColor.Location = new System.Drawing.Point(339, 308);
            this.btnSetBackgroundColor.Size = new System.Drawing.Size(136, 22);
            this.btnSetBackgroundColor.Text = "Background color";
            this.btnSetBackgroundColor.Click += new System.EventHandler(this.backgroundColorButtonClick);

            this.button1.Location = new System.Drawing.Point(339, 336);
            this.button1.Size = new System.Drawing.Size(136, 22);
            this.button1.Text = "Reset to default colors";
            this.button1.Click += new System.EventHandler(this.resetColorsButton);

            this.btnSetLevelColor.Location = new System.Drawing.Point(155, 134);
            this.btnSetLevelColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetLevelColor.Text = "Color";
            this.btnSetLevelColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetDifficultyColor.Location = new System.Drawing.Point(155, 180);
            this.btnSetDifficultyColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetDifficultyColor.Text = "Color";
            this.btnSetDifficultyColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetPoisonResColor.Location = new System.Drawing.Point(287, 157);
            this.btnSetPoisonResColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetPoisonResColor.Text = "Pois.";
            this.btnSetPoisonResColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetLightningResColor.Location = new System.Drawing.Point(243, 157);
            this.btnSetLightningResColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetLightningResColor.Text = "Light.";
            this.btnSetLightningResColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetColdResColor.Location = new System.Drawing.Point(199, 157);
            this.btnSetColdResColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetColdResColor.Text = "Cold";
            this.btnSetColdResColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetFireResColor.Location = new System.Drawing.Point(155, 157);
            this.btnSetFireResColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetFireResColor.Text = "Fire";
            this.btnSetFireResColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetDeathsColor.Location = new System.Drawing.Point(155, 111);
            this.btnSetDeathsColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetDeathsColor.Text = "Color";
            this.btnSetDeathsColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetAdvancedStatsColor.Location = new System.Drawing.Point(155, 88);
            this.btnSetAdvancedStatsColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetAdvancedStatsColor.Text = "Color";
            this.btnSetAdvancedStatsColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetBaseStatsColor.Location = new System.Drawing.Point(155, 66);
            this.btnSetBaseStatsColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetBaseStatsColor.Text = "Color";
            this.btnSetBaseStatsColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetGoldColor.Location = new System.Drawing.Point(155, 43);
            this.btnSetGoldColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetGoldColor.Text = "Color";
            this.btnSetGoldColor.Click += new System.EventHandler(this.btnSelectColor);

            this.btnSetNameColor.Location = new System.Drawing.Point(155, 20);
            this.btnSetNameColor.Size = new System.Drawing.Size(44, 22);
            this.btnSetNameColor.Text = "Color";
            this.btnSetNameColor.Click += new System.EventHandler(this.btnSelectColor);

            this.chkDisplayDifficultyPercents.AutoSize = true;
            this.chkDisplayDifficultyPercents.Location = new System.Drawing.Point(14, 184);
            this.chkDisplayDifficultyPercents.Size = new System.Drawing.Size(77, 17);
            this.chkDisplayDifficultyPercents.Text = "Difficulty %";

            this.chkDisplayAdvancedStats.AutoSize = true;
            this.chkDisplayAdvancedStats.Location = new System.Drawing.Point(14, 92);
            this.chkDisplayAdvancedStats.Size = new System.Drawing.Size(105, 17);
            this.chkDisplayAdvancedStats.Text = "Fcr, Frw, Fhr, Ias";

            this.chkDisplayLevel.AutoSize = true;
            this.chkDisplayLevel.Location = new System.Drawing.Point(14, 138);
            this.chkDisplayLevel.Size = new System.Drawing.Size(101, 17);
            this.chkDisplayLevel.Text = "Character Level";

            this.chkDisplayGold.AutoSize = true;
            this.chkDisplayGold.Location = new System.Drawing.Point(14, 47);
            this.chkDisplayGold.Size = new System.Drawing.Size(48, 17);
            this.chkDisplayGold.Text = "Gold";

            this.chkDisplayResistances.AutoSize = true;
            this.chkDisplayResistances.Location = new System.Drawing.Point(14, 161);
            this.chkDisplayResistances.Size = new System.Drawing.Size(84, 17);
            this.chkDisplayResistances.Text = "Resistances";

            this.chkDisplayBaseStats.AutoSize = true;
            this.chkDisplayBaseStats.Location = new System.Drawing.Point(14, 70);
            this.chkDisplayBaseStats.Size = new System.Drawing.Size(107, 17);
            this.chkDisplayBaseStats.Text = "Str, Dex, Vit, Ene";

            this.chkDisplayDeathCounter.AutoSize = true;
            this.chkDisplayDeathCounter.Location = new System.Drawing.Point(14, 115);
            this.chkDisplayDeathCounter.Size = new System.Drawing.Size(60, 17);
            this.chkDisplayDeathCounter.Text = "Deaths";

            this.chkDisplayName.AutoSize = true;
            this.chkDisplayName.Location = new System.Drawing.Point(14, 24);
            this.chkDisplayName.Size = new System.Drawing.Size(54, 17);
            this.chkDisplayName.Text = "Name";

            this.tabPageSettingsRunes.Controls.Add(this.runeSettingsPage);
            this.tabPageSettingsRunes.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsRunes.Size = new System.Drawing.Size(505, 476);
            this.tabPageSettingsRunes.Text = "Runes";

            this.runeSettingsPage.Dock = DockStyle.Fill;
            this.runeSettingsPage.Location = new System.Drawing.Point(0, 0);
            this.runeSettingsPage.Margin = new Padding(4);
            this.runeSettingsPage.SettingsList = (IReadOnlyList<ClassRuneSettings>)resources.GetObject("runeSettingsPage.SettingsList");
            this.runeSettingsPage.Size = new System.Drawing.Size(505, 476);

            this.mainPanel.ColumnCount = 1;
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainPanel.Controls.Add(this.VerticalSplitContainer, 0, 0);
            this.mainPanel.Controls.Add(this.panel1, 0, 1);
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new Padding(0);
            this.mainPanel.RowCount = 2;
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            this.mainPanel.Size = new System.Drawing.Size(694, 540);

            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.btnUndo);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 508);
            this.panel1.Margin = new Padding(0);
            this.panel1.Size = new System.Drawing.Size(694, 32);

            this.btnSaveAs.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnSaveAs.Location = new System.Drawing.Point(520, 6);
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.Click += new System.EventHandler(this.SaveSettingsAsMenuItem_Click);

            this.btnUndo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnUndo.Location = new System.Drawing.Point(600, 6);
            this.btnUndo.Size = new System.Drawing.Size(90, 23);
            this.btnUndo.Text = "Undo Changes";
            this.btnUndo.Click += new System.EventHandler(this.btnCancel_Click);

            this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(438, 6);
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(694, 540);
            this.Controls.Add(this.mainPanel);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MinimumSize = new System.Drawing.Size(700, 538);
            this.Text = "Settings";
            this.FormClosing += new FormClosingEventHandler(this.SettingsWindowOnFormClosing);
            this.FontGroup.ResumeLayout(false);
            this.FontGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleFontSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeNumeric)).EndInit();
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
            this.mainPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        bool IsDirty
        {
            get
            {
                var settings = di.settings.CurrentSettings;

                return dirty
                    || di.plugins.EditedSettingsDirty
                    || !CompareClassRuneSettings(settings)

                    || settings.FontName != GetFontName()
                    || settings.FontSize != (int)fontSizeNumeric.Value
                    || settings.FontSizeTitle != (int)titleFontSizeNumeric.Value
                    || settings.DisplayName != chkDisplayName.Checked
                    || settings.DisplayGold != chkDisplayGold.Checked
                    || settings.DisplayDeathCounter != chkDisplayDeathCounter.Checked
                    || settings.DisplayLevel != chkDisplayLevel.Checked
                    || settings.DisplayResistances != chkDisplayResistances.Checked
                    || settings.DisplayBaseStats != chkDisplayBaseStats.Checked
                    || settings.DisplayAdvancedStats != chkDisplayAdvancedStats.Checked
                    || settings.DisplayRunes != chkDisplayRunes.Checked
                    || settings.DisplayRunesHorizontal != (comboBoxRunesOrientation.SelectedIndex == 0)
                    || settings.DisplayRunesHighContrast != chkHighContrastRunes.Checked
                    || settings.DisplayDifficultyPercentages != chkDisplayDifficultyPercents.Checked
                    || settings.DisplayLayoutHorizontal != (comboBoxLayout.SelectedIndex == 0)
                    || settings.VerticalLayoutPadding != (int)numericUpDownPaddingInVerticalLayout.Value
                    || settings.DisplayRealFrwIas != chkShowRealValues.Checked
                    || settings.DisplayPlayersX != chkShowPlayersX.Checked
                    || settings.DisplayGameCounter != chkShowGameCounter.Checked
                    || settings.DisplayCharCounter != chkDisplayCharCount.Checked
                    || settings.DisplayMagicFind != checkBoxMagicFind.Checked
                    || settings.DisplayMonsterGold != checkBoxMonsterGold.Checked
                    || settings.DisplayAttackerSelfDamage != checkBoxAttackerSelfDamage.Checked
                    || settings.DisplayHardcoreSoftcore != chkDisplayHardcoreSoftcore.Checked
                    || settings.DisplayExpansionClassic != chkDisplayExpansionClassic.Checked

                    || settings.ColorName != btnSetNameColor.ForeColor
                    || settings.ColorDeaths != btnSetDeathsColor.ForeColor
                    || settings.ColorLevel != btnSetLevelColor.ForeColor
                    || settings.ColorDifficultyPercentages != btnSetDifficultyColor.ForeColor
                    || settings.ColorGold != btnSetGoldColor.ForeColor
                    || settings.ColorBaseStats != btnSetBaseStatsColor.ForeColor
                    || settings.ColorAdvancedStats != btnSetAdvancedStatsColor.ForeColor
                    || settings.ColorFireRes != btnSetFireResColor.ForeColor
                    || settings.ColorColdRes != btnSetColdResColor.ForeColor
                    || settings.ColorLightningRes != btnSetLightningResColor.ForeColor
                    || settings.ColorPoisonRes != btnSetPoisonResColor.ForeColor
                    || settings.ColorPlayersX != btnSetPlayersXColor.ForeColor
                    || settings.ColorGameCounter != btnSetGameCounterColor.ForeColor
                    || settings.ColorCharCounter != btnColorCharCount.ForeColor
                    || settings.ColorMagicFind != btnSetMFColor.ForeColor
                    || settings.ColorMonsterGold != btnSetExtraGoldColor.ForeColor
                    || settings.ColorAttackerSelfDamage != btnSetAttackerSelfDamageColor.ForeColor
                    || settings.ColorHardcoreSoftcore != btnColorHardcoreSoftcore.ForeColor
                    || settings.ColorExpansionClassic != btnColorExpansionClassic.ForeColor

                    || settings.ColorBackground != btnSetBackgroundColor.BackColor
                ;
            }
        }

        bool CompareClassRuneSettings(ApplicationSettings settings)
        {
            IReadOnlyList<ClassRuneSettings> a = settings.ClassRunes;
            IReadOnlyList<ClassRuneSettings> b = runeSettingsPage.SettingsList;

            if (a.Count != b.Count) return false;

            for (var i = 0; i < a.Count; ++i)
            {
                var settingsA = a[i];
                var settingsB = b[i];

                if (settingsA.Class != settingsB.Class) return false;
                if (settingsA.Difficulty != settingsB.Difficulty) return false;
                if (settingsA.Runes.Count != settingsB.Runes.Count) return false;
                if (!settingsA.Runes.SequenceEqual(settingsB.Runes)) return false;
            }

            return true;
        }

        void RegisterServiceEventHandlers()
        {
            di.settings.Changed += SettingsServiceSettingsChanged;
            di.settings.CollectionChanged += SettingsServiceOnSettingsCollectionChanged;
        }

        private void PluginDataChanged(object sender, IPlugin e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => PluginDataChanged(sender, e)));
                return;
            }

            e.GetRenderer<IPluginSettingsRenderer>().ApplyChanges();
        }

        void UnregisterServiceEventHandlers()
        {
            di.settings.Changed -= SettingsServiceSettingsChanged;
            di.settings.CollectionChanged -= SettingsServiceOnSettingsCollectionChanged;
        }

        void SettingsServiceSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceSettingsChanged(sender, e)));
                return;
            }

            // NOTE: This may have been due to loading settings from elsewhere. For now the
            ////     behavior will be to refresh the settings window in that case.
            ReloadComponentsWithCurrentSettings(e.Settings);
        }

        void UpdateTitle()
        {
            Text = $@"Settings ({Path.GetFileName(di.settings.CurrentSettingsFile)})";
        }

        void ReloadComponentsWithCurrentSettings(ApplicationSettings s)
        {
            UpdateTitle();

            var settings = s.DeepCopy();

            runeSettingsPage.SettingsList = settings.ClassRunes;

            fontComboBox.SelectedIndex = fontComboBox.Items.IndexOf(settings.FontName);

            fontSizeNumeric.Value = settings.FontSize;
            titleFontSizeNumeric.Value = settings.FontSizeTitle;
            numericUpDownPaddingInVerticalLayout.Value = settings.VerticalLayoutPadding;

            chkDisplayName.Checked = settings.DisplayName;
            chkDisplayGold.Checked = settings.DisplayGold;
            chkDisplayDeathCounter.Checked = settings.DisplayDeathCounter;
            chkDisplayLevel.Checked = settings.DisplayLevel;
            chkDisplayResistances.Checked = settings.DisplayResistances;
            chkDisplayBaseStats.Checked = settings.DisplayBaseStats;
            chkDisplayAdvancedStats.Checked = settings.DisplayAdvancedStats;
            chkDisplayRunes.Checked = settings.DisplayRunes;
            comboBoxRunesOrientation.SelectedIndex = settings.DisplayRunesHorizontal ? 0 : 1;
            chkDisplayDifficultyPercents.Checked = settings.DisplayDifficultyPercentages;
            chkHighContrastRunes.Checked = settings.DisplayRunesHighContrast;
            comboBoxLayout.SelectedIndex = settings.DisplayLayoutHorizontal ? 0 : 1;
            chkShowRealValues.Checked = settings.DisplayRealFrwIas;
            chkShowPlayersX.Checked = settings.DisplayPlayersX;
            chkShowGameCounter.Checked = settings.DisplayGameCounter;
            chkDisplayCharCount.Checked = settings.DisplayCharCounter;
            checkBoxMagicFind.Checked = settings.DisplayMagicFind;
            checkBoxMonsterGold.Checked = settings.DisplayMonsterGold;
            checkBoxAttackerSelfDamage.Checked = settings.DisplayAttackerSelfDamage;
            chkDisplayHardcoreSoftcore.Checked = settings.DisplayHardcoreSoftcore;
            chkDisplayExpansionClassic.Checked = settings.DisplayExpansionClassic;

            btnSetNameColor.ForeColor = settings.ColorName;
            btnSetDeathsColor.ForeColor = settings.ColorDeaths;
            btnSetLevelColor.ForeColor = settings.ColorLevel;
            btnSetDifficultyColor.ForeColor = settings.ColorDifficultyPercentages;
            btnSetGoldColor.ForeColor = settings.ColorGold;
            btnSetBaseStatsColor.ForeColor = settings.ColorBaseStats;
            btnSetAdvancedStatsColor.ForeColor = settings.ColorAdvancedStats;
            btnSetFireResColor.ForeColor = settings.ColorFireRes;
            btnSetColdResColor.ForeColor = settings.ColorColdRes;
            btnSetLightningResColor.ForeColor = settings.ColorLightningRes;
            btnSetPoisonResColor.ForeColor = settings.ColorPoisonRes;
            btnSetPlayersXColor.ForeColor = settings.ColorPlayersX;
            btnSetGameCounterColor.ForeColor = settings.ColorGameCounter;
            btnColorCharCount.ForeColor = settings.ColorCharCounter;
            btnSetMFColor.ForeColor = settings.ColorMagicFind;
            btnSetExtraGoldColor.ForeColor = settings.ColorMonsterGold;
            btnSetAttackerSelfDamageColor.ForeColor = settings.ColorAttackerSelfDamage;
            btnColorHardcoreSoftcore.ForeColor = settings.ColorHardcoreSoftcore;
            btnColorExpansionClassic.ForeColor = settings.ColorExpansionClassic;

            SetBackgroundColor(settings.ColorBackground);

            // Loading the settings will dirty mark pretty much everything, here
            // we just verify that nothing has actually changed yet.
            dirty = false;
        }

        private string GetFontName()
        {
            if (fontComboBox.SelectedItem != null)
                return fontComboBox.SelectedItem.ToString();

            foreach (string comboBoxFontName in fontComboBox.Items)
            {
                if (comboBoxFontName.Equals(fontComboBox.Text))
                    return fontComboBox.Text;
            }
            return null;
        }

        ApplicationSettings CopyModifiedSettings()
        {
            var settings = di.settings.CurrentSettings.DeepCopy();

            settings.ClassRunes = runeSettingsPage.SettingsList ?? new List<ClassRuneSettings>();

            foreach (var p in di.plugins.GetEditedConfigs)
                settings.Plugins[p.Key] = p.Value;

            settings.FontSize = (int)fontSizeNumeric.Value;
            settings.FontSizeTitle = (int)titleFontSizeNumeric.Value;
            settings.VerticalLayoutPadding = (int)numericUpDownPaddingInVerticalLayout.Value;
            settings.FontName = GetFontName();

            settings.DisplayName = chkDisplayName.Checked;
            settings.DisplayGold = chkDisplayGold.Checked;
            settings.DisplayDeathCounter = chkDisplayDeathCounter.Checked;
            settings.DisplayLevel = chkDisplayLevel.Checked;
            settings.DisplayResistances = chkDisplayResistances.Checked;
            settings.DisplayBaseStats = chkDisplayBaseStats.Checked;
            settings.DisplayAdvancedStats = chkDisplayAdvancedStats.Checked;
            settings.DisplayDifficultyPercentages = chkDisplayDifficultyPercents.Checked;
            settings.DisplayRealFrwIas = chkShowRealValues.Checked;
            settings.DisplayPlayersX = chkShowPlayersX.Checked;
            settings.DisplayGameCounter = chkShowGameCounter.Checked;
            settings.DisplayCharCounter = chkDisplayCharCount.Checked;
            settings.DisplayMagicFind = checkBoxMagicFind.Checked;
            settings.DisplayMonsterGold = checkBoxMonsterGold.Checked;
            settings.DisplayAttackerSelfDamage = checkBoxAttackerSelfDamage.Checked;
            settings.DisplayHardcoreSoftcore = chkDisplayHardcoreSoftcore.Checked;
            settings.DisplayExpansionClassic = chkDisplayExpansionClassic.Checked;

            settings.DisplayRunes = chkDisplayRunes.Checked;
            settings.DisplayRunesHorizontal = comboBoxRunesOrientation.SelectedIndex == 0;
            settings.DisplayRunesHighContrast = chkHighContrastRunes.Checked;
            settings.DisplayLayoutHorizontal = comboBoxLayout.SelectedIndex == 0;

            settings.ColorName = btnSetNameColor.ForeColor;
            settings.ColorDeaths = btnSetDeathsColor.ForeColor;
            settings.ColorLevel = btnSetLevelColor.ForeColor;
            settings.ColorDifficultyPercentages = btnSetDifficultyColor.ForeColor;
            settings.ColorGold = btnSetGoldColor.ForeColor;
            settings.ColorBaseStats = btnSetBaseStatsColor.ForeColor;
            settings.ColorAdvancedStats = btnSetAdvancedStatsColor.ForeColor;
            settings.ColorFireRes = btnSetFireResColor.ForeColor;
            settings.ColorColdRes = btnSetColdResColor.ForeColor;
            settings.ColorLightningRes = btnSetLightningResColor.ForeColor;
            settings.ColorPoisonRes = btnSetPoisonResColor.ForeColor;
            settings.ColorPlayersX = btnSetPlayersXColor.ForeColor;
            settings.ColorGameCounter = btnSetGameCounterColor.ForeColor;
            settings.ColorCharCounter = btnColorCharCount.ForeColor;
            settings.ColorMagicFind = btnSetMFColor.ForeColor;
            settings.ColorMonsterGold = btnSetExtraGoldColor.ForeColor;
            settings.ColorAttackerSelfDamage = btnSetAttackerSelfDamageColor.ForeColor;
            settings.ColorHardcoreSoftcore = btnColorHardcoreSoftcore.ForeColor;
            settings.ColorExpansionClassic = btnColorExpansionClassic.ForeColor;
            settings.ColorBackground = btnSetBackgroundColor.BackColor;

            return settings;
        }

        void SettingsWindowOnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing || !IsDirty) return;

            DialogResult result = MessageBox.Show(
                @"Would you like to save your settings before closing?",
                @"Save Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Yes:
                    SaveSettings();
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        void SaveSettings(string filename = null)
        {
            UseWaitCursor = true;
            var path = filename ?? di.settings.CurrentSettingsFile;
            di.settings.SaveSettings(path, CopyModifiedSettings());
            di.settings.LoadSettings(path);
            UseWaitCursor = false;
        }

        // TODO: Differentiate between loading and reverting settings.
        bool LoadSettings(string filename)
        {
            UseWaitCursor = true;
            var success = di.settings.LoadSettings(filename);
            UseWaitCursor = false;
            return success;
        }

        void SaveSettingsAsMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SimpleSaveDialog(string.Empty))
            {
                saveDialog.StartPosition = FormStartPosition.CenterParent;
                var result = saveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SaveSettings(Path.Combine(SettingsFilePath, saveDialog.NewFileName) + ".conf");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                LoadSettings(Properties.Settings.Default.SettingsFile);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        void SettingsServiceOnSettingsCollectionChanged(object sender, SettingsCollectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsCollectionChanged(sender, e)));
                return;
            }

            PopulateSettingsFileList(e.Collection);
        }

        void PopulateSettingsFileList(IEnumerable<FileInfo> settingsFileCollection)
        {
            lstConfigFiles.Items.Clear();
            IEnumerable<ConfigEntry> items = settingsFileCollection.Select(CreateConfigEntry);
            lstConfigFiles.Items.AddRange(items.Cast<object>().ToArray());
        }

        static ConfigEntry CreateConfigEntry(FileInfo fileInfo) => new ConfigEntry()
        {
            DisplayName = Path.GetFileNameWithoutExtension(fileInfo.Name),
            Path = fileInfo.FullName
        };

        class ConfigEntry
        {
            public string DisplayName { get; set; }

            public string Path { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private void lstConfigFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // make sure we actually dbl click an item, not just anywhere in the box.
            int index = lstConfigFiles.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                LoadSettings(((ConfigEntry)lstConfigFiles.Items[index]).Path);
            }
        }

        private void lstConfigFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = this.lstConfigFiles.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    menuClone.Enabled = true;
                    menuLoad.Enabled = true;
                    menuNew.Enabled = true;
                    menuDelete.Enabled = true;
                }
                else
                {
                    menuClone.Enabled = false;
                    menuLoad.Enabled = false;
                    menuNew.Enabled = true;
                    menuDelete.Enabled = false;
                }
                lstConfigFiles.ContextMenuStrip.Show(lstConfigFiles, new Point(e.X, e.Y));
            }
        }

        void menuNew_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SimpleSaveDialog(string.Empty))
            {
                var result = saveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    NewSettings(Path.Combine(SettingsFilePath, saveDialog.NewFileName) + ".conf");
                }
            }
        }

        void NewSettings(string path)
        {
            UseWaitCursor = true;
            var settings = new ApplicationSettings();
            di.settings.SaveSettings(path, settings);
            di.settings.LoadSettings(path);
            UseWaitCursor = false;
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            LoadSettings(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
        }

        void menuClone_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SimpleSaveDialog(string.Empty))
            {
                var result = saveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    CloneSettings(
                        ((ConfigEntry)lstConfigFiles.SelectedItem).Path,
                        Path.Combine(SettingsFilePath, saveDialog.NewFileName) + ".conf"
                    );
                }
            }
        }

        void RenameSettings(string oldPath, string newPath)
        {
            File.Copy(oldPath, newPath);
            File.Delete(oldPath);
            LoadSettings(newPath);
        }

        void CloneSettings(string oldPath, string newPath)
        {
            File.Copy(oldPath, newPath);
            LoadSettings(newPath);
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            DeleteSettings(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
        }

        void DeleteSettings(string path)
        {
            File.Delete(path);
        }

        void SetBackgroundColor(Color c)
        {
            btnColorAll.BackColor = c;
            btnColorAll.ForeColor = (384 - c.R - c.G - c.B) > 0 ? Color.White : Color.Black;
            btnSetBackgroundColor.BackColor = c;
            btnSetBackgroundColor.ForeColor = (384 - c.R - c.G - c.B) > 0 ? Color.White : Color.Black;
            btnSetNameColor.BackColor = c;
            btnSetDeathsColor.BackColor = c;
            btnSetLevelColor.BackColor = c;
            btnSetDifficultyColor.BackColor = c;
            btnSetGoldColor.BackColor = c;
            btnSetBaseStatsColor.BackColor = c;
            btnSetAdvancedStatsColor.BackColor = c;
            btnSetFireResColor.BackColor = c;
            btnSetColdResColor.BackColor = c;
            btnSetLightningResColor.BackColor = c;
            btnSetPoisonResColor.BackColor = c;
            btnSetPlayersXColor.BackColor = c;
            btnSetGameCounterColor.BackColor = c;
            btnColorCharCount.BackColor = c;
            btnSetAttackerSelfDamageColor.BackColor = c;
            btnSetMFColor.BackColor = c;
            btnSetExtraGoldColor.BackColor = c;
            btnColorHardcoreSoftcore.BackColor = c;
            btnColorExpansionClassic.BackColor = c;
        }

        void SetForegroundColor(Color c)
        {
            btnSetNameColor.ForeColor = c;
            btnSetDeathsColor.ForeColor = c;
            btnSetLevelColor.ForeColor = c;
            btnSetDifficultyColor.ForeColor = c;
            btnSetGoldColor.ForeColor = c;
            btnSetBaseStatsColor.ForeColor = c;
            btnSetAdvancedStatsColor.ForeColor = c;
            btnSetFireResColor.ForeColor = c;
            btnSetColdResColor.ForeColor = c;
            btnSetLightningResColor.ForeColor = c;
            btnSetPoisonResColor.ForeColor = c;
            btnSetPlayersXColor.ForeColor = c;
            btnSetGameCounterColor.ForeColor = c;
            btnColorCharCount.ForeColor = c;
            btnSetAttackerSelfDamageColor.ForeColor = c;
            btnSetMFColor.ForeColor = c;
            btnSetExtraGoldColor.ForeColor = c;
            btnColorHardcoreSoftcore.ForeColor = c;
            btnColorExpansionClassic.ForeColor = c;
        }

        private void btnSelectColor(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
                ((Control)sender).ForeColor = d.Color;
        }

        private void resetColorsButton(object sender, EventArgs e)
        {
            btnSetNameColor.ForeColor = Color.RoyalBlue;
            btnSetDeathsColor.ForeColor = Color.Snow;
            btnSetLevelColor.ForeColor = Color.Snow;
            btnSetDifficultyColor.ForeColor = Color.Snow;
            btnSetGoldColor.ForeColor = Color.Gold;
            btnSetBaseStatsColor.ForeColor = Color.Coral;
            btnSetAdvancedStatsColor.ForeColor = Color.Coral;
            btnSetFireResColor.ForeColor = Color.Red;
            btnSetColdResColor.ForeColor = Color.DodgerBlue;
            btnSetLightningResColor.ForeColor = Color.Yellow;
            btnSetPoisonResColor.ForeColor = Color.YellowGreen;
            btnSetPlayersXColor.ForeColor = Color.Snow;
            btnSetGameCounterColor.ForeColor = Color.Snow;
            btnColorCharCount.ForeColor = Color.Snow;
            btnSetAttackerSelfDamageColor.ForeColor = Color.Snow;
            btnSetMFColor.ForeColor = Color.Gold;
            btnSetExtraGoldColor.ForeColor = Color.Gold;
            btnColorHardcoreSoftcore.ForeColor = Color.Snow;
            btnColorExpansionClassic.ForeColor = Color.Snow;
            SetBackgroundColor(Color.Black);
        }

        private void backgroundColorButtonClick(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SetBackgroundColor(d.Color);
            }
        }

        private void btnColorAll_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SetForegroundColor(d.Color);
            }
        }

        void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string current = ((ConfigEntry)lstConfigFiles.SelectedItem).Path;
            string fileName = Path.GetFileNameWithoutExtension(current);

            using (var saveDialog = new SimpleSaveDialog(fileName))
            {
                var result = saveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string newPath = Path.Combine(SettingsFilePath, saveDialog.NewFileName) + ".conf";
                    RenameSettings(current, newPath);
                }
            }
        }

        private void comboBoxLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxRunesOrientation.Enabled = comboBoxLayout.SelectedIndex == 0;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
