using Zutatensuppe.D2Reader.Struct.Item.Modifier;
using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x128)]
    public class D2SetDescription
    {
        [ExpectOffset(0x000)] public UInt16 TableIndex;                   // 0x000
        [ExpectOffset(0x002)] public UInt16 StringIdentifier;             // 0x002
        [ExpectOffset(0x004)] public UInt32 Version;                      // 0x004
        [ExpectOffset(0x008)] public UInt32 __Unknown1;                   // 0x008
        [ExpectOffset(0x00C)] public UInt32 SetItemCount;                 // 0x00C
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x8)]
        [ExpectOffset(0x010)] public D2ItemModifier[] PartialModifiers;   // 0x010
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x8)]
        [ExpectOffset(0x090)] public D2ItemModifier[] FullModifiers;      // 0x090
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 0x6)]
        [ExpectOffset(0x110)] public DataPointer[] SetItems;              // 0x110
    }
}
