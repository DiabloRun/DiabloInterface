using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x144)]
    public class D2ItemStatCost
    {
        public UInt16 StatId;              // 0x00
        public UInt16 __Unknown1;          // 0x02
        public UInt16 ItemStatFlags;       // 0x04
        public UInt16 __Unknown2;          // 0x06
        public UInt8  SendBits;            // 0x08
        public UInt8  SendParamBits;       // 0x09
        public UInt8  CsvBits;             // 0x0A
        public UInt8  CsvParam;            // 0x0B
        public UInt32 Divide;              // 0x0C
        public UInt32 Multiply;            // 0x10
        public UInt32 Add;                 // 0x14
        public UInt8  ValShift;            // 0x18
        public UInt8  SaveBits;            // 0x19
        public UInt8  SaveBits09;          // 0x1A
        public UInt8  __Unknown3;          // 0x1B
        public UInt32 SaveAdd;             // 0x1C
        public UInt32 SaveAdd09;           // 0x20
        public UInt32 SaveParamBits;       // 0x24
        public UInt32 __Unknown4;          // 0x28
        public UInt32 MinAccr;             // 0x2C
        public UInt8  Encode;              // 0x30
        public UInt8  __Unknown5;          // 0x31
        public UInt16 MaxAtat;             // 0x32
        public UInt16 DescPriority;        // 0x34
        public UInt8  DescFunc;            // 0x36
        public UInt8  DescVal;             // 0x37
        public UInt16 DescStrPos;          // 0x38
        public UInt16 DescStrNeg;          // 0x3A
        public UInt16 DescStr2;            // 0x3C
        public UInt16 DGrp;                // 0x3E
        public UInt8  DGrpFunc;            // 0x40
        public UInt8  DGrpVal;             // 0x41
        public UInt16 DGrpStrPos;          // 0x42
        public UInt16 DGrpStrNeg;          // 0x44
        public UInt16 DGrpStr2;            // 0x46
        public UInt16 ItemEvent1;          // 0x48
        public UInt16 ItemEvent2;          // 0x4A
        public UInt16 ItemEventFunc1;      // 0x4C
        public UInt16 ItemEventFunc2;      // 0x4E
        public UInt32 KeepZero;            // 0x50
        public UInt8  Op;                  // 0x54
        public UInt8  OpParam;             // 0x55
        public UInt16 OpBase;              // 0x56
        public UInt16 OpStat1;             // 0x58
        public UInt16 OpStat2;             // 0x5A
        public UInt16 OpStat3;             // 0x5C
        // Rest unknown.
    }
}
