using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin;

namespace DiabloInterface.Plugin.HttpClient
{
    class SettingsRenderer : IPluginSettingsRenderer
    {
        private Plugin p;

        private FlowLayoutPanel pluginBox;
        private RichTextBox txtHttpClientHeaders;
        private Label label6;
        private RichTextBox txtHttpClientStatus;
        private Label label4;
        private CheckBox chkHttpClientEnabled;
        private TextBox textBoxHttpClientUrl;
        private Label label5;

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
            label6 = new Label();
            label6.AutoSize = true;
            label6.Size = new Size(50, 13);
            label6.Text = "Headers:";

            txtHttpClientHeaders = new RichTextBox();
            txtHttpClientHeaders.Size = new Size(288, 69);
            txtHttpClientHeaders.Text = "";

            label5 = new Label();
            label5.AutoSize = true;
            label5.Size = new Size(288, 13);
            label5.Text = "URL:";

            textBoxHttpClientUrl = new TextBox();
            textBoxHttpClientUrl.Size = new Size(288, 20);
            textBoxHttpClientUrl.TabIndex = 1;

            chkHttpClientEnabled = new CheckBox();
            chkHttpClientEnabled.AutoSize = true;
            chkHttpClientEnabled.Size = new Size(288, 17);
            chkHttpClientEnabled.Text = "Enable";

            label4 = new Label();
            label4.AutoSize = true;
            label4.Size = new Size(288, 13);
            label4.Text = "Status:";

            txtHttpClientStatus = new RichTextBox();
            txtHttpClientStatus.ReadOnly = true;
            txtHttpClientStatus.Size = new Size(288, 100);
            txtHttpClientStatus.Text = "";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(label5);
            pluginBox.Controls.Add(textBoxHttpClientUrl);
            pluginBox.Controls.Add(label6);
            pluginBox.Controls.Add(txtHttpClientHeaders);
            pluginBox.Controls.Add(chkHttpClientEnabled);
            pluginBox.Controls.Add(label4);
            pluginBox.Controls.Add(txtHttpClientStatus);
            pluginBox.Dock = DockStyle.Fill;

            ApplyConfig();
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.config.Url != textBoxHttpClientUrl.Text
                || p.config.Headers != txtHttpClientHeaders.Text
                || p.config.Enabled != chkHttpClientEnabled.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = chkHttpClientEnabled.Checked;
            conf.Url = textBoxHttpClientUrl.Text;
            conf.Headers = txtHttpClientHeaders.Text;
            return conf;
        }

        public void ApplyConfig()
        {
            textBoxHttpClientUrl.Text = p.config.Url;
            chkHttpClientEnabled.Checked = p.config.Enabled;
            txtHttpClientHeaders.Text = p.config.Headers;
        }

        public void ApplyChanges()
        {
            txtHttpClientStatus.Text = p.content;
        }
    }
}
