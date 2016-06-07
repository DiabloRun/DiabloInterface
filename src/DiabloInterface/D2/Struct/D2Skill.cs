using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2Skill
    {
        #region struct sizeof = 0x40
        public int pSkillData;          // 0x00 pointer skill data
        public int pNextSkill;          // 0x04 pointer to next skill
        public int __unknown1;          // 0x08 some number
        public int __unknown2;          // 0x0C
        public int __unknown3;          // 0x10
        public int __unknown4;          // 0x14
        public int __unknown5;          // 0x18
        public int __unknown6;          // 0x1C
        public int __unknown7;          // 0x20
        public int __unknown8;          // 0x24
        public int numberOfSkillPoints; // 0x28
        public int __unknown9;          // 0x2C
        public int __unknown10;         // 0x30
        public int __unknown11;         // 0x34 always 0xFFFFFFFF ?
        public int __unknown12;         // 0x38
        public int __unknown13;         // 0x3C
        #endregion
    }
}
