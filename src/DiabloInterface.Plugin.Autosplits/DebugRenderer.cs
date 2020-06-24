using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class DebugRenderer : IPluginDebugRenderer
    {
        private Plugin plugin;
        private Panel control;
        List<AutosplitBinding> bindings = new List<AutosplitBinding>();

        public DebugRenderer(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public Control CreateControl()
        {
            control = new Panel();
            control.Dock = DockStyle.Fill;
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
            
            // Unbinds and clears the binding list.
            foreach (var binding in bindings)
                binding.Unbind();
            bindings.Clear();

            int y = 0;
            int height = 16;
            Color colorOn = Color.Green;
            Color colorOff = Color.Red;
            control.Controls.Clear();
            foreach (AutoSplit autoSplit in plugin.Config.Splits)
            {
                var label = new Label();
                label.SetBounds(0, y, control.Bounds.Width, height);
                label.Text = autoSplit.Name;
                label.ForeColor = autoSplit.IsReached ? colorOn : colorOff;

                // Bind autosplit events.
                bindings.Add(new AutosplitBinding(
                    autoSplit,
                    // reached
                    s => label.ForeColor = colorOn,
                    // reset
                    s => label.ForeColor = colorOff
                ));

                control.Controls.Add(label);
                y += height;
            }
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
        public void Unbind()
        {
            if (didUnbind) return;

            didUnbind = true;
            autoSplit.Reached -= reachedHandler;
            autoSplit.Reset -= resetHandler;
        }
    }
}
