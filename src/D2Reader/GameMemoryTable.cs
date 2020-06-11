using System;

namespace Zutatensuppe.D2Reader
{
    public class GameMemoryTable
    {
        public IntPtr World;
        public IntPtr PlayersX;
        public IntPtr GameId; // gameId (each time a game is started, this is increased by 1. note: doesnt start with 0/1)
        public IntPtr Area;
        public IntPtr InventoryTab; // points to byte 0|1
        public IntPtr ItemDescriptions;
        public IntPtr MagicModifierTable;
        public IntPtr RareModifierTable;

        public IntPtr? Units114; // 1.14 uses unit lists 
        public IntPtr? Units113; // 1.13 uses one unit list (idx = guid)  (linked list by pPrev)

        public IntPtr PlayerUnit;
        public IntPtr Pets; // not sure if the name will be final
        public IntPtr GlobalData;
        public IntPtr LowQualityItems;
        public IntPtr StringIndexerTable;
        public IntPtr StringAddressTable;
        public IntPtr PatchStringIndexerTable;
        public IntPtr PatchStringAddressTable;
        public IntPtr ExpansionStringIndexerTable;
        public IntPtr ExpansionStringAddressTable;

        // states
        public IntPtr Loading;
        public IntPtr? Saving; // not used yet
        public IntPtr? Saving2; // not used yet
        public IntPtr? InGame; // not used yet
        public IntPtr? InMenu; // not used yet
    }
}
