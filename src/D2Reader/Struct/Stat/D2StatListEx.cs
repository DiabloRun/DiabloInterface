using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    [Flags]
    public enum StatListFlag : uint
    {
        None             = 0,
        HasProperties    = 0x40,
        HasCompleteStats = 0x80000000
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2StatListEx
    {
        #region structure (sizeof = 0x64)
        [ExpectOffset(0x0000)] public int pMemPool;            // 0x00
        [ExpectOffset(0x0004)] public int __unknown1;          // 0x04
        [ExpectOffset(0x0008)] public int eOwnerType;          // 0x08
        // OwnerGUID:
        // see FN game.625B70
        // highest 3 bits of skills PassiveState are the index in this array
        // lowest 5 bits are index in the global array at 0x6CE268
        // values of both arrays are combined with AND to get some value for passive stats calculation
        [ExpectOffset(0x000C)] public int OwnerGUID;           // 0x0C

        [ExpectOffset(0x0010)] public StatListFlag ListFlags;  // 0x10
        [ExpectOffset(0x0014)] public uint NodeType;           // 0x14
        [ExpectOffset(0x0018)] public int __unknown3;          // 0x18
        [ExpectOffset(0x001C)] public int __unknown4;          // 0x1C
        [ExpectOffset(0x0020)] public int __unknown5;          // 0x20
        // d2statArray is 2 int long
        [ExpectOffset(0x0024)] public D2StatArray BaseStats;   // 0x24
        [ExpectOffset(0x002C)] public DataPointer pLastList;   // 0x2C ptr to D2StatList
        [ExpectOffset(0x0030)] public int __unknown7;          // 0x30
        [ExpectOffset(0x0034)] public int pStatListEx;         // 0x34
        [ExpectOffset(0x0038)] public int pNextListEx;         // 0x38
        [ExpectOffset(0x003C)] public DataPointer pMyLastList; // 0x3C ptr to D2StatList
        [ExpectOffset(0x0040)] public DataPointer pMyStats;    // 0x40 ptr to D2StatList
        [ExpectOffset(0x0044)] public DataPointer pUnit;       // 0x44 list owner /*D2Unit*/
        // d2statArray is 2 int long
        [ExpectOffset(0x0048)] public D2StatArray FullStats;   // 0x48
        [ExpectOffset(0x0050)] public int ModStats;            // 0x50
        [ExpectOffset(0x0054)] public int __unknown9;          // 0x54
        [ExpectOffset(0x0058)] public DataPointer StatFlags;   // 0x58 pointer to an array of size 8. 

        [ExpectOffset(0x005C)] public int fCallback;           // 0x5C
        [ExpectOffset(0x0060)] public int pGame;               // 0x60
        #endregion

        internal D2StatArray BestStatsArray()
        {
            if (ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                return FullStats;
            return BaseStats;
        }
    }
}
