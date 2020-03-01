using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Server;
using Zutatensuppe.DiabloInterface.Server.Handlers;

namespace Zutatensuppe.DiabloInterface.Business.Services
{
    public class ServerService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        public ServerService(GameService gameService, SettingsService settingsService)
        {
            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                if (!Servers.ContainsKey(args.Settings.PipeName))
                {
                    Stop();
                    CreateServer(args.Settings.PipeName, gameService.DataReader);
                }
            };
            CreateServer(settingsService.CurrentSettings.PipeName, gameService.DataReader);
        }

        private void CreateServer(string pipeName, D2DataReader dataReader)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var pipeServer = new DiabloInterfaceServer(pipeName);
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(dataReader));
            pipeServer.Start();
            Servers.Add(pipeName, pipeServer);
        }

        public void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
            {
                s.Value.Stop();
            }
            Servers.Clear();
        }
    }
}
