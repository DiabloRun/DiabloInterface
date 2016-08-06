using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    class D2PlayerData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        [ExpectOffset(0x00)] public string PlayerName;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 3)]
        [ExpectOffset(0x10)] public DataPointer[] Quests; // 0x10, 0x14, 0x18

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 3)]
        [ExpectOffset(0x1C)] public DataPointer[] Waypoints; // 0x1c, 0x20, 0x24

        [ExpectOffset(0x28)] public int __unknown01;
        [ExpectOffset(0x2C)] public int __unknown02; // some number (maybe always 0x01 ?)
        [ExpectOffset(0x30)] public int __unknown03;
        [ExpectOffset(0x34)] public int __unknown__maybe_skill_related01; // probably an address?
        [ExpectOffset(0x38)] public int __unknown05;
        [ExpectOffset(0x3C)] public int __unknown05b;
        [ExpectOffset(0x40)] public int __unknown05c;
        [ExpectOffset(0x44)] public int __unknown__maybe_skill_related02; // probably an address? something that is read when reading skills .. ? 
        [ExpectOffset(0x48)] public int __unknown06;
        [ExpectOffset(0x4C)] public int __unknown07;
        [ExpectOffset(0x50)] public int __unknown08;
        [ExpectOffset(0x54)] public int __unknown09;
        [ExpectOffset(0x58)] public int __unknown10;
        [ExpectOffset(0x5C)] public int __unknown11;
        [ExpectOffset(0x60)] public int __unknown__maybe_skill_related03; // probably an adress
        [ExpectOffset(0x64)] public int __unknown__maybe_skill_related04; // probably an adress
        [ExpectOffset(0x68)] public int __unknown__maybe_skill_related05; // probably an adress
        [ExpectOffset(0x6C)] public int __unknown12;
        [ExpectOffset(0x70)] public int currentRightSkillId; // probably right skill picker active skill id
        [ExpectOffset(0x74)] public int currentLeftSkillId; // probably left skill picker active skill id

        // probably this struct is even longer...
    }
}
