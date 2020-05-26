using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Monster
{
    // information from https://d2mods.info/forum/viewtopic.php?f=8&t=47809&p=360419

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class D2AiGeneral
    {
        #region structure (sizeof = 0x040)
        [FieldOffset(0x00)] public int SpecialState; // stuff like terror, confusion goes here
        [FieldOffset(0x04)] public int fpAiFunction; // the primary ai function to call(void* __fastcall)(pGame, pUnit, pAiTickArgs);
        [FieldOffset(0x08)] public int AiFlags;
        [FieldOffset(0x0C)] public int OwnerGUID; // the global unique identifier of the boss or minion owner
        [FieldOffset(0x10)] public int eOwnerType; // the unit type of the boss or minion owner
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        [FieldOffset(0x14)] public int[] dwArgs; // three dwords holding custom data used by ai func to store counters(etc)
        [FieldOffset(0x20)] public int pCmdCurrent;
        [FieldOffset(0x24)] public int pCmdLast;
        [FieldOffset(0x28)] public int pGame;
        [FieldOffset(0x2C)] public int OwnerGUID2; //
        [FieldOffset(0x30)] public int eOwnerType2; //
        [FieldOffset(0x34)] public int pMinionList; // - list of all minions, for boss units(SetBoss in MonStats, Unique, SuperUnique etc)
        // ...
        [FieldOffset(0x3C)] public int eTrapNo; // - used by shadows for summoning traps(so they stick to one type usually)
        #endregion
    }
}
