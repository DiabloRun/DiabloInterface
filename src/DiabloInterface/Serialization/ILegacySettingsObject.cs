using System.Collections.Generic;

namespace DiabloInterface.Serialization
{
    public interface ILegacySettingsObject
    {
        bool Contains(string key);
        T Value<T>(string key);
        IEnumerable<T> Values<T>(string key);
    }
}
