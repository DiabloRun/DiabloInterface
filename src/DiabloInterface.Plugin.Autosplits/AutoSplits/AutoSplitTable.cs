namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Zutatensuppe.DiabloInterface.Lib.Extensions;

    public class AutoSplitTable : UserControl
    {
        static string[] HeaderTitles = new string[]
        {
            "Name",
            "Type",
            "Value",
            "Difficulty"
        };

        private List<AutoSplitSettingsRow> rows = new List<AutoSplitSettingsRow>();

        bool Dirty = false;
        public bool IsDirty
        {
            get
            {
                // Table itself dirty (something added/removed).
                if (Dirty) return true;

                // Check if any of the rows were changed.
                var anyDirty = Controls.OfType<AutoSplitSettingsRow>().Any(row => row.IsDirty);
                if (anyDirty) return true;

                // Nothing changed.
                return false;
            }
        }

        public IEnumerable<AutoSplit> AutoSplits => from row in Controls.OfType<AutoSplitSettingsRow>() select row.AutoSplit;

        private TableLayoutPanel HeaderTable;
        private void InitializeComponent()
        {
            HeaderTable = new TableLayoutPanel();
            SuspendLayout();

            HeaderTable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            HeaderTable.AutoSize = true;
            HeaderTable.ColumnCount = 5;
            HeaderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            HeaderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            HeaderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            HeaderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            HeaderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 41F));
            HeaderTable.Location = new Point(63, 3);
            HeaderTable.Name = "HeaderTable";
            HeaderTable.RowCount = 1;
            HeaderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            HeaderTable.Size = new Size(325, 68);
            HeaderTable.TabIndex = 0;

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(HeaderTable);
            Name = "AutoSplitTable";
            Size = new Size(506, 80);
            ResumeLayout(false);
            PerformLayout();
        }

        internal void Set(Config conf)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => Set(conf)));
                return;
            }

            ReloadWithCurrentSettings(conf);
            MarkClean();
        }

        internal AutoSplitTable(Config conf)
        {
            Layout += AutoSplitTable_Layout;
            InitializeComponent();
            InitializeHeader();
            Set(conf);
        }

        void InitializeHeader()
        {
            foreach (var title in HeaderTitles)
            {
                HeaderTable.Controls.Add(new Label() { Text = title });
            }
        }

        private void ReloadWithCurrentSettings(Config conf)
        {
            var cpy = conf.DeepCopy();
            var splits = cpy.Splits;

            int autosplitCount = splits == null ? 0 : splits.Count;
            int rowCount = rows.Count;
            for (int i = 0; i < autosplitCount; i++)
            {
                if (rowCount <= i)
                {
                    AddAutoSplit(splits[i]);
                } else
                {
                    rows[i].SetAutosplit(splits[i]);
                }
            }
            for (int i = autosplitCount; i < rows.Count; i++)
            {
                Controls.Remove(rows[i]);
            }
            rows = rows.GetRange(0, autosplitCount);
        }

        public Control AddAutoSplit(AutoSplit autosplit)
        {
            if (autosplit == null) return null;

            // Operate on a copy.
            autosplit = new AutoSplit(autosplit);

            // Create and show the autosplit row.
            AutoSplitSettingsRow row = new AutoSplitSettingsRow(autosplit);
            Controls.Add(row);
            rows.Add(row);
            row.OnDelete += (item) =>
            {
                Controls.Remove(row);
                rows.Remove(row);
            };
            return row;
        }

        public void MarkClean()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => MarkClean()));
                return;
            }

            Dirty = false;
            foreach (AutoSplitSettingsRow row in Controls.OfType<AutoSplitSettingsRow>())
            {
                row.MarkClean();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is AutoSplitSettingsRow)
            {
                Dirty = true;
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            if (e.Control is AutoSplitSettingsRow)
            {
                Dirty = true;
            }
        }

        private void AutoSplitTable_Layout(object sender, LayoutEventArgs e)
        {
            // Set the scroll bar size.
            AutoScrollMinSize = new Size(0, CalculateTotalHeight());

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
