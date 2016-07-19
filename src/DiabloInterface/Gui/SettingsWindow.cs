using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using DiabloInterface.Gui.Controls;
using System.IO;
using Newtonsoft.Json;

namespace DiabloInterface.Gui
{
    public partial class SettingsWindow : Form
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
                    || settings.FontName != fontComboBox.SelectedItem as string
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
                    || settings.AutosplitHotkey != autoSplitHotkeyControl.Hotkey
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
            if (fontComboBox.SelectedItem != null)
            {
                settings.FontName = fontComboBox.SelectedItem.ToString();
            } else
            {
                foreach (string fontName in fontComboBox.Items)
                {
                    if (fontName.Equals(fontComboBox.Text))
                    {
                        settings.FontName = fontComboBox.Text;
                        break;
                    }
                }
            }
            settings.D2Version = (string)VersionComboBox.SelectedItem;

            settings.DisplayName = chkDisplayName.Checked;
            settings.DisplayGold = chkDisplayGold.Checked;
            settings.DisplayDeathCounter = chkDisplayDeathCounter.Checked;
            settings.DisplayLevel = chkDisplayLevel.Checked;
            settings.DisplayResistances = chkDisplayResistances.Checked;
            settings.DisplayBaseStats = chkDisplayBaseStats.Checked;
            settings.DisplayAdvancedStats = chkDisplayAdvancedStats.Checked;
            settings.DisplayRunes = chkDisplayRunes.Checked;

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
            //todo: should use a flow layout here so dont have to fuck around with all this crap for positioning

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
            //todo: should check isdirty, but isdirty not checking runes
            LoadSettings(Properties.Settings.Default.SettingsFile);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void RuneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeRunes()
        {
            //todo: Load these from file
            RuneComboBox.Items.AddRange(new object[] {
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

            List<Runeword> runeWords;
            
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sr = new StreamReader(@".\Resources\runewords.json"))
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

            LayoutControls();
        }

        private void AddIndividualRune(string runeString)
        {
            Rune rune = (Rune)(Enum.Parse(typeof(Rune), runeString));

            RuneDisplayElement element = new RuneDisplayElement(rune, this, null);
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
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                LoadSettings(((ConfigEntry)lstConfigFiles.Items[index]).Path);
            }
        }

        private void lstConfigFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = this.lstConfigFiles.IndexFromPoint(e.Location);
                if (index != System.Windows.Forms.ListBox.NoMatches)
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
    }

    public class Runeword
    {
        public string Name { get; set; }
        public List<string> Runes { get; set; }

        public Runeword()
        {
            Runes = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}