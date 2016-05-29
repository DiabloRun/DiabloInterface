using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiabloInterface.D2
{
    public static class StringConstants
    {
        public const ushort Superior    = 0x06BF;

        public const ushort Durability          = 0x0D81; // "Durability:"
        public const ushort Defense             = 0x0D85; // "Defense:"
        public const ushort DurabilityBetween   = 0x0D87; // "of"

        public const ushort OnlyAmazon      = 0x2AA5;
        public const ushort OnlySorceress   = 0x2AA6;
        public const ushort OnlyNecromancer = 0x2AA7;
        public const ushort OnlyPaladin     = 0x2AA8;
        public const ushort OnlyBarbarian   = 0x2AA9;
        public const ushort OnlyDruid       = 0x2AAA;
        public const ushort OnlyAssassin    = 0x2AAB;
    }

    class StringLookupTable
    {
        ProcessMemoryReader reader;
        D2MemoryAddressTable memory;

        static Dictionary<ushort, string> StringCache = new Dictionary<ushort, string>();

        public StringLookupTable(ProcessMemoryReader reader, D2MemoryAddressTable memory)
        {
            this.reader = reader;
            this.memory = memory;
        }

        private string LookupStringTable(ushort identifier, IntPtr indexerTable, IntPtr addressTable)
        {
            /*
                Info by [qhris].

                Unicode strings are stored contiguous in a big buffer. To access this buffer
                the address table is used. The address table maps an address index to a string
                in the contiguous buffer.

                Note that all strings end with a null terminator and string length does not seem to
                be stored anywhere.

                To get the address index, the string identifier (unsigned short) is looked up in the
                item info table, along with a few checks to validate.
            */
            var tableInfo = reader.Read<D2StringTableInfo>(indexerTable);

            // We use the identifier 0x1F4 for invalid identifiers.
            if (identifier >= tableInfo.IdentifierCount)
                identifier = 0x01F4;

            // Right after the string info table (in memory) lies the identifier -> address index
            // mapping array.
            IntPtr stringDataRegion = indexerTable + Marshal.SizeOf<D2StringTableInfo>();
            Func<ushort, IntPtr> GetAddressIndexLocation = (index) => stringDataRegion + index * sizeof(ushort);

            // Read address index; must be in valid range.
            ushort addressTableIndex = reader.ReadUInt16(GetAddressIndexLocation(identifier));
            if (addressTableIndex >= tableInfo.AddressTableSize)
                return null;

            // Get the address containing string information.
            IntPtr stringInfoBlock = GetAddressIndexLocation(tableInfo.IdentifierCount);
            IntPtr stringInfoAddress = stringInfoBlock + addressTableIndex * Marshal.SizeOf<D2StringInfo>();

            // Make sure it's in range.
            IntPtr endTest = indexerTable + tableInfo.DataBlockSize;
            if ((long)stringInfoAddress >= (long)endTest)
                return null;

            // Check if the string has been loaded into the address table.
            D2StringInfo stringInfo = reader.Read<D2StringInfo>(stringInfoAddress);
            if (!stringInfo.IsLoadedUnicode) return null;

            // If we get a null string address, just ignore it.
            IntPtr stringAddress = reader.ReadAddress32(addressTable + addressTableIndex * sizeof(uint));
            if (stringAddress == IntPtr.Zero) return null;

            // A maximum of 0x4000 sized buffer **should** be enough to read all strings, bump if too low.
            return reader.GetNullTerminatedString(stringAddress, 0x100, 0x4000, Encoding.Unicode, AddressingMode.Absolute);
        }

        public string GetString(ushort identifier)
        {
            // Check cache before reading from process.
            string identifierString;
            if (StringCache.TryGetValue(identifier, out identifierString))
                return identifierString;

            IntPtr indexerTable = IntPtr.Zero;
            IntPtr addressTable = IntPtr.Zero;

            // Handle expansion strings.
            if (identifier >= 0x4E20)
            {
                identifier -= 0x4E20;
                indexerTable = memory.ExpansionStringIndexerTable;
                addressTable = memory.ExpansionStringAddressTable;
            }
            // Handle patch strings.
            else if (identifier >= 0x2710)
            {
                identifier -= 0x2710;
                indexerTable = memory.PatchStringIndexerTable;
                addressTable = memory.PatchStringAddressTable;
            }
            // Handle default strings.
            else
            {
                indexerTable = memory.StringIndexerTable;
                addressTable = memory.StringAddressTable;
            }

            // Get tables pointers.
            indexerTable = reader.ReadAddress32(indexerTable, AddressingMode.Relative);
            addressTable = reader.ReadAddress32(addressTable, AddressingMode.Relative);
            if (indexerTable == IntPtr.Zero) return null;
            if (addressTable == IntPtr.Zero) return null;

            // Look up the string using the correct tables.
            identifierString = LookupStringTable(identifier, indexerTable, addressTable);

            // Only cache valid strings.
            if (identifierString != null)
                StringCache[identifier] = identifierString;
            return identifierString;
        }
    }
}
