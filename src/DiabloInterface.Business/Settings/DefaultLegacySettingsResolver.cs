namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader.Models;

    public class DefaultLegacySettingsResolver : ILegacySettingsResolver
    {
        public ApplicationSettings ResolveSettings(ApplicationSettings settings, ILegacySettingsObject obj)
        {
            ResolveHotkeys(settings, obj);

            return settings;
        }

        void ResolveHotkeys(ApplicationSettings settings, ILegacySettingsObject obj)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            ConvertRuneSettings(settings, obj);

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
                if (Enum.TryParse(keyValue, true, out Keys key))
                {
                    hotkey |= key;
                }
            }

            // Assign specified hotkey.
            settings.AutosplitHotkey = hotkey;
        }

        void ConvertRuneSettings(ApplicationSettings settings, ILegacySettingsObject legacy)
        {
            if (!legacy.Contains("Runes")) return;

            List<int> runes = legacy.Values<int>("Runes").ToList();
            if (runes.Count == 0) return;

            settings.ClassRunes = new List<ClassRuneSettings>
            {
                new ClassRuneSettings()
                {
                    Class = null,
                    Difficulty = null,
                    Runes = runes.Select(runeId => (Rune)runeId).ToList()
                }
            };
        }
    }
}
