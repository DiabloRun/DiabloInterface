using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using DiabloInterface.Gui.Controls;

namespace DiabloInterface.Gui
{
    public partial class SettingsWindow : Form
    {
        private const string WindowTitleFormat = "Settings ({0})"; // {0} => Settings File Path

        AutoSplitTable autoSplitTable;
        ApplicationSettings settings;

        public event Action<ApplicationSettings> SettingsUpdated;

        bool dirty = false;
        public bool IsDirty
        {
            get
            {
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
                    || settings.DisplayRunesHorizontal != chkRuneDisplayRunesHorizontal.Checked
                    || settings.DisplayRunesHighContrast != chkHighContrastRunes.Checked
                    || settings.AutosplitHotkey != autoSplitHotkeyControl.Hotkey
                    || settings.DisplayDifficultyPercentages != chkDisplayDifficultyPercents.Checked
                ;
            }
        }

        public SettingsWindow(ApplicationSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            InitializeAutoSplitTable();

            // Select first rune (don't leave combo box empty).
            RuneComboBox.SelectedIndex = 0;

            InitializeSettings();

            // Loading the settings will dirty mark pretty much everything, here
            // we just verify that nothing has actually changed yet.
            MarkClean();
        }

        private void SettingsWindow_Shown(object sender, EventArgs e)
        {
            // Settings was closed with dirty settings, reload the original settings.
            if (IsDirty)
            {
                InitializeSettings();
                MarkClean();
            }
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

        private void UpdateTitle()
        {
            Text = string.Format(WindowTitleFormat, Properties.Settings.Default.SettingsFile);
        }

        private void InitializeSettings()
        {
            UpdateTitle();

            fontComboBox.SelectedIndex = fontComboBox.Items.IndexOf(settings.FontName);

            fontSizeNumeric.Value = settings.FontSize;
            titleFontSizeNumeric.Value = settings.FontSizeTitle;
            CreateFilesCheckBox.Checked = settings.CreateFiles;
            EnableAutosplitCheckBox.Checked = settings.DoAutosplit;
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
            chkRuneDisplayRunesHorizontal.Checked = settings.DisplayRunesHorizontal;
            chkDisplayDifficultyPercents.Checked = settings.DisplayDifficultyPercentages;
            chkHighContrastRunes.Checked = settings.DisplayRunesHighContrast;

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
                    RuneDisplayElement element = new RuneDisplayElement((Rune)rune, this, null);
                    RuneDisplayPanel.Controls.Add(element);
                }
            }

            LayoutControls(checkVisible: false);
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

        private void UpdateSettings()
        {
            List<int> runesList = new List<int>();
            foreach (RuneDisplayElement c in RuneDisplayPanel.Controls)
            {
                if (!c.Visible) continue;
                runesList.Add((int)c.getRune());
            }

            settings.Runes = runesList;
            settings.Autosplits = autoSplitTable.AutoSplits.ToList();
            settings.CreateFiles = CreateFilesCheckBox.Checked;
            settings.CheckUpdates = CheckUpdatesCheckBox.Checked;
            settings.DoAutosplit = EnableAutosplitCheckBox.Checked;
            settings.AutosplitHotkey = autoSplitHotkeyControl.Hotkey;
            settings.FontSize = (int)fontSizeNumeric.Value;
            settings.FontSizeTitle = (int)titleFontSizeNumeric.Value;
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
            settings.DisplayRunes = chkDisplayRunes.Checked;
            settings.DisplayRunesHorizontal = chkRuneDisplayRunesHorizontal.Checked;
            settings.DisplayRunesHighContrast = chkHighContrastRunes.Checked;

        }

        private void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            var splits = autoSplitTable.AutoSplits;
            var factory = new AutoSplitFactory();

            AddAutoSplit(factory.CreateSequential(splits.LastOrDefault()));
        }

        private void AddAutoSplit(AutoSplit autosplit)
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

        public void LayoutControls(bool checkVisible = true)
        {
            int x = 0;
            int y = 0;
            int scroll = RuneDisplayPanel.VerticalScroll.Value;
            foreach (Control c in RuneDisplayPanel.Controls)
            {
                if (c is RuneDisplayElement && (!checkVisible || c.Visible))
                {

                    if (x + c.Width > RuneDisplayPanel.Width && RuneDisplayPanel.Width >= c.Width)
                    {
                        y += c.Height;
                        x = 0;
                    }
                    c.Location = new Point(x, -scroll + y);
                    x += c.Width;
                }
            }
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && IsDirty)
            {
                DialogResult result = MessageBox.Show(
                    "Would you like to save your settings before closing?",
                    "Save Changes",
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
        }

        private void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            KeyManager.TriggerHotkey(autoSplitHotkeyControl.Hotkey);
        }

        private void AddRuneButton_Click(object sender, EventArgs e)
        {
            int rune = RuneComboBox.SelectedIndex;
            if (rune >= 0)
            {
                RuneDisplayElement element = new RuneDisplayElement((Rune)rune, this, null);
                RuneDisplayPanel.Controls.Add(element);
                LayoutControls();
            }
        }

        void OnSettingsUpdated()
        {
            var updateEvent = SettingsUpdated;
            if (updateEvent != null)
            {
                updateEvent(settings);
            }
        }

        void SaveSettings(string filename = null)
        {
            UseWaitCursor = true;

            UpdateSettings();

            // Persist settings to file.
            var persistense = new SettingsPersistence();
            if (string.IsNullOrEmpty(filename))
                 persistense.Save(settings);
            else persistense.Save(settings, filename);

            // file name may be a different one now
            UpdateTitle();

            MarkClean();
            OnSettingsUpdated();

            UseWaitCursor = false;
        }

        bool LoadSettings(string filename)
        {
            UseWaitCursor = true;

            var persistence = new SettingsPersistence();
            var settings = persistence.Load(filename);
            if (settings != null)
            {
                this.settings = settings;

                UpdateTitle();
                InitializeSettings();

                MarkClean();
                OnSettingsUpdated();

                UseWaitCursor = false;

                return true;
            }

            // Failed to persist settings.
            UseWaitCursor = false;
            return false;
        }

        private void LoadSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = SettingsPersistence.FileFilter;
            if (d.ShowDialog() == DialogResult.OK)
            {
                if (!LoadSettings(d.FileName))
                {
                    MessageBox.Show("Failed to open settings file, make sure it is a valid settings file.",
                        "Settings Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveSettingsMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettingsAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = SettingsPersistence.FileFilter;
            if (d.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(d.FileName))
            {
                SaveSettings(d.FileName);
            }
        }

        private void CloseSettingsMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            VersionChecker.CheckForUpdate(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }
    }
}