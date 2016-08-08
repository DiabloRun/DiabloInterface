using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;

namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    public partial class AbstractLayout : UserControl
    {

        protected IEnumerable<Label> infoLabels;
        protected IEnumerable<FlowLayoutPanel> runePanels;
        protected Dictionary<Control, string> defaultTexts;

        protected bool RealFrwIas;

        public void Reset()
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => Reset()));
                return;
            }

            foreach (FlowLayoutPanel fp in runePanels)
            {
                if (fp.Controls.Count <= 0)
                    continue;

                foreach (RuneDisplayElement c in fp.Controls)
                {
                    c.SetHaveRune(false);
                }
            }

            foreach (KeyValuePair<Control, string> pair in defaultTexts)
            {
                pair.Key.Text = pair.Value;
            }
        }

        virtual protected void InitializeElements()
        {
            infoLabels = Enumerable.Empty<Label>();
            runePanels = Enumerable.Empty<FlowLayoutPanel>();
            defaultTexts = new Dictionary<Control, string>();
        }

        protected void ChangeVisibility(Control c, bool visible)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => ChangeVisibility(c, visible)));
                return;
            }

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
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateMinWidth(labels)));
                return;
            }

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

        protected void UpdateRuneDisplay(Dictionary<int, int> itemClassMap)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateRuneDisplay(itemClassMap)));
                return;
            }

            foreach (FlowLayoutPanel fp in runePanels)
            {
                if (fp.Controls.Count <= 0)
                    continue;

                Dictionary<int, int> dict = new Dictionary<int, int>(itemClassMap);
                foreach (RuneDisplayElement c in fp.Controls)
                {
                    int eClass = (int)c.Rune + 610;
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
