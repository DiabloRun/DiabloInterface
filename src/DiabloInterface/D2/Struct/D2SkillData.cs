using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct D2SkillData
    {
        #region structure ( sizeof = 0x023C most likely)
        public int skillId;             // 0x0004 skill id (from skills.txt)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)]
        public int[] __unknown_1;    // 0x0008
        public int param1;              // 0x0148
        public int param2;              // 0x014C
        public int param3;              // 0x0150
        public int param4;              // 0x0154
        public int param5;              // 0x0158
        public int param6;              // 0x015C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public int[] __unknown_2;       // 0x0160
        public short __unknown_3;       // 0x0188 maybe manaShift
        public short mana;              // 0x018A (manaCostChangePer skillpoint invested)
        public short lvlMana;           // 0x018C
        public short attackRating;      // 0x018E
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)]
        public int[] __unknown_4;       // 0x0190
        public int costAdd;             // 0x0234
        public int costMult;            // 0x0238
        #endregion
    }
}
