using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DiabloInterface.Gui;

namespace DiabloInterface
{
    public partial class SettingsWindow : Form
    {
        private const string WindowTitleFormat = "Settings ({0})"; // {0} => Settings File Path
        
        private MainWindow main;

        public SettingsWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
            
            foreach (FontFamily font in FontFamily.Families)
            {
                cmbFonts.Items.Add(font.Name);
            }

            init();
        }

        private void updateTitle()
        {
            Text = string.Format(WindowTitleFormat, Properties.Settings.Default.SettingsFile);
        }

        private void init() {
            updateTitle();
            
            foreach (string item in cmbFonts.Items)
            {
                if (item == main.Settings.FontName)
                {
                    this.cmbFonts.SelectedItem = item;
                }
            }

            this.txtFontSize.Text = main.Settings.FontSize.ToString();
            this.txtTitleFontSize.Text = main.Settings.TitleFontSize.ToString();
            this.chkCreateFiles.Checked = main.Settings.CreateFiles;
            this.chkAutosplit.Checked = main.Settings.DoAutosplit;
            this.txtAutoSplitHotkey.Text = main.Settings.TriggerKeys;
            this.chkShowDebug.Checked = main.Settings.ShowDebug;
            this.chkCheckUpdates.Checked = main.Settings.CheckUpdates;

            // Show the selected diablo version.
            int versionIndex = this.cmbVersion.FindString(main.Settings.D2Version);
            if (versionIndex < 0) versionIndex = 0;
            this.cmbVersion.SelectedIndex = versionIndex;

            panel1.Controls.Clear();
            foreach (AutoSplit a in main.Settings.Autosplits)
            {
                addAutosplit(a, false);
            }

            runeDisplayPanel.Controls.Clear();
            foreach (int rune in main.Settings.Runes)
            {
                if (rune >= 0)
                {
                    RuneDisplayElement element = new RuneDisplayElement((Rune)rune, this, null);
                    runeDisplayPanel.Controls.Add(element);
                }
            }
            relayout(false);
        }

        private void resetSettings()
        {
            //todo : implement
        }
        private void updateSettings()
        {
            List<AutoSplit> asList = new List<AutoSplit>();
            foreach (AutoSplit a in main.Settings.Autosplits)
            {
                if (!a.Deleted)
                {
                    asList.Add(a);
                }
            }
            List<int> runesList = new List<int>();
            foreach (RuneDisplayElement c in runeDisplayPanel.Controls)
            {
                if (!c.Visible) continue;
                runesList.Add((int)c.getRune());
            }
            main.Settings.Runes = runesList;
            main.Settings.Autosplits = asList;
            main.Settings.CreateFiles = chkCreateFiles.Checked;
            main.Settings.CheckUpdates = chkCheckUpdates.Checked;
            main.Settings.DoAutosplit = chkAutosplit.Checked;
            main.Settings.TriggerKeys = txtAutoSplitHotkey.Text;
            main.Settings.FontSize = Int32.Parse(txtFontSize.Text);
            main.Settings.TitleFontSize = Int32.Parse(txtTitleFontSize.Text);
            main.Settings.FontName = cmbFonts.SelectedItem.ToString();
            main.Settings.ShowDebug = chkShowDebug.Checked;
            main.Settings.D2Version = (string)cmbVersion.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addAutosplit(new AutoSplit());
            relayout();
        }

        private void addAutosplit(AutoSplit autosplit, bool addToMain = true)
        {
            AutoSplitSettingsRow row = new AutoSplitSettingsRow(autosplit, this);
            panel1.Controls.Add(row);
            int scroll = panel1.VerticalScroll.Value;

            if (addToMain)
            {
                main.Settings.Autosplits.Add(autosplit);
            }
        }
        
        public void relayout( bool checkVisible = true)
        {
            int i = 0;
            int y = 0;
            int x = 0;
            int scroll = panel1.VerticalScroll.Value;
            foreach (Control c in panel1.Controls)
            {
                if (c is AutoSplitSettingsRow && (!checkVisible || c.Visible))
                {
                    c.Location = new Point(0, -scroll + i * 24);
                    i++;
                }
            }

            scroll = runeDisplayPanel.VerticalScroll.Value;
            foreach (Control c in runeDisplayPanel.Controls )
            {
                if ( c is RuneDisplayElement && (! checkVisible || c.Visible))
                { 
                    
                    if (x+c.Width > runeDisplayPanel.Width && runeDisplayPanel.Width >= c.Width)
                    {
                        y += c.Height;
                        x = 0;
                    }
                    c.Location = new Point(x, -scroll + y);
                    x += c.Width;
                }
            }

        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            AutoSplit a = (AutoSplit)b.Tag;
            a.Deleted = true;
        }

        private void txtAutoSplitHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void txtAutoSplitHotkey_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9
                || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9
                || e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z
                || e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                txtAutoSplitHotkey.Text = KeyManager.KeyEventArgsToKeyString(e);
            } else if (e.KeyCode == Keys.Escape)
            {
                txtAutoSplitHotkey.Text = "";
            }
            e.Handled = true;
        }

        private void txtAutoSplitHotkey_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( e.CloseReason == CloseReason.UserClosing )
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnTestHotkey_Click(object sender, EventArgs e)
        {
            if (txtAutoSplitHotkey.Text != "")
            {
                KeyManager.sendKeys(txtAutoSplitHotkey.Text);
            }
        }

        private void btnAddRune_Click(object sender, EventArgs e)
        {
            int rune = this.comboBoxRunes.SelectedIndex;
            if (rune >= 0)
            {
                RuneDisplayElement element = new RuneDisplayElement((Rune)rune, this, null);
                runeDisplayPanel.Controls.Add(element);
                relayout();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateSettings();

            main.Settings.save();
            main.applySettings();
            main.updateAutosplits();
        }

        private void closeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void loadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                main.Settings.loadFrom(d.FileName);
                updateTitle();
                init();
                main.applySettings();
                main.updateAutosplits();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                updateSettings();
                main.Settings.saveAs(d.FileName);
                updateTitle();
                main.applySettings();
                main.updateAutosplits();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VersionChecker.CheckForUpdate(true);
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
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