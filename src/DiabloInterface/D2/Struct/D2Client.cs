using System;
using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Explicit)]
    public class D2Client
    {
        [FieldOffset(0x16C)] public UInt32 UnitType;
        [FieldOffset(0x170)] public Int32  UnitId;
    }
}
