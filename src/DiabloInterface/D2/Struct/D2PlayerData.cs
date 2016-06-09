using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    struct D2QuestArray
    {
#pragma warning disable 0649
        public DataPointer Buffer;
        public int Length;
#pragma warning restore 0649
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    class D2PlayerData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        [ExpectOffset(0x00)] public string PlayerName;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 3)]
        [ExpectOffset(0x10)] public DataPointer[] Quests;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 3)]
        [ExpectOffset(0x1C)] public DataPointer[] Waypoints;
        // Struct is bigger but rest is not needed.
    }
}
