namespace Zutatensuppe.DiabloInterface.Services
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Lib.Services;

    public class GameService : IGameService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        bool isDisposed;
        public D2DataReader DataReader { get; private set; }

        private DiabloInterface di;

        public GameService(DiabloInterface di)
        {
            this.di = di;
        }

        public void Initialize()
        {
            Logger.Info("Initializing game service.");

            DataReader = new D2DataReader(
                new GameMemoryTableFactory(),
                di.configService.CurrentConfig.ProcessDescriptions
            );
            DataReader.DataRead += OnDataRead;

            Logger.Info("Initializing data reader thread.");
            new Thread(DataReader.RunReadOperation) { IsBackground = true }.Start();
        }

        ~GameService()
        {
            Dispose(false);
        }

        public GameDifficulty TargetDifficulty { get; set; } = GameDifficulty.Normal;

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

                    DataReader.Dispose();
                    DataReader = null;
                }
            }

            isDisposed = true;
        }

        void OnDataRead(object sender, DataReadEventArgs e) =>
            DataRead?.Invoke(sender, e);
    }
}
