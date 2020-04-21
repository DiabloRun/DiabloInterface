using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2StatList
    {
        [ExpectOffset(0x0000)] public int pMemPool;                // 0x00
        [ExpectOffset(0x0004)] public int pUnit;                   // 0x04
        [ExpectOffset(0x0008)] public int eOwnerType;              // 0x08
        [ExpectOffset(0x000C)] public int OwnerGUID;               // 0x0C
        [ExpectOffset(0x0010)] public StatListFlag Flags;          // 0x10
        [ExpectOffset(0x0014)] public uint State;                  // 0x14 (values seen: 0x6A)
        [ExpectOffset(0x0018)] public int ExpireFrame;             // 0x18
        [ExpectOffset(0x001C)] public int skillNo;                 // 0x1C
        [ExpectOffset(0x0020)] public int sLvl;                    // 0x20
        [ExpectOffset(0x0024)] public D2StatArray Stats;           // 0x24
        [ExpectOffset(0x002C)] public DataPointer PreviousList;    // 0x2C
        [ExpectOffset(0x0030)] public DataPointer NextList;        // 0x30
        [ExpectOffset(0x0034)] public int pPrevious;               // 0x34
        [ExpectOffset(0x0038)] public int fpStatExpires;           // 0x38
        [ExpectOffset(0x003C)] public DataPointer Next;            // 0x3C
    }
}
