using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DiabloInterface.Serialization
{
    public class JsonLegacySettings : ILegacySettingsObject
    {
        JToken token;

        public JsonLegacySettings(JToken token)
        {
            if (token == null) throw new ArgumentNullException("token");

            this.token = token;
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
