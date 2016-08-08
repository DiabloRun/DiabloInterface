using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface
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

        public static void TriggerHotkey(Keys key)
        {
            if (key == Keys.None)
            {
                Logger.Instance.WriteLine("Not triggering `None` hotkey...");
                return;
            }

            Logger.Instance.WriteLine("Triggering hotkey: " + key);

            VirtualKeyCode virtualKey = (VirtualKeyCode)(key & Keys.KeyCode);

            // Construct modifier list.
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();
            if (key.HasFlag(Keys.Control))
            {
                modifiers.Add(VirtualKeyCode.CONTROL);
                Logger.Instance.WriteLine("Key has `Control` Flag");
            }

            if (key.HasFlag(Keys.Shift))
            {
                modifiers.Add(VirtualKeyCode.SHIFT);
                Logger.Instance.WriteLine("Key has `Shift` Flag");
            }

            if (key.HasFlag(Keys.Alt))
            {
                modifiers.Add(VirtualKeyCode.MENU);
                Logger.Instance.WriteLine("Key has `Alt` Flag");
            }

            Logger.Instance.WriteLine("Virtual Key Code: " + virtualKey.ToString());

            TriggerHotkey(modifiers, virtualKey);
        }

        public static void TriggerHotkey(IEnumerable<VirtualKeyCode> modifiers, VirtualKeyCode key)
        {

            if (modifiers == null)
            {
                modifiers = new List<VirtualKeyCode>();
            }

            if (key == 0)
            {
                Logger.Instance.WriteLine("Not triggering 0 key...");
                return;
            }

            // Unpress untanted modifier keys, the user have to repress them.
            var invalidModifiers = BuildInvalidModifiers(modifiers);
            foreach (var modifier in invalidModifiers)
            {
                Logger.Instance.WriteLine("Keyupping modifier " + modifier.ToString());
                Simulator.Keyboard.KeyUp(modifier);
            }

            if (key == VirtualKeyCode.XBUTTON1 || key == VirtualKeyCode.XBUTTON2)
            {
                // livesplit takes the -2 codes.
                Simulator.Mouse.XButtonClick((int) key - 2);
            }
            else if (key == VirtualKeyCode.MBUTTON)
            {
                // todo: make this work
                // not working yet.. why is InputSimulator not supporting it ? :o
            }
            else
            {
                // Trigger hotkey.
                Simulator.Keyboard.ModifiedKeyStroke(modifiers, key);
            }
        }

        static IEnumerable<VirtualKeyCode> BuildInvalidModifiers(IEnumerable<VirtualKeyCode> keys)
        {
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>() {
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.MENU,
                VirtualKeyCode.SHIFT,
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
