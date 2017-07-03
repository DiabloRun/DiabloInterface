namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System.Collections.Generic;

    public interface ILegacySettingsObject
    {
        bool Contains(string key);
        T Value<T>(string key);
        IEnumerable<T> Values<T>(string key);
    }
}
