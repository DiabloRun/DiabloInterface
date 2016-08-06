using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct D2GameUnitList
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 128)]
        [ExpectOffset(0x00)] public DataPointer[] Units;

        public DataPointer this[int index]
        {
            get { return Units[index]; }
        }
    }
}
