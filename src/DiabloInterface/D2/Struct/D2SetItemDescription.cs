using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x1B8)]
    public class D2SetItemDescription
    {
        public UInt16 TableIndex;               // 0x000
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string Index;                    // 0x002
        public UInt16 Version;                  // 0x022
        public UInt16 StringIdentifier;         // 0x024
        public UInt16 __Padding1;               // 0x026
        public UInt32 ItemCode;                 // 0x028
        public UInt16 SetCode;                  // 0x02C
        public UInt16 SetPointerIndex;          // 0x02E
        public UInt16 Level;                    // 0x030
        public UInt16 LevelRequirement;         // 0x032
        public UInt32 Rarity;                   // 0x034
        public UInt32 CostMultiplier;           // 0x038
        public UInt32 CostAdd;                  // 0x03C
        public UInt8  ChrTransform;             // 0x040
        public UInt8  InvTransform;             // 0x041
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string FlippyFile;               // 0x042
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string InvFile;                  // 0x062
        public UInt16 DropSound;                // 0x082
        public UInt16 UseSound;                 // 0x084
        public UInt8  DropSfxFrame;             // 0x086
        public UInt8  AddFunction;              // 0x087
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x9)]
        public D2ItemModifier[] Modifiers;      // 0x088
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0xA)]
        public D2ItemModifier[] SetModifiers;   // 0x118
    }
}
