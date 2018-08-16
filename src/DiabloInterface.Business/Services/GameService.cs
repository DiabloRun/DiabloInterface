namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class GameService : IGameService, IDisposable
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISettingsService settingsService;

        bool isDisposed;
        D2DataReader dataReader;

        public GameService(ISettingsService settingsService)
        {
            Logger.Info("Initializing game service.");

            this.settingsService = settingsService;

            InitializeDataReader();
            InitializeDataReaderThread();
        }

        ~GameService()
        {
            Dispose(false);
        }

        public GameDifficulty TargetDifficulty { get; set; } = GameDifficulty.Normal;

        public event EventHandler<CharacterCreatedEventArgs> CharacterCreated;

        public event EventHandler<DataReadEventArgs> DataRead;

        public D2DataReader DataReader => dataReader;

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
                if (dataReader != null)
                {
                    dataReader.DataRead -= OnDataRead;
                    dataReader.CharacterCreated -= OnCharacterCreated;

                    dataReader.Dispose();
                    dataReader = null;
                }
            }

            isDisposed = true;
        }

        void InitializeDataReader()
        {
            var memoryTableFactory = new GameMemoryTableFactory();

            dataReader = new D2DataReader(memoryTableFactory);
            dataReader.CharacterCreated += OnCharacterCreated;
            dataReader.DataRead += OnDataRead;
        }

        void InitializeDataReaderThread()
        {
            Logger.Info("Initializing data reader thread.");
            new Thread(dataReader.RunReadOperation) { IsBackground = true }.Start();
        }

        void OnCharacterCreated(object sender, CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(sender, e);

        void OnDataRead(object sender, DataReadEventArgs e) =>
            DataRead?.Invoke(sender, e);

    }
}
