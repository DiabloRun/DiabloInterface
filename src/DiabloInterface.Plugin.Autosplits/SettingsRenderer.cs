using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private Plugin p;

        private TableLayoutPanel AutoSplitLayout;
        AutoSplitTable autoSplitTable;
        HotkeyControl autoSplitHotkeyControl;
        private Button AddAutoSplitButton;
        private CheckBox EnableAutosplitCheckBox;
        private Label AutoSplitHotkeyLabel;
        private Button AutoSplitTestHotkeyButton;
        private FlowLayoutPanel AutoSplitToolbar;

        public SettingsRenderer(Plugin p)
        {
            this.p = p;
        }

        public Control Render()
        {
            if (AutoSplitLayout == null || AutoSplitLayout.IsDisposed)
                Init();
            return AutoSplitLayout;
        }

        private void Init()
        {
            autoSplitHotkeyControl = new HotkeyControl();
            autoSplitHotkeyControl.Value = new Hotkey();
            autoSplitHotkeyControl.AutoSize = true;
            autoSplitHotkeyControl.Text = "None";
            autoSplitHotkeyControl.UseKeyWhitelist = true;
            autoSplitHotkeyControl.HotkeyChanged += new EventHandler<Hotkey>(AutoSplitHotkeyControlOnHotkeyChanged);

            AutoSplitHotkeyLabel = new Label();
            AutoSplitHotkeyLabel.AutoSize = true;
            AutoSplitHotkeyLabel.Text = "Split-Hotkey:";

            AutoSplitTestHotkeyButton = new Button();
            AutoSplitTestHotkeyButton.AutoSize = true;
            AutoSplitTestHotkeyButton.Text = "Test Hotkey";
            AutoSplitTestHotkeyButton.Click += new EventHandler(AutoSplitTestHotkey_Click);

            EnableAutosplitCheckBox = new CheckBox();
            EnableAutosplitCheckBox.AutoSize = true;
            EnableAutosplitCheckBox.Text = "Enable";

            AddAutoSplitButton = new Button();
            AddAutoSplitButton.AutoSize = true;
            AddAutoSplitButton.Text = "Add Split";
            AddAutoSplitButton.Click += new EventHandler(AddAutoSplitButton_Clicked);

            AutoSplitToolbar = new FlowLayoutPanel();
            AutoSplitToolbar.FlowDirection = FlowDirection.LeftToRight;
            AutoSplitToolbar.AutoSize = true;
            AutoSplitToolbar.Controls.Add(AutoSplitHotkeyLabel);
            AutoSplitToolbar.Controls.Add(autoSplitHotkeyControl);
            AutoSplitToolbar.Controls.Add(EnableAutosplitCheckBox);
            AutoSplitToolbar.Controls.Add(AutoSplitTestHotkeyButton);
            AutoSplitToolbar.Controls.Add(AddAutoSplitButton);
            AutoSplitToolbar.Dock = DockStyle.Fill;
            AutoSplitToolbar.Margin = new Padding(0);

            autoSplitTable = new AutoSplitTable(p.config) { Dock = DockStyle.Fill };

            AutoSplitLayout = new TableLayoutPanel();
            AutoSplitLayout.ColumnCount = 1;
            AutoSplitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AutoSplitLayout.Dock = DockStyle.Fill;
            AutoSplitLayout.Location = new Point(0, 0);
            AutoSplitLayout.Margin = new Padding(0);
            AutoSplitLayout.RowCount = 2;
            AutoSplitLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AutoSplitLayout.RowStyles.Add(new RowStyle());
            AutoSplitLayout.Size = new Size(800, 476);
            AutoSplitLayout.Controls.Add(AutoSplitToolbar, 0, 1);
            AutoSplitLayout.Controls.Add(autoSplitTable);

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
            EnableAutosplitCheckBox.Checked = p.config.Enabled;
            autoSplitHotkeyControl.ForeColor = p.config.Hotkey.ToKeys() == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Value = p.config.Hotkey;
            autoSplitTable.Set(p.config);
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
            {
                autoSplitTable.ScrollControlIntoView(row);
            }

            // Automatically enable auto splits when adding.
            EnableAutosplitCheckBox.Checked = true;
        }

        void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            p.keyService.TriggerHotkey(autoSplitHotkeyControl.Value.ToKeys());
        }

        void AutoSplitHotkeyControlOnHotkeyChanged(object sender, Hotkey e)
        {
            autoSplitHotkeyControl.ForeColor = e.ToKeys() == Keys.None ? Color.Red : SystemColors.WindowText;
        }

        public bool IsDirty()
        {
            return autoSplitTable.IsDirty
                || p.config.Enabled != EnableAutosplitCheckBox.Checked
                || p.config.Hotkey.ToKeys() != autoSplitHotkeyControl.Value.ToKeys();
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = EnableAutosplitCheckBox.Checked;
            conf.Hotkey = autoSplitHotkeyControl.Value;
            conf.Splits = autoSplitTable.AutoSplits.ToList();
            return conf;
        }
    }
}
