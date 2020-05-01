using System.Drawing;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private Plugin plugin;
        private FlowLayoutPanel control;
        private TextBox textBoxPipeName;
        private CheckBox chkPipeServerEnabled;
        private RichTextBox txtPipeServer;

        public SettingsRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            var labelPipeName = new Label();
            labelPipeName.AutoSize = true;
            labelPipeName.Size = new Size(288, 20);
            labelPipeName.Text = "Pipe Name:";

            textBoxPipeName = new TextBox();
            textBoxPipeName.Size = new Size(288, 20);

            chkPipeServerEnabled = new CheckBox();
            chkPipeServerEnabled.AutoSize = true;
            chkPipeServerEnabled.Size = new Size(288, 20);
            chkPipeServerEnabled.Text = "Enable";

            var lblPipeServerStatus = new Label();
            lblPipeServerStatus.AutoSize = true;
            lblPipeServerStatus.Size = new Size(288, 20);
            lblPipeServerStatus.Text = "Status:";

            txtPipeServer = new RichTextBox();
            txtPipeServer.ReadOnly = true;
            txtPipeServer.Size = new Size(288, 34);
            txtPipeServer.Text = "";

            control = new FlowLayoutPanel();
            control.FlowDirection = FlowDirection.TopDown;
            control.Controls.Add(labelPipeName);
            control.Controls.Add(textBoxPipeName);
            control.Controls.Add(chkPipeServerEnabled);
            control.Controls.Add(lblPipeServerStatus);
            control.Controls.Add(txtPipeServer);
            control.Dock = DockStyle.Fill;
            return control;
        }

        public bool IsDirty()
        {
            return plugin.config.PipeName != textBoxPipeName.Text
                || plugin.config.Enabled != chkPipeServerEnabled.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.PipeName = textBoxPipeName.Text;
            conf.Enabled = chkPipeServerEnabled.Checked;
            return conf;
        }

        public void ApplyConfig()
        {
            textBoxPipeName.Text = plugin.config.PipeName;
            chkPipeServerEnabled.Checked = plugin.config.Enabled;
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = plugin.StatusTextMsg();
        }
    }
}
