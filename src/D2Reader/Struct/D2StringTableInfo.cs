using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x15)]
    public class D2StringTableInfo
    {
        [ExpectOffset(0x00)] public ushort __unknown1;       // 0x00
        [ExpectOffset(0x02)] public ushort IdentifierCount;  // 0x02
        [ExpectOffset(0x04)] public uint AddressTableSize;   // 0x04
        [ExpectOffset(0x08)] public byte __unknown2;         // 0x08
        [ExpectOffset(0x09)] public uint __unknown3;         // 0x09
        [ExpectOffset(0x0D)] public uint __unknown4;         // 0x0D
        [ExpectOffset(0x11)] public int DataBlockSize;       // 0x11
    }
}
