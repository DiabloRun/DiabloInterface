using System;
using System.Reflection;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    public class Plugin : BasePlugin
    {
        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "Filewriter";

        internal Config Config { get; private set; } = new Config();

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
            ApplyConfig();
        }

        public override void Initialize(IDiabloInterface di)
        {
            Logger.Info("Creating character stat file writer service.");

            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
            di.game.DataRead += Game_DataRead;
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Config.Enabled) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, Config.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }
    }
}
