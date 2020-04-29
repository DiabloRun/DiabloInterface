using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Item.Modifier
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct D2ItemModifier
    {
        [ExpectOffset(0x00)] public uint Property;
        [ExpectOffset(0x04)] public int Parameter;
        [ExpectOffset(0x08)] public int MinimumValue;
        [ExpectOffset(0x0C)] public int MaximumValue;
    }
}
