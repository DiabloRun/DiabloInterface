using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item.Modifier
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x48)]
    public class RareModifier
    {
        [ExpectOffset(0x00)] public int __unknown1;          // 0x00
        [ExpectOffset(0x04)] public int __unknown2;          // 0x04
        [ExpectOffset(0x08)] public int __unknown3;          // 0x08
        [ExpectOffset(0x0C)] public ushort ModifierNameHash; // 0x0C
    }
}
