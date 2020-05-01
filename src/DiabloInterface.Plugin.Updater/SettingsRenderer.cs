using System;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private GroupBox control;
        private CheckBox chkEnabled;

        Plugin plugin;

        public SettingsRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void ApplyChanges()
        {
        }

        public void ApplyConfig()
        {
            chkEnabled.Checked = plugin.config.Enabled;
        }

        public Control CreateControl()
        {
            var btnCheck = new Button();
            btnCheck.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnCheck.Location = new System.Drawing.Point(6, 42);
            btnCheck.Size = new System.Drawing.Size(265, 23);
            btnCheck.Text = "Check for updates now";
            btnCheck.Click += new EventHandler(CheckUpdatesButton_Click);

            chkEnabled = new CheckBox();
            chkEnabled.AutoSize = true;
            chkEnabled.Location = new System.Drawing.Point(10, 19);
            chkEnabled.Size = new System.Drawing.Size(148, 17);
            chkEnabled.Text = "Check for updates at start";

            control = new GroupBox();
            control.SuspendLayout();
            control.Controls.Add(btnCheck);
            control.Controls.Add(chkEnabled);
            control.Location = new System.Drawing.Point(0, 0);
            control.Margin = new Padding(0);
            control.Size = new System.Drawing.Size(277, 70);
            control.Text = "Updates";
            control.ResumeLayout(false);
            control.PerformLayout();
            return control;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = chkEnabled.Checked;
            return conf;
        }

        public bool IsDirty()
        {
            return plugin.config.Enabled != chkEnabled.Checked;
        }

        private void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            plugin.ManuallyCheckVersion();
        }
    }
}
