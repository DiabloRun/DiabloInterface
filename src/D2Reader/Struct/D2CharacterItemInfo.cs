using System;
using System.Runtime.InteropServices;
using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterItemInfo
    {
        [ExpectOffset(0x00)] public UInt32 Item;
        [ExpectOffset(0x04)] public UInt8 Location;
        [ExpectOffset(0x05)] public UInt8 Count;
        [ExpectOffset(0x06)] public UInt8 __Unknown1;
        [ExpectOffset(0x07)] public UInt8 __Unknown2;
    }
}
