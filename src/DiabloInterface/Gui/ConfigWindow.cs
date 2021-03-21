namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.DiabloInterface.Lib.Extensions;
    using Zutatensuppe.DiabloInterface.Lib.Plugin;
    using Zutatensuppe.DiabloInterface.Lib.Services;
    using Zutatensuppe.DiabloInterface.Gui.Forms;
    using Zutatensuppe.DiabloInterface.Lib;

    public class ConfigWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string ConfigFilePath = Application.StartupPath + @"\Settings";

        private readonly IDiabloInterface di;

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
        private Button btnSetSeedColor;
        private CheckBox chkShowSeed;
        private Button btnSetLifeColor;
        private CheckBox chkShowLife;
        private Button btnSetManaColor;
        private CheckBox chkShowMana;
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

        public ConfigWindow(IDiabloInterface di)
        {
            Logger.Info("Creating config window.");

            this.di = di;

            RegisterServiceEventHandlers();
            InitializeComponent();
            PopulateConfigFileList(this.di.configService.ConfigFileCollection);

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing config window.");
                UnregisterServiceEventHandlers();
            };

            ReloadWithConfig(this.di.configService.CurrentConfig);
        }
        private void InitializeComponent()
        {
            FontLabel = new Label();
            FontGroup = new GroupBox();
            fontComboBox = new Controls.FontComboBox();
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
            chkShowPlayersX = new CheckBox();
            btnSetSeedColor = new Button();
            chkShowSeed = new CheckBox();
            btnSetLifeColor = new Button();
            chkShowLife = new CheckBox();
            btnSetManaColor = new Button();
            chkShowMana = new CheckBox();
            btnSetGameCounterColor = new Button();
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
            runeSettingsPage = new Controls.RuneSettingsPage();
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
            ((System.ComponentModel.ISupportInitialize)numericUpDownPaddingInVerticalLayout).BeginInit();
            groupBox1.SuspendLayout();
            tabPageSettingsRunes.SuspendLayout();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();

            fontComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            fontComboBox.DropDownWidth = 250;
            fontComboBox.FormattingEnabled = true;
            fontComboBox.Location = new Point(105, 21);
            fontComboBox.Size = new Size(117, 21);
            fontComboBox.Sorted = true;

            titleFontSizeNumeric = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)titleFontSizeNumeric).BeginInit();
            titleFontSizeNumeric.Location = new Point(105, 74);
            titleFontSizeNumeric.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            titleFontSizeNumeric.Size = new Size(118, 20);
            titleFontSizeNumeric.Value = new decimal(new int[] { 20, 0, 0, 0 });

            fontSizeNumeric = new NumericUpDown();
            fontSizeNumeric.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fontSizeNumeric.Location = new Point(105, 48);
            fontSizeNumeric.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            fontSizeNumeric.Size = new Size(118, 20);
            fontSizeNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });

            FontSizeLabel.AutoSize = true;
            FontSizeLabel.Location = new Point(6, 51);
            FontSizeLabel.Size = new Size(52, 13);
            FontSizeLabel.Text = "Font size:";

            TitleFontSizeLabel.AutoSize = true;
            TitleFontSizeLabel.Location = new Point(6, 77);
            TitleFontSizeLabel.Size = new Size(80, 13);
            TitleFontSizeLabel.Text = "Name font size:";

            FontLabel.AutoSize = true;
            FontLabel.Location = new Point(6, 24);
            FontLabel.Size = new Size(31, 13);
            FontLabel.TabIndex = 2;
            FontLabel.Text = "Font:";

            FontGroup.Controls.Add(fontComboBox);
            FontGroup.Controls.Add(titleFontSizeNumeric);
            FontGroup.Controls.Add(fontSizeNumeric);
            FontGroup.Controls.Add(FontSizeLabel);
            FontGroup.Controls.Add(TitleFontSizeLabel);
            FontGroup.Controls.Add(FontLabel);
            FontGroup.Location = new Point(9, 6);
            FontGroup.Margin = new Padding(0);
            FontGroup.Size = new Size(231, 104);
            FontGroup.Text = "Font";

            grpConfigFiles.Controls.Add(lstConfigFiles);
            grpConfigFiles.Dock = DockStyle.Fill;
            grpConfigFiles.Location = new Point(3, 3);
            grpConfigFiles.Size = new Size(169, 502);
            grpConfigFiles.Text = "Config Files";

            tabControl1.Controls.Add(tabPageSettingsLayout);
            tabControl1.Controls.Add(tabPageSettingsRunes);

            foreach (var p in di.plugins.CreateControls<IPluginConfigEditRenderer>())
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
            HorizontalSplitContainer.Controls.Add(tabControl1, 0, 0);
            HorizontalSplitContainer.Dock = DockStyle.Fill;
            HorizontalSplitContainer.Location = new Point(175, 0);
            HorizontalSplitContainer.Margin = new Padding(0);
            HorizontalSplitContainer.RowCount = 2;
            HorizontalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            HorizontalSplitContainer.RowStyles.Add(new RowStyle());
            HorizontalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            HorizontalSplitContainer.Size = new Size(519, 508);

            VerticalSplitContainer.ColumnCount = 2;
            VerticalSplitContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
            VerticalSplitContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            VerticalSplitContainer.Controls.Add(grpConfigFiles, 0, 0);
            VerticalSplitContainer.Controls.Add(HorizontalSplitContainer, 1, 0);
            VerticalSplitContainer.Dock = DockStyle.Fill;
            VerticalSplitContainer.Location = new Point(0, 0);
            VerticalSplitContainer.Margin = new Padding(0);
            VerticalSplitContainer.RowCount = 1;
            VerticalSplitContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            VerticalSplitContainer.Size = new Size(694, 508);

            lstConfigFiles.ContextMenuStrip = ctxConfigFileList;
            lstConfigFiles.Dock = DockStyle.Fill;
            lstConfigFiles.FormattingEnabled = true;
            lstConfigFiles.Location = new Point(3, 16);
            lstConfigFiles.Size = new Size(163, 483);
            lstConfigFiles.MouseDoubleClick += new MouseEventHandler(lstConfigFiles_MouseDoubleClick);
            lstConfigFiles.MouseUp += new MouseEventHandler(lstConfigFiles_MouseUp);

            ctxConfigFileList.ImageScalingSize = new Size(20, 20);
            ctxConfigFileList.Items.AddRange(new ToolStripItem[] {
            menuNew,
            menuLoad,
            renameToolStripMenuItem,
            menuClone,
            menuDelete});
            ctxConfigFileList.Size = new Size(118, 114);

            menuNew.Size = new Size(117, 22);
            menuNew.Text = "New";
            menuNew.Click += new EventHandler(menuNew_Click);

            menuLoad.Size = new Size(117, 22);
            menuLoad.Text = "Load";
            menuLoad.Click += new EventHandler(menuLoad_Click);

            renameToolStripMenuItem.Size = new Size(117, 22);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += new EventHandler(renameToolStripMenuItem_Click);

            menuClone.Size = new Size(117, 22);
            menuClone.Text = "Clone";
            menuClone.Click += new EventHandler(menuClone_Click);

            menuDelete.Size = new Size(117, 22);
            menuDelete.Text = "Delete";
            menuDelete.Click += new EventHandler(menuDelete_Click);

            tabPageSettingsLayout.Controls.Add(groupBox2);
            tabPageSettingsLayout.Controls.Add(FontGroup);
            tabPageSettingsLayout.Controls.Add(groupBox1);
            tabPageSettingsLayout.Dock = DockStyle.Fill;
            tabPageSettingsLayout.Text = "Layout";

            groupBox2.Controls.Add(numericUpDownPaddingInVerticalLayout);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(comboBoxRunesOrientation);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(comboBoxLayout);
            groupBox2.Location = new Point(249, 6);
            groupBox2.Size = new Size(241, 104);
            groupBox2.Text = "Layout";

            numericUpDownPaddingInVerticalLayout.Location = new Point(145, 75);
            numericUpDownPaddingInVerticalLayout.Size = new Size(81, 20);
            numericUpDownPaddingInVerticalLayout.TabIndex = 23;

            label3.AutoSize = true;
            label3.Location = new Point(6, 51);
            label3.Size = new Size(93, 13);
            label3.Text = "Runes orientation:";

            comboBoxRunesOrientation.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRunesOrientation.FormattingEnabled = true;
            comboBoxRunesOrientation.Items.AddRange(new object[] { "Horizontal", "Vertical" });
            comboBoxRunesOrientation.Location = new Point(145, 48);
            comboBoxRunesOrientation.Size = new Size(80, 21);

            label2.AutoSize = true;
            label2.Location = new Point(6, 24);
            label2.Size = new Size(61, 13);
            label2.Text = "Orientation:";

            label1.AutoSize = true;
            label1.Location = new Point(6, 77);
            label1.Size = new Size(128, 13);
            label1.Text = "Padding in vertical layout:";

            comboBoxLayout.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLayout.FormattingEnabled = true;
            comboBoxLayout.Items.AddRange(new object[] { "Horizontal", "Vertical" });
            comboBoxLayout.Location = new Point(145, 21);
            comboBoxLayout.Size = new Size(80, 21);
            comboBoxLayout.SelectedIndexChanged += new EventHandler(comboBoxLayout_SelectedIndexChanged);

            btnColorCharCount.Location = new Point(155, 249);
            btnColorCharCount.Size = new Size(44, 22);
            btnColorCharCount.Text = "Color";
            btnColorCharCount.Click += new EventHandler(btnSelectColor);

            chkDisplayCharCount.AutoSize = true;
            chkDisplayCharCount.Location = new Point(14, 253);
            chkDisplayCharCount.Size = new Size(116, 17);
            chkDisplayCharCount.Text = "Characters created";

            var i = 1;
            btnSetSeedColor.Location = new Point(155, 249 + i * 23);
            btnSetSeedColor.Margin = new Padding(0);
            btnSetSeedColor.Size = new Size(44, 22);
            btnSetSeedColor.Text = "Color";
            btnSetSeedColor.Click += new EventHandler(btnSelectColor);

            chkShowSeed.AutoSize = true;
            chkShowSeed.Location = new Point(14, 253 + i * 23);
            chkShowSeed.Size = new Size(136, 17);
            chkShowSeed.Text = "Seed (-seed)";

            i++;

            btnSetLifeColor.Location = new Point(155, 249 + i * 23);
            btnSetLifeColor.Margin = new Padding(0);
            btnSetLifeColor.Size = new Size(44, 22);
            btnSetLifeColor.Text = "Color";
            btnSetLifeColor.Click += new EventHandler(btnSelectColor);

            chkShowLife.AutoSize = true;
            chkShowLife.Location = new Point(14, 253 + i * 23);
            chkShowLife.Size = new Size(136, 17);
            chkShowLife.Text = "Life";

            i++;

            btnSetManaColor.Location = new Point(155, 249 + i * 23);
            btnSetManaColor.Margin = new Padding(0);
            btnSetManaColor.Size = new Size(44, 22);
            btnSetManaColor.Text = "Color";
            btnSetManaColor.Click += new EventHandler(btnSelectColor);

            chkShowMana.AutoSize = true;
            chkShowMana.Location = new Point(14, 253 + i * 23);
            chkShowMana.Size = new Size(136, 17);
            chkShowMana.Text = "Mana";

            btnColorAll.Location = new Point(338, 280);
            btnColorAll.Size = new Size(136, 22);
            btnColorAll.Text = "Text Color (All)";
            btnColorAll.Click += new EventHandler(btnColorAll_Click);

            chkDisplayExpansionClassic.AutoSize = true;
            chkDisplayExpansionClassic.Location = new Point(264, 138);
            chkDisplayExpansionClassic.Size = new Size(86, 17);
            chkDisplayExpansionClassic.Text = "Classic/LOD";

            btnColorExpansionClassic.Location = new Point(397, 134);
            btnColorExpansionClassic.Size = new Size(44, 22);
            btnColorExpansionClassic.Text = "Color";
            btnColorExpansionClassic.Click += new EventHandler(btnSelectColor);

            groupBox1.Controls.Add(btnColorCharCount);
            groupBox1.Controls.Add(chkDisplayCharCount);
            groupBox1.Controls.Add(btnColorAll);
            groupBox1.Controls.Add(chkDisplayExpansionClassic);
            groupBox1.Controls.Add(btnColorExpansionClassic);
            groupBox1.Controls.Add(chkDisplayHardcoreSoftcore);
            groupBox1.Controls.Add(btnColorHardcoreSoftcore);
            groupBox1.Controls.Add(checkBoxAttackerSelfDamage);
            groupBox1.Controls.Add(checkBoxMonsterGold);
            groupBox1.Controls.Add(checkBoxMagicFind);
            groupBox1.Controls.Add(btnSetAttackerSelfDamageColor);
            groupBox1.Controls.Add(btnSetExtraGoldColor);
            groupBox1.Controls.Add(btnSetMFColor);
            groupBox1.Controls.Add(btnSetPlayersXColor);
            groupBox1.Controls.Add(chkShowPlayersX);
            groupBox1.Controls.Add(btnSetSeedColor);
            groupBox1.Controls.Add(chkShowSeed);
            groupBox1.Controls.Add(btnSetLifeColor);
            groupBox1.Controls.Add(chkShowLife);
            groupBox1.Controls.Add(btnSetManaColor);
            groupBox1.Controls.Add(chkShowMana);
            groupBox1.Controls.Add(btnSetGameCounterColor);
            groupBox1.Controls.Add(chkShowGameCounter);
            groupBox1.Controls.Add(chkShowRealValues);
            groupBox1.Controls.Add(chkHighContrastRunes);
            groupBox1.Controls.Add(chkDisplayRunes);
            groupBox1.Controls.Add(btnSetBackgroundColor);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(btnSetLevelColor);
            groupBox1.Controls.Add(btnSetDifficultyColor);
            groupBox1.Controls.Add(btnSetPoisonResColor);
            groupBox1.Controls.Add(btnSetLightningResColor);
            groupBox1.Controls.Add(btnSetColdResColor);
            groupBox1.Controls.Add(btnSetFireResColor);
            groupBox1.Controls.Add(btnSetDeathsColor);
            groupBox1.Controls.Add(btnSetAdvancedStatsColor);
            groupBox1.Controls.Add(btnSetBaseStatsColor);
            groupBox1.Controls.Add(btnSetGoldColor);
            groupBox1.Controls.Add(btnSetNameColor);
            groupBox1.Controls.Add(chkDisplayDifficultyPercents);
            groupBox1.Controls.Add(chkDisplayAdvancedStats);
            groupBox1.Controls.Add(chkDisplayLevel);
            groupBox1.Controls.Add(chkDisplayGold);
            groupBox1.Controls.Add(chkDisplayResistances);
            groupBox1.Controls.Add(chkDisplayBaseStats);
            groupBox1.Controls.Add(chkDisplayDeathCounter);
            groupBox1.Controls.Add(chkDisplayName);
            groupBox1.Location = new Point(9, 110);
            groupBox1.Margin = new Padding(0);
            groupBox1.Size = new Size(481, 366);
            groupBox1.Text = "Display";

            chkDisplayHardcoreSoftcore.AutoSize = true;
            chkDisplayHardcoreSoftcore.Location = new Point(264, 115);
            chkDisplayHardcoreSoftcore.Size = new Size(60, 17);
            chkDisplayHardcoreSoftcore.Text = "HC/SC";

            btnColorHardcoreSoftcore.Location = new Point(397, 111);
            btnColorHardcoreSoftcore.Size = new Size(44, 22);
            btnColorHardcoreSoftcore.Text = "Color";
            btnColorHardcoreSoftcore.Click += new EventHandler(btnSelectColor);

            checkBoxAttackerSelfDamage.AutoSize = true;
            checkBoxAttackerSelfDamage.Location = new Point(264, 70);
            checkBoxAttackerSelfDamage.Size = new Size(130, 17);
            checkBoxAttackerSelfDamage.Text = "Attacker Self Damage";

            checkBoxMonsterGold.AutoSize = true;
            checkBoxMonsterGold.Location = new Point(264, 47);
            checkBoxMonsterGold.Size = new Size(75, 17);
            checkBoxMonsterGold.Text = "Extra Gold";

            checkBoxMagicFind.AutoSize = true;
            checkBoxMagicFind.Location = new Point(264, 24);
            checkBoxMagicFind.Size = new Size(78, 17);
            checkBoxMagicFind.Text = "Magic Find";

            btnSetAttackerSelfDamageColor.Location = new Point(397, 66);
            btnSetAttackerSelfDamageColor.Margin = new Padding(0);
            btnSetAttackerSelfDamageColor.Size = new Size(44, 22);
            btnSetAttackerSelfDamageColor.Text = "Color";
            btnSetAttackerSelfDamageColor.Click += new EventHandler(btnSelectColor);

            btnSetExtraGoldColor.Location = new Point(397, 43);
            btnSetExtraGoldColor.Margin = new Padding(0);
            btnSetExtraGoldColor.Size = new Size(44, 22);
            btnSetExtraGoldColor.Text = "Color";
            btnSetExtraGoldColor.Click += new EventHandler(btnSelectColor);

            btnSetMFColor.Location = new Point(397, 20);
            btnSetMFColor.Margin = new Padding(0);
            btnSetMFColor.Size = new Size(44, 22);
            btnSetMFColor.Text = "Color";
            btnSetMFColor.Click += new EventHandler(btnSelectColor);

            btnSetPlayersXColor.Location = new Point(155, 203);
            btnSetPlayersXColor.Margin = new Padding(0);
            btnSetPlayersXColor.Size = new Size(44, 22);
            btnSetPlayersXColor.Text = "Color";
            btnSetPlayersXColor.Click += new EventHandler(btnSelectColor);

            chkShowPlayersX.AutoSize = true;
            chkShowPlayersX.Location = new Point(14, 207);
            chkShowPlayersX.Size = new Size(74, 17);
            chkShowPlayersX.Text = "/players X";

            btnSetGameCounterColor.Location = new Point(155, 226);
            btnSetGameCounterColor.Margin = new Padding(0);
            btnSetGameCounterColor.Size = new Size(44, 22);
            btnSetGameCounterColor.Text = "Color";
            btnSetGameCounterColor.Click += new EventHandler(btnSelectColor);

            chkShowGameCounter.AutoSize = true;
            chkShowGameCounter.Location = new Point(14, 230);
            chkShowGameCounter.Size = new Size(106, 17);
            chkShowGameCounter.Text = "Games launched";

            chkShowRealValues.AutoSize = true;
            chkShowRealValues.Location = new Point(203, 92);
            chkShowRealValues.Size = new Size(109, 17);
            chkShowRealValues.Text = "calculated values";

            chkHighContrastRunes.AutoSize = true;
            chkHighContrastRunes.Location = new Point(82, 341);
            chkHighContrastRunes.Size = new Size(89, 17);
            chkHighContrastRunes.Text = "High contrast";

            chkDisplayRunes.AutoSize = true;
            chkDisplayRunes.Location = new Point(14, 341);
            chkDisplayRunes.Size = new Size(57, 17);
            chkDisplayRunes.Text = "Runes";

            btnSetBackgroundColor.Location = new Point(339, 308);
            btnSetBackgroundColor.Size = new Size(136, 22);
            btnSetBackgroundColor.Text = "Background color";
            btnSetBackgroundColor.Click += new EventHandler(backgroundColorButtonClick);

            button1.Location = new Point(339, 336);
            button1.Size = new Size(136, 22);
            button1.Text = "Reset to default colors";
            button1.Click += new EventHandler(resetColorsButton);

            btnSetLevelColor.Location = new Point(155, 134);
            btnSetLevelColor.Size = new Size(44, 22);
            btnSetLevelColor.Text = "Color";
            btnSetLevelColor.Click += new EventHandler(btnSelectColor);

            btnSetDifficultyColor.Location = new Point(155, 180);
            btnSetDifficultyColor.Size = new Size(44, 22);
            btnSetDifficultyColor.Text = "Color";
            btnSetDifficultyColor.Click += new EventHandler(btnSelectColor);

            btnSetPoisonResColor.Location = new Point(287, 157);
            btnSetPoisonResColor.Size = new Size(44, 22);
            btnSetPoisonResColor.Text = "Pois.";
            btnSetPoisonResColor.Click += new EventHandler(btnSelectColor);

            btnSetLightningResColor.Location = new Point(243, 157);
            btnSetLightningResColor.Size = new Size(44, 22);
            btnSetLightningResColor.Text = "Light.";
            btnSetLightningResColor.Click += new EventHandler(btnSelectColor);

            btnSetColdResColor.Location = new Point(199, 157);
            btnSetColdResColor.Size = new Size(44, 22);
            btnSetColdResColor.Text = "Cold";
            btnSetColdResColor.Click += new EventHandler(btnSelectColor);

            btnSetFireResColor.Location = new Point(155, 157);
            btnSetFireResColor.Size = new Size(44, 22);
            btnSetFireResColor.Text = "Fire";
            btnSetFireResColor.Click += new EventHandler(btnSelectColor);

            btnSetDeathsColor.Location = new Point(155, 111);
            btnSetDeathsColor.Size = new Size(44, 22);
            btnSetDeathsColor.Text = "Color";
            btnSetDeathsColor.Click += new EventHandler(btnSelectColor);

            btnSetAdvancedStatsColor.Location = new Point(155, 88);
            btnSetAdvancedStatsColor.Size = new Size(44, 22);
            btnSetAdvancedStatsColor.Text = "Color";
            btnSetAdvancedStatsColor.Click += new EventHandler(btnSelectColor);

            btnSetBaseStatsColor.Location = new Point(155, 66);
            btnSetBaseStatsColor.Size = new Size(44, 22);
            btnSetBaseStatsColor.Text = "Color";
            btnSetBaseStatsColor.Click += new EventHandler(btnSelectColor);

            btnSetGoldColor.Location = new Point(155, 43);
            btnSetGoldColor.Size = new Size(44, 22);
            btnSetGoldColor.Text = "Color";
            btnSetGoldColor.Click += new EventHandler(btnSelectColor);

            btnSetNameColor.Location = new Point(155, 20);
            btnSetNameColor.Size = new Size(44, 22);
            btnSetNameColor.Text = "Color";
            btnSetNameColor.Click += new EventHandler(btnSelectColor);

            chkDisplayDifficultyPercents.AutoSize = true;
            chkDisplayDifficultyPercents.Location = new Point(14, 184);
            chkDisplayDifficultyPercents.Size = new Size(77, 17);
            chkDisplayDifficultyPercents.Text = "Difficulty %";

            chkDisplayAdvancedStats.AutoSize = true;
            chkDisplayAdvancedStats.Location = new Point(14, 92);
            chkDisplayAdvancedStats.Size = new Size(105, 17);
            chkDisplayAdvancedStats.Text = "Fcr, Frw, Fhr, Ias";

            chkDisplayLevel.AutoSize = true;
            chkDisplayLevel.Location = new Point(14, 138);
            chkDisplayLevel.Size = new Size(101, 17);
            chkDisplayLevel.Text = "Character Level";

            chkDisplayGold.AutoSize = true;
            chkDisplayGold.Location = new Point(14, 47);
            chkDisplayGold.Size = new Size(48, 17);
            chkDisplayGold.Text = "Gold";

            chkDisplayResistances.AutoSize = true;
            chkDisplayResistances.Location = new Point(14, 161);
            chkDisplayResistances.Size = new Size(84, 17);
            chkDisplayResistances.Text = "Resistances";

            chkDisplayBaseStats.AutoSize = true;
            chkDisplayBaseStats.Location = new Point(14, 70);
            chkDisplayBaseStats.Size = new Size(107, 17);
            chkDisplayBaseStats.Text = "Str, Dex, Vit, Ene";

            chkDisplayDeathCounter.AutoSize = true;
            chkDisplayDeathCounter.Location = new Point(14, 115);
            chkDisplayDeathCounter.Size = new Size(60, 17);
            chkDisplayDeathCounter.Text = "Deaths";

            chkDisplayName.AutoSize = true;
            chkDisplayName.Location = new Point(14, 24);
            chkDisplayName.Size = new Size(54, 17);
            chkDisplayName.Text = "Name";

            tabPageSettingsRunes.Controls.Add(runeSettingsPage);
            tabPageSettingsRunes.Location = new Point(4, 22);
            tabPageSettingsRunes.Size = new Size(505, 476);
            tabPageSettingsRunes.Text = "Runes";

            runeSettingsPage.Dock = DockStyle.Fill;
            runeSettingsPage.Location = new Point(0, 0);
            runeSettingsPage.Margin = new Padding(4);
            runeSettingsPage.SettingsList = new List<ClassRuneSettings>();
            runeSettingsPage.Size = new Size(505, 476);

            mainPanel.ColumnCount = 1;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(VerticalSplitContainer, 0, 0);
            mainPanel.Controls.Add(panel1, 0, 1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Margin = new Padding(0);
            mainPanel.RowCount = 2;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            mainPanel.Size = new Size(694, 540);

            panel1.Controls.Add(btnSaveAs);
            panel1.Controls.Add(btnUndo);
            panel1.Controls.Add(btnSave);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 508);
            panel1.Margin = new Padding(0);
            panel1.Size = new Size(694, 32);

            btnSaveAs.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveAs.Location = new Point(520, 6);
            btnSaveAs.Size = new Size(75, 23);
            btnSaveAs.Text = "Save As";
            btnSaveAs.Click += new EventHandler(SaveConfigAsMenuItem_Click);

            btnUndo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnUndo.Location = new Point(600, 6);
            btnUndo.Size = new Size(90, 23);
            btnUndo.Text = "Undo Changes";
            btnUndo.Click += new EventHandler(btnCancel_Click);

            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(438, 6);
            btnSave.Size = new Size(75, 23);
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 540);
            Controls.Add(mainPanel);
            Icon = Properties.Resources.di;
            MinimumSize = new Size(700, 538);
            Text = "Config";
            FormClosing += new FormClosingEventHandler(ConfigWindowOnFormClosing);
            FontGroup.ResumeLayout(false);
            FontGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)titleFontSizeNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)fontSizeNumeric).EndInit();
            VerticalSplitContainer.ResumeLayout(false);
            grpConfigFiles.ResumeLayout(false);
            ctxConfigFileList.ResumeLayout(false);
            HorizontalSplitContainer.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPageSettingsLayout.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPaddingInVerticalLayout).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPageSettingsRunes.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        bool IsDirty
        {
            get
            {
                var config = di.configService.CurrentConfig;

                return dirty
                    || di.plugins.EditedConfigsDirty
                    || !CompareClassRuneSettings(config)

                    || config.FontName != GetFontName()
                    || config.FontSize != (int)fontSizeNumeric.Value
                    || config.FontSizeTitle != (int)titleFontSizeNumeric.Value
                    || config.DisplayName != chkDisplayName.Checked
                    || config.DisplayGold != chkDisplayGold.Checked
                    || config.DisplayDeathCounter != chkDisplayDeathCounter.Checked
                    || config.DisplayLevel != chkDisplayLevel.Checked
                    || config.DisplayResistances != chkDisplayResistances.Checked
                    || config.DisplayBaseStats != chkDisplayBaseStats.Checked
                    || config.DisplayAdvancedStats != chkDisplayAdvancedStats.Checked
                    || config.DisplayRunes != chkDisplayRunes.Checked
                    || config.DisplayRunesHorizontal != (comboBoxRunesOrientation.SelectedIndex == 0)
                    || config.DisplayRunesHighContrast != chkHighContrastRunes.Checked
                    || config.DisplayDifficultyPercentages != chkDisplayDifficultyPercents.Checked
                    || config.DisplayLayoutHorizontal != (comboBoxLayout.SelectedIndex == 0)
                    || config.VerticalLayoutPadding != (int)numericUpDownPaddingInVerticalLayout.Value
                    || config.DisplayRealFrwIas != chkShowRealValues.Checked
                    || config.DisplayPlayersX != chkShowPlayersX.Checked
                    || config.DisplaySeed != chkShowSeed.Checked
                    || config.DisplayLife != chkShowLife.Checked
                    || config.DisplayMana != chkShowMana.Checked
                    || config.DisplayGameCounter != chkShowGameCounter.Checked
                    || config.DisplayCharCounter != chkDisplayCharCount.Checked
                    || config.DisplayMagicFind != checkBoxMagicFind.Checked
                    || config.DisplayMonsterGold != checkBoxMonsterGold.Checked
                    || config.DisplayAttackerSelfDamage != checkBoxAttackerSelfDamage.Checked
                    || config.DisplayHardcoreSoftcore != chkDisplayHardcoreSoftcore.Checked
                    || config.DisplayExpansionClassic != chkDisplayExpansionClassic.Checked

                    || config.ColorName != btnSetNameColor.ForeColor
                    || config.ColorDeaths != btnSetDeathsColor.ForeColor
                    || config.ColorLevel != btnSetLevelColor.ForeColor
                    || config.ColorDifficultyPercentages != btnSetDifficultyColor.ForeColor
                    || config.ColorGold != btnSetGoldColor.ForeColor
                    || config.ColorBaseStats != btnSetBaseStatsColor.ForeColor
                    || config.ColorAdvancedStats != btnSetAdvancedStatsColor.ForeColor
                    || config.ColorFireRes != btnSetFireResColor.ForeColor
                    || config.ColorColdRes != btnSetColdResColor.ForeColor
                    || config.ColorLightningRes != btnSetLightningResColor.ForeColor
                    || config.ColorPoisonRes != btnSetPoisonResColor.ForeColor
                    || config.ColorPlayersX != btnSetPlayersXColor.ForeColor
                    || config.ColorSeed != btnSetSeedColor.ForeColor
                    || config.ColorLife != btnSetLifeColor.ForeColor
                    || config.ColorMana != btnSetManaColor.ForeColor
                    || config.ColorGameCounter != btnSetGameCounterColor.ForeColor
                    || config.ColorCharCounter != btnColorCharCount.ForeColor
                    || config.ColorMagicFind != btnSetMFColor.ForeColor
                    || config.ColorMonsterGold != btnSetExtraGoldColor.ForeColor
                    || config.ColorAttackerSelfDamage != btnSetAttackerSelfDamageColor.ForeColor
                    || config.ColorHardcoreSoftcore != btnColorHardcoreSoftcore.ForeColor
                    || config.ColorExpansionClassic != btnColorExpansionClassic.ForeColor

                    || config.ColorBackground != btnSetBackgroundColor.BackColor
                ;
            }
        }

        bool CompareClassRuneSettings(ApplicationConfig config)
        {
            IReadOnlyList<IClassRuneSettings> a = config.ClassRunes;
            IReadOnlyList<IClassRuneSettings> b = runeSettingsPage.SettingsList;

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
            di.configService.Changed += ConfigChanged;
            di.configService.CollectionChanged += ConfigCollectionChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            di.configService.Changed -= ConfigChanged;
            di.configService.CollectionChanged -= ConfigCollectionChanged;
        }

        void ConfigChanged(object sender, ApplicationConfigEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => ConfigChanged(sender, e)));
                return;
            }

            // NOTE: This may have been due to loading config from elsewhere. For now the
            ////     behavior will be to refresh the config window in that case.
            ReloadWithConfig(e.Config);
        }

        void ReloadWithConfig(ApplicationConfig c)
        {
            Text = $@"Config ({Path.GetFileName(di.configService.CurrentConfigFile)})";

            var config = c.DeepCopy();

            runeSettingsPage.SettingsList = config.ClassRunes;

            if (config.FontName != null)
                fontComboBox.SelectedIndex = fontComboBox.Items.IndexOf(config.FontName);

            fontSizeNumeric.Value = config.FontSize;
            titleFontSizeNumeric.Value = config.FontSizeTitle;
            numericUpDownPaddingInVerticalLayout.Value = config.VerticalLayoutPadding;

            chkDisplayName.Checked = config.DisplayName;
            chkDisplayGold.Checked = config.DisplayGold;
            chkDisplayDeathCounter.Checked = config.DisplayDeathCounter;
            chkDisplayLevel.Checked = config.DisplayLevel;
            chkDisplayResistances.Checked = config.DisplayResistances;
            chkDisplayBaseStats.Checked = config.DisplayBaseStats;
            chkDisplayAdvancedStats.Checked = config.DisplayAdvancedStats;
            chkDisplayRunes.Checked = config.DisplayRunes;
            comboBoxRunesOrientation.SelectedIndex = config.DisplayRunesHorizontal ? 0 : 1;
            chkDisplayDifficultyPercents.Checked = config.DisplayDifficultyPercentages;
            chkHighContrastRunes.Checked = config.DisplayRunesHighContrast;
            comboBoxLayout.SelectedIndex = config.DisplayLayoutHorizontal ? 0 : 1;
            chkShowRealValues.Checked = config.DisplayRealFrwIas;
            chkShowPlayersX.Checked = config.DisplayPlayersX;
            chkShowSeed.Checked = config.DisplaySeed;
            chkShowLife.Checked = config.DisplayLife;
            chkShowMana.Checked = config.DisplayMana;
            chkShowGameCounter.Checked = config.DisplayGameCounter;
            chkDisplayCharCount.Checked = config.DisplayCharCounter;
            checkBoxMagicFind.Checked = config.DisplayMagicFind;
            checkBoxMonsterGold.Checked = config.DisplayMonsterGold;
            checkBoxAttackerSelfDamage.Checked = config.DisplayAttackerSelfDamage;
            chkDisplayHardcoreSoftcore.Checked = config.DisplayHardcoreSoftcore;
            chkDisplayExpansionClassic.Checked = config.DisplayExpansionClassic;

            btnSetNameColor.ForeColor = config.ColorName;
            btnSetDeathsColor.ForeColor = config.ColorDeaths;
            btnSetLevelColor.ForeColor = config.ColorLevel;
            btnSetDifficultyColor.ForeColor = config.ColorDifficultyPercentages;
            btnSetGoldColor.ForeColor = config.ColorGold;
            btnSetBaseStatsColor.ForeColor = config.ColorBaseStats;
            btnSetAdvancedStatsColor.ForeColor = config.ColorAdvancedStats;
            btnSetFireResColor.ForeColor = config.ColorFireRes;
            btnSetColdResColor.ForeColor = config.ColorColdRes;
            btnSetLightningResColor.ForeColor = config.ColorLightningRes;
            btnSetPoisonResColor.ForeColor = config.ColorPoisonRes;
            btnSetPlayersXColor.ForeColor = config.ColorPlayersX;
            btnSetSeedColor.ForeColor = config.ColorSeed;
            btnSetLifeColor.ForeColor = config.ColorLife;
            btnSetManaColor.ForeColor = config.ColorMana;
            btnSetGameCounterColor.ForeColor = config.ColorGameCounter;
            btnColorCharCount.ForeColor = config.ColorCharCounter;
            btnSetMFColor.ForeColor = config.ColorMagicFind;
            btnSetExtraGoldColor.ForeColor = config.ColorMonsterGold;
            btnSetAttackerSelfDamageColor.ForeColor = config.ColorAttackerSelfDamage;
            btnColorHardcoreSoftcore.ForeColor = config.ColorHardcoreSoftcore;
            btnColorExpansionClassic.ForeColor = config.ColorExpansionClassic;

            SetBackgroundColor(config.ColorBackground);

            // Loading the settings will dirty mark pretty much everything, here
            // we just verify that nothing has actually changed yet.
            dirty = false;
        }

        private string GetFontName()
        {
            if (fontComboBox.SelectedItem != null)
                return fontComboBox.SelectedItem.ToString();

            foreach (string comboBoxFontName in fontComboBox.Items)
                if (comboBoxFontName.Equals(fontComboBox.Text))
                    return fontComboBox.Text;

            return null;
        }

        ApplicationConfig CopyModifiedConfig()
        {
            var config = di.configService.CurrentConfig.DeepCopy();

            foreach (var p in di.plugins.GetEditedConfigs)
                config.Plugins[p.Key] = p.Value;

            config.ClassRunes = runeSettingsPage.SettingsList ?? new List<ClassRuneSettings>();

            config.FontSize = (int)fontSizeNumeric.Value;
            config.FontSizeTitle = (int)titleFontSizeNumeric.Value;
            config.VerticalLayoutPadding = (int)numericUpDownPaddingInVerticalLayout.Value;
            config.FontName = GetFontName();

            config.DisplayName = chkDisplayName.Checked;
            config.DisplayGold = chkDisplayGold.Checked;
            config.DisplayDeathCounter = chkDisplayDeathCounter.Checked;
            config.DisplayLevel = chkDisplayLevel.Checked;
            config.DisplayResistances = chkDisplayResistances.Checked;
            config.DisplayBaseStats = chkDisplayBaseStats.Checked;
            config.DisplayAdvancedStats = chkDisplayAdvancedStats.Checked;
            config.DisplayDifficultyPercentages = chkDisplayDifficultyPercents.Checked;
            config.DisplayRealFrwIas = chkShowRealValues.Checked;
            config.DisplayPlayersX = chkShowPlayersX.Checked;
            config.DisplaySeed = chkShowSeed.Checked;
            config.DisplayLife = chkShowLife.Checked;
            config.DisplayMana = chkShowMana.Checked;
            config.DisplayGameCounter = chkShowGameCounter.Checked;
            config.DisplayCharCounter = chkDisplayCharCount.Checked;
            config.DisplayMagicFind = checkBoxMagicFind.Checked;
            config.DisplayMonsterGold = checkBoxMonsterGold.Checked;
            config.DisplayAttackerSelfDamage = checkBoxAttackerSelfDamage.Checked;
            config.DisplayHardcoreSoftcore = chkDisplayHardcoreSoftcore.Checked;
            config.DisplayExpansionClassic = chkDisplayExpansionClassic.Checked;

            config.DisplayRunes = chkDisplayRunes.Checked;
            config.DisplayRunesHorizontal = comboBoxRunesOrientation.SelectedIndex == 0;
            config.DisplayRunesHighContrast = chkHighContrastRunes.Checked;
            config.DisplayLayoutHorizontal = comboBoxLayout.SelectedIndex == 0;

            config.ColorName = btnSetNameColor.ForeColor;
            config.ColorDeaths = btnSetDeathsColor.ForeColor;
            config.ColorLevel = btnSetLevelColor.ForeColor;
            config.ColorDifficultyPercentages = btnSetDifficultyColor.ForeColor;
            config.ColorGold = btnSetGoldColor.ForeColor;
            config.ColorBaseStats = btnSetBaseStatsColor.ForeColor;
            config.ColorAdvancedStats = btnSetAdvancedStatsColor.ForeColor;
            config.ColorFireRes = btnSetFireResColor.ForeColor;
            config.ColorColdRes = btnSetColdResColor.ForeColor;
            config.ColorLightningRes = btnSetLightningResColor.ForeColor;
            config.ColorPoisonRes = btnSetPoisonResColor.ForeColor;
            config.ColorPlayersX = btnSetPlayersXColor.ForeColor;
            config.ColorSeed = btnSetSeedColor.ForeColor;
            config.ColorLife = btnSetLifeColor.ForeColor;
            config.ColorMana = btnSetManaColor.ForeColor;
            config.ColorGameCounter = btnSetGameCounterColor.ForeColor;
            config.ColorCharCounter = btnColorCharCount.ForeColor;
            config.ColorMagicFind = btnSetMFColor.ForeColor;
            config.ColorMonsterGold = btnSetExtraGoldColor.ForeColor;
            config.ColorAttackerSelfDamage = btnSetAttackerSelfDamageColor.ForeColor;
            config.ColorHardcoreSoftcore = btnColorHardcoreSoftcore.ForeColor;
            config.ColorExpansionClassic = btnColorExpansionClassic.ForeColor;
            config.ColorBackground = btnSetBackgroundColor.BackColor;

            return config;
        }

        void ConfigWindowOnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing || !IsDirty) return;

            DialogResult result = MessageBox.Show(
                @"Would you like to save your config before closing?",
                @"Save Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            switch (result)
            {
                case DialogResult.Yes:
                    SaveConfig(di.configService.CurrentConfigFile);
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        void SaveConfig(string path)
        {
            UseWaitCursor = true;
            di.configService.Save(path, CopyModifiedConfig());
            UseWaitCursor = false;
        }

        void LoadConfig(string path)
        {
            UseWaitCursor = true;
            di.configService.Load(path);
            UseWaitCursor = false;
        }

        void NewConfig(string path)
        {
            UseWaitCursor = true;
            di.configService.Save(path, new ApplicationConfig());
            UseWaitCursor = false;
        }

        void DeleteConfig(string path)
        {
            UseWaitCursor = true;
            di.configService.Delete(path);
            UseWaitCursor = false;
        }

        void Rename(string oldPath, string newPath)
        {
            UseWaitCursor = true;
            di.configService.Rename(oldPath, newPath);
            di.configService.Load(newPath);
            UseWaitCursor = false;
        }

        void Clone(string oldPath, string newPath)
        {
            UseWaitCursor = true;
            di.configService.Clone(oldPath, newPath);
            di.configService.Load(newPath);
            UseWaitCursor = false;
        }

        void SaveConfigAsMenuItem_Click(object sender, EventArgs e)
        {
            using (var d = new SimpleSaveDialog("Save as", string.Empty))
            {
                d.StartPosition = FormStartPosition.CenterParent;
                if (d.ShowDialog() == DialogResult.OK)
                    SaveConfig(Path.Combine(ConfigFilePath, d.NewFileName) + ".conf");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsDirty)
                LoadConfig(Properties.Settings.Default.SettingsFile);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig(di.configService.CurrentConfigFile);
        }

        void ConfigCollectionChanged(object sender, ConfigCollectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => ConfigCollectionChanged(sender, e)));
                return;
            }

            PopulateConfigFileList(e.Collection);
        }

        void PopulateConfigFileList(IEnumerable<FileInfo> configFileCollection)
        {
            lstConfigFiles.Items.Clear();
            IEnumerable<ConfigEntry> items = configFileCollection.Select(CreateConfigEntry);
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
                LoadConfig(((ConfigEntry)lstConfigFiles.Items[index]).Path);
        }

        private void lstConfigFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lstConfigFiles.IndexFromPoint(e.Location);
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
            using (var d = new SimpleSaveDialog("New config", string.Empty))
                if (d.ShowDialog() == DialogResult.OK)
                    NewConfig(Path.Combine(ConfigFilePath, d.NewFileName) + ".conf");
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            LoadConfig(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
        }

        void menuClone_Click(object sender, EventArgs e)
        {
            using (var d = new SimpleSaveDialog("Clone config", string.Empty))
                if (d.ShowDialog() == DialogResult.OK)
                    Clone(
                        ((ConfigEntry)lstConfigFiles.SelectedItem).Path,
                        Path.Combine(ConfigFilePath, d.NewFileName) + ".conf"
                    );
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            DeleteConfig(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
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
            btnSetSeedColor.BackColor = c;
            btnSetLifeColor.BackColor = c;
            btnSetManaColor.BackColor = c;
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
            btnSetSeedColor.ForeColor = c;
            btnSetLifeColor.ForeColor = c;
            btnSetManaColor.ForeColor = c;
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
            using (var d = new ColorDialog())
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
            btnSetSeedColor.ForeColor = Color.Snow;
            btnSetLifeColor.ForeColor = Color.Snow;
            btnSetManaColor.ForeColor = Color.Snow;
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
            using (var d = new ColorDialog())
                if (d.ShowDialog() == DialogResult.OK)
                    SetBackgroundColor(d.Color);
        }

        private void btnColorAll_Click(object sender, EventArgs e)
        {
            using (var d = new ColorDialog())
                if (d.ShowDialog() == DialogResult.OK)
                    SetForegroundColor(d.Color);
        }

        void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string current = ((ConfigEntry)lstConfigFiles.SelectedItem).Path;
            string fileName = Path.GetFileNameWithoutExtension(current);

            using (var d = new SimpleSaveDialog("Rename config", fileName))
                if (d.ShowDialog() == DialogResult.OK)
                    Rename(current, Path.Combine(ConfigFilePath, d.NewFileName) + ".conf");
        }

        private void comboBoxLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxRunesOrientation.Enabled = comboBoxLayout.SelectedIndex == 0;
        }
    }
}
