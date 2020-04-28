using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader
{
    public struct Pointer
    {
        public IntPtr Base;
        public int[] Offsets;
    }

    public enum AddressingMode
    {
        Absolute,
        Relative
    }

    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException(string processName, string moduleName) :
            base($"Could not find process {processName}[{moduleName}]")
        { }
    }

    public class ProcessMemoryReadException : Exception
    {
        public ProcessMemoryReadException(
            IntPtr address,
            bool success,
            uint bytesRead,
            int dataLength
        ) :
            base($"Failed to read memory at: 0x{address.ToInt64():X8} {success} {bytesRead} {dataLength}")
        { }
    }

    public class ProcessMemoryReader : IProcessMemoryReader, IDisposable
    {
        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_BASIC_INFORMATION64
        {
            public ulong BaseAddress;
            public IntPtr AllocationBase;
            public int AllocationProtect;
            public int __alignment1;
            public ulong RegionSize;
            public int State;
            public int Protect;
            public int Type;
            public int __alignment2;
        }
        private enum AllocationProtect : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        const uint ProcessStillActive = 259;

        IntPtr baseAddress;
        IntPtr processHandle;
        public Dictionary<string, IntPtr> ModuleBaseAddresses { get; }

        public string FileVersion { get; }
        public string ModuleName { get; }

        public bool IsValid
        {
            get
            {
                if (!GetExitCodeProcess(processHandle, out uint exitCode))
                    return false;
                return exitCode == ProcessStillActive;
            }
        }

        private Dictionary<string, IntPtr> FindModuleAddresses(Process p, string[] modules)
        {
            Dictionary<string, IntPtr> addresses = new Dictionary<string, IntPtr>();
            long MaxAddress = 0x7fffffff;
            long address = 0;
            const int nChars = 1024;
            StringBuilder filename;
            do
            {
                filename = new StringBuilder(nChars);
                int result = VirtualQueryEx(p.Handle, (IntPtr)address, out MEMORY_BASIC_INFORMATION64 m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));
                if (address == (long)m.BaseAddress + (long)m.RegionSize)
                {
                    break;
                }

                address = (long)m.BaseAddress + (long)m.RegionSize;

                GetModuleFileNameEx(p.Handle, m.AllocationBase, filename, nChars);

                foreach (string module in modules)
                {
                    if (addresses.ContainsKey(module)) continue;
                    if (!filename.ToString().Contains(module)) continue;
                    addresses.Add(module, m.AllocationBase);
                }
            } while (address <= MaxAddress);
            return addresses;
        }

        public ProcessMemoryReader(string processName, string moduleName, string[] submodules)
        {
            bool foundModule = false;
            uint foundProcessId = 0;
            String foundFileVersion = null;
            IntPtr foundBaseAddress = IntPtr.Zero;

            Process[] processes = Process.GetProcessesByName(processName);
            try
            {
                foreach (var process in processes)
                {
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (module.ModuleName != moduleName)
                        {
                            continue;
                        }

                        foundModule = true;
                        foundProcessId = (uint)process.Id;
                        foundBaseAddress = module.BaseAddress;
                        foundFileVersion = module.FileVersionInfo.FileVersion;

                        // the modules we are looking for are managed in the game.exe in older d2 versions.
                        // cant get them via process.Modules
                        ModuleBaseAddresses = FindModuleAddresses(process, submodules);
                    }
                }
            }
            catch
            {
                throw new ProcessNotFoundException(processName, moduleName);
            }

            // Throw if the module cannot be found.
            if (!foundModule) throw new ProcessNotFoundException(processName, moduleName);

            // Open up handle.
            baseAddress = foundBaseAddress;
            processHandle = OpenProcess(
                ProcessAccessFlags.QueryLimitedInfo | ProcessAccessFlags.MemoryRead,
                false,
                foundProcessId
            );
            FileVersion = foundFileVersion;
            ModuleName = moduleName;

            // Make sure we succeeded in opening the handle.
            if (processHandle == IntPtr.Zero) throw new ProcessNotFoundException(processName, moduleName);
        }

        #region Disposable

        ~ProcessMemoryReader()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
                processHandle = IntPtr.Zero;
            }
        }

        #endregion

        #region Reading

        public IntPtr ResolveAddress(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            // Get relative address if wanted.
            if (addressingMode == AddressingMode.Relative)
            {
                return new IntPtr((long)baseAddress + (long)address);
            }

            // Already in absolute form.
            return address;
        }

        public IntPtr ResolvePointer(Pointer pointer, AddressingMode addressing = AddressingMode.Absolute)
        {
            return ResolveAddressPath(pointer.Base, pointer.Offsets, addressing);
        }

        public IntPtr ResolveAddressPath(IntPtr baseAddress, int[] pathOffsets, AddressingMode addressing = AddressingMode.Absolute)
        {
            if (pathOffsets == null)
                return baseAddress;

            IntPtr address = baseAddress;
            foreach (int offset in pathOffsets)
            {
                // Read at the current address and then offset.
                address = IntPtr.Add(ReadAddress32(address, addressing), offset);

                // Assume all subsequent addressing to be absolute.
                addressing = AddressingMode.Absolute;
            }

            return address;
        }

        public byte[] Read(IntPtr address, int size, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            byte[] data = new byte[size];
            address = ResolveAddress(address, addressingMode);
            bool success = ReadProcessMemory(processHandle, address, data, (uint)data.Length, out uint bytesRead);

            // Make sure we read successfully.
            if (!success || bytesRead != (uint)data.Length)
            {
                throw new ProcessMemoryReadException(
                    address,
                    success,
                    bytesRead,
                    data.Length
                );
            }
            return data;
        }

        /// <summary>
        /// Read a class/struct from process memory.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="address">Process memory address of struct.</param>
        /// <param name="addressingMode">Absolute or relative memory addressing.</param>
        /// <returns>The deserialized value or null.</returns>
        public T Read<T>(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            // Read struct memory.
            byte[] buffer = Read(address, Marshal.SizeOf<T>(), addressingMode);

            // Convert to structure.
            T data = default(T);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            try {
                data = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally {
                handle.Free();
            }

            return data;
        }

        public T IndexIntoArray<T>(DataPointer array, int index, uint length) where T : class
        {
            // Index out of range.
            if (index >= length) return null;

            // Indexing is just taking the size of each element added to the base.
            int offset = index * Marshal.SizeOf<T>();
            return Read<T>(array.Address + offset);
        }

        public T[] ReadArray<T>(IntPtr address, int length, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            T[] array = new T[length];
            if (length == 0)
            {
                return array;
            }

            // Read array memory.
            int elementSize = Marshal.SizeOf<T>();
            byte[] buffer = Read(address, elementSize * length, addressingMode);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var pinnedAddress = handle.AddrOfPinnedObject();
                for (int i = 0; i < length; ++i)
                {
                    array[i] = Marshal.PtrToStructure<T>(pinnedAddress);
                    pinnedAddress += elementSize;
                }
            }
            finally
            {
                handle.Free();
            }

            return array;
        }

        public byte ReadByte(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return Read(address, 1, addressingMode)[0];
        }

        public short ReadInt16(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToInt16(Read(address, 2, addressingMode), 0);
        }

        public ushort ReadUInt16(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToUInt16(Read(address, 2, addressingMode), 0);
        }

        public int ReadInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToInt32(Read(address, 4, addressingMode), 0);
        }

        public uint ReadUInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToUInt32(Read(address, 4, addressingMode), 0);
        }

        public long ReadInt64(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToInt64(Read(address, 8, addressingMode), 0);
        }

        public ulong ReadUInt64(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return BitConverter.ToUInt64(Read(address, 8, addressingMode), 0);
        }

        public IntPtr ReadAddress32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return new IntPtr(ReadUInt32(address, addressingMode));
        }

        public string ReadStringRaw(IntPtr address, int size, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute)
        {
            return encoding.GetString(Read(address, size, addressing));
        }

        /// <summary>
        /// Reads a string from memory and chops it off at the null terminator.
        /// If there is no null terminator it's just returned with the size specified, this
        /// might chop up a word if the string is too long.
        /// </summary>
        /// <param name="address">What address to read from.</param>
        /// <param name="size">Size of buffer to read string into.</param>
        /// <param name="encoding">String encoding type.</param>
        /// <param name="addressing">Module addressing mode.</param>
        /// <returns></returns>
        public string ReadNullTerminatedString(
            IntPtr address,
            int size,
            Encoding encoding,
            AddressingMode addressing = AddressingMode.Absolute
        ) {
            // Get entire string buffer as string.
            string value = ReadStringRaw(address, size, encoding, addressing);

            // Find the null terminator and chop the string there.
            int nullTerminatorIndex = value.IndexOf('\0');
            if (nullTerminatorIndex >= 0)
                value = value.Remove(nullTerminatorIndex);
            return value;
        }

        /// <summary>
        /// Reads a string from memory and chops it off at the null terminator.
        /// This version of the method attempts to gradually grow the buffer size until the
        /// null terminator is found.
        /// An upper bound for the buffer is set at <paramref name="maximumSize"/>.
        /// </summary>
        /// <param name="address">What address to read from.</param>
        /// <param name="size">Initial buffer size.</param>
        /// <param name="maximumSize">Maximum allowed buffer size before giving up.</param>
        /// <param name="encoding">String encoding type.</param>
        /// <param name="addressing">Module addressing mode.</param>
        /// <returns></returns>
        public string GetNullTerminatedString(
            IntPtr address,
            int size,
            int maximumSize,
            Encoding encoding,
            AddressingMode addressing = AddressingMode.Absolute
        ) {
            string value = null;
            int nullTerminatorIndex;
            for (int bufferSize = size; bufferSize <= maximumSize; bufferSize *= 2)
            {
                value = ReadStringRaw(address, bufferSize, encoding, addressing);
                nullTerminatorIndex = value.IndexOf('\0');
                if (nullTerminatorIndex >= 0)
                {
                    // Found end of null terminated string, return it.
                    value = value.Remove(nullTerminatorIndex);
                    return value;
                }
            }

            return value;
        }

        #endregion

        #region Kernel32

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess([In] ProcessAccessFlags dwDesiredAccess, [In] bool bInheritHandle, [In] uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle([In] IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ReadProcessMemory([In] IntPtr hProcess, [In] IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, [In] uint nSize, [Out] out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetExitCodeProcess([In] IntPtr hProcess, [Out] out uint lpExitCode);

        #endregion

        #region Flags

        [Flags]
        enum ProcessAccessFlags : uint
        {
            AllAccess =
                CreateProcess | CreateThread | DuplicateHandle | QueryInformation |
                QueryLimitedInfo | SetInformation | SetQuota | SuspendResume | Terminate |
                MemoryOperation | MemoryRead | MemoryWrite | Synchronize,
            CreateProcess = 0x0080,
            CreateThread = 0x0002,
            DuplicateHandle = 0x0040,
            QueryInformation = 0x0400,
            QueryLimitedInfo = 0x1000,
            SetInformation = 0x0200,
            SetQuota = 0x0100,
            SuspendResume = 0x0800,
            Terminate = 0x0001,
            MemoryOperation = 0x0008,   // PROCESS_VM_OPERATION
            MemoryRead = 0x0010,        // PROCESS_VM_READ
            MemoryWrite = 0x0020,       // PROCESS_VM_WRITE
            Synchronize = 0x00100000
        }

        #endregion
    }
}
