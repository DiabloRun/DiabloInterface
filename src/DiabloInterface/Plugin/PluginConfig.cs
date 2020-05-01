using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zutatensuppe.DiabloInterface.Plugin
{
    [Serializable]
    public class PluginConfig : ISerializable
    {
        protected readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        public PluginConfig(Dictionary<string, object> dict = null)
        {
            if (dict != null)
                this.dict = dict;
        }

        public string GetString(string key) => get(key, "");
        public void SetString(string key, string val) => set(key, val);

        public bool GetBool(string key) => get(key, false);
        public void SetBool(string key, bool val) => set(key, val);

        public object GetObject(string key) => get(key);
        public void SetObject(string key, object val) => set(key, val);

        public PluginConfig(SerializationInfo info, StreamingContext context)
        {
            var e = info.GetEnumerator();
            for (var i = 0; i < info.MemberCount; i++)
            {
                e.MoveNext();
                dict.Add(e.Name, e.Value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var p in dict)
                info.AddValue(p.Key, p.Value);
        }

        private object get(string key) => dict.ContainsKey(key) ? dict[key] : null;
        private void set(string key, object val) { dict[key] = val; }

        private T get<T>(string key, T def)
        {
            var val = get(key);
            if (val != null && val.GetType() == typeof(T))
                return (T)val;
            return def;
        }
    }
}
