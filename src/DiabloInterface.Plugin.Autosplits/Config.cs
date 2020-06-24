using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    internal class Config : IPluginConfig
    {
        public bool Enabled = false;
        public bool EnabledForExistingChars = false;
        public Hotkey Hotkey = new Hotkey();
        public Hotkey ResetHotkey = new Hotkey();
        public List<AutoSplit> Splits = new List<AutoSplit>();
    }
}
