using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterItemInfo
    {
        public UInt32 Item;
        public UInt8  Location;
        public UInt8  Count;
        public UInt8  __Unknown1;
        public UInt8  __Unknown2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC4)]
    public class D2CharacterStats
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string ClassName;            // 0x00
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 0x10)]
        public char[] ClassNameNarrow;      // 0x20
        public UInt8  Strength;             // 0x30
        public UInt8  Dexterity;            // 0x31
        public UInt8  Energy;               // 0x32
        public UInt8  Vitality;             // 0x33
        public UInt8  Stamina;              // 0x34
        public UInt8  HpAdd;                // 0x35
        public UInt8  PercentStrength;      // 0x36
        public UInt8  PercentEnergy;        // 0x37
        public UInt8  PercentDexterity;     // 0x38
        public UInt8  PercentVitality;      // 0x39
        public UInt8  ManaRegen;            // 0x3A
        public UInt8  __Unknown1;           // 0x3B
        public UInt32 ToHitFactor;          // 0x3C
        public UInt8  WalkVelocity;         // 0x40
        public UInt8  RunVelocity;          // 0x41
        public UInt8  RunDrain;             // 0x42
        public UInt8  LifePerLevel;         // 0x43
        public UInt8  StaminaPerLevel;      // 0x44
        public UInt8  ManaPerLevel;         // 0x45
        public UInt8  LifePerVitality;      // 0x46
        public UInt8  StaminaPerVitality;   // 0x47
        public UInt8  ManaPerMagic;         // 0x48
        public UInt8  BlockFactor;          // 0x49
        public UInt8  __Unknown2;           // 0x4A
        public UInt8  __Unknown3;           // 0x4B
        public UInt32 BaseWClass;           // 0x4C
        public UInt8  StatPerLevel;         // 0x50
        public UInt8  __Unknown4;           // 0x51
        public UInt16 AllSkillsStringId;    // 0x52
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 3)]
        public UInt16[] SkillTabStrings;    // 0x54
        public UInt16 StrClassOnly;         // 0x5A
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 10)]
        public CharacterItemInfo[] Items;   // 0x5C
        public UInt16 StartSkill;           // 0xAC
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 10)]
        public UInt16[] Skills;             // 0xAE
        public UInt16 __Padding1;           // 0xC2
    }
}
