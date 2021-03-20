using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Unknown
{
    // 1.14D: game.7BB5BC
    // has to do with pets
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class D2UnknownUnitStruct
    {
        [FieldOffset(0x00)] public int unknown_00;
        [FieldOffset(0x04)] public int eClass;
        [FieldOffset(0x08)] public int GUID; // GUID
        [FieldOffset(0x0C)] public int OwnerGUID; // OwnerGuid
        [FieldOffset(0x10)] public int unknown_10;
        [FieldOffset(0x14)] public int unknown_14;
        [FieldOffset(0x18)] public int unknown_18;
        [FieldOffset(0x1C)] public int unknown_1C;
        [FieldOffset(0x20)] public int unknown_20; // unknown.. when this is 0, guid of this unit is returned without checking pNext in game.7BB5BC

        [FieldOffset(0x24)] public int unknown_24;
        [FieldOffset(0x28)] public int unknown_28;
        [FieldOffset(0x2C)] public int unknown_2C;
        [FieldOffset(0x30)] public DataPointer pNext; // next (or prev)
        // ...
    }
}
