using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2Inventory
    {
        #region structure (sizeof = 0x40)
        public int dwInvStamp;      // 0x00  always 0x1020304
        public int pMemPool;        // 0x04
        public int pOwnerUnit;      // 0x08
        public DataPointer pFirstItem;      // 0x0C
        public DataPointer pLastItem;       // 0x10
        public int pInvInfo;        // 0x14
        public int nInvInfo;        // 0x18
        public int WeaponGUID;      // 0x1C
        public int pInvOwnerItem;   // 0x20
        public int OwnerGUID;       // 0x24
        public int nFilledSockets;  // 0x28
        public int __unknown2;      // 0x2C
        public int __unknown3;      // 0x30
        public int pFirstCorpse;    // 0x34
        public int __unknown4;      // 0x38
        public int NextCorpseGUID;  // 0x3C
        #endregion
    }
}
