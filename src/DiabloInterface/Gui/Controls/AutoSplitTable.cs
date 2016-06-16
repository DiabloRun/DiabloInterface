using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public partial class AutoSplitTable : UserControl
    {
        static string[] HeaderTitles = new string[]
        {
            "Name",
            "Type",
            "Value",
            "Difficulty"
        };

        bool dirty = false;
        public bool IsDirty
        {
            get
            {
                // Table itself dirty (something added/removed).
                if (dirty) return true;

                // Check if any of the rows were changed.
                foreach (Control control in Controls)
                {
                    var row = control as AutoSplitSettingsRow;
                    if (row != null && row.IsDirty)
                    {
                        return true;
                    }
                }

                // Nothing changed.
                return false;
            }
        }

        public IEnumerable<AutoSplit> AutoSplits
        {
            get
            {
                return (from control in Controls.Cast<Control>()
                        let row = control as AutoSplitSettingsRow
                        where row != null
                        select row.AutoSplit);
            }
        }

        public AutoSplitTable()
        {
            Layout += AutoSplitTable_Layout;

            InitializeComponent();
            InitializeHeader();
        }

        void InitializeHeader()
        {
            foreach (var title in HeaderTitles)
            {
                HeaderTable.Controls.Add(new Label() { Text = title });
            }
        }

        public void MarkClean()
        {
            dirty = false;
            foreach (Control control in Controls)
            {
                var row = control as AutoSplitSettingsRow;
                if (row != null)
                {
                    row.MarkClean();
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is AutoSplitSettingsRow)
            {
                dirty = true;
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            if (e.Control is AutoSplitSettingsRow)
            {
                dirty = true;
            }
        }

        private void AutoSplitTable_Layout(object sender, LayoutEventArgs e)
        {
            // Set the scroll bar size.
            int height = CalculateTotalHeight();
            AutoScrollMinSize = new Size(0, height);

            Point cursor = new Point(0, -VerticalScroll.Value);
            int width = ClientSize.Width;
            foreach (Control control in Controls)
            {
                cursor.X = control.Margin.Left;
                cursor.Y += control.Margin.Top;
                control.SetBounds(cursor.X, cursor.Y, width, control.Height);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                cursor.Y += control.Height + control.Margin.Bottom;
            }
        }

        int CalculateTotalHeight()
        {
            int height = 0;
            foreach (Control control in Controls)
            {
                if (control.AutoSize)
                {
                    control.Size = control.GetPreferredSize(ClientSize);
                }
                height += control.Height + control.Margin.Vertical;
            }

            return height;
        }
    }
}
