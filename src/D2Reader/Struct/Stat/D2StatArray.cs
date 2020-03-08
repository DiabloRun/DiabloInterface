using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    [StructLayout(LayoutKind.Sequential)]
    public class D2StatArray
    {
        [ExpectOffset(0x00)] public DataPointer Address;
        [ExpectOffset(0x04)] public ushort Length;
        [ExpectOffset(0x06)] public ushort ByteSize;

        // StatCostClassId => looks like statcost is in high bits and class is in low bits
        // example: 
        // 00AC0000
        // 00AC => statcost idx
        // 0000 => class id
        public static int GetArrayIndexByStatCostClassId(IProcessMemoryReader r, IntPtr addr, int StatCostClassId)
        {
            D2StatArray statArray = r.Read<D2StatArray>(addr);
            ushort statArrayLen = statArray.Length;
            if (statArrayLen < 1)
                return -1;

            for ( int i = statArrayLen-1; i>=0; i-- )
            {
                // read the itemstatcost 
                // 
                int value = r.ReadInt32(statArray.Address.Address+i*8);
                if ( value == StatCostClassId)
                {
                    return i;
                }
            }
            

            return -1;
        }
    }
}
