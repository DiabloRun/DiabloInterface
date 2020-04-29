using System;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Core.Logging;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    public class Plugin : IPlugin
    {
        public string Name => "Filewriter";

        internal Config config { get; private set; } = new Config();

        public PluginConfig Config { get => config; set {
            config = new Config(value);
            ApplyConfig();
        }}

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(SettingsRenderer)},
        };

        private ILogger Logger;

        DiabloInterface di;

        public Plugin(DiabloInterface di)
        {
            Logger = di.Logger(this);
            Logger.Info("Creating character stat file writer service.");
            this.di = di;
        }

        public void Initialize()
        {
            di.game.DataRead += Game_DataRead;
            Config = di.settings.CurrentSettings.PluginConf(Name);
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!config.Enabled) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, config.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }

        public void Reset()
        {
        }

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
    }
}
