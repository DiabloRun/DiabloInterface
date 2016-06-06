using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class D2GlobalData
    {
        [FieldOffset(0xB8C)] public DataPointer SkillDescriptions;
        [FieldOffset(0xB94)] public uint SkillDescriptionCount;
        [FieldOffset(0xB98)] public DataPointer Skills;
        [FieldOffset(0xBA0)] public uint SkillCount;
        [FieldOffset(0xBC4)] public DataPointer Characters;
        [FieldOffset(0xBC8)] public uint CharacterCount;
        [FieldOffset(0xBCC)] public DataPointer ItemStatCost;
        [FieldOffset(0xBD4)] public uint ItemStatCostCount;
        [FieldOffset(0xBD8)] public DataPointer OpStatNesting;
        [FieldOffset(0xBDC)] public uint OpStatNestingCount;
        [FieldOffset(0xC18)] public DataPointer SetItemDescriptions;
        [FieldOffset(0xC1C)] public uint SetItemDescriptionCount;
        [FieldOffset(0xC24)] public DataPointer UniqueItemDescriptions;
        [FieldOffset(0xC28)] public uint UniqueItemDescriptionCount;
        [FieldOffset(0xC34)] public uint MonsterPropCount;
        [FieldOffset(0xC3C)] public DataPointer MonsterTypes;
        [FieldOffset(0xC6C)] public uint ItemSkillShift;
        [FieldOffset(0xC70)] public uint ItemSkillMask;
    }
}
