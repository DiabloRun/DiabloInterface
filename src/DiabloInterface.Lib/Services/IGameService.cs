namespace Zutatensuppe.DiabloInterface.Lib.Services
{
    using System;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;

    public interface IGameService: IDisposable
    {
        void Initialize();

        /// <summary>
        ///     The targeted (intended) difficulty for a run selected by the player.
        /// </summary>
        GameDifficulty TargetDifficulty { get; set; }

        /// <summary>
        ///     Occurs when a game process has been found
        /// </summary>
        event EventHandler<ProcessFoundEventArgs> ProcessFound;

        /// <summary>
        ///     Occurs when data has been successfully read from the game.
        /// </summary>
        event EventHandler<DataReadEventArgs> DataRead;

        /// <summary>
        ///     Gets the data reader associated with the game service.
        /// </summary>
        D2DataReader DataReader { get; }
    }
}
