using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2SafeArray
    {
        [ExpectOffset(0x00)]public uint Length;
        [ExpectOffset(0x04)]public DataPointer Memory;
    }
}
