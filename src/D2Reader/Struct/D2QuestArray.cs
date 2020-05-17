using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    struct D2QuestArray
    {
#pragma warning disable 0649
        [ExpectOffset(0x00)] public DataPointer Buffer;
        [ExpectOffset(0x04)] public int Length;
#pragma warning restore 0649
    }
}
