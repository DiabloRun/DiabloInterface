using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private Plugin p;

        private FlowLayoutPanel pluginBox;
        private CheckBox chkBox;

        public SettingsRenderer(Plugin p)
        {
            this.p = p;
        }

        public Control Render()
        {
            if (pluginBox == null || pluginBox.IsDisposed)
                Init();
            return pluginBox;
        }

        private void Init()
        {
            chkBox = new CheckBox();
            chkBox.AutoSize = true;
            chkBox.Location = new System.Drawing.Point(10, 19);
            chkBox.Size = new System.Drawing.Size(78, 17);
            chkBox.Text = "Create files";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(chkBox);

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
            chkBox.Checked = p.config.Enabled;
        }

        public void ApplyChanges()
        {
        }

        public bool IsDirty()
        {
            return p.config.Enabled != chkBox.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = chkBox.Checked;
            return conf;
        }
    }
}
