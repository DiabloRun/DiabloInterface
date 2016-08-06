using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x22)]
    public class D2LowQualityItemDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x00)] public string Index;            // 0x00
        [ExpectOffset(0x20)] public ushort StringIdentifier; // 0x20
    }
}
