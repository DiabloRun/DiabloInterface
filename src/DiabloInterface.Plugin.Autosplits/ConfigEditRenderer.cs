using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private Plugin plugin;

        private TableLayoutPanel control;
        AutoSplitTable autoSplitTable;
        HotkeyControl SplitKeyControl;
        HotkeyControl ResetKeyControl;
        private CheckBox EnabledCheckbox;
        private CheckBox EnabledForExistingCharsCheckbox;

        public ConfigEditRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            var SplitKeyLabel = new Label();
            SplitKeyLabel.AutoSize = true;
            SplitKeyLabel.Padding = new Padding(0, 6, 0, 0);
            SplitKeyLabel.Text = "Split-Key:";

            SplitKeyControl = new HotkeyControl();
            SplitKeyControl.Value = new Hotkey();
            SplitKeyControl.AutoSize = true;
            SplitKeyControl.HotkeyChanged += new EventHandler<Hotkey>(SplitKeyChanged);

            var SplitKeyTestButton = new Button();
            SplitKeyTestButton.AutoSize = true;
            SplitKeyTestButton.Text = "Test";
            SplitKeyTestButton.Click += new EventHandler(SplitKeyTestClicked);

            var ResetKeyLabel = new Label();
            ResetKeyLabel.AutoSize = true;
            ResetKeyLabel.Padding = new Padding(0, 6, 0, 0);
            ResetKeyLabel.Text = "Reset-Key:";

            ResetKeyControl = new HotkeyControl();
            ResetKeyControl.Value = new Hotkey();
            ResetKeyControl.AutoSize = true;
            ResetKeyControl.HotkeyChanged += new EventHandler<Hotkey>(ResetKeyChanged);

            var ResetKeyTestButton = new Button();
            ResetKeyTestButton.AutoSize = true;
            ResetKeyTestButton.Text = "Test";
            ResetKeyTestButton.Click += new EventHandler(ResetKeyTestClicked);

            EnabledCheckbox = new CheckBox();
            EnabledCheckbox.AutoSize = true;
            EnabledCheckbox.Padding = new Padding(0, 2, 0, 0);
            EnabledCheckbox.Text = "Enable autosplits";

            EnabledForExistingCharsCheckbox = new CheckBox();
            EnabledForExistingCharsCheckbox.AutoSize = true;
            EnabledForExistingCharsCheckbox.Padding = new Padding(0, 2, 0, 0);
            EnabledForExistingCharsCheckbox.Text = "Split for existing chars";

            var AddAutoSplitButton = new Button();
            AddAutoSplitButton.AutoSize = true;
            AddAutoSplitButton.Text = "Add Split";
            AddAutoSplitButton.Click += new EventHandler(AddAutoSplitButton_Clicked);

            var Toolbar1 = new FlowLayoutPanel();
            Toolbar1.FlowDirection = FlowDirection.LeftToRight;
            Toolbar1.AutoSize = true;
            Toolbar1.Controls.Add(EnabledCheckbox);
            Toolbar1.Controls.Add(EnabledForExistingCharsCheckbox);
            Toolbar1.Controls.Add(AddAutoSplitButton);
            Toolbar1.Dock = DockStyle.Fill;
            Toolbar1.Margin = new Padding(0);

            var Toolbar2 = new TableLayoutPanel();
            Toolbar2.AutoSize = true;
            Toolbar2.ColumnCount = 3;
            Toolbar2.ColumnStyles.Add(new ColumnStyle());
            Toolbar2.ColumnStyles.Add(new ColumnStyle());
            Toolbar2.ColumnStyles.Add(new ColumnStyle());
            Toolbar2.ColumnStyles.Add(new ColumnStyle());
            Toolbar2.RowCount = 2;
            Toolbar2.RowStyles.Add(new RowStyle());
            Toolbar2.RowStyles.Add(new RowStyle());

            Toolbar2.Controls.Add(SplitKeyLabel, 0, 0);
            Toolbar2.Controls.Add(SplitKeyControl, 1, 0);
            Toolbar2.Controls.Add(SplitKeyTestButton, 2, 0);

            Toolbar2.Controls.Add(ResetKeyLabel, 0, 1);
            Toolbar2.Controls.Add(ResetKeyControl, 1, 1);
            Toolbar2.Controls.Add(ResetKeyTestButton, 2, 1);

            Toolbar2.Dock = DockStyle.Fill;
            Toolbar2.Margin = new Padding(0);

            autoSplitTable = new AutoSplitTable(plugin.Config) { Dock = DockStyle.Fill };

            control = new TableLayoutPanel();
            control.ColumnCount = 1;
            control.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            control.Dock = DockStyle.Fill;
            control.Location = new Point(0, 0);
            control.Margin = new Padding(0);
            control.RowCount = 3;
            control.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            control.RowStyles.Add(new RowStyle());
            control.RowStyles.Add(new RowStyle());
            control.Size = new Size(800, 476);
            control.Controls.Add(Toolbar1, 0, 1);
            control.Controls.Add(Toolbar2, 0, 2);
            control.Controls.Add(autoSplitTable);
            return control;
        }

        public void ApplyConfig()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyConfig()));
                return;
            }

            EnabledCheckbox.Checked = plugin.Config.Enabled;
            EnabledForExistingCharsCheckbox.Checked = plugin.Config.EnabledForExistingChars;
            SplitKeyControl.ForeColor = plugin.Config.Hotkey.ToKeys() == Keys.None ? Color.Red : Color.Black;
            SplitKeyControl.Value = plugin.Config.Hotkey;
            ResetKeyControl.ForeColor = plugin.Config.ResetHotkey.ToKeys() == Keys.None ? Color.Red : Color.Black;
            ResetKeyControl.Value = plugin.Config.ResetHotkey;
            autoSplitTable.Set(plugin.Config);
        }

        public void ApplyChanges()
        {
        }

        void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            var splits = autoSplitTable.AutoSplits;
            var factory = new AutoSplitFactory();

            var row = autoSplitTable.AddAutoSplit(factory.CreateSequential(splits.LastOrDefault()));
            if (row != null)
                autoSplitTable.ScrollControlIntoView(row);

            // Automatically enable auto splits when adding.
            EnabledCheckbox.Checked = true;
        }

        void SplitKeyTestClicked(object sender, EventArgs e)
        {
            plugin.keyService.TriggerHotkey(SplitKeyControl.Value.ToKeys());
        }

        void SplitKeyChanged(object sender, Hotkey e)
        {
            SplitKeyControl.ForeColor = e.ToKeys() == Keys.None ? Color.Red : Color.Black;
        }

        void ResetKeyTestClicked(object sender, EventArgs e)
        {
            plugin.keyService.TriggerHotkey(ResetKeyControl.Value.ToKeys());
        }

        void ResetKeyChanged(object sender, Hotkey e)
        {
            ResetKeyControl.ForeColor = e.ToKeys() == Keys.None ? Color.Red : Color.Black;
        }

        public bool IsDirty()
        {
            return autoSplitTable.IsDirty
                || plugin.Config.Enabled != EnabledCheckbox.Checked
                || plugin.Config.EnabledForExistingChars != EnabledForExistingCharsCheckbox.Checked
                || plugin.Config.Hotkey.ToKeys() != SplitKeyControl.Value.ToKeys()
                || plugin.Config.ResetHotkey.ToKeys() != ResetKeyControl.Value.ToKeys()
            ;
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = EnabledCheckbox.Checked;
            conf.EnabledForExistingChars = EnabledForExistingCharsCheckbox.Checked;
            conf.Hotkey = SplitKeyControl.Value;
            conf.ResetHotkey = ResetKeyControl.Value;
            conf.Splits = autoSplitTable.AutoSplits.ToList();
            return conf;
        }
    }
}
