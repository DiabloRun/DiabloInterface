using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DiabloInterface.Gui;

namespace DiabloInterface
{
    public partial class SettingsWindow : Form
    {

        private MainWindow main;

        public SettingsWindow( MainWindow main )
        {
            this.main = main;
            InitializeComponent();

            this.lblFontExample.Text = main.settings.fontName;
            this.txtFontSize.Text = main.settings.fontSize.ToString();
            this.txtTitleFontSize.Text = main.settings.titleFontSize.ToString();
            this.chkCreateFiles.Checked = main.settings.createFiles;
            this.chkAutosplit.Checked = main.settings.doAutosplit;
            this.txtAutoSplitHotkey.Text = main.settings.triggerKeys;
            this.chkShowDebug.Checked = main.settings.showDebug;

            // Show the selected diablo version.
            int versionIndex = this.cmbVersion.FindString(main.settings.d2Version);
            if (versionIndex < 0) versionIndex = 0;
            this.cmbVersion.SelectedIndex = versionIndex;

            foreach (AutoSplit a in main.settings.autosplits)
            {
                addAutosplit(a, false);
            }

            foreach (int rune in main.settings.runes)
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

        private void saveSettings()
        {
            List<AutoSplit> asList = new List<AutoSplit>();
            foreach (AutoSplit a in main.settings.autosplits) {
                if (!a.deleted)
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
            main.settings.runes = runesList;
            main.settings.autosplits = asList;
            main.settings.createFiles = chkCreateFiles.Checked;
            main.settings.doAutosplit = chkAutosplit.Checked;
            main.settings.triggerKeys = txtAutoSplitHotkey.Text;
            main.settings.fontSize = Int32.Parse(txtFontSize.Text);
            main.settings.titleFontSize = Int32.Parse(txtTitleFontSize.Text);
            main.settings.fontName = lblFontExample.Text;
            main.settings.showDebug = chkShowDebug.Checked;
            main.settings.d2Version = (string)cmbVersion.SelectedItem;

            main.settings.save();
            main.applySettings();
            main.updateAutosplits();
        }

        private void btnFont_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveSettings();
            Hide();
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetSettings();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addAutosplit(new AutoSplit());
            relayout();
        }

        private void addAutosplit(AutoSplit autosplit, bool addToMain = true)
        {
            AutoSplitSettingsRow row = new AutoSplitSettingsRow(autosplit, this);
            this.panel1.Controls.Add(row);
            int scroll = this.panel1.VerticalScroll.Value;

            if (addToMain)
            {
                main.settings.autosplits.Add(autosplit);
            }
        }
        
        public void relayout( bool checkVisible = true)
        {
            int i = 0;
            int y = 0;
            int x = 0;
            int scroll = this.panel1.VerticalScroll.Value;
            foreach (Control c in this.panel1.Controls)
            {
                if (c is AutoSplitSettingsRow && (!checkVisible || c.Visible))
                {
                    i = i + 1;
                    c.Location = new Point(0, -scroll + i * 24);
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
            a.deleted = true;
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

        private void btnFont_Click_1(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            fontDialog1.Font = new Font(lblFontExample.Text, 8);
            DialogResult result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblFontExample.Text = fontDialog1.Font.Name;
            }

        }

        private void btnTestHotkey_Click(object sender, EventArgs e)
        {
            if (txtAutoSplitHotkey.Text != "")
            {
                KeyManager.sendKeys(txtAutoSplitHotkey.Text);
            }
        }

        private void chkAutosplit_CheckedChanged(object sender, EventArgs e)
        {

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
    }

}