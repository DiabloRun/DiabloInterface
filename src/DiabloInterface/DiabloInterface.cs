using System;
using System.IO;
using Zutatensuppe.DiabloInterface.Services;

namespace Zutatensuppe.DiabloInterface
{
    public class DiabloInterface : IDisposable
    {
        public IGameService game;
        public IConfigService configService;
        public PluginService plugins;

        internal static DiabloInterface Create()
        {
            var di = new DiabloInterface();
            di.configService = new ConfigService(new ApplicationStorage());
            di.game = new GameService(di);
            di.plugins = new PluginService(di, Path.Combine(Environment.CurrentDirectory, "Plugins"));
            di.Initialize();
            return di;
        }

        private void Initialize()
        {
            configService.Initialize();
            game.Initialize();
            plugins.Initialize();
        }

        public void Dispose()
        {
            configService.Dispose();
            game.Dispose();
            plugins.Dispose();
        }
    }
}
