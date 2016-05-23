using System;

namespace DiabloInterface
{
    public class D2MemoryAdressTable
    {
        public IntPtr Character;
        public IntPtr Quests;
        public IntPtr Difficulty;
        public IntPtr Area;
        public IntPtr ItemDescriptions;
        public IntPtr MagicModifierTable;
        public IntPtr RareModifierTable;

        // These all probably belong to a global struct the compiler optimized away.
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
        public bool SupportsItemReading;
        public D2MemoryAdressTable Address = new D2MemoryAdressTable();
        public D2MemoryOffsetTable Offset = new D2MemoryOffsetTable();
    }
}
