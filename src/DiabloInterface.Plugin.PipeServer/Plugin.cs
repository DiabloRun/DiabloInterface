using System;
using System.Collections.Generic;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class Plugin : BasePlugin
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "PipeServer";

        internal Config Config { get; private set; } = new Config();

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        protected override Type DebugRendererType => typeof(DebugRenderer);

        private DiabloInterface di;

        private Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
            ApplyConfig();

            Stop();

            ApplyChanges();

            if (Config.Enabled)
                CreateServer(Config.PipeName, Config.CacheMs);
        }

        public override void Initialize(DiabloInterface di)
        {
            this.di = di;
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
        }
        
        private void CreateServer(string pipeName, uint cacheMs)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var s = new DiabloInterfaceServer(pipeName, cacheMs);
            s.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            s.AddRequestHandler(@"game", () => new GameRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"items", () => new AllItemsRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"characters/current", () => new CharacterRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(di.game.DataReader));
            s.Start();
            Servers.Add(pipeName, s);

            ApplyChanges();
        }

        private void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (var s in Servers.Values)
                s.Stop();

            Servers.Clear();
        }

        public override void Dispose()
        {
            Stop();
        }

        internal string StatusTextMsg()
        {
            var txt = "";
            foreach (var pair in Servers)
                txt += pair.Key + ": " + (pair.Value.Running ? "RUNNING" : "NOT RUNNING") + "\n";
            return txt;
        }
    }
}
