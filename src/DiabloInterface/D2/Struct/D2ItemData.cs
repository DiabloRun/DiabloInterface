using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface.D2.Struct
{
    struct D2ItemData
    {
        #region structure (sizeof = 0x74)
        public int qualityNo;           // 0x00
        public int LoSeed;              // 0x04
        public int HiSeed;              // 0x08
        public int OwnerGUID;           // 0x0C 
        public int FingerPrint;         // 0x10
        public int CommandFlags;        // 0x14
        public int ItemFlags;           // 0x18
        public int __unknown1;          // 0x1C
        public int __unknown2;          // 0x20
        public int ActionStamp;         // 0x24
        public int FileIndex;           // 0x28
        public int iLvl;                // 0x2C
        public short ItemFormat;        // 0x30
        public short RarePrefix;        // 0x32
        public short RareSuffix;        // 0x34
        public short AutoPrefix;        // 0x36
        public short MagicPrefix1;      // 0x38
        public short MagicPrefix2;      // 0x3A
        public short MagicPrefix3;      // 0x3C
        public short MagicSuffix1;      // 0x3E
        public short MagicSuffix2;      // 0x40
        public short MagicSuffix3;      // 0x42
        public byte BodyLoc;            // 0x44
        public byte InvPage;            // 0x45
        public short __unknown3;        // 0x46
        public byte EarLevel;           // 0x48
        public byte InvGfxIdx;          // 0x49
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szPlayerName;     // 0x4A
        public short __unknown4;        // 0x5A
        public int pNodeOwnerInventory; // 0x5C
        public int pPrevItem;           // 0x60
        public int pNextItem;           // 0x64
        public byte nNodePosition;      // 0x68
        public byte nNodePositionOther; // 0x69
        public short __unknown6;        // 0x6A
        public int __unknown7;          // 0x6C
        public int __unknown8;          // 0x70
        #endregion
    }
}
