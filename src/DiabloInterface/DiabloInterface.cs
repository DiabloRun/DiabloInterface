using System;
using System.Collections.Generic;
using System.IO;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Services;
using Zutatensuppe.DiabloInterface.Services;

namespace Zutatensuppe.DiabloInterface
{
    public class DiabloInterface : IDisposable, IDiabloInterface
    {
        public IGameService game { get; private set; }
        public IApplicationInfo appInfo { get; private set; }
        public IConfigService configService { get; private set; }
        public IPluginService plugins { get; private set; }

        internal static DiabloInterface Create(IApplicationInfo appInfo, List<Type> pluginTypes)
        {
            var di = new DiabloInterface();
            di.appInfo = appInfo;
            di.configService = new ConfigService(new ApplicationStorage());
            di.game = new GameService(di);
            di.plugins = new PluginService(di, pluginTypes);
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
