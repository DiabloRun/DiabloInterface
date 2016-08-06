using Zutatensuppe.D2Reader;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    public partial class AbstractLayout : UserControl
    {

        protected IEnumerable<Label> infoLabels;
        protected IEnumerable<FlowLayoutPanel> runePanels;
        protected bool RealFrwIas;

        virtual protected void InitializeElements()
        {
            infoLabels = Enumerable.Empty<Label>();
            runePanels = Enumerable.Empty<FlowLayoutPanel>();
        }

        protected void ChangeVisibility(Control c, bool visible)
        {
            if (visible)
            {
                c.Show();
            }
            else
            {
                c.Hide();
            }
        }

        protected void UpdateMinWidth(Label[] labels)
        {
            int w = 0;
            foreach (Label label in labels)
            {
                var measuredSize = TextRenderer.MeasureText(label.Text, label.Font, Size.Empty, TextFormatFlags.SingleLine);
                if (measuredSize.Width > w) w = measuredSize.Width;
            }

            foreach (Label label in labels)
            {
                label.MinimumSize = new Size(w, 0);
            }
        }


        public void Reset()
        {
            foreach (FlowLayoutPanel fp in runePanels)
            {
                if (fp.Controls.Count > 0)
                {

                    foreach (RuneDisplayElement c in fp.Controls)
                    {
                        c.SetHaveRune(false);
                    }
                }
            }
        }

        protected void UpdateRuneDisplay(Dictionary<int, int> itemClassMap)
        {

            foreach (FlowLayoutPanel fp in runePanels)
            {
                if (fp.Controls.Count > 0)
                {

                    Dictionary<int, int> dict = new Dictionary<int, int>(itemClassMap);
                    foreach (RuneDisplayElement c in fp.Controls)
                    {
                        int eClass = (int)c.getRune() + 610;
                        if (dict.ContainsKey(eClass) && dict[eClass] > 0)
                        {
                            dict[eClass]--;
                            c.SetHaveRune(true);
                        }
                    }
                }
            }

        }
    }
}
