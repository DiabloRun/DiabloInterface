
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    [StructLayout(LayoutKind.Sequential)]
    public class D2Stat
    {
        [ExpectOffset(0x00)] public ushort HiStatID;
        [ExpectOffset(0x02)] public ushort LoStatID;
        [ExpectOffset(0x04)] public int Value;

        public bool IsOfType(StatIdentifier id)
        {
            return LoStatID == (ushort)id;
        }
    }
}
