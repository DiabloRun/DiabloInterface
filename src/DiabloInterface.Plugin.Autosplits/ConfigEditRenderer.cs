using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private Plugin plugin;

        private TableLayoutPanel control;
        AutoSplitTable autoSplitTable;
        HotkeyControl autoSplitHotkeyControl;
        HotkeyControl autoSplitResetHotkeyControl;
        private Button AddAutoSplitButton;
        private CheckBox EnabledCheckbox;
        private CheckBox EnabledForExistingCharsCheckbox;
        private Label AutoSplitHotkeyLabel;
        private Label AutoSplitResetHotkeyLabel;
        private Button AutoSplitTestHotkeyButton;
        private Button AutoSplitTestResetHotkeyButton;
        private FlowLayoutPanel AutoSplitToolbar;
        private FlowLayoutPanel AutoSplitToolbar2;

        public ConfigEditRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            AutoSplitHotkeyLabel = new Label();
            AutoSplitHotkeyLabel.AutoSize = true;
            AutoSplitHotkeyLabel.Padding = new Padding(0, 6, 0, 0);
            AutoSplitHotkeyLabel.Text = "Split-Key:";

            autoSplitHotkeyControl = new HotkeyControl();
            autoSplitHotkeyControl.Value = new Hotkey();
            autoSplitHotkeyControl.AutoSize = true;
            autoSplitHotkeyControl.Text = "None";
            autoSplitHotkeyControl.UseKeyWhitelist = true;
            autoSplitHotkeyControl.HotkeyChanged += new EventHandler<Hotkey>(AutoSplitHotkeyControlOnHotkeyChanged);

            AutoSplitTestHotkeyButton = new Button();
            AutoSplitTestHotkeyButton.AutoSize = true;
            AutoSplitTestHotkeyButton.Text = "Test";
            AutoSplitTestHotkeyButton.Click += new EventHandler(AutoSplitTestHotkey_Click);

            AutoSplitResetHotkeyLabel = new Label();
            AutoSplitResetHotkeyLabel.AutoSize = true;
            AutoSplitResetHotkeyLabel.Padding = new Padding(0, 6, 0, 0);
            AutoSplitResetHotkeyLabel.Text = "Reset-Key:";

            autoSplitResetHotkeyControl = new HotkeyControl();
            autoSplitResetHotkeyControl.Value = new Hotkey();
            autoSplitResetHotkeyControl.AutoSize = true;
            autoSplitResetHotkeyControl.Text = "None";
            autoSplitResetHotkeyControl.UseKeyWhitelist = true;
            autoSplitResetHotkeyControl.HotkeyChanged += new EventHandler<Hotkey>(AutoSplitResetHotkeyControlOnHotkeyChanged);

            AutoSplitTestResetHotkeyButton = new Button();
            AutoSplitTestResetHotkeyButton.AutoSize = true;
            AutoSplitTestResetHotkeyButton.Text = "Test";
            AutoSplitTestResetHotkeyButton.Click += new EventHandler(AutoSplitTestResetHotkey_Click);

            EnabledCheckbox = new CheckBox();
            EnabledCheckbox.AutoSize = true;
            EnabledCheckbox.Padding = new Padding(0, 2, 0, 0);
            EnabledCheckbox.Text = "Enable autosplits";

            EnabledForExistingCharsCheckbox = new CheckBox();
            EnabledForExistingCharsCheckbox.AutoSize = true;
            EnabledForExistingCharsCheckbox.Padding = new Padding(0, 2, 0, 0);
            EnabledForExistingCharsCheckbox.Text = "Split for existing chars";

            AddAutoSplitButton = new Button();
            AddAutoSplitButton.AutoSize = true;
            AddAutoSplitButton.Text = "Add Split";
            AddAutoSplitButton.Click += new EventHandler(AddAutoSplitButton_Clicked);

            AutoSplitToolbar = new FlowLayoutPanel();
            AutoSplitToolbar.FlowDirection = FlowDirection.LeftToRight;
            AutoSplitToolbar.AutoSize = true;
            AutoSplitToolbar.Controls.Add(EnabledCheckbox);
            AutoSplitToolbar.Controls.Add(EnabledForExistingCharsCheckbox);
            AutoSplitToolbar.Controls.Add(AddAutoSplitButton);
            AutoSplitToolbar.Dock = DockStyle.Fill;
            AutoSplitToolbar.Margin = new Padding(0);

            AutoSplitToolbar2 = new FlowLayoutPanel();
            AutoSplitToolbar2.FlowDirection = FlowDirection.LeftToRight;
            AutoSplitToolbar2.AutoSize = true;

            AutoSplitToolbar2.Controls.Add(AutoSplitHotkeyLabel);
            AutoSplitToolbar2.Controls.Add(autoSplitHotkeyControl);
            AutoSplitToolbar2.Controls.Add(AutoSplitTestHotkeyButton);

            AutoSplitToolbar2.Controls.Add(AutoSplitResetHotkeyLabel);
            AutoSplitToolbar2.Controls.Add(autoSplitResetHotkeyControl);
            AutoSplitToolbar2.Controls.Add(AutoSplitTestResetHotkeyButton);
            AutoSplitToolbar2.Dock = DockStyle.Fill;
            AutoSplitToolbar2.Margin = new Padding(0);

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
            control.Controls.Add(AutoSplitToolbar, 0, 1);
            control.Controls.Add(AutoSplitToolbar2, 0, 2);
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
            autoSplitHotkeyControl.ForeColor = plugin.Config.Hotkey.ToKeys() == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Value = plugin.Config.Hotkey;
            autoSplitResetHotkeyControl.ForeColor = plugin.Config.ResetHotkey.ToKeys() == Keys.None ? Color.Red : Color.Black;
            autoSplitResetHotkeyControl.Value = plugin.Config.ResetHotkey;
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

        void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            plugin.keyService.TriggerHotkey(autoSplitHotkeyControl.Value.ToKeys());
        }

        void AutoSplitHotkeyControlOnHotkeyChanged(object sender, Hotkey e)
        {
            autoSplitHotkeyControl.ForeColor = e.ToKeys() == Keys.None ? Color.Red : Color.Black;
        }

        void AutoSplitTestResetHotkey_Click(object sender, EventArgs e)
        {
            plugin.keyService.TriggerHotkey(autoSplitResetHotkeyControl.Value.ToKeys());
        }

        void AutoSplitResetHotkeyControlOnHotkeyChanged(object sender, Hotkey e)
        {
            autoSplitResetHotkeyControl.ForeColor = e.ToKeys() == Keys.None ? Color.Red : Color.Black;
        }

        public bool IsDirty()
        {
            return autoSplitTable.IsDirty
                || plugin.Config.Enabled != EnabledCheckbox.Checked
                || plugin.Config.EnabledForExistingChars != EnabledForExistingCharsCheckbox.Checked
                || plugin.Config.Hotkey.ToKeys() != autoSplitHotkeyControl.Value.ToKeys()
                || plugin.Config.ResetHotkey.ToKeys() != autoSplitResetHotkeyControl.Value.ToKeys()
            ;
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = EnabledCheckbox.Checked;
            conf.EnabledForExistingChars = EnabledForExistingCharsCheckbox.Checked;
            conf.Hotkey = autoSplitHotkeyControl.Value;
            conf.ResetHotkey = autoSplitResetHotkeyControl.Value;
            conf.Splits = autoSplitTable.AutoSplits.ToList();
            return conf;
        }
    }
}
