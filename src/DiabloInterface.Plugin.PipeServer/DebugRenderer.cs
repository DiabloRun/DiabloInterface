using System.Drawing;
using System.Windows.Forms;

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
            control.Text = plugin.StatusTextMsg();
        }
    }
}
