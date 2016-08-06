using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item.Modifier
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ModifierTable
    {
        [ExpectOffset(0x00)] public uint Length;         // 0x00
        [ExpectOffset(0x04)] public DataPointer Memory;  // 0x04
    }
}
