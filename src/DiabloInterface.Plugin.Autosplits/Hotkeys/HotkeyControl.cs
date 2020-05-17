using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys
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

        public event EventHandler<Hotkey> HotkeyChanged;

        Hotkey hotkey = new Hotkey();

        public Hotkey Value
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
        public bool HasHotkey { get { return hotkey.ToKeys() != Keys.None; } }

        public HotkeyControl()
        {
            TextChanged += HotkeyControl_TextChanged;
            KeyPress += HotkeyControl_KeyPress;
            KeyUp += HotkeyControl_KeyUp;
            KeyDown += HotkeyControl_KeyDown;
            MouseUp += HotkeyControl_MouseUp;
        }

        void HotkeyControl_TextChanged(object sender, EventArgs e)
        {
            // This handles cut/paste from context menues.
            if (Text != hotkey.ToString())
                Text = hotkey.ToString();
        }

        void HotkeyControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;
            if (e.Button == MouseButtons.Right)
                return;

            if (e.Button == MouseButtons.Middle)
                // Hotkey = Keys.MButton; not working with InputSimulator, so not valid hotkey
                return;
            else if (e.Button == MouseButtons.XButton1)
                Value = new Hotkey(Keys.XButton1);
            else if (e.Button == MouseButtons.XButton2)
                Value = new Hotkey(Keys.XButton2);
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

            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                ResetHotkey();
                return;
            }

            // Check key whitelist if wanted.
            if (UseKeyWhitelist && !IsKeyWhitelisted(e.KeyCode))
            {
                return;
            }

            Value = new Hotkey(e.KeyCode | e.Modifiers);
        }

        bool IsKeyWhitelisted(Keys keyCode)
        {
            Func<Keys, Keys, Keys, bool> isKeyInRange = (code, min, max) => code >= min && code <= max;

            if (   isKeyInRange(keyCode, Keys.D0, Keys.D9) // this inclueds numbers
                || isKeyInRange(keyCode, Keys.A, Keys.Z) // this includes letters
                || isKeyInRange(keyCode, Keys.NumPad0, Keys.F24) // this includes: numpad0-9, math operators, F-keys
            ){
                return true;
            }
            else return false;
        }

        public void ResetHotkey()
        {
            Value = new Hotkey();
        }

        protected void OnHotkeyChanged()
        {
            HotkeyChanged?.Invoke(this, Value);
        }

        void UpdateDisplay()
        {
            Text = hotkey.ToString();
        }
    }
}
