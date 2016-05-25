using System.Runtime.InteropServices;

namespace DiabloInterface.D2.Struct
{
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class D2GlobalData
    {
        [FieldOffset(0xC18)] public DataPointer SetItemDescriptions;
        [FieldOffset(0xC1C)] public uint SetItemDescriptionCount;
        [FieldOffset(0xC24)] public DataPointer UniqueItemDescriptions;
        [FieldOffset(0xC28)] public uint UniqueItemDescriptionCount;
    }
}
