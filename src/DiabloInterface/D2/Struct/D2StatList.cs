using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public class D2StatList
    {
        public int pMemPool;                // 0x00
        public int pUnit;                   // 0x04
        public int eOwnerType;              // 0x08
        public int OwnerGUID;               // 0x0C
        public StatListFlag Flags;          // 0x10
        public uint State;                  // 0x14
        public int ExpireFrame;             // 0x18
        public int skillNo;                 // 0x1C
        public int sLvl;                    // 0x20
        public D2StatArray Stats;           // 0x24
        public DataPointer PreviousList;    // 0x2C
        public DataPointer NextList;        // 0x30
        public int pPrevious;               // 0x34
        public int fpStatExpires;           // 0x38
        public DataPointer Next;            // 0x3C
    }
}
