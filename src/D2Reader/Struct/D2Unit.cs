using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    public enum D2UnitType : uint
    {
        Player,
        Monster,
        Object,
        Missile,
        Item,
        VisTile
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xF4)]
    public class D2Unit
    {
        #region structure (sizeof = 0xF4)
        [ExpectOffset(0x0000)] public D2UnitType eType;    // 0x00
        [ExpectOffset(0x0004)] public int eClass;          // 0x04
        [ExpectOffset(0x0008)] public int pMempool;        // 0x08
        [ExpectOffset(0x000C)] public int GUID;            // 0x0C
        [ExpectOffset(0x0010)] public int eMode;           // 0x10
        [ExpectOffset(0x0014)] public DataPointer UnitData; // 0x14
        [ExpectOffset(0x0018)] public byte actNo;          // 0x18
        [ExpectOffset(0x0019)] public byte __unknown1;     // 0x19
        [ExpectOffset(0x001A)] public byte __unknown2;     // 0x1A
        [ExpectOffset(0x001B)] public byte __unknown3;     // 0x1B
        [ExpectOffset(0x001C)] public int pDrlgAct;        // 0x1C
        [ExpectOffset(0x0020)] public int loSeed;          // 0x20
        [ExpectOffset(0x0024)] public int hiSeed;          // 0x24
        [ExpectOffset(0x0028)] public int dwInitSeed;      // 0x28
        [ExpectOffset(0x002C)] public int pPath;           // 0x2C
        [ExpectOffset(0x0030)] public int pSeqMode;        // 0x30
        [ExpectOffset(0x0034)] public int nSeqFrameCount;  // 0x34
        [ExpectOffset(0x0038)] public int nSeqFrame;       // 0x38
        [ExpectOffset(0x003C)] public int AnimSpeed;       // 0x3C  32 bit .. ?
        [ExpectOffset(0x0040)] public int eSeqMode;        // 0x40
        [ExpectOffset(0x0044)] public int CurrentFrame;    // 0x44
        [ExpectOffset(0x0048)] public int FrameCount;      // 0x48
        [ExpectOffset(0x004C)] public short AnimSpeed2;    // 0x4C
        [ExpectOffset(0x004E)] public byte bActionFrame;   // 0x4E
        [ExpectOffset(0x004F)] public byte __unknown4;     // 0x4F
        [ExpectOffset(0x0050)] public int pAnimData;       // 0x50
        [ExpectOffset(0x0054)] public int pGfxData;        // 0x54
        [ExpectOffset(0x0058)] public int pGfxData2;       // 0x58 another copy of pGfxData
        [ExpectOffset(0x005C)] public DataPointer StatListNode; /*D2StatListEx*/
        [ExpectOffset(0x0060)] public DataPointer pInventory;
        // clientside
        [ExpectOffset(0x0064)] public DataPointer pLightMap;   // 0x64
        [ExpectOffset(0x0068)] public int dwStartLightRadius;  // 0x68
        [ExpectOffset(0x006C)] public short nPl2ShiftIndex;    // 0x6C
        // end clientside
        [ExpectOffset(0x006E)] public short UpdateType;    // 0x6E
        [ExpectOffset(0x0070)] public int pUpdateUnit;     // 0x70
        [ExpectOffset(0x0074)] public int pQuestRecord;    // 0x74
        [ExpectOffset(0x0078)] public int bSparkyChest;    // 0x78
        [ExpectOffset(0x007C)] public int pTimerArgs;      // 0x7C
        // clientside
        [ExpectOffset(0x0080)]  public int dwSoundSync;         // 0x80 used by summons and ambient stuff
        // end clientside
        [ExpectOffset(0x0084)] public int __unknown5;      // 0x84
        [ExpectOffset(0x0088)] public int __unknown5b;     // 0x88
        [ExpectOffset(0x008C)] public int __unknown6;      // 0x8C
        [ExpectOffset(0x0090)] public int pEvent;          // 0x90 this is a queue of events to execute (chance to cast skills for example)
        [ExpectOffset(0x0094)] public int eOwnerType;      // 0x94 unit type of missile or minion owner (also used by portals)
        [ExpectOffset(0x0098)] public int OwnerGUID;       // 0x98 is this a 12length string?? global unique identifier of minion or missile owner (also used by portals)
        [ExpectOffset(0x009C)] public int __unknown7;      // 0x9C
        [ExpectOffset(0x00A0)] public int __unknown8;      // 0xA0
        [ExpectOffset(0x00A4)] public int pHoverText;      // 0xA4 hovering text controller (such as the shrine message)
        [ExpectOffset(0x00A8)] public DataPointer pSkills; // 0xA8 controller holding a list of all skills the unit has (pointers to pSkill)
        [ExpectOffset(0x00AC)] public int pCombat;         // 0xAC
        [ExpectOffset(0x00B0)] public int dwHitClass;      // 0xB0
        [ExpectOffset(0x00B4)] public int __unknown9;      // 0xB4
        [ExpectOffset(0x00B8)] public int DropCode;        // 0xB8
        [ExpectOffset(0x00BC)] public int __unknown10;     // 0xBC
        [ExpectOffset(0x00C0)] public int __unknown11;     // 0xC0
        [ExpectOffset(0x00C4)] public long UnitFlags;      // 0xC4
        // long is 0x08 long...  nothing is missing here !  :)
        [ExpectOffset(0x00CC)] public int __unknown11c;    // 0xCC
        [ExpectOffset(0x00D0)] public int __unknown12;     // 0xD0
        [ExpectOffset(0x00D4)] public int GetTickCount;    // 0xD4
        // clientside
        [ExpectOffset(0x00D8)] public int pParticleStream;     // 0xD8
        // end clientside
        [ExpectOffset(0x00DC)] public int pTimer;          // 0xDC
        [ExpectOffset(0x00E0)] public int __unknown13;     // 0xE0
        [ExpectOffset(0x00E4)] public int pPrevUnit;       // 0xE4
        [ExpectOffset(0x00E8)] public int pPrevUnitInRoom; // 0xE8
        [ExpectOffset(0x00EC)] public int pMsgFirst;       // 0xEC
        [ExpectOffset(0x00F0)] public int pMsgLast;        // 0xF0
        #endregion

        public bool IsCharm()
        {
            return eClass == 603 || eClass == 604 || eClass == 605;
        }
    }
}
