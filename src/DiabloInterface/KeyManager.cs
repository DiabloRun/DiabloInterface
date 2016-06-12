using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using System;

namespace DiabloInterface
{
    /// <summary>
    /// ugly with way too much hardcoding. but functional so far
    /// </summary>
    class KeyManager
    {
        static IInputSimulator simulatorInstance;
        static IInputSimulator Simulator
        {
            get
            {
                if (simulatorInstance == null)
                {
                    // Create a default input simulator.
                    simulatorInstance = new InputSimulator();
                }

                return simulatorInstance;
            }
        }

        public static string KeyEventArgsToKeyString(KeyEventArgs e)
        {
            //ok keys
            List<string> keys = new List<string>();
            if (e.Control)
            {
                keys.Add("Ctrl");
            }
            if (e.Shift)
            {
                keys.Add("Shift");
            }
            if (e.Alt)
            {
                keys.Add("Alt");
            }
            keys.Add(KeyToString(e.KeyCode));
            return string.Join("+", keys.ToArray());
        }

        public static string KeyToString(Keys key)
        {
            string c = "";
            if ((key >= Keys.D0) && (key <= Keys.D9))
            {
                c = ((char)('0' + key - Keys.D0)).ToString();
            } else {
                c = key.ToString();
            }

            return c.ToString();
        }

        public static void sendKeys(string keys)
        {
            string[] keyArr = keys.Split('+');
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();
            VirtualKeyCode key = 0;

            foreach ( string k in keyArr )
            {
                string kLower = k.ToLower();
                if (kLower == "shift") { modifiers.Add(VirtualKeyCode.SHIFT); }
                else if (kLower == "ctrl") { modifiers.Add(VirtualKeyCode.CONTROL); }
                else if (kLower == "alt") { modifiers.Add(VirtualKeyCode.MENU); }
                else if (k.Length == 1) { key = (VirtualKeyCode)(int)k[0]; }
                else if (
                    kLower == "f1" || kLower == "f2" || kLower == "f3" || kLower == "f4"
                    || kLower == "f5" || kLower == "f6" || kLower == "f7" || kLower == "f8"
                    || kLower == "f9" || kLower == "f10" || kLower == "f11" || kLower == "f12"
                    || kLower.Substring(0, 6) == "numpad"
                    ) {
                    key = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), kLower.ToUpper());
                }
            }

            if (key == 0)
            {
                return;
            }

            // Unpress untanted modifier keys, the user have to repress them.
            var invalidModifiers = BuildInvalidModifiers(modifiers);
            foreach (var modifier in invalidModifiers) {
                Simulator.Keyboard.KeyUp(modifier);
            }

            // Trigger split.
            Simulator.Keyboard.ModifiedKeyStroke(modifiers, key);
        }

        static IEnumerable<VirtualKeyCode> BuildInvalidModifiers(List<VirtualKeyCode> keys)
        {
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>() {
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.MENU
            };

            List<VirtualKeyCode> invalidModifiers = new List<VirtualKeyCode>();
            foreach (VirtualKeyCode modifier in modifiers)
            {
                if (keys.Contains(modifier))
                    continue;

                // Modifier is down, but our hotkey doesn't have the modifier.
                if (Simulator.InputDeviceState.IsHardwareKeyDown(modifier))
                    invalidModifiers.Add(modifier);
            }

            return invalidModifiers;
        }
    }
}
