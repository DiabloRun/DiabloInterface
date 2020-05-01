using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    class Config : PluginConfig
    {
        public bool Enabled {
            get { return GetBool("Enabled"); }
            set { SetBool("Enabled", value); }
        }
        public Hotkey Hotkey {
            get { return new Hotkey(GetString("Hotkey")); }
            set { SetString("Hotkey", value.ToString()); }
        }
        public Hotkey ResetHotkey {
            get { return new Hotkey(GetString("ResetHotkey")); }
            set { SetString("ResetHotkey", value.ToString()); }
        }
        public List<AutoSplit> Splits {
            get { return (List<AutoSplit>)GetObject("Splits"); }
            set { SetObject("Splits", value); }
        }

        internal Config()
        {
            Enabled = false;
            Hotkey = new Hotkey();
            ResetHotkey = new Hotkey();
            Splits = new List<AutoSplit>();
        }

        public Config(PluginConfig s) : this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                Hotkey = new Hotkey(s.GetString("Hotkey"));
                ResetHotkey = new Hotkey(s.GetString("ResetHotkey"));
                Splits = s.GetObject("Splits") as List<AutoSplit>;
                if (Splits == null)
                    Splits = new List<AutoSplit>();
            }
        }
    }
}
