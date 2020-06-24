using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private Plugin plugin;
        private FlowLayoutPanel control;
        private RichTextBox txtHttpClientHeaders;
        private Label label6;
        private RichTextBox txtHttpClientStatus;
        private Label label4;
        private CheckBox chkHttpClientEnabled;
        private TextBox textBoxHttpClientUrl;
        private Label label5;

        public ConfigEditRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
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

            control = new FlowLayoutPanel();
            control.FlowDirection = FlowDirection.TopDown;
            control.Controls.Add(label5);
            control.Controls.Add(textBoxHttpClientUrl);
            control.Controls.Add(label6);
            control.Controls.Add(txtHttpClientHeaders);
            control.Controls.Add(chkHttpClientEnabled);
            control.Controls.Add(label4);
            control.Controls.Add(txtHttpClientStatus);
            control.Dock = DockStyle.Fill;
            return control;
        }

        public bool IsDirty()
        {
            return plugin.Config.Url != textBoxHttpClientUrl.Text
                || plugin.Config.Headers != txtHttpClientHeaders.Text
                || plugin.Config.Enabled != chkHttpClientEnabled.Checked;
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.Enabled = chkHttpClientEnabled.Checked;
            conf.Url = textBoxHttpClientUrl.Text;
            conf.Headers = txtHttpClientHeaders.Text;
            return conf;
        }

        public void ApplyConfig()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyConfig()));
                return;
            }

            textBoxHttpClientUrl.Text = plugin.Config.Url;
            chkHttpClientEnabled.Checked = plugin.Config.Enabled;
            txtHttpClientHeaders.Text = plugin.Config.Headers;
        }

        public void ApplyChanges()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyChanges()));
                return;
            }

            txtHttpClientStatus.Text = plugin.content;
        }
    }
}
