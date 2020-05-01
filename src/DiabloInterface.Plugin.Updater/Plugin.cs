using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    public class Plugin : IPlugin
    {
        public string Name => "Updater";

        private readonly string VersionFile = "last_found_version.txt";
        private VersionChecker versionChecker;

        internal Config config { get; private set; } = new Config();

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(SettingsRenderer)},
        };

        public PluginConfig Config { get => config; set { config = new Config(value); } }

        public void Dispose()
        {
        }

        Dictionary<Type, IPluginRenderer> renderers = new Dictionary<Type, IPluginRenderer>();
        private void ApplyChanges()
        {
            foreach (var p in renderers)
                p.Value.ApplyChanges();
        }

        private void ApplyConfig()
        {
            foreach (var p in renderers)
                p.Value.ApplyConfig();
        }

        public T GetRenderer<T>() where T : IPluginRenderer
        {
            var type = typeof(T);
            if (!RendererMap.ContainsKey(type))
                return default(T);
            if (!renderers.ContainsKey(type))
                renderers[type] = (T)Activator.CreateInstance(RendererMap[type], this);
            return (T)renderers[type];
        }

        public void Initialize(DiabloInterface di)
        {
            versionChecker = new VersionChecker();
            Config = di.settings.CurrentSettings.PluginConf(Name);
            AutomaticallyCheckVersion();
        }

        public void Reset()
        {
        }

        internal string LastFoundVersion {
            get => File.Exists(VersionFile) ? File.ReadAllText(VersionFile) : null;
            set { if (value != null) { File.WriteAllText(VersionFile, value); } }
        }

        internal void ManuallyCheckVersion()
        {
            var r = versionChecker.CheckForUpdate(LastFoundVersion, true);
            LastFoundVersion = r.updateUrl;
            Ask(r);
        }

        internal void AutomaticallyCheckVersion()
        {
            if (!config.Enabled) return;

            var r = versionChecker.CheckForUpdate(LastFoundVersion, false);
            LastFoundVersion = r.updateUrl;
            Ask(r);
        }

        private void Ask(VersionCheckerResult r)
        {
            if (r.target == null || r.question == null || r.title == null) return;

            if (MessageBox.Show(r.question, r.title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(r.target);
            }
        }
    }
}
