using System;
using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataPointer
    {
        uint address;

        public bool IsNull { get { return address == 0; } }
        public IntPtr Address { get { return new IntPtr(address); } }

        public int ToInt32()
        {
            return (int)address;
        }

        public uint ToUInt32()
        {
            return address;
        }

        public override string ToString()
        {
            return address.ToString("X8");
        }
    }
}
