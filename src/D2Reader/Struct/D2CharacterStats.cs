using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct
{

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC4)]
    public class D2CharacterStats
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x00)] public string ClassName;            // 0x00
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 0x10)]
        [ExpectOffset(0x20)] public char[] ClassNameNarrow;      // 0x20
        [ExpectOffset(0x30)] public UInt8  Strength;             // 0x30
        [ExpectOffset(0x31)] public UInt8  Dexterity;            // 0x31
        [ExpectOffset(0x32)] public UInt8  Energy;               // 0x32
        [ExpectOffset(0x33)] public UInt8  Vitality;             // 0x33
        [ExpectOffset(0x34)] public UInt8  Stamina;              // 0x34
        [ExpectOffset(0x35)] public UInt8  HpAdd;                // 0x35
        [ExpectOffset(0x36)] public UInt8  PercentStrength;      // 0x36
        [ExpectOffset(0x37)] public UInt8  PercentEnergy;        // 0x37
        [ExpectOffset(0x38)] public UInt8  PercentDexterity;     // 0x38
        [ExpectOffset(0x39)] public UInt8  PercentVitality;      // 0x39
        [ExpectOffset(0x3A)] public UInt8  ManaRegen;            // 0x3A
        [ExpectOffset(0x3B)] public UInt8  __Unknown1;           // 0x3B
        [ExpectOffset(0x3C)] public UInt32 ToHitFactor;          // 0x3C
        [ExpectOffset(0x40)] public UInt8  WalkVelocity;         // 0x40
        [ExpectOffset(0x41)] public UInt8  RunVelocity;          // 0x41
        [ExpectOffset(0x42)] public UInt8  RunDrain;             // 0x42
        [ExpectOffset(0x43)] public UInt8  LifePerLevel;         // 0x43
        [ExpectOffset(0x44)] public UInt8  StaminaPerLevel;      // 0x44
        [ExpectOffset(0x45)] public UInt8  ManaPerLevel;         // 0x45
        [ExpectOffset(0x46)] public UInt8  LifePerVitality;      // 0x46
        [ExpectOffset(0x47)] public UInt8  StaminaPerVitality;   // 0x47
        [ExpectOffset(0x48)] public UInt8  ManaPerMagic;         // 0x48
        [ExpectOffset(0x49)] public UInt8  BlockFactor;          // 0x49
        [ExpectOffset(0x4A)] public UInt8  __Unknown2;           // 0x4A
        [ExpectOffset(0x4B)] public UInt8  __Unknown3;           // 0x4B
        [ExpectOffset(0x4C)] public UInt32 BaseWClass;           // 0x4C
        [ExpectOffset(0x50)] public UInt8  StatPerLevel;         // 0x50
        [ExpectOffset(0x51)] public UInt8  __Unknown4;           // 0x51
        [ExpectOffset(0x52)] public UInt16 AllSkillsStringId;    // 0x52
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 3)]
        [ExpectOffset(0x54)] public UInt16[] SkillTabStrings;    // 0x54
        [ExpectOffset(0x5A)] public UInt16 StrClassOnly;         // 0x5A
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 10)]
        [ExpectOffset(0x5C)] public CharacterItemInfo[] Items;   // 0x5C
        [ExpectOffset(0xAC)] public UInt16 StartSkill;           // 0xAC
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 10)]
        [ExpectOffset(0xAE)] public UInt16[] Skills;             // 0xAE
        [ExpectOffset(0xC2)] public UInt16 __Padding1;           // 0xC2
    }
}
