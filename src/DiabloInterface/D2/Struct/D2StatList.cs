using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    class D2StatList
    {
        #region structure (sizeof = 0x40)
        public int pMemPool;        // 0x00
        public int pUnit;           // 0x04
        public int eOwnerType;      // 0x08
        public int OwnerGUID;       // 0x0C
        public int ListFlags;       // 0x10
        public int stateNo;         // 0x14
        public int ExpireFrame;     // 0x18
        public int skillNo;         // 0x1C
        public int sLvl;            // 0x20
        public int Stats;           // 0x24
        public short __unknown;     // 0x28 unknown thing (count for Stats?)
        public short __unknown2;    // 0x2A unknown thing (count for Stats?)
        public int pPrevList;       // 0x2C
        public int pNextList;       // 0x30
        public int pPrevious;       // 0x34
        public int fpStatExpires;   // 0x38
        public int pNext;           // 0x3C
        #endregion
    }
}
