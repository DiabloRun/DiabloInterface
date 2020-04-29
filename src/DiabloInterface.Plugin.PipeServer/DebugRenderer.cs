using System.Drawing;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class DebugRenderer : IPluginDebugRenderer
    {
        Plugin s;
        public DebugRenderer(Plugin s)
        {
            this.s = s;
        }

        private RichTextBox txtPipeServer;
        public Control Render()
        {
            if (txtPipeServer == null || txtPipeServer.IsDisposed)
                Init();
            return txtPipeServer;
        }

        private void Init()
        {
            txtPipeServer = new RichTextBox();
            txtPipeServer.Location = new Point(6, 19);
            txtPipeServer.Size = new Size(272, 62);
            txtPipeServer.TabIndex = 0;
            txtPipeServer.Text = "";

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = s.StatusTextMsg();
        }
    }
}
