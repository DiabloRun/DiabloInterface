using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using WindowsInput;
using WindowsInput.Native;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface
{
    /// <summary>
    /// ugly with way too much hardcoding. but functional so far
    /// </summary>
    internal static class KeyManager
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

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

        static void TriggerHotkey(IEnumerable<VirtualKeyCode> modifiers, VirtualKeyCode key)
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
            var modifiers = new List<VirtualKeyCode>() {
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
