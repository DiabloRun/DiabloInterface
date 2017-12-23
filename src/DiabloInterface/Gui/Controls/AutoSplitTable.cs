namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;

    public partial class AutoSplitTable : UserControl
    {
        static string[] HeaderTitles = new string[]
        {
            "Name",
            "Type",
            "Value",
            "Difficulty"
        };

        readonly ISettingsService settingsService;

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

        public IEnumerable<AutoSplit> AutoSplits
        {
            get
            {
                return (from row in Controls.OfType<AutoSplitSettingsRow>() select row.AutoSplit);
            }
        }

        public AutoSplitTable(ISettingsService settingsService)
        {
            Layout += AutoSplitTable_Layout;

            this.settingsService = settingsService;
            RegisterServiceEventHandlers();
            InitializeComponent();
            InitializeHeader();

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                UnregisterServiceEventHandlers();
            };

            ReloadWithCurrentSettings(settingsService.CurrentSettings);
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceSettingsChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            settingsService.SettingsChanged -= SettingsServiceSettingsChanged;
        }

        void SettingsServiceSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceSettingsChanged(sender, e)));
                return;
            }

            ReloadWithCurrentSettings(e.Settings);

            MarkClean();
        }

        private void ReloadWithCurrentSettings(ApplicationSettings s)
        {
            var settings = s.DeepCopy();
            int autosplitCount = settings.Autosplits.Count;
            int rowCount = rows.Count;
            for (int i = 0; i < autosplitCount; i++)
            {
                if (rowCount <= i)
                {
                    AddAutoSplit(settings.Autosplits[i]);
                } else
                {
                    rows[i].SetAutosplit(settings.Autosplits[i]);
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

        void InitializeHeader()
        {
            foreach (var title in HeaderTitles)
            {
                HeaderTable.Controls.Add(new Label() { Text = title });
            }
        }

        public void MarkClean()
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
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
