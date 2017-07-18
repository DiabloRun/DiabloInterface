namespace Zutatensuppe.DiabloInterface.Business.Settings
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    public class JsonLegacySettings : ILegacySettingsObject
    {
        readonly JToken token;

        public JsonLegacySettings(JToken token)
        {
            this.token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public bool Contains(string key)
        {
            return token[key] != null;
        }

        public T Value<T>(string key)
        {
            return token[key].ToObject<T>();
        }

        public IEnumerable<T> Values<T>(string key)
        {
            return token[key].ToObject<IEnumerable<T>>();
        }
    }
}
