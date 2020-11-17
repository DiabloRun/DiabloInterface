using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item
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

    public enum ItemContainer : byte
    {
        Equipment   =  0,
        Belt        =  1,
        Inventory   =  2,
        __unknown_1 =  3,
        __unknown_2 =  4,
        Cube        =  5,
        Stash       =  6,
        Hireling    = 10,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2ItemData
    {
        #region structure (sizeof = 0x74)
        [ExpectOffset(0x00)] public ItemQuality Quality;     // 0x00
        [ExpectOffset(0x04)] public int LoSeed;              // 0x04
        [ExpectOffset(0x08)] public int HiSeed;              // 0x08
        [ExpectOffset(0x0C)] public int OwnerGUID;           // 0x0C
        [ExpectOffset(0x10)] public int FingerPrint;         // 0x10
        [ExpectOffset(0x14)] public int CommandFlags;        // 0x14
        [ExpectOffset(0x18)] public ItemFlag ItemFlags;      // 0x18
        [ExpectOffset(0x1C)] public int __unknown1;          // 0x1C
        [ExpectOffset(0x20)] public int __unknown2;          // 0x20
        [ExpectOffset(0x24)] public int ActionStamp;         // 0x24
        [ExpectOffset(0x28)] public int FileIndex;           // 0x28
        [ExpectOffset(0x2C)] public int ItemLevel;           // 0x2C
        [ExpectOffset(0x30)] public short ItemFormat;        // 0x30
        [ExpectOffset(0x32)] public ushort RarePrefix;       // 0x32
        [ExpectOffset(0x34)] public ushort RareSuffix;       // 0x34
        [ExpectOffset(0x36)] public ushort AutoPrefix;       // 0x36
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        [ExpectOffset(0x38)] public ushort[] MagicPrefix;    // 0x38
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        [ExpectOffset(0x3E)] public ushort[] MagicSuffix;    // 0x3E
        [ExpectOffset(0x44)] public BodyLocation BodyLoc;    // 0x44
        [ExpectOffset(0x45)] public InventoryPage InvPage;   // 0x45
        [ExpectOffset(0x46)] public short __unknown3;        // 0x46
        [ExpectOffset(0x48)] public byte EarLevel;           // 0x48
        [ExpectOffset(0x49)] public byte InvGfxIdx;          // 0x49
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        [ExpectOffset(0x4A)] public string szPlayerName;     // 0x4A
        [ExpectOffset(0x5A)] public short __unknown4;        // 0x5A
        [ExpectOffset(0x5C)] public DataPointer pNodeOwnerInventory; // 0x5C
        [ExpectOffset(0x60)] public DataPointer PreviousItem; // 0x60
        [ExpectOffset(0x64)] public DataPointer NextItem;    // 0x64
        [ExpectOffset(0x68)] public byte nNodePosition;      // 0x68
        [ExpectOffset(0x69)] public byte nNodePositionOther; // 0x69
        [ExpectOffset(0x6A)] public short __unknown6;        // 0x6A
        [ExpectOffset(0x6C)] public int __unknown7;          // 0x6C
        [ExpectOffset(0x70)] public int __unknown8;          // 0x70
        #endregion

        internal bool IsEquipped()
        {
            return InvPage == InventoryPage.Equipped;
        }

        internal bool IsEquippedInSlot(BodyLocation loc)
        {
            return InvPage == InventoryPage.Equipped
                && BodyLoc == loc;
        }
    }
}
