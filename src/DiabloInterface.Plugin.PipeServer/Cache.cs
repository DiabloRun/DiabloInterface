using System;
using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class Cache
    {
        public long Miss { get; private set; } = 0;
        public long Hit { get; private set; } = 0;

        Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>();
        public void Set(string key, object value, double expireMs)
        {
            cache[key] = new CacheEntry
            {
                value = value,
                expires = DateTime.Now.Add(TimeSpan.FromMilliseconds(expireMs))
            };
        }

        public object Get(string key)
        {
            if (!cache.ContainsKey(key))
            {
                Miss++;
                return null;
            }

            CacheEntry entry = cache[key];
            if (DateTime.Now.CompareTo(entry.expires) >= 0)
            {
                Miss++;
                cache.Remove(key);
                return null;
            }

            Hit++;
            return entry.value;
        }
    }

    class CacheEntry
    {
        public DateTime expires;
        public object value;
    }
}
