using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin;
using Zutatensuppe.DiabloInterface.Services;

namespace Zutatensuppe.DiabloInterface
{
    public class DiabloInterface : IDisposable
    {
        public IGameService game;
        public ISettingsService settings;
        public PluginService plugins;

        internal static DiabloInterface Create()
        {
            var appStorage = new ApplicationStorage();
            var settings = new SettingsService(appStorage);
            var pluginDir = Path.Combine(Environment.CurrentDirectory, "Plugins");

            var di = new DiabloInterface();
            di.game = new GameService(settings);
            di.settings = settings;
            di.plugins = new PluginService(di, pluginDir);

            // is dependant on di.plugins being there
            di.settings.LoadSettingsFromPreviousSession();

            di.plugins.Initialize();
            return di;
        }

        public ILogger Logger(object obj)
        {
            return LogServiceLocator.Get(obj.GetType());
        }

        public void Dispose()
        {
            settings.Dispose();
            plugins.Dispose();
            game.Dispose();
        }
    }
}
