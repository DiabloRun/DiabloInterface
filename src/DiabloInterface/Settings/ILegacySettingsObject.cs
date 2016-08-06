using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Settings
{
    public interface ILegacySettingsObject
    {
        bool Contains(string key);
        T Value<T>(string key);
        IEnumerable<T> Values<T>(string key);
    }
}
