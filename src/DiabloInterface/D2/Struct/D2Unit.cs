using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2Unit
    {
        #region structure (sizeof = 0xF4)
        public D2UnitType eType;    // 0x00
        public int eClass;          // 0x04
        public int pMempool;        // 0x08
        public int GUID;            // 0x0C
        public int eMode;           // 0x10
        public DataPointer UnitData; // 0x14
        public byte actNo;          // 0x18
        public byte __unknown1;     // 0x19
        public byte __unknown2;     // 0x1A
        public byte __unknown3;     // 0x1B
        public int pDrlgAct;        // 0x1C
        public int loSeed;          // 0x20
        public int hiSeed;          // 0x24
        public int dwInitSeed;      // 0x28
        public int pPath;           // 0x2C
        public int pSeqMode;        // 0x30
        public int nSeqFrameCount;  // 0x34
        public int nSeqFrame;       // 0x38
        public int AnimSpeed;       // 0x3C  32 bit .. ?
        public int eSeqMode;        // 0x40
        public int CurrentFrame;    // 0x44
        public int FrameCount;      // 0x48
        public short AnimSpeed2;    // 0x4C
        public byte bActionFrame;   // 0x4E
        public byte __unknown4;     // 0x4F
        public int pAnimData;       // 0x50
        public int pGfxData;        // 0x54
        public int pGfxData2;       // 0x58 another copy of pGfxData
        public DataPointer StatListNode; // 0x5C
        public DataPointer pInventory; // 0x60
        // clientside
        public int pLightMap;           // 0x64
        public int dwStartLightRadius;  // 0x68
        public short nPl2ShiftIndex;    // 0x6C
        // end clientside
        public short UpdateType;    // 0x6E
        public int pUpdateUnit;     // 0x70
        public int pQuestRecord;    // 0x74
        public int bSparkyChest;    // 0x78
        public int pTimerArgs;      // 0x7C
        // clientside
        public int dwSoundSync;         // 0x80 used by summons and ambient stuff
        // end clientside
        public int __unknown5;      // 0x84
        public int __unknown6;      // 0x8C
        public int pEvent;          // 0x90 this is a queue of events to execute (chance to cast skills for example)
        public int eOwnerType;      // 0x94 unit type of missile or minion owner (also used by portals)
        public int OwnerGUID;       // 0x98 is this a 12length string?? global unique identifier of minion or missile owner (also used by portals)
        public int __unknown7;      // 0x9C
        public int __unknown8;      // 0xA0
        public int pHoverText;      // 0xA4 hovering text controller (such as the shrine message)
        public int pSkills;         // 0xA8 controller holding a list of all skills the unit has (pointers to pSkill)
        public int pCombat;         // 0xAC
        public int dwHitClass;      // 0xB0
        public int __unknown9;      // 0xB4
        public int DropCode;        // 0xB8
        public int __unknown10;     // 0xBC
        public int __unknown11;     // 0xC0
        public long UnitFlags;      // 0xC4
        public int __unknown12;     // 0xD0
        public int GetTickCount;    // 0xD4
        // clientside
        public int pParticleStream;     // 0xD8
        // end clientside
        public int pTimer;          // 0xDC
        public int __unknown13;     // 0xE0
        public int pPrevUnit;       // 0xE4
        public int pPrevUnitInRoom; // 0xE8
        public int pMsgFirst;       // 0xEC
        public int pMsgLast;        // 0xF0
        #endregion
    }
}
