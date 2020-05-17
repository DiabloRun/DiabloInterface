using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x11)]
    public class D2StringInfo
    {
        [MarshalAs(UnmanagedType.I1)]
        [ExpectOffset(0x00)] public bool IsLoadedUnicode;    // 0x00
        // Rest (0x01-0x10) bytes unknown [size = 0x11]
    }
}
