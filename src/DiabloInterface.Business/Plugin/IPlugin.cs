using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;

namespace Zutatensuppe.DiabloInterface.Business.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        PluginData Data { get; }
        PluginConfig Cfg { get; }

        event EventHandler<IPlugin> Changed;

        void OnSettingsChanged();
        void OnCharacterCreated(CharacterCreatedEventArgs e);
        void OnDataRead(DataReadEventArgs e);
        void OnReset();

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

    public interface IPluginSettingsRenderer: IPluginRenderer
    {
        bool IsDirty();
        PluginConfig Get();
        void Set(PluginConfig cfg);
    }

    public interface IPluginDebugRenderer: IPluginRenderer
    {
    }


    public class PluginConfig: PluginData
    {
        public PluginConfig(Dictionary<string, object> dict = null) : base(dict) { }

        public void Apply(PluginConfig other)
        {
            foreach (var p in other.dict)
                Set(p.Key, p.Value);
        }
    }

    // TODO: change
    public class PluginData
    {
        protected readonly Dictionary<string, object> dict = new Dictionary<string, object>();
        private readonly Dictionary<string, object> def = new Dictionary<string, object>();

        public PluginData(Dictionary<string, object> dict = null)
        {
            if (dict != null)
            {
                def = dict;
                foreach (var p in def)
                    Set(p.Key, p.Value);
            }
        }

        public Dictionary<string, object> All()
        {
            return dict;
        }

        public void Set(string key, object val)
        {
            dict[key] = val;
        }

        public object Get(string key)
        {
            return dict.ContainsKey(key) ? dict[key] : null;
        }

        private T get<T>(string key, T def)
        {
            var val = Get(key);
            if (val != null && val.GetType() == typeof(T))
                return (T)val;
            return def;
        }

        public bool GetBool(string key) => get<bool>(key, false);
        public string GetString(string key) => get<string>(key, "");
        public Keys GetKeys(string key) => get<Keys>(key, Keys.None);
    }
}
