using System;

namespace DiabloInterface
{
    public class D2MemoryAddressTable
    {
        public IntPtr World;
        public IntPtr GameId;
        public IntPtr Area;
        public IntPtr ItemDescriptions;
        public IntPtr MagicModifierTable;
        public IntPtr RareModifierTable;

        // These all probably belong to a global struct the compiler optimized away.
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

    public class D2MemoryOffsetTable
    {
        public int[] Quests;
    }

    public class D2MemoryTable
    {
        public D2MemoryAddressTable Address = new D2MemoryAddressTable();
        public D2MemoryOffsetTable Offset = new D2MemoryOffsetTable();
    }
}
