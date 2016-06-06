using System;
using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public class D2Stat
    {
        public ushort HiStatID;
        public ushort LoStatID;
        public int Value;

        public bool IsOfType(StatIdentifier id)
        {
            return LoStatID == (ushort)id;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D2StatArray
    {
        public DataPointer Address;
        public ushort Length;
        public ushort ByteSize;
    }

    [Flags]
    public enum StatListFlag : uint
    {
        None             = 0,
        HasProperties    = 0x40,
        HasCompleteStats = 0x80000000
    }

    [StructLayout(LayoutKind.Sequential)]
    class D2StatListEx
    {
        #region structure (sizeof = 0x64)
        public int pMemPool;            // 0x00
        public int __unknown1;          // 0x04
        public int eOwnerType;          // 0x08
        public int OwnerGUID;           // 0x0C
        public StatListFlag ListFlags;  // 0x10
        public uint NodeType;           // 0x14
        public int __unknown3;          // 0x18
        public int __unknown4;          // 0x1C
        public int __unknown5;          // 0x20
        public D2StatArray BaseStats;   // 0x24
        public DataPointer pLastList;   // 0x2C
        public int __unknown7;          // 0x30
        public int pStatListEx;         // 0x34
        public int pNextListEx;         // 0x38
        public DataPointer pMyLastList; // 0x3C
        public DataPointer pMyStats;    // 0x40
        public int pUnit;               // 0x44  list owner
        public D2StatArray FullStats;   // 0x48
        public int ModStats;            // 0x50
        public int __unknown9;          // 0x54
        public int StatFlags;           // 0x58 pointer to an array
        public int fCallback;           // 0x5C
        public int pGame;               // 0x60
        #endregion
    }
}
