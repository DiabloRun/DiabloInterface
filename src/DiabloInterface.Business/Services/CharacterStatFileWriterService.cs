namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Reflection;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.DiabloInterface.Business.Data;
    using Zutatensuppe.DiabloInterface.Business.IO;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class CharacterStatFileWriterService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISettingsService settingsService;

        readonly IGameService gameService;

        public CharacterStatFileWriterService(ISettingsService settingsService, IGameService gameService)
        {
            Logger.Info("Creating character stat file writer service.");

            if (settingsService == null) throw new ArgumentNullException(nameof(settingsService));
            if (gameService == null) throw new ArgumentNullException(nameof(gameService));

            this.settingsService = settingsService;
            this.gameService = gameService;

            RegisterServiceEventHandlers();
        }

        void RegisterServiceEventHandlers()
        {
            gameService.DataRead += GameServiceOnDataRead;
        }

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            var settings = settingsService.CurrentSettings;
            if (!settings.CreateFiles) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, settings.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }
    }
}
