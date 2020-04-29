using System.Drawing;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private TextBox textBoxPipeName;
        private Label labelPipeName;
        private CheckBox chkPipeServerEnabled;
        private RichTextBox txtPipeServer;
        private Label lblPipeServerStatus;

        private FlowLayoutPanel pluginBox;

        private Plugin p;
        public SettingsRenderer(Plugin s)
        {
            this.p = s;
        }

        public Control Render()
        {
            if (pluginBox == null || pluginBox.IsDisposed)
                Init();
            return pluginBox;
        }

        private void Init()
        {
            labelPipeName = new Label();
            labelPipeName.AutoSize = true;
            labelPipeName.Margin = new Padding(2);
            labelPipeName.Size = new Size(288, 20);
            labelPipeName.Text = "Pipe Name:";

            textBoxPipeName = new TextBox();
            textBoxPipeName.Margin = new Padding(2);
            textBoxPipeName.Size = new Size(288, 20);

            chkPipeServerEnabled = new CheckBox();
            chkPipeServerEnabled.AutoSize = true;
            chkPipeServerEnabled.Size = new Size(288, 20);
            chkPipeServerEnabled.Text = "Enable";

            lblPipeServerStatus = new Label();
            lblPipeServerStatus.AutoSize = true;
            lblPipeServerStatus.Size = new Size(288, 20);
            lblPipeServerStatus.Text = "Status:";

            txtPipeServer = new RichTextBox();
            txtPipeServer.ReadOnly = true;
            txtPipeServer.Size = new Size(288, 34);
            txtPipeServer.Text = "";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(labelPipeName);
            pluginBox.Controls.Add(textBoxPipeName);
            pluginBox.Controls.Add(chkPipeServerEnabled);
            pluginBox.Controls.Add(lblPipeServerStatus);
            pluginBox.Controls.Add(txtPipeServer);
            pluginBox.Dock = DockStyle.Fill;

            ApplyConfig();
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.config.PipeName != textBoxPipeName.Text
                || p.config.Enabled != chkPipeServerEnabled.Checked;
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
            textBoxPipeName.Text = p.config.PipeName;
            chkPipeServerEnabled.Checked = p.config.Enabled;
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = p.StatusTextMsg();
        }
    }
}
