using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataPointer
    {
        uint address;

        public bool IsNull { get { return address == 0; } }
        public IntPtr Address { get { return new IntPtr(address); } }

        public DataPointer(uint address)
        {
            this.address = address;
        }

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

        public static implicit operator IntPtr(DataPointer instance)
        {
            return instance.Address;
        }

        public static DataPointer operator +(DataPointer pointer, int offset)
        {
            return new DataPointer((uint)(pointer.address + offset));
        }

        public static DataPointer operator +(DataPointer pointer, uint offset)
        {
            return new DataPointer(pointer.address + offset);
        }
    }
}
