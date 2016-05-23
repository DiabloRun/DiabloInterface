using System;
using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ModifierTable
    {
        public uint Length;         // 0x00
        public DataPointer Memory;  // 0x04
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x90)]
    class MagicModifier
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string ModifierName;             // 0x00
        public ushort ModifierNameHash;         // 0x20
                                                // Rest are unknown for now...
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 0x48)]
    class RareModifier
    {
        public int __unknown1;          // 0x00
        public int __unknown2;          // 0x04
        public int __unknown3;          // 0x08
        public ushort ModifierNameHash; // 0x0C
    }
}
