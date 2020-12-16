using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    public enum D2UnitType : uint
    {
        Player, // 0
        Monster, // 1
        Object, // 2
        Missile, // 3
        Item, // 4
        VisTile // 5
    }

    // @see https://d2mods.info/forum/viewtopic.php?f=8&t=62973&p=487011&hilit=pets#p487011
    [Flags]
    public enum D2UnitFlags_C4 : uint
    {
        UPDATE_REQUIRED = 0x00000001, // - tells the game to update the unit ( set after operateFn for objects, when reinitializing a unit etc )
        TARGETABLE = 0x00000002, // - whenever the unit can be selected as a target
        ATTACKABLE = 0x00000004, // - whenever the unit can be attacked
        VALID_TARGET = 0x00000008, // - used to check if the unit is a valid target, curses use this
        UNIT_SEED_INITIALIZED = 0x00000010, // - whenever the unit seed has been initialized
        DRAW_SHADOW = 0x00000020, // - whenever to draw a shadow or not ( client only )
        SKILL_DO_EXECUTED = 0x00000040, // - whenever the SkillDo func has executed for the active skill
        UNKNOWN_80 = 0x00000080, // - saw this used only with objects so far, when set the pre_operate is disabled
        HAS_TEXT_MESSAGE = 0x00000100, // - whenever the unit has a text message attached to it ( do not set this randomly )
        IS_HIRELING = 0x00000200, // - if this is set the unit will be treated as a hireling for certain things like lifebar display (but also for gameplay relevant aspects)
        HAS_SOUND = 0x00000400, // - whenever the unit has a event sound specified ( server-side, do not set this randomly )
        SUMMONER_FLAG = 0x00000800, // - this is only used for the summoner as far as I can tell, don't know what exactly for.
        REQUIRE_REFRESH_MSG = 0x00001000, // - used by items to send a refresh message when they drop to the ground (etc)
        IS_LINKED_TO_MSG_CHAIN = 0x00002000, // - whenever the unit is linked into a update message chain ( don't set this randomly )
        NEW_GFX_REQUIRED = 0x00004000, // - whenever to load new graphics when using a skill sequence(ie the current sequence frame uses a different animation mode then the previous one).
        LIFE_UPDATE_REQUIRED = 0x00008000, // - updates the client with the most recent life percentage and hitclass(used mostly by softhit attacks)
        IS_DEAD = 0x00010000, // - the unit is dead
        TREASURECLASS_DROP_DISABLED = 0x00020000, // - disables treasureclass drops
        MON_MODE_CHANGED = 0x00080000, // - this is set when the MonMode changes, didn't see exact use yet
        PREDRAW_UNIT = 0x00100000, // - whenever to predraw this unit (ie treat it as a floor tile for display purposes, client specific )
        IS_ASYNC_UNIT = 0x00200000, // - whenever this unit is an async unit ( exists only clientside like critters )
        IS_CLIENT_UNIT = 0x00400000, // - whenever this unit is a client unit
        INITIALIZED = 0x01000000, // - set once the unit has been initialized, didn't check specifics
        RESURRECTED_OR_FLOOR_ITEM = 0x02000000, // - set for resurrected units and items on the floor
        NO_XP = 0x04000000, // - never gives experience when slain
        AUTOMAP_FLAG1 = 0x10000000, // - automap related, not documented here
        AUTOMAP_FLAG2 = 0x20000000, // - ditto
        PET_AI_SHOULD_IGNORE = 0x40000000, // - units that pet ais should ignore
        IS_REVIVED = 0x80000000, // - this is a revived monster
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xF4)]
    public class D2Unit
    {
        #region structure (sizeof = 0xF4)
        [ExpectOffset(0x0000)] public D2UnitType eType;    // 0x00
        [ExpectOffset(0x0004)] public int eClass;          // 0x04 // aka dwTxtFileNo or dwTextFileNo
        [ExpectOffset(0x0008)] public int pMempool;        // 0x08
        [ExpectOffset(0x000C)] public int GUID;            // 0x0C // aka dwUnitId
        [ExpectOffset(0x0010)] public int eMode;           // 0x10 // aka dwMode
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
        [ExpectOffset(0x00C4)] public D2UnitFlags_C4 UnitFlags_C4;      // 0xC4
        [ExpectOffset(0x00C8)] public uint UnitFlags_C8;      // 0xC8
        [ExpectOffset(0x00CC)] public int __unknown11c;    // 0xCC
        [ExpectOffset(0x00D0)] public int __unknown12;     // 0xD0
        [ExpectOffset(0x00D4)] public int GetTickCount;    // 0xD4
        // clientside
        [ExpectOffset(0x00D8)] public int pParticleStream;     // 0xD8
        // end clientside
        [ExpectOffset(0x00DC)] public int pTimer;          // 0xDC
        [ExpectOffset(0x00E0)] public int __unknown13;     // 0xE0
        [ExpectOffset(0x00E4)] public DataPointer pPrevUnit;       // 0xE4
        [ExpectOffset(0x00E8)] public int pPrevUnitInRoom; // 0xE8
        [ExpectOffset(0x00EC)] public int pMsgFirst;       // 0xEC
        [ExpectOffset(0x00F0)] public int pMsgLast;        // 0xF0
        #endregion

        public bool IsCharm()
        {
            return eClass == 603 || eClass == 604 || eClass == 605;
        }

        public bool IsItem()
        {
            return eType == D2UnitType.Item;
        }

        public bool IsMonster()
        {
            return eType == D2UnitType.Monster;
        }
    }
}
