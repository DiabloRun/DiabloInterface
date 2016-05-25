using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace DiabloInterface.D2.Struct
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
        public UInt16 TableIndex;              // 0x000
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string Index;                   // 0x002
        public UInt16 StringIdentifier;        // 0x022
        public UInt32 Version;                 // 0x024
        public UInt32 ItemCode;                // 0x028
        public UniqueItemFlags Flags;          // 0x02C
        public UInt32 Rarity;                  // 0x030
        public UInt16 Level;                   // 0x034
        public UInt16 LevelRequirement;        // 0x036
        public UInt8  ChrTransform;            // 0x038
        public UInt8  InvTransform;            // 0x039
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string FlippyFile;              // 0x03A
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string InvFile;                 // 0x05A
        public UInt16 __Padding;               // 0x07A
        public UInt32 CostMultiplier;          // 0x07C
        public UInt32 CostAdd;                 // 0x080
        public UInt16 DropSound;               // 0x084
        public UInt16 UseSound;                // 0x086
        public UInt32 DropSfxFrame;            // 0x088
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 12)]
        public D2ItemModifier[] Modifiers;     // 0x08C
    }
}
