using System;
using System.Collections.Generic;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class Plugin : IPlugin
    {
        public string Name => "PipeServer";

        internal Config config { get; private set; } = new Config();

        public PluginConfig Config { get => config; set {
            config = new Config(value);
            ApplyConfig();
            
            Stop();

            if (config.Enabled)
                CreateServer(config.PipeName);
        }}

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(SettingsRenderer)},
            {typeof(IPluginDebugRenderer), typeof(DebugRenderer)},
        };

        private ILogger Logger;

        private DiabloInterface di;

        private Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        public void Initialize(DiabloInterface di)
        {
            Logger = di.Logger(this);
            this.di = di;
            Config = di.settings.CurrentSettings.PluginConf(Name);
        }
        
        private void CreateServer(string pipeName)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var pipeServer = new DiabloInterfaceServer(pipeName);
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"game", () => new GameRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(di.game.DataReader));
            pipeServer.Start();
            Servers.Add(pipeName, pipeServer);

            ApplyChanges();
        }

        private void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
                s.Value.Stop();

            Servers.Clear();

            ApplyChanges();
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
            Stop();
        }

        internal string StatusTextMsg()
        {
            var txt = "";
            foreach (var s in Servers)
                txt += s.Key + ": " + (s.Value.Running ? "RUNNING" : "NOT RUNNING") + "\n";
            return txt;
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
