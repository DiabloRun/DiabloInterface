using System;
using System.Collections.Generic;
using System.Text;
using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader
{
    public interface IProcessMemoryReader: IDisposable
    {
        bool IsValid { get; }

        string FileVersion { get; }

        IntPtr BaseAddress { get; }
        Dictionary<string, IntPtr> ModuleBaseAddresses { get; }

        string[] CommandLineArgs { get; }

        T Read<T>(IntPtr address);

        T IndexIntoArray<T>(DataPointer array, int index, uint length) where T : class;

        uint ReadUInt32(IntPtr address);

        IntPtr ReadAddress32(IntPtr address);

        byte ReadByte(IntPtr address);

        byte[] Read(IntPtr address, int size);

        ushort ReadUInt16(IntPtr address);

        T[] ReadArray<T>(IntPtr address, int length);

        string GetNullTerminatedString(IntPtr address, int size, int maximumSize, Encoding encoding);

        IntPtr ResolvePointer(Pointer pointer);

        string ReadNullTerminatedString(IntPtr address, int size, Encoding encoding);

        int ReadInt32(IntPtr address);
    }
}
