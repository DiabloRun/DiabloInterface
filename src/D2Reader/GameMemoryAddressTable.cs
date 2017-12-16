namespace Zutatensuppe.D2Reader
{
    using System;

    public class GameMemoryAddressTable
    {
        public IntPtr World;
        public IntPtr GameId; // gameId (each time a game is started, this is increased by 1)
        public IntPtr Area;
        public IntPtr ItemDescriptions;
        public IntPtr MagicModifierTable;
        public IntPtr RareModifierTable;

        public IntPtr PlayerUnit;
        public IntPtr GlobalData;
        public IntPtr LowQualityItems;
        public IntPtr StringIndexerTable;
        public IntPtr StringAddressTable;
        public IntPtr PatchStringIndexerTable;
        public IntPtr PatchStringAddressTable;
        public IntPtr ExpansionStringIndexerTable;
        public IntPtr ExpansionStringAddressTable;
    }
}
