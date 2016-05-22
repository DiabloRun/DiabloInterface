using System;

namespace DiabloInterface
{
    public class D2MemoryAdressTable
    {
        public IntPtr Character;
        public IntPtr Quests;
        public IntPtr Difficulty;
        public IntPtr Area;
    }

    public class D2MemoryOffsetTable
    {
        public int[] Quests;
    }

    public class D2MemoryTable
    {
        public D2MemoryAdressTable Address = new D2MemoryAdressTable();
        public D2MemoryOffsetTable Offset = new D2MemoryOffsetTable();
    }
}
