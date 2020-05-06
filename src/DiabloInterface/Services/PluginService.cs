using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin;

namespace Zutatensuppe.DiabloInterface.Services
{
    public class PluginService : IDisposable
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private List<IPlugin> plugins;

        private DiabloInterface di;

        public PluginService(DiabloInterface di, string pluginDir)
        {
            this.di = di;

            if (!Directory.Exists(pluginDir))
                Directory.CreateDirectory(pluginDir);
            List<Type> types = new List<Type>();
            foreach (FileInfo file in new DirectoryInfo(pluginDir).GetFiles("*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(file.FullName);
                    types.AddRange(assembly.GetTypes());
                } catch (Exception e)
                {
                    Logger.Error($"Plugin not loaded {file.FullName} {e.Message}");
                }
            }
            plugins = types
                .FindAll((Type t) => new List<Type>(t.GetInterfaces())
                .Contains(typeof(IPlugin)))
                .ConvertAll((Type t) => Activator.CreateInstance(t) as IPlugin);
        }

        public Dictionary<string, IPluginConfig> GetEditedConfigs => plugins
            .Select(s => new KeyValuePair<string, IPluginConfigEditRenderer>(s.Name, s.GetRenderer<IPluginConfigEditRenderer>()))
            .Where(s => s.Value != null)
            .ToDictionary(s => s.Key, s => s.Value.GetEditedConfig());

        public bool EditedConfigsDirty => plugins
            .Select(s => s.GetRenderer<IPluginConfigEditRenderer>())
            .Where(r => r != null)
            .Any(r => r.IsDirty());

        public Dictionary<string, Control> CreateControls<T>() where T : IPluginRenderer
        {
            var l = new Dictionary<string, Control>();
            foreach (IPlugin p in plugins)
            {
                var r = p.GetRenderer<T>();
                if (r != null)
                {
                    l.Add(p.Name, r.CreateControl());
                    r.ApplyConfig();
                    r.ApplyChanges();
                }
            }
            return l;
        }

        public void Reset()
        {
            foreach (IPlugin p in plugins)
                p.Reset();
        }

        public void Initialize()
        {
            foreach (IPlugin p in plugins)
                p.Initialize(di);

            di.configService.Changed += ConfigChanged;
        }

        public void Dispose()
        {
            di.configService.Changed -= ConfigChanged;

            foreach (IPlugin p in plugins)
                p.Dispose();
        }

        private void ConfigChanged(object sender, ApplicationConfigEventArgs e)
        {
            foreach (IPlugin p in plugins)
                p.SetConfig(e.Config.PluginConf(p.Name));
        }
    }
}
