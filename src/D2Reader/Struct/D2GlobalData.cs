using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    // Where to find this?
    // 1.14D: [game.744304] points to this struct 
    // 
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class D2GlobalData
    {
        // list of calculation modes for all the stat stuff? 
        [FieldOffset(0x040)] public DataPointer StatCalcList; // maybe (Aura)StatCalcList .. array of StatCalcType bytes
        [FieldOffset(0x044)] public uint StatCalcCount; // maybe (Aura)StatCalcCount

        [FieldOffset(0x0C4)] public uint UnknownValue3; // (0xB9 => 185) related to the skill.PassiveState value (maybe it is some kind of "max" value for passive state)

        // size of each ClassDescription is probably 0x1A8
        [FieldOffset(0xA78)] public DataPointer ClassDescriptions;
        [FieldOffset(0xA80)] public uint ClassCount;

        [FieldOffset(0xB8C)] public DataPointer SkillDescriptions;
        [FieldOffset(0xB94)] public uint SkillDescriptionCount;

        [FieldOffset(0xB98)] public DataPointer Skills;
        [FieldOffset(0xBA0)] public uint SkillCount;
        
        [FieldOffset(0xBB0)] public uint PassiveSkillsCount; // UnknownCount2
        [FieldOffset(0xBB4)] public DataPointer PassiveSkillIds; // UnknownThing2 seems like an address that points to a byte array of length PassiveSkillsCount
                                                                 // the items in that array are somehow related to SkillCount (maybe they are skill ids too?..)

        [FieldOffset(0xBC4)] public DataPointer Characters; // pointer to array of D2CharacterStats
        [FieldOffset(0xBC8)] public uint CharacterCount; // how many different classes exist. D2Unit.eClass is checked against this count to see if it is valid

        [FieldOffset(0xBCC)] public DataPointer ItemStatCost; 
        [FieldOffset(0xBD4)] public uint ItemStatCostCount;

        [FieldOffset(0xBD8)] public DataPointer OpStatNesting;
        [FieldOffset(0xBDC)] public uint OpStatNestingCount;

        // 1.14D: used in game.493240
        // 1.13D: D2Client.dll+894D0
        // pet type infos?
        // list items each have lenth of 0xE0
        [FieldOffset(0xBE8)] public DataPointer unknownList1; 
        [FieldOffset(0xBF0)] public uint unknownCount1;

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
