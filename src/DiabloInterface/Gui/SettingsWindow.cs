namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

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

        static readonly string SettingsFilePath = Application.StartupPath + @"\Settings";

        readonly ISettingsService settingsService;

        AutoSplitTable autoSplitTable;

        bool dirty;

        public SettingsWindow(ISettingsService settingsService)
        {
            Logger.Info("Creating settings window.");

            this.settingsService = settingsService;

            RegisterServiceEventHandlers();
            InitializeComponent();
            InitializeAutoSplitTable();
            PopulateSettingsFileList(settingsService.SettingsFileCollection);

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing settings window.");
                UnregisterServiceEventHandlers();
            };

            ReloadComponentsWithCurrentSettings(settingsService.CurrentSettings);

            // Loading the settings will dirty mark pretty much everything, here
            // we just verify that nothing has actually changed yet.
            MarkClean();
        }

        bool IsDirty
        {
            get
            {
                var settings = settingsService.CurrentSettings;

                return dirty
                    || (autoSplitTable != null && autoSplitTable.IsDirty)
                    || !CompareClassRuneSettings(settings)

                    || settings.FontName != GetFontName()
                    || settings.FontSize != (int)fontSizeNumeric.Value
                    || settings.FontSizeTitle != (int)titleFontSizeNumeric.Value
                    || settings.CreateFiles != CreateFilesCheckBox.Checked
                    || settings.CheckUpdates != CheckUpdatesCheckBox.Checked
                    || settings.PipeName != textBoxPipeName.Text
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
                    || settings.VerticalLayoutPadding != (int)numericUpDownPaddingInVerticalLayout.Value
                    || settings.DisplayRealFrwIas != chkShowRealValues.Checked
                    || settings.DisplayPlayersX != chkShowPlayersX.Checked

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
                    || btnSetPlayersXColor.ForeColor != settings.ColorPlayersX

                    || btnSetBackgroundColor.BackColor != settings.ColorBackground;
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
            ReloadComponentsWithCurrentSettings(e.Settings);
            MarkClean();
        }

        void InitializeAutoSplitTable()
        {
            autoSplitTable = new AutoSplitTable(settingsService) { Dock = DockStyle.Fill };
            AutoSplitLayout.Controls.Add(autoSplitTable);
        }

        void UpdateTitle()
        {
            Text = $@"Settings ({Path.GetFileName(settingsService.CurrentSettingsFile)})";
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

            CreateFilesCheckBox.Checked = settings.CreateFiles;
            EnableAutosplitCheckBox.Checked = settings.DoAutosplit;
            autoSplitHotkeyControl.ForeColor = settings.AutosplitHotkey == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Hotkey = settings.AutosplitHotkey;
            CheckUpdatesCheckBox.Checked = settings.CheckUpdates;
            textBoxPipeName.Text = settings.PipeName;
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
            btnSetPlayersXColor.ForeColor = settings.ColorPlayersX;
        }

        void MarkClean()
        {
            dirty = false;
            autoSplitTable?.MarkClean();
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
            var settings = settingsService.CurrentSettings.DeepCopy();

            settings.Autosplits = autoSplitTable.AutoSplits.ToList();
            settings.ClassRunes = runeSettingsPage.SettingsList ?? new List<ClassRuneSettings>();
            settings.CreateFiles = CreateFilesCheckBox.Checked;
            settings.CheckUpdates = CheckUpdatesCheckBox.Checked;
            settings.PipeName = textBoxPipeName.Text;
            settings.DoAutosplit = EnableAutosplitCheckBox.Checked;
            settings.AutosplitHotkey = autoSplitHotkeyControl.Hotkey;
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

            settings.ColorBackground = btnSetBackgroundColor.BackColor;

            return settings;
        }

        void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            var splits = autoSplitTable.AutoSplits;
            var factory = new AutoSplitFactory();

            var row = autoSplitTable.AddAutoSplit(factory.CreateSequential(splits.LastOrDefault()));
            if (row != null)
            {
                autoSplitTable.ScrollControlIntoView(row);
            }

            // Automatically enable auto splits when adding.
            EnableAutosplitCheckBox.Checked = true;
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

        void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            KeyManager.TriggerHotkey(autoSplitHotkeyControl.Hotkey);
        }

        void SaveSettings(string filename = null)
        {
            UseWaitCursor = true;
            settingsService.SaveSettings(filename ?? settingsService.CurrentSettingsFile, CopyModifiedSettings());
            settingsService.LoadSettings(filename ?? settingsService.CurrentSettingsFile);
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
            VersionChecker.ManuallyCheckForUpdate();
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

        private void btnSetColorPlayersX_Click(object sender, EventArgs e)
        {
            SelectColor(sender);
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
