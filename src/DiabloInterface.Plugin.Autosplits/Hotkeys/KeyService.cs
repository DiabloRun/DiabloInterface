using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys
{
    public class KeyService
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IInputSimulator Simulator;

        public KeyService()
        {
            Simulator = new InputSimulator();
        }

        public void TriggerHotkey(Keys key)
        {
            if (key == Keys.None)
            {
                Logger.Debug("Not triggering `None` hotkey...");
                return;
            }

            Logger.Info("Triggering hotkey: " + key);

            var virtualKey = (VirtualKeyCode)(key & Keys.KeyCode);

            // Construct modifier list.
            var modifiers = new List<VirtualKeyCode>();
            if (key.HasFlag(Keys.Control))
            {
                modifiers.Add(VirtualKeyCode.CONTROL);
                Logger.Debug("Key has `Control` Flag");
            }

            if (key.HasFlag(Keys.Shift))
            {
                modifiers.Add(VirtualKeyCode.SHIFT);
                Logger.Debug("Key has `Shift` Flag");
            }

            if (key.HasFlag(Keys.Alt))
            {
                modifiers.Add(VirtualKeyCode.MENU);
                Logger.Debug("Key has `Alt` Flag");
            }

            Logger.Debug($"Virtual Key Code: {virtualKey}");

            TriggerHotkey(modifiers, virtualKey);
        }

        private void TriggerHotkey(IEnumerable<VirtualKeyCode> modifiers, VirtualKeyCode key)
        {
            if (modifiers == null)
            {
                modifiers = new List<VirtualKeyCode>();
            }

            if (key == 0)
            {
                Logger.Debug("Not triggering 0 key...");
                return;
            }

            // Unpress untanted modifier keys, the user have to repress them.
            IEnumerable<VirtualKeyCode> invalidModifiers = BuildInvalidModifiers(modifiers);
            foreach (var modifier in invalidModifiers)
            {
                Logger.Debug("Keyupping modifier " + modifier);
                Simulator.Keyboard.KeyUp(modifier);
            }

            if (key == VirtualKeyCode.XBUTTON1 || key == VirtualKeyCode.XBUTTON2)
            {
                // livesplit takes the -2 codes.
                Simulator.Mouse.XButtonClick((int)key - 2);
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

        private IEnumerable<VirtualKeyCode> BuildInvalidModifiers(IEnumerable<VirtualKeyCode> keys)
        {
            var modifiers = new List<VirtualKeyCode>
            {
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.MENU,
                VirtualKeyCode.SHIFT,
            };

            var invalidModifiers = new List<VirtualKeyCode>();
            foreach (var modifier in modifiers)
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
