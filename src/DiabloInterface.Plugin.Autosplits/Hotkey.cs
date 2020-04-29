using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    public class Hotkey
    {
        private Keys hotkey { get; set; } = Keys.None;

        public Hotkey()
        {
        }

        public Hotkey(string hotkey)
        {
            foreach (var k in hotkey.Split('+'))
            {
                switch (k)
                {
                    case "Ctrl": this.hotkey |= Keys.Control; break;
                    case "Shift": this.hotkey |= Keys.Shift; break;
                    case "Alt": this.hotkey |= Keys.Alt; break;
                    case "": break;
                    default: this.hotkey |= (Keys)Enum.Parse(typeof(Keys), k); break;
                }
            }
        }

        public Hotkey(Keys hotkey)
        {
            this.hotkey = hotkey;
        }

        public Keys ToKeys()
        {
            return hotkey;
        }

        override public string ToString()
        {
            if (hotkey == Keys.None)
                return "";

            StringBuilder sb = new StringBuilder();
            if (hotkey.HasFlag(Keys.Control))
                sb.Append("Ctrl+");
            if (hotkey.HasFlag(Keys.Shift))
                sb.Append("Shift+");
            if (hotkey.HasFlag(Keys.Alt))
                sb.Append("Alt+");
            sb.Append(hotkey & Keys.KeyCode);
            return sb.ToString();
        }
    }
}
