using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private Plugin plugin;
        private CheckBox control;

        public SettingsRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            control = new CheckBox();
            control.AutoSize = true;
            control.Size = new System.Drawing.Size(78, 17);
            control.Text = "Create files";
            return control;
        }

        public void ApplyConfig()
        {
            control.Checked = plugin.config.Enabled;
        }

        public void ApplyChanges()
        {
        }

        public bool IsDirty()
        {
            return plugin.config.Enabled != control.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = control.Checked;
            return conf;
        }
    }
}
