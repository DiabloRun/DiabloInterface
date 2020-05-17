using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Zutatensuppe.D2Reader.Struct.Item
{
    // indexes seen in ASM: (indexes in globalData.ItemStatCost array)
    public enum ItemStatCostIndex : uint
    {
        Unknown0  = 0x00,
        Unknown2  = 0x02,
        Unknown11 = 0x11,
        Unknown12 = 0x12,
        Unknown15 = 0x15,
        Unknown16 = 0x16,
        Unknown17 = 0x17,
        Unknown18 = 0x18,
        Unknown53 = 0x53,
        Unknown61 = 0x61,
        Unknown6B = 0x6B,
        Unknown7E = 0x7E,
        Unknown7F = 0x7F,
        UnknownBC = 0xBC,
        Unknown15F = 0x15F,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x144)]
    public class D2ItemStatCost
    {
        [ExpectOffset(0x0000)] public UInt16 StatId;              // 0x00
        [ExpectOffset(0x0002)] public UInt16 __Unknown1;          // 0x02
        [ExpectOffset(0x0004)] public UInt16 ItemStatFlags;       // 0x04
        [ExpectOffset(0x0006)] public UInt16 __Unknown2;          // 0x06
        [ExpectOffset(0x0008)] public UInt8  SendBits;            // 0x08
        [ExpectOffset(0x0009)] public UInt8  SendParamBits;       // 0x09
        [ExpectOffset(0x000A)] public UInt8  CsvBits;             // 0x0A
        [ExpectOffset(0x000B)] public UInt8  CsvParam;            // 0x0B
        [ExpectOffset(0x000C)] public UInt32 Divide;              // 0x0C
        [ExpectOffset(0x0010)] public UInt32 Multiply;            // 0x10
        [ExpectOffset(0x0014)] public UInt32 Add;                 // 0x14
        [ExpectOffset(0x0018)] public UInt8  ValShift;            // 0x18
        [ExpectOffset(0x0019)] public UInt8  SaveBits;            // 0x19
        [ExpectOffset(0x001A)] public UInt8  SaveBits09;          // 0x1A
        [ExpectOffset(0x001B)] public UInt8  __Unknown3;          // 0x1B
        [ExpectOffset(0x001C)] public UInt32 SaveAdd;             // 0x1C
        [ExpectOffset(0x0020)] public UInt32 SaveAdd09;           // 0x20
        [ExpectOffset(0x0024)] public UInt32 SaveParamBits;       // 0x24
        [ExpectOffset(0x0028)] public UInt32 __Unknown4;          // 0x28
        [ExpectOffset(0x002C)] public UInt32 MinAccr;             // 0x2C
        [ExpectOffset(0x0030)] public UInt8  Encode;              // 0x30
        [ExpectOffset(0x0031)] public UInt8  __Unknown5;          // 0x31
        [ExpectOffset(0x0032)] public UInt16 MaxAtat;             // 0x32
        [ExpectOffset(0x0034)] public UInt16 DescPriority;        // 0x34
        [ExpectOffset(0x0036)] public UInt8  DescFunc;            // 0x36
        [ExpectOffset(0x0037)] public UInt8  DescVal;             // 0x37
        [ExpectOffset(0x0038)] public UInt16 DescStrPos;          // 0x38
        [ExpectOffset(0x003A)] public UInt16 DescStrNeg;          // 0x3A
        [ExpectOffset(0x003C)] public UInt16 DescStr2;            // 0x3C
        [ExpectOffset(0x003E)] public UInt16 DGrp;                // 0x3E
        [ExpectOffset(0x0040)] public UInt8  DGrpFunc;            // 0x40
        [ExpectOffset(0x0041)] public UInt8  DGrpVal;             // 0x41
        [ExpectOffset(0x0042)] public UInt16 DGrpStrPos;          // 0x42
        [ExpectOffset(0x0044)] public UInt16 DGrpStrNeg;          // 0x44
        [ExpectOffset(0x0046)] public UInt16 DGrpStr2;            // 0x46
        [ExpectOffset(0x0048)] public UInt16 ItemEvent1;          // 0x48
        [ExpectOffset(0x004A)] public UInt16 ItemEvent2;          // 0x4A
        [ExpectOffset(0x004C)] public UInt16 ItemEventFunc1;      // 0x4C
        [ExpectOffset(0x004E)] public UInt16 ItemEventFunc2;      // 0x4E
        [ExpectOffset(0x0050)] public UInt32 KeepZero;            // 0x50
        [ExpectOffset(0x0054)] public UInt8  Op;                  // 0x54
        [ExpectOffset(0x0055)] public UInt8  OpParam;             // 0x55
        [ExpectOffset(0x0056)] public UInt16 OpBase;              // 0x56
        [ExpectOffset(0x0058)] public UInt16 OpStat1;             // 0x58
        [ExpectOffset(0x005A)] public UInt16 OpStat2;             // 0x5A
        [ExpectOffset(0x005C)] public UInt16 OpStat3;             // 0x5C
        // Rest unknown.
    }
}
