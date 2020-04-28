using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Plugin;

namespace Zutatensuppe.DiabloInterface.Services
{
    public class PluginService : IDisposable
    {
        private List<IPlugin> plugins;

        internal static PluginService Create(List<Type> list, DiabloInterface di)
        {
            var s = new PluginService();
            s.plugins = list.ConvertAll((Type t) => Activator.CreateInstance(t, di) as IPlugin);
            return s;
        }

        public Dictionary<string, PluginConfig> EditedSettings => plugins.ToDictionary(s => s.Name, s => s.SettingsRenderer().Get());
        public bool EditedSettingsDirty => plugins.Any(p => p.SettingsRenderer().IsDirty());

        public Dictionary<string, Control> SettingsControls
        {
            get
            {
                var l = new Dictionary<string, Control>();
                foreach (IPlugin p in plugins)
                {
                    var r = p.SettingsRenderer();
                    if (r != null) l.Add(p.Name, r.Render());
                }
                return l;
            }
        }

        public Dictionary<string, Control> DebugControls
        {
            get
            {
                var l = new Dictionary<string, Control>();
                foreach (IPlugin p in plugins)
                {
                    var r = p.DebugRenderer();
                    if (r != null) l.Add(p.Name, r.Render());
                }
                return l;
            }
        }

        public void Initialize()
        {
            foreach (IPlugin p in plugins)
                p.Initialize();
        }

        public void Reset()
        {
            foreach (IPlugin p in plugins)
                p.Reset();
        }

        public void Dispose()
        {
            foreach (IPlugin p in plugins)
                p.Dispose();
        }
    }
}
