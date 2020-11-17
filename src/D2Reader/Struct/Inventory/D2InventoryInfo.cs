using System.Runtime.InteropServices;

namespace Zutatensuppe.D2Reader.Struct.Inventory
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2InventoryInfo
    {
        #region structure (sizeof = 0x70)
        [ExpectOffset(0x00)] public D2ItemContainerInfo Equipment;      // 1 dimensional array (13x1) starting with index 1 (item 0 seems to be always null)
        [ExpectOffset(0x10)] public D2ItemContainerInfo Belt;           // belt has a width of 16 and height of 1 (instead of 4x4)
        [ExpectOffset(0x20)] public D2ItemContainerInfo Inventory;
        [ExpectOffset(0x30)] public D2ItemContainerInfo __unknown_30;
        [ExpectOffset(0x40)] public D2ItemContainerInfo __unknown_40;
        [ExpectOffset(0x50)] public D2ItemContainerInfo Cube;
        [ExpectOffset(0x60)] public D2ItemContainerInfo Stash;
        #endregion
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class D2ItemContainerInfo
    {
        #region structure (sizeof = 0x10)
        [ExpectOffset(0x00)] public DataPointer pFirstItem;
        [ExpectOffset(0x04)] public DataPointer pLastItem;
        [ExpectOffset(0x08)] public byte Width;
        [ExpectOffset(0x09)] public byte Height;
        [ExpectOffset(0x0A)] public short __unknown_0A;
        [ExpectOffset(0x0C)] public DataPointer pArray;
        #endregion
    }
}
