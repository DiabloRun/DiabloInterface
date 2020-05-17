using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Forms
{
    public class WsExCompositedForm : Form
    {
        // reduces flickering (@see http://stackoverflow.com/questions/3718380/winforms-double-buffering)
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
