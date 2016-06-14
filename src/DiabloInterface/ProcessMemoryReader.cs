using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace DiabloInterface
{
    public enum AddressingMode
    {
        Absolute,
        Relative
    }

    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException(string processName, string moduleName) :
            base(string.Format("Could not find process {0}[{1}]", processName, moduleName))
        { }
    }

    public class ProcessMemoryReadException : Exception
    {
        public ProcessMemoryReadException(IntPtr address) :
            base(string.Format("Failed to read memory at: 0x{0:X8}", address.ToInt64()))
        { }
    }

    public class ProcessMemoryReader : IDisposable
    {
        const uint PROCESS_STILL_ACTIVE = 259;

        IntPtr baseAddress;
        IntPtr processHandle;

        public bool IsValid
        {
            get
            {
                uint exitCode = 0;
                if (!GetExitCodeProcess(processHandle, out exitCode))
                    return false;
                return exitCode == PROCESS_STILL_ACTIVE;
            }
        }

        public ProcessMemoryReader(string processName, string moduleName)
        {
            bool foundModule = false;
            uint foundProcessId = 0;
            IntPtr foundBaseAddress = IntPtr.Zero;

            Process[] processes = Process.GetProcessesByName(processName);

            try
            {
                foreach (var process in processes)
                {
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (module.ModuleName == moduleName)
                        {
                            foundModule = true;
                            foundProcessId = (uint)process.Id;
                            foundBaseAddress = module.BaseAddress;
                        }
                    }
                }
            } catch
            {
                throw new ProcessNotFoundException(processName, moduleName);
            }

            // Throw if the module cannot be found.
            if (!foundModule) throw new ProcessNotFoundException(processName, moduleName);

            // Open up handle.
            baseAddress = foundBaseAddress;
            ProcessAccessFlags flags = ProcessAccessFlags.QueryLimitedInfo | ProcessAccessFlags.MemoryRead;
            processHandle = OpenProcess(flags, false, foundProcessId);

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
            if (processHandle != IntPtr.Zero) {
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
                return new IntPtr((long)baseAddress + (long)address);
            // Already in absolute form.
            else return address;
        }

        public IntPtr ResolveAddressPath(IntPtr baseAddress, int[] pathOffsets, AddressingMode addressing = AddressingMode.Absolute)
        {
            if (pathOffsets == null)
                return baseAddress;

            IntPtr address = baseAddress;
            foreach (int offset in pathOffsets)
            {
                // Read at the current address and then offset.
                address = ReadAddress32(address, addressing);
                address = IntPtr.Add(address, offset);

                // Assume all subsequent addressing to be absolute.
                addressing = AddressingMode.Absolute;
            }

            return address;
        }

        public byte[] Read(IntPtr address, int size, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            uint bytesRead;
            byte[] data = new byte[size];
            address = ResolveAddress(address, addressingMode);
            bool success = ReadProcessMemory(processHandle, address, data, (uint)data.Length, out bytesRead);

            // Make sure we read successfully.
            if (!success || bytesRead != (uint)data.Length)
                throw new ProcessMemoryReadException(address);
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

        public T[] ReadArray<T>(IntPtr address, int length, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            // Read array memory.
            int elementSize = Marshal.SizeOf<T>();
            byte[] buffer = Read(address, elementSize * length, addressingMode);

            T[] array = new T[length];
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
            var buffer = Read(address, 2, addressingMode);
            return BitConverter.ToInt16(buffer, 0);
        }

        public ushort ReadUInt16(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            var buffer = Read(address, 2, addressingMode);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public int ReadInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            var buffer = Read(address, 4, addressingMode);
            return BitConverter.ToInt32(buffer, 0);
        }

        public uint ReadUInt32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            var buffer = Read(address, 4, addressingMode);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public long ReadInt64(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            var buffer = Read(address, 8, addressingMode);
            return BitConverter.ToInt64(buffer, 0);
        }

        public ulong ReadUInt64(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            var buffer = Read(address, 8, addressingMode);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public IntPtr ReadAddress32(IntPtr address, AddressingMode addressingMode = AddressingMode.Absolute)
        {
            return new IntPtr(ReadUInt32(address, addressingMode));
        }

        public string ReadStringRaw(IntPtr address, int size, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute)
        {
            byte[] buffer = Read(address, size, addressing);
            return encoding.GetString(buffer);
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
        public string ReadNullTerminatedString(IntPtr address, int size, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute)
        {
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
        public string GetNullTerminatedString(IntPtr address, int size, int maximumSize, Encoding encoding, AddressingMode addressing = AddressingMode.Absolute)
        {
            string value = null;
            for (int bufferSize = size; bufferSize <= maximumSize; bufferSize *= 2)
            {
                value = ReadStringRaw(address, bufferSize, encoding, addressing);
                int nullTerminatorIndex = value.IndexOf('\0');
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