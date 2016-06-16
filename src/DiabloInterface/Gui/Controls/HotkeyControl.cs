using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public class HotkeyControl : TextBox
    {
        static HashSet<Keys> IgnoredKeys = new HashSet<Keys>()
        {
            Keys.ControlKey,
            Keys.ShiftKey,
            Keys.Menu,
            Keys.LWin,
            Keys.RWin,
            Keys.Capital,
            Keys.Apps,
            Keys.Space,
            Keys.NumLock,
            Keys.Scroll
        };

        /// <summary>
        /// Called when the hotkey has been changed, might be None.
        /// </summary>
        public event EventHandler<Keys> HotkeyChanged;

        Keys hotkey = Keys.None;

        /// <summary>
        /// Get or set the hotkey assigned.
        /// </summary>
        public Keys Hotkey
        {
            get
            {
                return hotkey;
            }
            set
            {
                hotkey = value;
                OnHotkeyChanged();
                UpdateDisplay();
            }
        }

        /// <summary>
        /// Use a simple whitelist, allowing only numpad keys, digits, A-Z and the function keys.
        /// </summary>
        public bool UseKeyWhitelist { get; set; }

        /// <summary>
        /// Determines whether a hotkey is assigned or not.
        /// </summary>
        public bool HasHotkey { get { return hotkey != Keys.None; } }

        public HotkeyControl()
        {
            TextChanged += HotkeyControl_TextChanged;
            KeyPress += HotkeyControl_KeyPress;
            KeyUp += HotkeyControl_KeyUp;
            KeyDown += HotkeyControl_KeyDown;
        }

        void HotkeyControl_TextChanged(object sender, EventArgs e)
        {
            // This handles cut/paste from context menues.
            if (Text != GetHotkeyString())
            {
                Text = GetHotkeyString();
            }
        }

        void HotkeyControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Consume key press events.
            e.Handled = true;
        }

        void HotkeyControl_KeyUp(object sender, KeyEventArgs e)
        {
            // Consume key up events.
            e.Handled = true;
        }

        void HotkeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Check key blacklist.
            if (IgnoredKeys.Contains(e.KeyCode))
            {
                return;
            }

            // Check key whitelist if wanted.
            if (UseKeyWhitelist && !IsKeyWhitelisted(e.KeyCode))
            {
                return;
            }

            // Reset hotkeys.
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
            {
                ResetHotkey();
            }
            else
            {
                Hotkey = e.KeyCode | e.Modifiers;
            }
        }

        bool IsKeyWhitelisted(Keys keyCode)
        {
            Func<Keys, Keys, Keys, bool> isKeyInRange = (code, min, max) => code >= min && code <= max;

            if (   isKeyInRange(keyCode, Keys.D0, Keys.D9)
                || isKeyInRange(keyCode, Keys.NumPad0, Keys.NumPad9)
                || isKeyInRange(keyCode, Keys.A, Keys.Z)
                || isKeyInRange(keyCode, Keys.F1, Keys.F12))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Unsets the hotkey.
        /// </summary>
        public void ResetHotkey()
        {
            Hotkey = Keys.None;
        }

        /// <summary>
        /// Invokes the hotkey changed event.
        /// </summary>
        protected void OnHotkeyChanged()
        {
            var hotkeyEvent = HotkeyChanged;
            if (hotkeyEvent != null)
            {
                hotkeyEvent(this, Hotkey);
            }
        }

        /// <summary>
        /// Get the hotkey in string form.
        /// </summary>
        /// <returns></returns>
        public string GetHotkeyString()
        {
            StringBuilder sb = new StringBuilder();
            if (Hotkey.HasFlag(Keys.Control))
                sb.Append("Ctrl+");
            if (Hotkey.HasFlag(Keys.Shift))
                sb.Append("Shift+");
            if (Hotkey.HasFlag(Keys.Alt))
                sb.Append("Alt+");
            sb.Append(Hotkey & Keys.KeyCode);
            return sb.ToString();
        }

        void UpdateDisplay()
        {
            Text = GetHotkeyString();
        }
    }
}
