using System;
using System.Collections.Generic;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class Plugin : BasePlugin
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "PipeServer";

        internal Config Config { get; private set; } = new Config();

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        protected override Type DebugRendererType => typeof(DebugRenderer);

        private IDiabloInterface di;

        private Dictionary<string, Server.Server> Servers = new Dictionary<string, Server.Server>();

        public override void SetConfig(IPluginConfig c)
        {
            var newConf = c == null ? new Config() : c as Config;
            var dirty = !Config.Equals(newConf);
            Config = newConf;

            ApplyConfig();

            if (dirty)
            {
                Stop();
                if (Config.Enabled)
                    CreateServer(Config.PipeName, Config.CacheMs);
            }

            ApplyChanges();
        }

        public override void Initialize(IDiabloInterface di)
        {
            this.di = di;
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
        }
        
        private void CreateServer(string pipeName, uint cacheMs)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var s = new Server.Server(pipeName, cacheMs);
            s.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            s.AddRequestHandler(@"game", () => new GameRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"items", () => new AllItemsRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"characters/current", () => new CharacterRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"hireling", () => new HirelingRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(di.game.DataReader));
            s.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(di.game.DataReader));
            s.Start();
            Servers.Add(pipeName, s);
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
