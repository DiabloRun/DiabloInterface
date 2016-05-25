using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x22)]
    public class D2LowQualityItemDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string Index;            // 0x00
        public ushort StringIdentifier; // 0x20
    }
}
