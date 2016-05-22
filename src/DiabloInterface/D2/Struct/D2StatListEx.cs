using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    struct D2StatListEx
    {
        #region structure (sizeof = 0x64)
        public int pMemPool;            // 0x00
        public int __unknown1;          // 0x04
        public int eOwnerType;          // 0x08
        public int OwnerGUID;           // 0x0C
        public int ListFlags;           // 0x10
        public int __unknown2;          // 0x14
        public int __unknown3;          // 0x18
        public int __unknown4;          // 0x1C
        public int __unknown5;          // 0x20
        public int BaseStats;           // 0x24  rly ?
        public short BaseStatsCount;    // 0x28
        public short __unknown6;        // 0x2A
        public int pLastList;           // 0x2C
        public int __unknown7;          // 0x30
        public int pStatListEx;         // 0x34
        public int pNextListEx;         // 0x38
        public int pMyLastList;         // 0x3C
        public int pMyStats;            // 0x40
        public int pUnit;               // 0x44  list owner
        public int FullStats;           // 0x48  
        public short FullStatsCount;    // 0x4C
        public short __unknown8;        // 0x4E
        public int ModStats;            // 0x50 
        public int __unknown9;          // 0x54
        public int StatFlags;           // 0x58 pointer to an array
        public int fCallback;           // 0x5C
        public int pGame;               // 0x60
        #endregion
    }
}
