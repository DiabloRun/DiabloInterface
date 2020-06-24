using System;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class DebugRenderer : IPluginDebugRenderer
    {
        private Plugin plugin;
        private RichTextBox control;

        public DebugRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            control = new RichTextBox();
            control.Size = new Size(272, 62);
            control.Text = "";
            return control;
        }

        public void ApplyConfig()
        {
        }

        public void ApplyChanges()
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() => ApplyChanges()));
                return;
            }

            control.Text = plugin.StatusTextMsg();
        }
    }
}
