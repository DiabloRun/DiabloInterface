using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Skill
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2Skill
    {
        #region struct sizeof = 0x40
        [ExpectOffset(0x0000)] public DataPointer pSkillData;  // 0x00 pointer skill data
        [ExpectOffset(0x0004)] public DataPointer pNextSkill;  // 0x04 pointer to next skill
        [ExpectOffset(0x0008)] public int __unknown1;          // 0x08 some number
        [ExpectOffset(0x000C)] public byte characterClass;    // 0x0C 255 for skills that apply to all, and charClass for hero classes (eg. 6 for assassin)
        [ExpectOffset(0x000D)] public byte __unknown2a;        // 0x0D
        [ExpectOffset(0x000E)] public short __unknown2b;        // 0x0E
        [ExpectOffset(0x0010)] public int __unknown3;          // 0x10
        [ExpectOffset(0x0014)] public int __unknown4;          // 0x14
        [ExpectOffset(0x0018)] public int __unknown5;          // 0x18
        [ExpectOffset(0x001C)] public int __unknown6;          // 0x1C
        [ExpectOffset(0x0020)] public int __unknown7;          // 0x20
        [ExpectOffset(0x0024)] public int __unknown8;          // 0x24
        [ExpectOffset(0x0028)] public int numberOfSkillPoints; // 0x28
        [ExpectOffset(0x002C)] public int __UnknownUsed;       // 0x2C  // used in func 0x6441BA
        [ExpectOffset(0x0030)] public int __unknown10;         // 0x30
        [ExpectOffset(0x0034)] public int __unknownAlwaysFFFFFF;         // 0x34 always 0xFFFFFFFF ?
        [ExpectOffset(0x0038)] public int __unknown12;         // 0x38
        [ExpectOffset(0x003C)] public int __unknown13;         // 0x3C this is checked when going through the globalData.PassiveSkills
                                                               // if this is != 0, the skill is skipped when going through the passive skills
        #endregion
    }
}
