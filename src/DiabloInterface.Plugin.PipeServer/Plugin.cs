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
        protected readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "PipeServer";

        internal Config Config { get; private set; } = new Config();

        protected override Type SettingsRendererType => typeof(SettingsRenderer);

        protected override Type DebugRendererType => typeof(DebugRenderer);

        private DiabloInterface di;

        private Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        public override void SetConfig(IPluginConfig c)
        {
            Config = c as Config;
            ApplyConfig();

            Stop();

            ApplyChanges();

            if (Config.Enabled)
                CreateServer(Config.PipeName);
        }

        public override void Initialize(DiabloInterface di)
        {
            this.di = di;
            SetConfig(di.settings.CurrentSettings.PluginConf(Name));
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
        }

        public override void Reset()
        {
        }

        public override void Dispose()
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
    }
}
