using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x15)]
    public class D2StringTableInfo
    {
        public ushort __unknown1;       // 0x00
        public ushort IdentifierCount;  // 0x02
        public uint AddressTableSize;   // 0x04
        public byte __unknown2;         // 0x08
        public uint __unknown3;         // 0x09
        public uint __unknown4;         // 0x0D
        public int DataBlockSize;       // 0x11
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x11)]
    public class D2StringInfo
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool IsLoadedUnicode;    // 0x00
        // Rest (0x01-0x10) bytes unknown [size = 0x11]
    }
}
