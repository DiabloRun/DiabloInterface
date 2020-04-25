namespace Zutatensuppe.DiabloInterface.Services
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class GameService : IGameService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        bool isDisposed;
        public D2DataReader DataReader { get; private set; }

        public GameService(ISettingsService settingsService)
        {
            Logger.Info("Initializing game service.");

            DataReader = new D2DataReader(
                new GameMemoryTableFactory(),
                settingsService.CurrentSettings.ProcessDescriptions
            );
            DataReader.CharacterCreated += OnCharacterCreated;
            DataReader.DataRead += OnDataRead;

            Logger.Info("Initializing data reader thread.");
            new Thread(DataReader.RunReadOperation) { IsBackground = true }.Start();
        }

        ~GameService()
        {
            Dispose(false);
        }

        public GameDifficulty TargetDifficulty { get; set; } = GameDifficulty.Normal;

        public event EventHandler<CharacterCreatedEventArgs> CharacterCreated;

        public event EventHandler<DataReadEventArgs> DataRead;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            Logger.Info("GameService disposed.");

            if (disposing)
            {
                if (DataReader != null)
                {
                    DataReader.DataRead -= OnDataRead;
                    DataReader.CharacterCreated -= OnCharacterCreated;

                    DataReader.Dispose();
                    DataReader = null;
                }
            }

            isDisposed = true;
        }

        void OnCharacterCreated(object sender, CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(sender, e);

        void OnDataRead(object sender, DataReadEventArgs e) =>
            DataRead?.Invoke(sender, e);
    }
}
