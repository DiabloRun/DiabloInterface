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

        Dictionary<string, IntPtr> ModuleBaseAddresses { get; }

        T Read<T>(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);

        T IndexIntoArray<T>(DataPointer array, int index, uint length) where T : class;

        uint ReadUInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);

        IntPtr ReadAddress32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);

        byte ReadByte(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);

        byte[] Read(IntPtr address, int size, AddressingMode addressingMode = AddressingMode.Absolute);

        ushort ReadUInt16(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);

        T[] ReadArray<T>(IntPtr address, int length, AddressingMode addressingMode = AddressingMode.Absolute);

        string GetNullTerminatedString(IntPtr address, int size, int maximumSize, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute);

        IntPtr ResolvePointer(Pointer pointer, AddressingMode addressing = AddressingMode.Absolute);

        string ReadNullTerminatedString(IntPtr address, int size, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute);

        int ReadInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute);
    }
}
