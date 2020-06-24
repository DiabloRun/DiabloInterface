using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private Plugin plugin;
        private CheckBox control;

        public ConfigEditRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            control = new CheckBox();
            control.AutoSize = true;
            control.Size = new Size(78, 17);
            control.Text = "Create files";
            return control;
        }

        public void ApplyConfig()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyConfig()));
                return;
            }

            control.Checked = plugin.Config.Enabled;
        }

        public void ApplyChanges()
        {
        }

        public bool IsDirty()
        {
            return plugin.Config.Enabled != control.Checked;
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = control.Checked;
            return conf;
        }
    }
}
