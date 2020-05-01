using System;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys
{
    [Serializable]
    public class Hotkey : ISerializable
    {
        private Keys hotkey { get; set; } = Keys.None;

        public Hotkey()
        {
        }

        public Hotkey(SerializationInfo info, StreamingContext context)
        {
            this.hotkey = parse(info.GetString("Keys"));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Keys", ToString());
        }

        public Hotkey(string hotkey)
        {
            this.hotkey = parse(hotkey);
        }

        public Hotkey(Keys hotkey)
        {
            this.hotkey = hotkey;
        }

        private Keys parse(string hotkey)
        {
            Keys hk = Keys.None;
            foreach (var k in hotkey.Split('+'))
            {
                switch (k)
                {
                    case "Ctrl": hk |= Keys.Control; break;
                    case "Shift": hk |= Keys.Shift; break;
                    case "Alt": hk |= Keys.Alt; break;
                    case "": break;
                    default: hk |= (Keys)Enum.Parse(typeof(Keys), k); break;
                }
            }
            return hk;
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
