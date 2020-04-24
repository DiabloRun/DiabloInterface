using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class Cache
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);
        public long Miss { get; private set; } = 0;
        public long Hit { get; private set; } = 0;

        Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>();
        public void Set(string key, object value, double timeoutMs)
        {
            cache[key] = new CacheEntry
            {
                value = value,
                expires = DateTime.Now.Add(TimeSpan.FromMilliseconds(timeoutMs))
            };
        }

        public object Get(string key)
        {
            if (!cache.ContainsKey(key))
            {
                // Logger.Info($"Cache miss {key}");
                Miss++;
                return null;
            }

            CacheEntry entry = cache[key];
            if (DateTime.Now.CompareTo(entry.expires) >= 0)
            {
                // Logger.Info($"Cache expired {key}");
                Miss++;
                cache.Remove(key);
                return null;
            }

            // Logger.Info($"Cache hit {key}");
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
