using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using DiabloInterface.Gui;

namespace DiabloInterface
{
    public partial class SettingsWindow : Form
    {
        private const string WindowTitleFormat = "Settings ({0})"; // {0} => Settings File Path

        AutoSplitTable autoSplitTable;
        SettingsHolder settings;

        public event Action<SettingsHolder> SettingsUpdated;

        bool dirty = false;
        public bool IsDirty
        {
            get
            {
                return dirty
                    || (autoSplitTable != null && autoSplitTable.IsDirty)
                    || settings.FontName != FontComboBox.SelectedItem.ToString()
                    || settings.FontSize != int.Parse(FontSizeText.Text)
                    || settings.TitleFontSize != int.Parse(TitleFontSizeText.Text)
                    || settings.CreateFiles != CreateFilesCheckBox.Checked
                    || settings.CheckUpdates != CheckUpdatesCheckBox.Checked
                    || settings.D2Version != VersionComboBox.SelectedItem.ToString();
            }
        }

        public SettingsWindow(SettingsHolder settings)
        {
            this.settings = settings;
            InitializeComponent();
            InitializeAutoSplitTable();

            foreach (FontFamily font in FontFamily.Families)
            {
                FontComboBox.Items.Add(font.Name);
            }

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

            foreach (string item in FontComboBox.Items)
            {
                if (item == settings.FontName)
                {
                    FontComboBox.SelectedItem = item;
                }
            }

            FontSizeText.Text = settings.FontSize.ToString();
            TitleFontSizeText.Text = settings.TitleFontSize.ToString();
            CreateFilesCheckBox.Checked = settings.CreateFiles;
            EnableAutosplitCheckBox.Checked = settings.DoAutosplit;
            AutoSplitHotkeyText.Text = settings.TriggerKeys;
            CheckUpdatesCheckBox.Checked = settings.CheckUpdates;

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
            settings.TriggerKeys = AutoSplitHotkeyText.Text;
            settings.FontSize = Int32.Parse(FontSizeText.Text);
            settings.TitleFontSize = Int32.Parse(TitleFontSizeText.Text);
            settings.FontName = FontComboBox.SelectedItem.ToString();
            settings.D2Version = (string)VersionComboBox.SelectedItem;
        }

        private void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            AddAutoSplit(new AutoSplit());
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

        private void AutoSplitHotkeyText_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void AutoSplitHotkeyText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9
                || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9
                || e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z
                || e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                AutoSplitHotkeyText.Text = KeyManager.KeyEventArgsToKeyString(e);
            } else if (e.KeyCode == Keys.Escape)
            {
                AutoSplitHotkeyText.Text = "";
            }
            e.Handled = true;
        }

        private void AutoSplitHotkeyText_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
                        Save();
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
            if (!string.IsNullOrEmpty(AutoSplitHotkeyText.Text))
            {
                KeyManager.sendKeys(AutoSplitHotkeyText.Text);
            }
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

        void Save(string filename = null)
        {
            UpdateSettings();

            // Save to current savefile.
            if (string.IsNullOrEmpty(filename))
            {
                settings.save();
            }
            // Save to selected savefile.
            else
            {
                settings.saveAs(filename);
                UpdateTitle();
            }

            MarkClean();
            if (SettingsUpdated != null)
            {
                SettingsUpdated(settings);
            }
        }

        private void LoadSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                settings.loadFrom(d.FileName);
                UpdateTitle();
                InitializeSettings();

                if (SettingsUpdated != null)
                {
                    SettingsUpdated(settings);
                }
            }
        }

        private void SaveSettingsMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void SaveSettingsAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(d.FileName))
            {
                Save(d.FileName);
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

        private void FontComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background
            e.DrawBackground();

            // Get the item text
            string text = ((ComboBox)sender).Items[e.Index].ToString();

            Font fSender = ((Control)sender).Font;
            Font f;
            try
            {
                f = new Font(text, 10f, fSender.Style);
            } catch
            {
                f = fSender;
            }

            // Draw the text
            e.Graphics.DrawString(text, f, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }
    }
}