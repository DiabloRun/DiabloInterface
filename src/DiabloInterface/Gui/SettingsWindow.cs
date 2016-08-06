using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Gui.Controls;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using Zutatensuppe.DiabloInterface.Autosplit;
using Zutatensuppe.DiabloInterface.Settings;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public partial class SettingsWindow : WsExCompositedForm
    {

        private const string WindowTitleFormat = "Settings ({0})"; // {0} => Settings File Path

        private string SettingsFilePath = Application.StartupPath + @"\Settings";

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
                ;
            }
        }

        public SettingsWindow(ApplicationSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            InitializeAutoSplitTable();
            InitializeRunes();
            LoadConfigFileList();

            // Select first rune (don't leave combo box empty).
            RuneComboBox.SelectedIndex = 0;
            cbRuneWord.SelectedIndex = 0;

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
            Text = string.Format(WindowTitleFormat, Properties.Settings.Default.SettingsFile.Substring(Properties.Settings.Default.SettingsFile.LastIndexOf("\\")+1));
        }

        private void InitializeSettings()
        {
            UpdateTitle();

            fontComboBox.SelectedIndex = fontComboBox.Items.IndexOf(settings.FontName);

            fontSizeNumeric.Value = settings.FontSize;
            titleFontSizeNumeric.Value = settings.FontSizeTitle;
            numericUpDownPaddingInVerticalLayout.Value = settings.VerticalLayoutPadding;

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
                if (c.Removed) continue;
                runesList.Add((int)c.getRune());
            }
            return runesList;
        }

        private void UpdateSettings()
        {
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
                RuneDisplayElement element = new RuneDisplayElement((Rune)rune);
                RuneDisplayPanel.Controls.Add(element);
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

        private void SaveSettingsAsMenuItem_Click(object sender, EventArgs e)
        {
            SimpleSaveDialog ssd = new SimpleSaveDialog(String.Empty);
            ssd.StartPosition = FormStartPosition.CenterParent;
            DialogResult res = ssd.ShowDialog();

            if (res == DialogResult.OK)
            {
                SaveSettings(Path.Combine(SettingsFilePath, ssd.NewFileName) + ".conf");
            }

            ssd.Dispose();

            LoadConfigFileList();
        }
   
        private void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            VersionChecker.CheckForUpdate(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ( IsDirty )
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

            var resourceName = "Zutatensuppe.DiabloInterface.Resources.runewords.json";
            var assembly = Assembly.GetExecutingAssembly();
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    runeWords = serializer.Deserialize<List<Runeword>>(reader);
                }
            }

            runeWords.ForEach(y => cbRuneWord.Items.Add(y));
        }

        private void btnAddRuneWord_Click(object sender, EventArgs e)
        {
            Runeword rw = (Runeword)cbRuneWord.SelectedItem;

            rw.Runes.ForEach(r => AddIndividualRune(r));
        }

        private void AddIndividualRune(Rune rune)
        {
            RuneDisplayElement element = new RuneDisplayElement(rune);
            RuneDisplayPanel.Controls.Add(element);
        }
        private void LoadConfigFileList()
        {
            lstConfigFiles.Items.Clear();

            DirectoryInfo di = new DirectoryInfo(SettingsFilePath);
            foreach (FileInfo fi in di.GetFiles("*.conf", SearchOption.AllDirectories))
            {
                ConfigEntry ce = new ConfigEntry()
                {
                    DisplayName = fi.Name.Substring(0, fi.Name.LastIndexOf('.')),
                    Path = fi.FullName
                };

                lstConfigFiles.Items.Add(ce);
            }

            
        }

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
            int index = this.lstConfigFiles.IndexFromPoint(e.Location);
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

        private void menuNew_Click(object sender, EventArgs e)
        {

            SimpleSaveDialog ssd = new SimpleSaveDialog(String.Empty);
            DialogResult res = ssd.ShowDialog();

            if (res == DialogResult.OK)
            {
                NewSettings(Path.Combine(SettingsFilePath, ssd.NewFileName) + ".conf");
            }

            ssd.Dispose();

            
        }

        private void NewSettings(string path)
        {
            settings = new ApplicationSettings();
            InitializeSettings();
            SaveSettings(path);
            LoadConfigFileList();
            LoadSettings(path);
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            LoadSettings(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
        }

        private void menuClone_Click(object sender, EventArgs e)
        {
            SimpleSaveDialog ssd = new SimpleSaveDialog(String.Empty);
            DialogResult res = ssd.ShowDialog();

            if (res == DialogResult.OK)
            {
                CloneSettings(((ConfigEntry)lstConfigFiles.SelectedItem).Path, Path.Combine(SettingsFilePath,ssd.NewFileName)+".conf");
            }

            ssd.Dispose();
        }
        private void ReneameSettings(string oldPath, string newPath)
        {
            File.Copy(oldPath, newPath);
            File.Delete(oldPath);
            LoadConfigFileList();
            LoadSettings(newPath);
        }
        private void CloneSettings(string oldPath, string newPath)
        {
            File.Copy(oldPath, newPath);
            LoadConfigFileList();
            LoadSettings(newPath);
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            DeleteSettings(((ConfigEntry)lstConfigFiles.SelectedItem).Path);
        }

        private void DeleteSettings(string path)
        {
            File.Delete(path);
            LoadConfigFileList();
        }

        private void selectColor(object sender)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
                ((Control)sender).ForeColor = d.Color;
        }
        private void SetBackgroundColor(Color c)
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
            selectColor(sender);
        }

        private void btnSetGoldColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetBaseSettingsColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetAdvancedStatsColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetLevelColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetFireResColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetColdResColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetLightningResColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetPoisonResColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
        }

        private void btnSetDifficultyColor_Click(object sender, EventArgs e)
        {
            selectColor(sender);
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

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string current = ((ConfigEntry)lstConfigFiles.SelectedItem).Path;
            SimpleSaveDialog ssd = new SimpleSaveDialog(Path.GetFileName(current).Replace(".conf", ""));
            DialogResult res = ssd.ShowDialog();

            if (res == DialogResult.OK)
            {
                string newPath = Path.Combine(SettingsFilePath, ssd.NewFileName) + ".conf";
                ReneameSettings(current, newPath);
            }

            ssd.Dispose();
        }

        private void comboBoxLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxRunesOrientation.Enabled = comboBoxLayout.SelectedIndex == 0;
        }
    }
}