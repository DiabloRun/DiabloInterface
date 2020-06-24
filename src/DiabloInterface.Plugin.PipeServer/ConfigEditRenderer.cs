using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class ConfigEditRenderer : IPluginConfigEditRenderer
    {
        private Plugin plugin;
        private FlowLayoutPanel control;
        private TextBox textBoxPipeName;
        private CheckBox chkPipeServerEnabled;
        private TextBox numCacheMs;
        private RichTextBox txtPipeServer;

        public ConfigEditRenderer(Plugin plugin)
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

            var labelCacheMs = new Label();
            labelCacheMs.AutoSize = true;
            labelCacheMs.Size = new Size(288, 20);
            labelCacheMs.Text = "Cache Lifetime (in ms, 0 to disable cache):";

            numCacheMs = new TextBox();
            numCacheMs.Size = new Size(288, 20);

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
            control.Controls.Add(labelCacheMs);
            control.Controls.Add(numCacheMs);
            control.Controls.Add(chkPipeServerEnabled);
            control.Controls.Add(lblPipeServerStatus);
            control.Controls.Add(txtPipeServer);
            control.Dock = DockStyle.Fill;
            return control;
        }

        private uint CacheMs()
        {
            try
            {
                return Convert.ToUInt32(numCacheMs.Text);
            } catch
            {
                return 0;
            }
        }

        public bool IsDirty()
        {
            return plugin.Config.PipeName != textBoxPipeName.Text
                || plugin.Config.Enabled != chkPipeServerEnabled.Checked
                || plugin.Config.CacheMs != CacheMs();
        }

        public IPluginConfig GetEditedConfig()
        {
            var conf = new Config();
            conf.PipeName = textBoxPipeName.Text;
            conf.Enabled = chkPipeServerEnabled.Checked;
            conf.CacheMs = CacheMs();
            return conf;
        }

        public void ApplyConfig()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyConfig()));
                return;
            }

            textBoxPipeName.Text = plugin.Config.PipeName;
            chkPipeServerEnabled.Checked = plugin.Config.Enabled;
            numCacheMs.Text = $"{plugin.Config.CacheMs}";
        }

        public void ApplyChanges()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyChanges()));
                return;
            }

            txtPipeServer.Text = plugin.StatusTextMsg();
        }
    }
}
