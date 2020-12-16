using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1A8)]
    public class D2ClassDescription
    {
        [FieldOffset(0x018)] public short __unknown_index_18; //

        [FieldOffset(0x0A8)] public short __unknown_A8; //
    }
}
