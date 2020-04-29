using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    [StructLayout(LayoutKind.Sequential)]
    public class D2Stat
    {
        [ExpectOffset(0x00)] public ushort HiStatID;
        [ExpectOffset(0x02)] public ushort LoStatID;
        [ExpectOffset(0x04)] public int Value;

        public D2Stat()
        {
        }

        public D2Stat(D2Stat other)
        {
            LoStatID = other.LoStatID;
            HiStatID = other.HiStatID;
            Value = other.Value;
        }

        public bool IsOfType(StatIdentifier id)
        {
            return LoStatID == (ushort)id;
        }

        public bool HasValidLoStatIdentifier()
        {
            return Enum.IsDefined(typeof(StatIdentifier), LoStatID);
        }
    }
}
