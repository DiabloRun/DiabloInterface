using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.DiabloInterface.Business.Settings;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Server;
using Zutatensuppe.DiabloInterface.Server.Handlers;

namespace Zutatensuppe.DiabloInterface.Business.Services
{
    public class ServerService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        private GameService gameService;

        public event EventHandler<ServerStatusEventArgs> StatusChanged;

        public ServerService(GameService gameService, SettingsService settingsService)
        {
            this.gameService = gameService;
            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                Init(args.Settings);
            };

            Init(settingsService.CurrentSettings);
        }

        private void Init(ApplicationSettings s)
        {
            Stop();
            if (s.PipeServerEnabled)
            {
                CreateServer(s.PipeName);
            }
        }

        private void CreateServer(string pipeName)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var pipeServer = new DiabloInterfaceServer(pipeName);
            var dataReader = gameService.DataReader;
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"game", () => new GameRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(dataReader));
            pipeServer.Start();
            Servers.Add(pipeName, pipeServer);

            StatusChanged?.Invoke(this, new ServerStatusEventArgs(Servers));
        }

        public void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
            {
                s.Value.Stop();
            }
            Servers.Clear();

            StatusChanged?.Invoke(this, new ServerStatusEventArgs(Servers));
        }
    }

    public class ServerStatusEventArgs : EventArgs
    {
        public ServerStatusEventArgs(Dictionary<string, DiabloInterfaceServer> servers)
        {
            Servers = servers;
        }

        public Dictionary<string, DiabloInterfaceServer> Servers { get; }
    }
}
