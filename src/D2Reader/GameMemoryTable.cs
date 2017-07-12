namespace Zutatensuppe.D2Reader
{
    public class GameMemoryTable
    {
        /// <summary>
        /// Gets addresses to the game's global data.
        /// </summary>
        public GameMemoryAddressTable Address { get; } = new GameMemoryAddressTable();

        /// <summary>
        /// Gets address jump lists to dynamic content.
        /// </summary>
        public GameMemoryOffsetTable Offset { get; } = new GameMemoryOffsetTable();
    }
}
