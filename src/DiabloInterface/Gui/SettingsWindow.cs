namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Newtonsoft.Json;

    using Zutatensuppe.DiabloInterface.Business;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Gui.Controls;
    using Zutatensuppe.DiabloInterface.Gui.Forms;

    public partial class SettingsWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISettingsService settingsService;

        string SettingsFilePath = Application.StartupPath + @"\Settings";

        AutoSplitTable autoSplitTable;

        bool dirty;

        public SettingsWindow(ISettingsService settingsService)
        {
            Logger.Info("Creating settings window.");

            this.settingsService = settingsService;

            InitializeComponent();
            InitializeAutoSplitTable();
            InitializeRunes();
            PopulateSettingsFileList(settingsService.SettingsFileCollection);

            // Select first rune (don't leave combo box empty).
            RuneComboBox.SelectedIndex = 0;
            cbRuneWord.SelectedIndex = 0;

            ReloadComponentsWithCurrentSettings();

            // Loading the settings will dirty mark pretty much everything, here
            // we just verify that nothing has actually changed yet.
            MarkClean();
        }

        public bool IsDirty
        {
            get
            {
                var settings = settingsService.CurrentSettings;

                return dirty
                    || (autoSplitTable != null && autoSplitTable.IsDirty)
                    || settings.FontName != GetFontName()
                    || settings.FontSize != (int)fontSizeNumeric.Value
                    || settings.FontSizeTitle != (int)titleFontSizeNumeric.Value
                    || settings.CreateFiles != CreateFilesCheckBox.Checked
                    || settings.CheckUpdates != CheckUpdatesCheckBox.Checked
                    || settings.D2Version != VersionComboBox.SelectedItem.ToString()
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
                    || settings.AutosplitHotkey != autoSplitHotkeyControl.Hotkey
                    || settings.DisplayDifficultyPercentages != chkDisplayDifficultyPercents.Checked
                    || settings.DisplayLayoutHorizontal != (comboBoxLayout.SelectedIndex == 0)
                    || !Enumerable.SequenceEqual(settings.Runes, RunesList())
                    || settings.VerticalLayoutPadding != (int)numericUpDownPaddingInVerticalLayout.Value
                    || settings.DisplayRealFrwIas != chkShowRealValues.Checked

                    || btnSetNameColor.ForeColor != settings.ColorName
                    || btnSetDeathsColor.ForeColor != settings.ColorDeaths
                    || btnSetLevelColor.ForeColor != settings.ColorLevel
                    || btnSetDifficultyColor.ForeColor != settings.ColorDifficultyPercentages
                    || btnSetGoldColor.ForeColor != settings.ColorGold
                    || btnSetBaseStatsColor.ForeColor != settings.ColorBaseStats
                    || btnSetAdvancedStatsColor.ForeColor != settings.ColorAdvancedStats
                    || btnSetFireResColor.ForeColor != settings.ColorFireRes
                    || btnSetColdResColor.ForeColor != settings.ColorColdRes
                    || btnSetLightningResColor.ForeColor != settings.ColorLightningRes
                    || btnSetPoisonResColor.ForeColor != settings.ColorPoisonRes

                    || button2.BackColor != settings.ColorBackground;
            }
        }

        void SettingsWindowOnShown(object sender, EventArgs e)
        {
            Logger.Info("Show settings window.");

            RegisterServiceEventHandlers();

            // Settings was closed with dirty settings, reload the original settings.
            if (IsDirty)
            {
                ReloadComponentsWithCurrentSettings();
                MarkClean();
            }
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceSettingsChanged;
            settingsService.SettingsCollectionChanged += SettingsServiceOnSettingsCollectionChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            settingsService.SettingsChanged -= SettingsServiceSettingsChanged;
            settingsService.SettingsCollectionChanged -= SettingsServiceOnSettingsCollectionChanged;
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
            ReloadComponentsWithCurrentSettings();
            MarkClean();
        }

        void InitializeAutoSplitTable()
        {
            if (autoSplitTable != null)
            {
                AutoSplitLayout.Controls.Remove(autoSplitTable);
            }

            // Create a scrollable layout.
            autoSplitTable = new AutoSplitTable();
            autoSplitTable.Dock = DockStyle.Fill;
            AutoSplitLayout.Controls.Add(autoSplitTable);
        }

        void UpdateTitle()
        {
            Text = $@"Settings ({Path.GetFileName(settingsService.CurrentSettingsFile)})";
        }

        void ReloadComponentsWithCurrentSettings()
        {
            UpdateTitle();

            var settings = settingsService.CurrentSettings;

            fontComboBox.SelectedIndex = fontComboBox.Items.IndexOf(settings.FontName);

            fontSizeNumeric.Value = settings.FontSize;
            titleFontSizeNumeric.Value = settings.FontSizeTitle;
            numericUpDownPaddingInVerticalLayout.Value = settings.VerticalLayoutPadding;

            CreateFilesCheckBox.Checked = settings.CreateFiles;
            EnableAutosplitCheckBox.Checked = settings.DoAutosplit;
            autoSplitHotkeyControl.ForeColor = settings.AutosplitHotkey == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Hotkey = settings.AutosplitHotkey;
            CheckUpdatesCheckBox.Checked = settings.CheckUpdates;
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

            SetBackgroundColor(settings.ColorBackground);

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


            // Show the selected diablo version.
            int versionIndex = this.VersionComboBox.FindString(settings.D2Version);
            if (versionIndex < 0) versionIndex = 0;
            this.VersionComboBox.SelectedIndex = versionIndex;

            InitializeAutoSplitTable();
            foreach (AutoSplit a in settings.Autosplits)
            {
                AddAutoSplit(a);
            }

            RuneDisplayPanel.Controls.Clear();
            foreach (int rune in settings.Runes)
            {
                if (rune >= 0)
                {
                    RuneDisplayElement element = new RuneDisplayElement((Rune)rune);
                    RuneDisplayPanel.Controls.Add(element);
                }
            }
        }

        void MarkClean()
        {
            dirty = false;
            if (autoSplitTable != null)
            {
                autoSplitTable.MarkClean();
            }
        }

        private string GetFontName()
        {
            string fontName = null;
            if (fontComboBox.SelectedItem != null)
            {
                fontName = fontComboBox.SelectedItem.ToString();
            }
            else
            {
                foreach (string comboBoxFontName in fontComboBox.Items)
                {
                    if (comboBoxFontName.Equals(fontComboBox.Text))
                    {
                        fontName = fontComboBox.Text;
                        break;
                    }
                }
            }
            return fontName;
        }

        private List<int> RunesList()
        {
            List<int> runesList = new List<int>();
            foreach (RuneDisplayElement c in RuneDisplayPanel.Controls)
            {
                runesList.Add((int)c.Rune);
            }
            return runesList;
        }

        ApplicationSettings CopyModifiedSettings()
        {
            var settings = settingsService.CurrentSettings.DeepCopy();

            settings.Runes = RunesList();
            settings.Autosplits = autoSplitTable.AutoSplits.ToList();
            settings.CreateFiles = CreateFilesCheckBox.Checked;
            settings.CheckUpdates = CheckUpdatesCheckBox.Checked;
            settings.DoAutosplit = EnableAutosplitCheckBox.Checked;
            settings.AutosplitHotkey = autoSplitHotkeyControl.Hotkey;
            settings.FontSize = (int)fontSizeNumeric.Value;
            settings.FontSizeTitle = (int)titleFontSizeNumeric.Value;
            settings.VerticalLayoutPadding = (int)numericUpDownPaddingInVerticalLayout.Value;
            settings.FontName = GetFontName();
            settings.D2Version = (string)VersionComboBox.SelectedItem;

            settings.DisplayName = chkDisplayName.Checked;
            settings.DisplayGold = chkDisplayGold.Checked;
            settings.DisplayDeathCounter = chkDisplayDeathCounter.Checked;
            settings.DisplayLevel = chkDisplayLevel.Checked;
            settings.DisplayResistances = chkDisplayResistances.Checked;
            settings.DisplayBaseStats = chkDisplayBaseStats.Checked;
            settings.DisplayAdvancedStats = chkDisplayAdvancedStats.Checked;
            settings.DisplayDifficultyPercentages = chkDisplayDifficultyPercents.Checked;
            settings.DisplayRealFrwIas = chkShowRealValues.Checked;

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

            settings.ColorBackground = button2.BackColor;

            return settings;
        }

        void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            var splits = autoSplitTable.AutoSplits;
            var factory = new AutoSplitFactory();

            AddAutoSplit(factory.CreateSequential(splits.LastOrDefault()));

            // Automatically enable auto splits when adding.
            EnableAutosplitCheckBox.Checked = true;
        }

        void AddAutoSplit(AutoSplit autosplit)
        {
            if (autosplit == null) return;

            // Operate on a copy.
            autosplit = new AutoSplit(autosplit);

            // Create and show the autosplit row.
            AutoSplitSettingsRow row = new AutoSplitSettingsRow(autosplit);
            row.OnDelete += (item) => autoSplitTable.Controls.Remove(row);
            autoSplitTable.Controls.Add(row);
            autoSplitTable.ScrollControlIntoView(row);
        }

        void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && IsDirty)
            {
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
                        return;
                }
            }

            UnregisterServiceEventHandlers();
        }

        void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            KeyManager.TriggerHotkey(autoSplitHotkeyControl.Hotkey);
        }

        private void AddRuneButton_Click(object sender, EventArgs e)
        {
            int rune = RuneComboBox.SelectedIndex;
            if (rune >= 0)
            {
                RuneDisplayElement element = new RuneDisplayElement((Rune)rune);
                RuneDisplayPanel.Controls.Add(element);
            }
        }

        void SaveSettings(string path = null)
        {
            path = path ?? settingsService.CurrentSettingsFile;
            var modifiedSettings = CopyModifiedSettings();

            UseWaitCursor = true;
            settingsService.SaveSettings(path, modifiedSettings);
            settingsService.LoadSettings(path ?? settingsService.CurrentSettingsFile);
            UseWaitCursor = false;
        }

        // TODO: Differentiate between loading and reverting settings.
        bool LoadSettings(string filename)
        {
            UseWaitCursor = true;
            var success = settingsService.LoadSettings(filename);
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
   
        private void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            VersionChecker.CheckForUpdate(true);
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

        private void InitializeRunes()
        {
            foreach ( Rune r in Enum.GetValues(typeof(Rune)))
            {
                RuneComboBox.Items.Add(r.ToString());
            }

            List<Runeword> runeWords;
            
            JsonSerializer serializer = new JsonSerializer();

            using ( MemoryStream stream = new MemoryStream(Properties.Resources.runewords))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        runeWords = serializer.Deserialize<List<Runeword>>(reader);
                    }
                }
            }

            runeWords.ForEach(y => cbRuneWord.Items.Add(y));
        }

        private void btnAddRuneWord_Click(object sender, EventArgs e)
        {
            Runeword rw = (Runeword)cbRuneWord.SelectedItem;

            rw.Runes.ForEach(r => AddIndividualRune(r));
        }

        void AddIndividualRune(Rune rune)
        {
            RuneDisplayElement element = new RuneDisplayElement(rune);
            RuneDisplayPanel.Controls.Add(element);
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
                    lstConfigFiles.ContextMenuStrip.Show(lstConfigFiles,new Point(e.X,e.Y));
                }
                else
                {
                    menuClone.Enabled = false;
                    menuLoad.Enabled = false;
                    menuNew.Enabled = true;
                    menuDelete.Enabled = false;
                    lstConfigFiles.ContextMenuStrip.Show(lstConfigFiles, new Point(e.X, e.Y));
                }
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
            settingsService.SaveSettings(path, settings);
            settingsService.LoadSettings(path);
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
                        Path.Combine(SettingsFilePath, saveDialog.NewFileName) + ".conf");
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

        void SelectColor(object sender)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
                ((Control)sender).ForeColor = d.Color;
        }

        void SetBackgroundColor(Color c)
        {
            button2.BackColor = c;
            button2.ForeColor = (384 - c.R - c.G - c.B) > 0 ? Color.White : Color.Black;
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
        }

        private void btnSetNameColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetGoldColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetBaseSettingsColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetAdvancedStatsColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetLevelColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetFireResColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetColdResColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetLightningResColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetPoisonResColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void btnSetDifficultyColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
        }

        private void button1_Click(object sender, EventArgs e)
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
            SetBackgroundColor(Color.Black);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                SetBackgroundColor(d.Color);
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

        void AutoSplitHotkeyControlOnHotkeyChanged(object sender, Keys e)
        {
            autoSplitHotkeyControl.ForeColor = e == Keys.None ? Color.Red : SystemColors.WindowText;
        }
    }
}
