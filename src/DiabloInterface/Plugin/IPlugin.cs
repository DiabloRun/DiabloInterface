using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        void Initialize();

        void Reset();

        void Dispose();

        // render in the settings window
        IPluginSettingsRenderer SettingsRenderer();

        // render in the debug window
        IPluginDebugRenderer DebugRenderer();

        // render in the gui?
        // 
    }

    public interface IPluginRenderer
    {
        Control Render();
        void ApplyChanges();
    }

    public interface IPluginSettingsRenderer : IPluginRenderer
    {
        bool IsDirty();
        PluginConfig Get();
    }

    public interface IPluginDebugRenderer: IPluginRenderer
    {
    }

    [Serializable]
    public class PluginConfig : ISerializable
    {
        protected readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        public PluginConfig(Dictionary<string, object> dict = null)
        {
            if (dict != null)
            {
                this.dict = dict;
            }
        }
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

        public Dictionary<string, object> All()
        {
            return dict;
        }

        protected void Set(string key, object val)
        {
            dict[key] = val;
        }

        private T get<T>(string key, T def)
        {
            var val = Get(key);
            if (val != null && val.GetType() == typeof(T))
                return (T)val;
            return def;
        }

        public object Get(string key) => dict.ContainsKey(key) ? dict[key] : null;
        public bool Is(string key) => get<bool>(key, false);
        public string GetString(string key) => get<string>(key, "");
        public Keys GetKeys(string key) => get<Keys>(key, Keys.None);
    }
}
