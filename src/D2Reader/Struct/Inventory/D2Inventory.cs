using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Inventory
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2Inventory
    {
        #region structure (sizeof = 0x40)
        [ExpectOffset(0x00)] public int dwInvStamp;      // 0x00 always 0x1020304
        [ExpectOffset(0x04)] public int pMemPool;        // 0x04
        [ExpectOffset(0x08)] public int pOwnerUnit;      // 0x08
        [ExpectOffset(0x0C)] public DataPointer pFirstItem;      // 0x0C
        [ExpectOffset(0x10)] public DataPointer pLastItem;       // 0x10
        [ExpectOffset(0x14)] public DataPointer pInvInfo;        // 0x14
        [ExpectOffset(0x18)] public int nInvInfo;        // 0x18
        [ExpectOffset(0x1C)] public int WeaponGUID;      // 0x1C
        [ExpectOffset(0x20)] public int pInvOwnerItem;   // 0x20
        [ExpectOffset(0x24)] public int OwnerGUID;       // 0x24
        [ExpectOffset(0x28)] public int nFilledSockets;  // 0x28
        [ExpectOffset(0x2C)] public int __unknown2;      // 0x2C
        [ExpectOffset(0x30)] public int __unknown3;      // 0x30
        [ExpectOffset(0x34)] public int pFirstCorpse;    // 0x34
        [ExpectOffset(0x38)] public int __unknown4;      // 0x38
        [ExpectOffset(0x3C)] public int NextCorpseGUID;  // 0x3C
        #endregion
    }
}
