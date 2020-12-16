using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader.Struct.Monster
{
    // maybe this is the same as D2ClassDescription?

    // the whole thing should have a length of 0x1A8 (424)
    // it kind of represents the rows in monStats.txt
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class D2MonStats
    {
        [ExpectOffset(0x00)] public byte unknown_00;
        [ExpectOffset(0x01)] public byte unknown_01;
        [ExpectOffset(0x02)] public byte unknown_02;
        [ExpectOffset(0x03)] public byte unknown_03;
        [ExpectOffset(0x04)] public byte unknown_04;
        [ExpectOffset(0x05)] public byte unknown_05;
        [ExpectOffset(0x06)] public ushort nameStrIdx;
        [ExpectOffset(0x08)] public byte unknown_08;
        [ExpectOffset(0x09)] public byte unknown_09;
        [ExpectOffset(0x0A)] public byte unknown_0A;
        [ExpectOffset(0x0B)] public byte unknown_0B;
        [ExpectOffset(0x0C)] public byte unknown_0C;
        [ExpectOffset(0x0D)] public D2MonTypeFlag typeFlags; // contains info about
                                                             // demon
                                                             // undead high
                                                             // undead low

    }

    [Flags]
    public enum D2MonTypeFlag : byte
    {
        None = 0,
        Undead1 = 0x08,
        Undead2 = 0x10,
        Demon = 0x20,
    }
}
