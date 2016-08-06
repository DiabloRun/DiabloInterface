using Zutatensuppe.D2Reader.Struct.Item.Modifier;
using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct.Item
{
    [Flags]
    public enum UniqueItemFlags : uint
    {
        None        = 0,
        Enabled     = 1 << 0,
        NoLimit     = 1 << 1,
        Carry       = 1 << 2,
        Ladder      = 1 << 3,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x14C)]
    public class D2UniqueItemDescription
    {
        [ExpectOffset(0x000)] public UInt16 TableIndex;              // 0x000
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x002)] public string Index;                   // 0x002
        [ExpectOffset(0x022)] public UInt16 StringIdentifier;        // 0x022
        [ExpectOffset(0x024)] public UInt32 Version;                 // 0x024
        [ExpectOffset(0x028)] public UInt32 ItemCode;                // 0x028
        [ExpectOffset(0x02C)] public UniqueItemFlags Flags;          // 0x02C
        [ExpectOffset(0x030)] public UInt32 Rarity;                  // 0x030
        [ExpectOffset(0x034)] public UInt16 Level;                   // 0x034
        [ExpectOffset(0x036)] public UInt16 LevelRequirement;        // 0x036
        [ExpectOffset(0x038)] public UInt8  ChrTransform;            // 0x038
        [ExpectOffset(0x039)] public UInt8  InvTransform;            // 0x039
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x03A)] public string FlippyFile;              // 0x03A
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x05A)] public string InvFile;                 // 0x05A
        [ExpectOffset(0x07A)] public UInt16 __Padding;               // 0x07A
        [ExpectOffset(0x07C)] public UInt32 CostMultiplier;          // 0x07C
        [ExpectOffset(0x080)] public UInt32 CostAdd;                 // 0x080
        [ExpectOffset(0x084)] public UInt16 DropSound;               // 0x084
        [ExpectOffset(0x086)] public UInt16 UseSound;                // 0x086
        [ExpectOffset(0x088)] public UInt32 DropSfxFrame;            // 0x088
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 12)]
        [ExpectOffset(0x08C)] public D2ItemModifier[] Modifiers;     // 0x08C
    }
}
