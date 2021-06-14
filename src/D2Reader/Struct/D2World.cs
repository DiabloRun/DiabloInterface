using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Explicit)]
    public class D2World
    {
        [FieldOffset(0x1C)] public DataPointer GameBuffer;
        [FieldOffset(0x24)] public uint GameMask;
        [FieldOffset(0x44)] public int unknown_44; // probably an addr?
        [FieldOffset(0x48)] public uint GameId; // game id is set in 1.14d in func Game.exe + 12E6CC
        [FieldOffset(0x4C)] public int unknown_4c;
        [FieldOffset(0x50)] public int unknown_50; // probably an addr?

        // used at 1.14d, Game.exe + 130C0F: lea [world + 50]
        // used at 1.14d, Game.exe + 130C21: lea [world + 44] some addr? 
    }
}
