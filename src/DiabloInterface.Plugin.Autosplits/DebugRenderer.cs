using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class DebugRenderer : IPluginDebugRenderer
    {
        private Plugin p;
        public DebugRenderer(Plugin p)
        {
            this.p = p;
        }

        private Panel autosplitPanel;
        List<AutosplitBinding> autoSplitBindings;
        public Control Render()
        {
            if (autosplitPanel == null || autosplitPanel.IsDisposed)
                Init();
            return autosplitPanel;
        }

        private void Init()
        {
            autosplitPanel = new Panel();
            autosplitPanel.AutoScroll = true;
            autosplitPanel.Dock = DockStyle.Fill;
            autosplitPanel.Location = new Point(3, 16);
            autosplitPanel.Size = new Size(281, 105);

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
        }

        public void ApplyChanges()
        {
            // Unbinds and clears the binding list.
            ClearAutoSplitBindings();

            int y = 0;
            autosplitPanel.Controls.Clear();
            foreach (AutoSplit autoSplit in p.config.Splits)
            {
                Label splitLabel = new Label();
                splitLabel.SetBounds(0, y, autosplitPanel.Bounds.Width, 16);
                splitLabel.Text = autoSplit.Name;
                splitLabel.ForeColor = autoSplit.IsReached ? Color.Green : Color.Red;

                Action<AutoSplit> splitReached = s => splitLabel.ForeColor = Color.Green;
                Action<AutoSplit> splitReset = s => splitLabel.ForeColor = Color.Red;

                // Bind autosplit events.
                var binding = new AutosplitBinding(autoSplit, splitReached, splitReset);
                autoSplitBindings.Add(binding);

                autosplitPanel.Controls.Add(splitLabel);
                y += 16;
            }
        }

        void ClearAutoSplitBindings()
        {
            if (autoSplitBindings == null)
            {
                autoSplitBindings = new List<AutosplitBinding>();
            }

            foreach (var binding in autoSplitBindings)
            {
                binding.Unbind();
            }

            autoSplitBindings.Clear();
        }
    }

    class AutosplitBinding
    {
        bool didUnbind;
        AutoSplit autoSplit;
        Action<AutoSplit> reachedHandler;
        Action<AutoSplit> resetHandler;

        public AutosplitBinding(
            AutoSplit autoSplit,
            Action<AutoSplit> reachedHandler,
            Action<AutoSplit> resetHandler
        )
        {
            this.autoSplit = autoSplit;
            this.reachedHandler = reachedHandler;
            this.resetHandler = resetHandler;

            this.autoSplit.Reached += reachedHandler;
            this.autoSplit.Reset += resetHandler;
        }

        ~AutosplitBinding()
        {
            Unbind();
        }

        /// <summary>
        /// Unbding the autosplit handlers.
        /// </summary>
        public void Unbind()
        {
            if (didUnbind) return;

            didUnbind = true;
            autoSplit.Reached -= reachedHandler;
            autoSplit.Reset -= resetHandler;
        }
    }
}
