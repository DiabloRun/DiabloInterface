using System;
using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct
{
    [StructLayout(LayoutKind.Explicit)]
    public class D2Client
    {
        [FieldOffset(0x00)] public UInt32 unknown00;
        [FieldOffset(0x04)] public UInt32 unknown04;
        [FieldOffset(0x08)] public UInt16 unknown08;
        [FieldOffset(0x0A)] public byte CharStatus; // same as in d2s (see https://github.com/krisives/d2s-format)
        [FieldOffset(0x0B)] public byte CharProgression; // probably char progression as in d2s 
        [FieldOffset(0x0C)] public UInt32 unknown0C;
        [FieldOffset(0x10)] public UInt32 unknown10;
        [FieldOffset(0x14)] public UInt32 unknown14;
        [FieldOffset(0x16C)] public UInt32 UnitType;
        [FieldOffset(0x170)] public Int32 UnitId;

        public bool IsHardcore() => (CharStatus & 4) == 4; // true if hardcore character
        public bool HasDied() => (CharStatus & 8) == 8; // true if the character died at least once
        public bool IsExpansion() => (CharStatus & 32) == 32; // true if LOD character (we can also just use D2Game.LODFlag)
    }
}
