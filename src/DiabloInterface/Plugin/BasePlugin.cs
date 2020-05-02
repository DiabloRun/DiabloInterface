using System;
using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin
{
    public abstract class BasePlugin : IPlugin
    {
        protected virtual Type SettingsRendererType { get; } = null;
        protected virtual Type DebugRendererType { get; } = null;
        
        public abstract string Name { get; }
        public abstract void SetConfig(IPluginConfig config);
        public abstract void Initialize(DiabloInterface di);
        public virtual void Reset() { }
        public virtual void Dispose() { }

        Dictionary<Type, IPluginRenderer> renderers = new Dictionary<Type, IPluginRenderer>();
        protected void ApplyChanges()
        {
            foreach (var r in renderers.Values)
                r.ApplyChanges();
        }

        protected void ApplyConfig()
        {
            foreach (var r in renderers.Values)
                r.ApplyConfig();
        }

        private Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), SettingsRendererType},
            {typeof(IPluginDebugRenderer), DebugRendererType},
        };

        public T GetRenderer<T>() where T : IPluginRenderer
        {
            var type = typeof(T);
            if (!RendererMap.ContainsKey(type) || RendererMap[type] == null)
                return default(T);
            if (!renderers.ContainsKey(type))
                renderers[type] = (T)Activator.CreateInstance(RendererMap[type], this);
            return (T)renderers[type];
        }
    }
}
