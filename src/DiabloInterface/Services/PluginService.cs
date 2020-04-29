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
            var s = new PluginService(di);
            s.plugins = list.ConvertAll((Type t) => Activator.CreateInstance(t, di) as IPlugin);
            return s;
        }

        PluginService(DiabloInterface di)
        {
            di.settings.Changed += Settings_Changed;
        }

        private void Settings_Changed(object sender, ApplicationSettingsEventArgs e)
        {
            foreach (IPlugin p in plugins)
                p.Config = e.Settings.PluginConf(p.Name);
        }

        public Dictionary<string, PluginConfig> EditedSettings => plugins.ToDictionary(s => s.Name, s => s.GetRenderer<IPluginSettingsRenderer>().GetEditedConfig());
        public bool EditedSettingsDirty => plugins.Any(s => s.GetRenderer<IPluginSettingsRenderer>().IsDirty());

        public Dictionary<string, Control> Controls<T>() where T : IPluginRenderer
        {
            var l = new Dictionary<string, Control>();
            foreach (IPlugin p in plugins)
            {
                var r = p.GetRenderer<T>();
                if (r != null) l.Add(p.Name, r.Render());
            }
            return l;
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
