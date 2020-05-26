using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x1DE8)]
    public class D2Game
    {
        [ExpectOffset(0x0000)] public DataPointer Owner;
        [ExpectOffset(0x0004)] public UInt32 __Unknown_004;
        [ExpectOffset(0x0008)] public UInt32 __Unknown_008;
        [ExpectOffset(0x000C)] public DataPointer GameData8;
        [ExpectOffset(0x0010)] public DataPointer NextGame;
        [ExpectOffset(0x0014)] public UInt32 __Unknown_014;
        [ExpectOffset(0x0018)] public DataPointer Lock;
        [ExpectOffset(0x001C)] public DataPointer MemoryPool;
        [ExpectOffset(0x0020)] public DataPointer GameData;
        [ExpectOffset(0x0024)] public UInt32 __Unknown_024;
        [ExpectOffset(0x0028)] public UInt16 ServerToken;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        [ExpectOffset(0x002A)] public string GameName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        [ExpectOffset(0x003A)] public string Password;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        [ExpectOffset(0x004A)] public string Description;
        [ExpectOffset(0x006A)] public UInt8  GameType;
        [ExpectOffset(0x006B)] public UInt8  __Unknown_06B;
        [ExpectOffset(0x006C)] public UInt8  __Unknown_06C;
        [ExpectOffset(0x006D)] public UInt8  Difficulty;
        [ExpectOffset(0x006E)] public UInt8  __Unknown_06E;
        [ExpectOffset(0x006F)] public UInt8  __Unknown_06F;
        [ExpectOffset(0x0070)] public UInt32 LODFlag;
        [ExpectOffset(0x0074)] public UInt32 GameTypeFull;
        [ExpectOffset(0x0078)] public UInt16 ItemFormat;
        [ExpectOffset(0x007A)] public UInt16 __Unknown_07A;
        [ExpectOffset(0x007C)] public UInt32 InitSeed;
        [ExpectOffset(0x0080)] public UInt32 ObjectSeed;
        [ExpectOffset(0x0084)] public UInt32 InitSeedOther;
        [ExpectOffset(0x0088)] public DataPointer Client;
        [ExpectOffset(0x008C)] public UInt32 ClientCount;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 6)]
        [ExpectOffset(0x0090)] public UInt32[] UnitCounters;
        [ExpectOffset(0x00A8)] public UInt32 CurrentFrame;
        [ExpectOffset(0x00AC)] public UInt32 __Unknown_0AC;
        [ExpectOffset(0x00B0)] public UInt32 __Unknown_0B0;
        [ExpectOffset(0x00B4)] public UInt32 __Unknown_0B4;
        [ExpectOffset(0x00B8)] public DataPointer TimerQueue;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
        [ExpectOffset(0x00BC)] public DataPointer[] ActDrlg;
        [ExpectOffset(0x00D0)] public UInt32 GameSeed;
        [ExpectOffset(0x00D4)] public UInt32 GameSeed2;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
        [ExpectOffset(0x00D8)] public DataPointer[] RoomList;
        [ExpectOffset(0x00EC)] public UInt32 MonsterSeed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1024)]
        [ExpectOffset(0x00F0)] public DataPointer[] MonsterRegions;
        [ExpectOffset(0x10F0)] public DataPointer ObjectController;
        [ExpectOffset(0x10F4)] public DataPointer QuestController;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 10)]
        [ExpectOffset(0x10F8)] public DataPointer[] UnitNodes;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
        // 5 unit lists, each type (player, monster, object, missile, item) has one (vistile is referenced by pointer)
        [ExpectOffset(0x1120)] public D2GameUnitList[] UnitLists;
        [ExpectOffset(0x1B20)] public DataPointer TileList;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 128)]
        [ExpectOffset(0x1B24)] public UInt32[] UniqueItemFlags;
        [ExpectOffset(0x1D24)] public DataPointer NpcController;
        [ExpectOffset(0x1D28)] public DataPointer ArenaController;
        [ExpectOffset(0x1D2C)] public DataPointer PartyController;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
        [ExpectOffset(0x1D30)] public UInt8[] SuperUniqueMonsterFlags;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 17)]
        [ExpectOffset(0x1D70)] public UInt32[] MonsterModeData;
        [ExpectOffset(0x1DB4)] public UInt32 MonsterModeDataNum;
        [ExpectOffset(0x1DB8)] public UInt32 __Unknown_1DB8;
        [ExpectOffset(0x1DBC)] public UInt32 CreationTime;
        [ExpectOffset(0x1DC0)] public UInt32 __Unknown_1DC0;
        [ExpectOffset(0x1DC4)] public UInt32 SyncTimer;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 8)]
        [ExpectOffset(0x1DC8)] public UInt32[] __Unknown_1DC8;
    }
}
