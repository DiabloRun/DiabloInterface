using System;
using System.Windows.Forms;

namespace DiabloInterface.Serialization
{
    public class DefaultLegacySettingsResolver : ILegacySettingsResolver
    {
        public ApplicationSettings ResolveSettings(ApplicationSettings settings, ILegacySettingsObject obj)
        {
            ResolveHotkeys(settings, obj);

            return settings;
        }

        void ResolveHotkeys(ApplicationSettings settings, ILegacySettingsObject obj)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (obj == null) throw new ArgumentNullException("obj");

            // Already has hotkeys, no need to resolve.
            if (settings.AutosplitHotkey != Keys.None)
                return;
            // No hotkeys assigned: default to None.
            if (!obj.Contains("TriggerKeys"))
                return;

            string triggerKeys = obj.Value<string>("TriggerKeys");
            if (string.IsNullOrEmpty(triggerKeys))
                return;

            // Convert old key string to key.
            Keys hotkey = Keys.None;
            string[] keys = triggerKeys.Split('+');
            foreach (string keyString in keys)
            {
                string keyValue = keyString;

                // Legacy system uses single character for digit keys.
                if (keyString.Length == 1 && keyString[0] >= '0' && keyString[0] <= '9')
                {
                    keyValue = "D" + keyValue;
                }

                // Combine modifiers and key.
                Keys key = Keys.None;
                if (Enum.TryParse(keyValue, true, out key))
                {
                    hotkey |= key;
                }
            }

            // Assign specified hotkey.
            settings.AutosplitHotkey = hotkey;
        }
    }
}
