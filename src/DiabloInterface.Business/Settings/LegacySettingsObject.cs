﻿namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;

    class LegacySettingsObject : ILegacySettingsObject
    {
        Dictionary<string, object> data;

        public LegacySettingsObject(Dictionary<string, object> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            this.data = data;
        }

        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }

        public T Value<T>(string key)
        {
            return (T)data[key];
        }

        public IEnumerable<T> Values<T>(string key)
        {
            return (IEnumerable<T>)data[key];
        }
    }
}
