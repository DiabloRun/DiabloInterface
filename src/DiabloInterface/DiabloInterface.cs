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
            var di = new DiabloInterface();
            di.game = new GameService(settings);
            di.settings = settings;
            di.plugins = PluginService.Create(GetPlugIns(), di);

            // is dependant on di.plugins being there
            di.settings.LoadSettingsFromPreviousSession();

            if (di.settings.CurrentSettings.CheckUpdates)
                VersionChecker.AutomaticallyCheckForUpdate();

            di.plugins.Initialize();
            return di;
        }

        private static List<Type> GetPlugIns()
        {
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            FileInfo[] files = dInfo.GetFiles("DiabloInterface.Plugin.*.dll");
            List<Type> types = new List<Type>();
            foreach (FileInfo file in files)
                types.AddRange(Assembly.LoadFile(file.FullName).GetTypes());
            return types
                .FindAll((Type t) => new List<Type>(t.GetInterfaces())
                .Contains(typeof(IPlugin)));
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
