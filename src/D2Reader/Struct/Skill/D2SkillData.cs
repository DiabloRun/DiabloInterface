using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct.Skill
{
    enum PetType
    {
        unknown0 = 0,
        unknown1 = 1,
        unknown2 = 2,
        unknown3 = 3,
        SKELETON_WARRIOR = 4,
        unknown5 = 5,
        unknown6 = 6,
        unknown7 = 7,
        unknown8 = 8,
        unknown9 = 9,
        unknown10 = 10,
        unknown11 = 11,
        unknown12 = 12,
        unknown13 = 13,
        unknown14 = 14,
        unknown15 = 15,
        unknown16 = 16,
        TRAP_WAKE_OF_FIRE = 17, // 0x11
    }

    [Flags]
    public enum SkillFlags : ulong
    {
        None                = 0,
        Decquant            = (1UL << 0),
        Lob                 = (1UL << 1),
        Progressive         = (1UL << 2),
        Finishing           = (1UL << 3),
        Passive             = (1UL << 4),
        Aura                = (1UL << 5),
        Periodic            = (1UL << 6),
        PrgStack            = (1UL << 7),
        InTown              = (1UL << 8),
        Kick                = (1UL << 9),
        InGame              = (1UL << 10),
        Repeat              = (1UL << 11),
        StSuccessOnly       = (1UL << 12),
        StSoundDelay        = (1UL << 13),
        WeaponSnd           = (1UL << 14),
        Immediate           = (1UL << 15),
        NoAmmo              = (1UL << 16),
        Enhanceable         = (1UL << 17),
        Durability          = (1UL << 18),
        UseAttackRating     = (1UL << 19),
        TargetableOnly      = (1UL << 20),
        SearchEnemyXY       = (1UL << 21),
        SearchEnemyNear     = (1UL << 22),
        SearchOpenXY        = (1UL << 23),
        TargetCorpse        = (1UL << 24),
        TargetPet           = (1UL << 25),
        TargetAlly          = (1UL << 26),
        TargetItem          = (1UL << 27),
        AttackNoMana        = (1UL << 28),
        ItemTgtDo           = (1UL << 29),
        LeftSkill           = (1UL << 30),
        Interrupt           = (1UL << 31),
        TgtPlaceCheck       = (1UL << 32),
        ItemCheckStart      = (1UL << 33),
        ItemCltCheckStart   = (1UL << 34),
        General             = (1UL << 35),
        Scroll              = (1UL << 36),
        UseManaOnDo         = (1UL << 37),
        Warp                = (1UL << 38),
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x23C)]
    public class D2SkillData
    {
        [ExpectOffset(0x000)] public UInt32 SkillId;              // 0x000
        [ExpectOffset(0x004)] public SkillFlags Flags;            // 0x004
        [ExpectOffset(0x00C)] public UInt32 ClassId;              // 0x00C
        [ExpectOffset(0x010)] public UInt8  Anim;                 // 0x010
        [ExpectOffset(0x011)] public UInt8  MonAnim;              // 0x011
        [ExpectOffset(0x012)] public UInt8  SeqTrans;             // 0x012
        [ExpectOffset(0x013)] public UInt8  SeqNum;               // 0x013
        [ExpectOffset(0x014)] public UInt8  Range;                // 0x014
        [ExpectOffset(0x015)] public UInt8  SelectProc;           // 0x015
        [ExpectOffset(0x016)] public UInt8  SeqInput;             // 0x016
        [ExpectOffset(0x017)] public UInt8  __Padding1;           // 0x017
        [ExpectOffset(0x018)] public UInt16 ITypeA1;              // 0x018
        [ExpectOffset(0x01A)] public UInt16 ITypeA2;              // 0x01A
        [ExpectOffset(0x01C)] public UInt16 ITypeA3;              // 0x01C
        [ExpectOffset(0x01E)] public UInt16 ITypeB1;              // 0x01E
        [ExpectOffset(0x020)] public UInt16 ITypeB2;              // 0x020
        [ExpectOffset(0x022)] public UInt16 ITypeB3;              // 0x022
        [ExpectOffset(0x024)] public UInt16 ETypeA1;              // 0x024
        [ExpectOffset(0x026)] public UInt16 ETypeA2;              // 0x026
        [ExpectOffset(0x028)] public UInt16 ETypeB1;              // 0x028
        [ExpectOffset(0x02A)] public UInt16 ETypeB2;              // 0x02A
        [ExpectOffset(0x02C)] public UInt16 SrvStartFunc;         // 0x02C
        [ExpectOffset(0x02E)] public UInt16 SrvDoFunc;            // 0x02E
        [ExpectOffset(0x030)] public UInt16 SrvPrgFunc1;          // 0x030
        [ExpectOffset(0x032)] public UInt16 SrvPrgFunc2;          // 0x032
        [ExpectOffset(0x034)] public UInt16 SrvPrgFunc3;          // 0x034
        [ExpectOffset(0x036)] public UInt16 __Padding2;           // 0x036
        [ExpectOffset(0x038)] public UInt32 PrgCalc1;             // 0x038
        [ExpectOffset(0x03C)] public UInt32 PrgCalc2;             // 0x03C
        [ExpectOffset(0x040)] public UInt32 PrgCalc3;             // 0x040
        [ExpectOffset(0x044)] public UInt16 PrgDamage;            // 0x044
        [ExpectOffset(0x046)] public UInt16 SrvMissile;           // 0x046
        [ExpectOffset(0x048)] public UInt16 SrvMissileA;          // 0x048
        [ExpectOffset(0x04A)] public UInt16 SrvMissileB;          // 0x04A
        [ExpectOffset(0x04C)] public UInt16 SrvMissileC;          // 0x04C
        [ExpectOffset(0x04E)] public UInt16 SrvOverlay;           // 0x04E
        [ExpectOffset(0x050)] public UInt32 AuraFilter;           // 0x050
        [ExpectOffset(0x054)] public UInt16 AuraStat1;            // 0x054
        [ExpectOffset(0x056)] public UInt16 AuraStat2;            // 0x056
        [ExpectOffset(0x058)] public UInt16 AuraStat3;            // 0x058
        [ExpectOffset(0x05A)] public UInt16 AuraStat4;            // 0x05A
        [ExpectOffset(0x05C)] public UInt16 AuraStat5;            // 0x05C
        [ExpectOffset(0x05E)] public UInt16 AuraStat6;            // 0x05E
        [ExpectOffset(0x060)] public UInt32 AuraLenCalc;          // 0x060
        [ExpectOffset(0x064)] public UInt32 AuraRangeCalc;        // 0x064
        [ExpectOffset(0x068)] public UInt32 AuraStatCalc1;        // 0x068 ID
        [ExpectOffset(0x06C)] public UInt32 AuraStatCalc2;        // 0x06C ID 
        [ExpectOffset(0x070)] public UInt32 AuraStatCalc3;        // 0x070 ID 
        [ExpectOffset(0x074)] public UInt32 AuraStatCalc4;        // 0x074 ID 
        [ExpectOffset(0x078)] public UInt32 AuraStatCalc5;        // 0x078 ID 
        [ExpectOffset(0x07C)] public UInt32 AuraStatCalc6;        // 0x07C ID 
        [ExpectOffset(0x080)] public UInt16 AuraState;            // 0x080
        [ExpectOffset(0x082)] public UInt16 AuraTargetState;      // 0x082
        [ExpectOffset(0x084)] public UInt16 AuraEvent1;           // 0x084
        [ExpectOffset(0x086)] public UInt16 AuraEvent2;           // 0x086
        [ExpectOffset(0x088)] public UInt16 AuraEvent3;           // 0x088
        [ExpectOffset(0x08A)] public UInt16 AuraEventFunc1;       // 0x08A
        [ExpectOffset(0x08C)] public UInt16 AuraEventFunc2;       // 0x08C
        [ExpectOffset(0x08E)] public UInt16 AuraEventFunc3;       // 0x08E
        [ExpectOffset(0x090)] public UInt16 AuraTgtEvent;         // 0x090
        [ExpectOffset(0x092)] public UInt16 AuraTgtEventFunc;     // 0x092
        [ExpectOffset(0x094)] public UInt16 PassiveState;         // 0x094   ....
        [ExpectOffset(0x096)] public UInt16 PassiveiType;         // 0x096
        [ExpectOffset(0x098)] public UInt16 PassiveStat1;         // 0x098
        [ExpectOffset(0x09A)] public UInt16 PassiveStat2;         // 0x09A
        [ExpectOffset(0x09C)] public UInt16 PassiveStat3;         // 0x09C
        [ExpectOffset(0x09E)] public UInt16 PassiveStat4;         // 0x09E
        [ExpectOffset(0x0A0)] public UInt16 PassiveStat5;         // 0x0A0
        [ExpectOffset(0x0A2)] public UInt16 __Padding3;           // 0x0A2
        [ExpectOffset(0x0A4)] public UInt32 PassiveCalc1;         // 0x0A4
        [ExpectOffset(0x0A8)] public UInt32 PassiveCalc2;         // 0x0A8
        [ExpectOffset(0x0AC)] public UInt32 PassiveCalc3;         // 0x0AC
        [ExpectOffset(0x0B0)] public UInt32 PassiveCalc4;         // 0x0B0
        [ExpectOffset(0x0B4)] public UInt32 PassiveCalc5;         // 0x0B4
        [ExpectOffset(0x0B8)] public UInt16 PassiveEvent;         // 0x0B8
        [ExpectOffset(0x0BA)] public UInt16 PassiveEventFunc;     // 0x0BA
        [ExpectOffset(0x0BC)] public UInt16 Summon;               // 0x0BC
        [ExpectOffset(0x0BE)] public UInt8  PetType;              // 0x0BE  => see enum PetType
        [ExpectOffset(0x0BF)] public UInt8  SumMode;              // 0x0BF
        [ExpectOffset(0x0C0)] public UInt32 PetMax;               // 0x0C0
        [ExpectOffset(0x0C4)] public UInt16 SumSkill1;            // 0x0C4
        [ExpectOffset(0x0C6)] public UInt16 SumSkill2;            // 0x0C6
        [ExpectOffset(0x0C8)] public UInt16 SumSkill3;            // 0x0C8
        [ExpectOffset(0x0CA)] public UInt16 SumSkill4;            // 0x0CA
        [ExpectOffset(0x0CC)] public UInt16 SumSkill5;            // 0x0CC
        [ExpectOffset(0x0CE)] public UInt16 __Padding4;           // 0x0CE
        [ExpectOffset(0x0D0)] public UInt32 SumSkCalc1;           // 0x0D0
        [ExpectOffset(0x0D4)] public UInt32 SumSkCalc2;           // 0x0D4
        [ExpectOffset(0x0D8)] public UInt32 SumSkCalc3;           // 0x0D8
        [ExpectOffset(0x0DC)] public UInt32 SumSkCalc4;           // 0x0DC
        [ExpectOffset(0x0E0)] public UInt32 SumSkCalc5;           // 0x0E0
        [ExpectOffset(0x0E4)] public UInt16 SumUMod;              // 0x0E4
        [ExpectOffset(0x0E6)] public UInt16 SumOverlay;           // 0x0E6
        [ExpectOffset(0x0E8)] public UInt16 CltMissile;           // 0x0E8
        [ExpectOffset(0x0EA)] public UInt16 CltMissileA;          // 0x0EA
        [ExpectOffset(0x0EC)] public UInt16 CltMissileB;          // 0x0EC
        [ExpectOffset(0x0EE)] public UInt16 CltMissileC;          // 0x0EE
        [ExpectOffset(0x0F0)] public UInt16 CltMissileD;          // 0x0F0
        [ExpectOffset(0x0F2)] public UInt16 CltStFunc;            // 0x0F2
        [ExpectOffset(0x0F4)] public UInt16 CltDoFunc;            // 0x0F4
        [ExpectOffset(0x0F6)] public UInt16 CltPrgFunc1;          // 0x0F6
        [ExpectOffset(0x0F8)] public UInt16 CltPrgFunc2;          // 0x0F8
        [ExpectOffset(0x0FA)] public UInt16 CltPrgFunc3;          // 0x0FA
        [ExpectOffset(0x0FC)] public UInt16 StSound;              // 0x0FC
        [ExpectOffset(0x0FE)] public UInt16 __Padding5;           // 0x0FE
        [ExpectOffset(0x100)] public UInt16 DoSound;              // 0x100
        [ExpectOffset(0x102)] public UInt16 DoSoundA;             // 0x102
        [ExpectOffset(0x104)] public UInt16 DoSoundB;             // 0x104
        [ExpectOffset(0x106)] public UInt16 CastOverlay;          // 0x106
        [ExpectOffset(0x108)] public UInt16 TgtOverlay;           // 0x108
        [ExpectOffset(0x10A)] public UInt16 TgtSound;             // 0x10A
        [ExpectOffset(0x10C)] public UInt16 PrgOverlay;           // 0x10C
        [ExpectOffset(0x10E)] public UInt16 PrgSound;             // 0x10E
        [ExpectOffset(0x110)] public UInt16 CltOverlayA;          // 0x110
        [ExpectOffset(0x112)] public UInt16 CltOverlayB;          // 0x112
        [ExpectOffset(0x114)] public UInt32 CltCalc1;             // 0x114
        [ExpectOffset(0x118)] public UInt32 CltCalc2;             // 0x118
        [ExpectOffset(0x11C)] public UInt32 CltCalc3;             // 0x11C
        [ExpectOffset(0x120)] public UInt16 ItemTarget;           // 0x120
        [ExpectOffset(0x122)] public UInt16 ItemCastSound;        // 0x122
        [ExpectOffset(0x124)] public UInt32 ItemCastOverlay;      // 0x124
        [ExpectOffset(0x128)] public UInt32 PerDelay;             // 0x128
        [ExpectOffset(0x12C)] public UInt16 MaxLvl;               // 0x12C
        [ExpectOffset(0x12E)] public UInt16 ResultFlags;          // 0x12E
        [ExpectOffset(0x130)] public UInt32 HitFlags;             // 0x130
        [ExpectOffset(0x134)] public UInt32 HitClass;             // 0x134
        [ExpectOffset(0x138)] public UInt32 Calc1;                // 0x138
        [ExpectOffset(0x13C)] public UInt32 Calc2;                // 0x13C
        [ExpectOffset(0x140)] public UInt32 Calc3;                // 0x140
        [ExpectOffset(0x144)] public UInt32 Calc4;                // 0x144
        [ExpectOffset(0x148)] public UInt32 Param1;               // 0x148
        [ExpectOffset(0x14C)] public UInt32 Param2;               // 0x14C
        [ExpectOffset(0x150)] public UInt32 Param3;               // 0x150
        [ExpectOffset(0x154)] public UInt32 Param4;               // 0x154
        [ExpectOffset(0x158)] public UInt32 Param5;               // 0x158
        [ExpectOffset(0x15C)] public UInt32 Param6;               // 0x15C
        [ExpectOffset(0x160)] public UInt32 Param7;               // 0x160
        [ExpectOffset(0x164)] public UInt32 Param8;               // 0x164
        [ExpectOffset(0x168)] public UInt16 WeapSel;              // 0x168
        [ExpectOffset(0x16A)] public UInt16 ItemEffect;           // 0x16A
        [ExpectOffset(0x16C)] public UInt32 ItemCltEffect;        // 0x16C
        [ExpectOffset(0x170)] public UInt32 SkPoints;             // 0x170
        [ExpectOffset(0x174)] public UInt16 ReqLevel;             // 0x174
        [ExpectOffset(0x176)] public UInt16 ReqStr;               // 0x176
        [ExpectOffset(0x178)] public UInt16 ReqDex;               // 0x178
        [ExpectOffset(0x17A)] public UInt16 ReqInt;               // 0x17A
        [ExpectOffset(0x17C)] public UInt16 ReqVit;               // 0x17C
        [ExpectOffset(0x17E)] public UInt16 ReqSkill1;            // 0x17E
        [ExpectOffset(0x180)] public UInt16 ReqSkill2;            // 0x180
        [ExpectOffset(0x182)] public UInt16 ReqSkill3;            // 0x182
        [ExpectOffset(0x184)] public UInt16 StartMana;            // 0x184
        [ExpectOffset(0x186)] public UInt16 MinMana;              // 0x186
        [ExpectOffset(0x188)] public UInt16 ManaShift;            // 0x188
        [ExpectOffset(0x18A)] public UInt16 Mana;                 // 0x18A
        [ExpectOffset(0x18C)] public UInt16 LevelMana;            // 0x18C
        [ExpectOffset(0x18E)] public UInt8  AttackRank;           // 0x18E
        [ExpectOffset(0x18F)] public UInt8  LineOfSight;          // 0x18F
        [ExpectOffset(0x190)] public UInt32 Delay;                // 0x190
        [ExpectOffset(0x194)] public UInt32 SkillDescriptionId;   // 0x194
        [ExpectOffset(0x198)] public UInt32 ToHit;                // 0x198
        [ExpectOffset(0x19C)] public UInt32 LevToHit;             // 0x19C
        [ExpectOffset(0x1A0)] public UInt32 ToHitCalc;            // 0x1A0
        [ExpectOffset(0x1A4)] public UInt8  ToHitShift;           // 0x1A4
        [ExpectOffset(0x1A5)] public UInt8  SrcDam;               // 0x1A5
        [ExpectOffset(0x1A6)] public UInt16 __Padding6;           // 0x1A6
        [ExpectOffset(0x1A8)] public UInt32 MinDam;               // 0x1A8
        [ExpectOffset(0x1AC)] public UInt32 MaxDam;               // 0x1AC
        [ExpectOffset(0x1B0)] public UInt32 MinLvlDam1;           // 0x1B0
        [ExpectOffset(0x1B4)] public UInt32 MinLvlDam2;           // 0x1B4
        [ExpectOffset(0x1B8)] public UInt32 MinLvlDam3;           // 0x1B8
        [ExpectOffset(0x1BC)] public UInt32 MinLvlDam4;           // 0x1BC
        [ExpectOffset(0x1C0)] public UInt32 MinLvlDam5;           // 0x1C0
        [ExpectOffset(0x1C4)] public UInt32 MaxLvlDam1;           // 0x1C4
        [ExpectOffset(0x1C8)] public UInt32 MaxLvlDam2;           // 0x1C8
        [ExpectOffset(0x1CC)] public UInt32 MaxLvlDam3;           // 0x1CC
        [ExpectOffset(0x1D0)] public UInt32 MaxLvlDam4;           // 0x1D0
        [ExpectOffset(0x1D4)] public UInt32 MaxLvlDam5;           // 0x1D4
        [ExpectOffset(0x1D8)] public UInt32 DmgSymPerCalc;        // 0x1D8
        [ExpectOffset(0x1DC)] public UInt8  EType;                // 0x1DC
        [ExpectOffset(0x1DD)] public UInt8  __Padding5b;          // 0x1DD
        [ExpectOffset(0x1DE)] public UInt16 __Padding7;           // 0x1DE
        [ExpectOffset(0x1E0)] public UInt32 EMin;                 // 0x1E0
        [ExpectOffset(0x1E4)] public UInt32 EMax;                 // 0x1E4
        [ExpectOffset(0x1E8)] public UInt32 EMinLev1;             // 0x1E8
        [ExpectOffset(0x1EC)] public UInt32 EMinLev2;             // 0x1EC
        [ExpectOffset(0x1F0)] public UInt32 EMinLev3;             // 0x1F0
        [ExpectOffset(0x1F4)] public UInt32 EMinLev4;             // 0x1F4
        [ExpectOffset(0x1F8)] public UInt32 EMinLev5;             // 0x1F8
        [ExpectOffset(0x1FC)] public UInt32 EMaxLev1;             // 0x1FC
        [ExpectOffset(0x200)] public UInt32 EMaxLev2;             // 0x200
        [ExpectOffset(0x204)] public UInt32 EMaxLev3;             // 0x204
        [ExpectOffset(0x208)] public UInt32 EMaxLev4;             // 0x208
        [ExpectOffset(0x20C)] public UInt32 EMaxLev5;             // 0x20C
        [ExpectOffset(0x210)] public UInt32 EDmgSymPerCalc;       // 0x210
        [ExpectOffset(0x214)] public UInt32 ELen;                 // 0x214
        [ExpectOffset(0x218)] public UInt32 ELevLen1;             // 0x218
        [ExpectOffset(0x21C)] public UInt32 ELevLen2;             // 0x21C
        [ExpectOffset(0x220)] public UInt32 ELevLen3;             // 0x220
        [ExpectOffset(0x224)] public UInt32 ELenSymPerCalc;       // 0x224
        [ExpectOffset(0x228)] public UInt16 Restrict;             // 0x228
        [ExpectOffset(0x22A)] public UInt16 State1;               // 0x22A
        [ExpectOffset(0x22C)] public UInt16 State2;               // 0x22C
        [ExpectOffset(0x22E)] public UInt16 State3;               // 0x22E
        [ExpectOffset(0x230)] public UInt16 AiType;               // 0x230
        [ExpectOffset(0x232)] public UInt16 AiBonus;              // 0x232
        [ExpectOffset(0x234)] public UInt32 CostMult;             // 0x234
        [ExpectOffset(0x238)] public UInt32 CostAdd;              // 0x238
    }
}
