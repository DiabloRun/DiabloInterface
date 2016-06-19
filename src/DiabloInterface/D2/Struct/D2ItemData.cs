using System;
using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    public enum ItemQuality : uint
    {
        Invalid,    // 0
        Low,        // 1
        Normal,     // 2
        Superior,   // 3
        Magic,      // 4
        Set,        // 5
        Rare,       // 6
        Unique,     // 7
        Crafted,    // 8
        Tempered    // 9
    }

    [Flags]
    public enum ItemFlag : uint
    {
        Identified         = 0x00000010,
        Socketed           = 0x00000800,
        RequirementsNotMet = 0x00004000,
        Named              = 0x00008000,
        Inexpensive        = 0x00020000,
        CompactSave        = 0x00200000,
        Ethereal           = 0x00400000,
        Runeword           = 0x04000000
    }

    public enum InventoryPage : sbyte
    {
        Equipped        = -1,
        Inventory       = 0,
        HoradricCube    = 3,
        Stash           = 4
    }

    public enum BodyLocation : byte
    {
        None,               // 0x0
        Head,               // 0x1
        Amulet,             // 0x2
        BodyArmor,          // 0x3
        PrimaryLeft,        // 0x4
        PrimaryRight,       // 0x5
        RingLeft,           // 0x6
        RingRight,          // 0x7
        Belt,               // 0x8
        Boots,              // 0x9
        Gloves,             // 0xA
        SecondaryLeft,      // 0xB
        SecondaryRight      // 0xC
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2ItemData
    {
        #region structure (sizeof = 0x74)
        public ItemQuality Quality;     // 0x00
        public int LoSeed;              // 0x04
        public int HiSeed;              // 0x08
        public int OwnerGUID;           // 0x0C
        public int FingerPrint;         // 0x10
        public int CommandFlags;        // 0x14
        public ItemFlag ItemFlags;      // 0x18
        public int __unknown1;          // 0x1C
        public int __unknown2;          // 0x20
        public int ActionStamp;         // 0x24
        public int FileIndex;           // 0x28
        public int ItemLevel;           // 0x2C
        public short ItemFormat;        // 0x30
        public ushort RarePrefix;       // 0x32
        public ushort RareSuffix;       // 0x34
        public ushort AutoPrefix;       // 0x36
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] MagicPrefix;    // 0x38
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] MagicSuffix;    // 0x3E
        public BodyLocation BodyLoc;    // 0x44
        public InventoryPage InvPage;   // 0x45
        public short __unknown3;        // 0x46
        public byte EarLevel;           // 0x48
        public byte InvGfxIdx;          // 0x49
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szPlayerName;     // 0x4A
        public short __unknown4;        // 0x5A
        public DataPointer pNodeOwnerInventory; // 0x5C
        public DataPointer PreviousItem; // 0x60
        public DataPointer NextItem;    // 0x64
        public byte nNodePosition;      // 0x68
        public byte nNodePositionOther; // 0x69
        public short __unknown6;        // 0x6A
        public int __unknown7;          // 0x6C
        public int __unknown8;          // 0x70
        #endregion
    }
}
