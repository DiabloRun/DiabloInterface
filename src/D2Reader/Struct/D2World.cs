using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Explicit)]
    public class D2World
    {
        [FieldOffset(0x1C)] public DataPointer GameBuffer;
        [FieldOffset(0x24)] public UInt32 GameMask;
    }
}
