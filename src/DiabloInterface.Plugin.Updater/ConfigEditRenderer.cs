using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private GroupBox control;
        private CheckBox chkEnabled;

        Plugin plugin;

        public ConfigEditRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void ApplyChanges()
        {
        }

        public void ApplyConfig()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyConfig()));
                return;
            }

            chkEnabled.Checked = plugin.Config.Enabled;
        }

        public Control CreateControl()
        {
            var btnCheck = new Button();
            btnCheck.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnCheck.Location = new Point(6, 42);
            btnCheck.Size = new Size(265, 23);
            btnCheck.Text = "Check for updates now";
            btnCheck.Click += new EventHandler(CheckUpdatesButton_Click);

            chkEnabled = new CheckBox();
            chkEnabled.AutoSize = true;
            chkEnabled.Location = new Point(10, 19);
            chkEnabled.Size = new Size(148, 17);
            chkEnabled.Text = "Check for updates at start";

            control = new GroupBox();
            control.SuspendLayout();
            control.Controls.Add(btnCheck);
            control.Controls.Add(chkEnabled);
            control.Location = new Point(0, 0);
            control.Margin = new Padding(0);
            control.Size = new Size(277, 70);
            control.Text = "Updates";
            control.ResumeLayout(false);
            control.PerformLayout();
            return control;
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = chkEnabled.Checked;
            return conf;
        }

        public bool IsDirty()
        {
            return plugin.Config.Enabled != chkEnabled.Checked;
        }

        private void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            plugin.ManuallyCheckVersion();
        }
    }
}
