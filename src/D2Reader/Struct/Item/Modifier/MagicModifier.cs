using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item.Modifier
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x90)]
    public class MagicModifier
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x00)] public string ModifierName;             // 0x00
        [ExpectOffset(0x20)] public ushort ModifierNameHash;         // 0x20
                                                // Rest are unknown for now...
    }
}
