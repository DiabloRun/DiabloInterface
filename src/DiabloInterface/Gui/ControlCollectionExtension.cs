namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Windows.Forms;

    internal static class ControlCollectionExtension
    {
        public static void ClearAndDispose(this Control.ControlCollection controls)
        {
            if (controls == null) throw new ArgumentNullException(nameof(controls));

            foreach (Control control in controls)
            {
                control.Dispose();
            }

            controls.Clear();
        }
    }
}
