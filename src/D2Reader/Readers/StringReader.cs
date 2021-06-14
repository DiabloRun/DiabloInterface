using Zutatensuppe.D2Reader.Struct;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Zutatensuppe.D2Reader.Readers
{
    public static class StringConstants
    {
        public const ushort SPACE = 0xF9B; // " "
        public const ushort EOL = 0xF9E; // "\n"

        public const ushort SinglePlayer = 0x13F2; // 5106

        public const ushort Superior = 0x06BF;

        public const ushort Durability              = 0x0D81; // "Durability:"
        public const ushort Defense                 = 0x0D85; // "Defense:"
        public const ushort DurabilityBetween       = 0x0D87; // "of"
        public const ushort FireDamage              = 0x0E1C;
        public const ushort FireDamageRange         = 0x0E1D;
        public const ushort ColdDamage              = 0x0E1E;
        public const ushort ColdDamageRange         = 0x0E1F;
        public const ushort LightningDamage         = 0x0E20;
        public const ushort LightningDamageRange    = 0x0E21;
        public const ushort MagicDamage             = 0x0E22;
        public const ushort MagicDamageRange        = 0x0E23;
        public const ushort PoisonOverTimeSame      = 0x0E24;
        public const ushort PoisonOverTime          = 0x0E25;
        public const ushort DamageRange             = 0x0E27;
        public const ushort BonusTo                 = 0x0FA3; // "to"

        public const ushort EnhancedDamage = 0x2727;

        public const ushort OnlyAmazon      = 0x2AA5;
        public const ushort OnlySorceress   = 0x2AA6;
        public const ushort OnlyNecromancer = 0x2AA7;
        public const ushort OnlyPaladin     = 0x2AA8;
        public const ushort OnlyBarbarian   = 0x2AA9;
        public const ushort OnlyDruid       = 0x2AAA;
        public const ushort OnlyAssassin    = 0x2AAB;

        public const ushort RepairsDurability   = 0x52F9;
        public const ushort RepairsDurabilityN  = 0x52FA;
        public const ushort ItemSkillLevel      = 0x5301;
    }

    class StringTable
    {
        public IntPtr indexTable;
        public IntPtr addressTable;
        public ushort identifierOffset;

        public StringTable(IntPtr i, IntPtr a, ushort io = 0)
        {
            indexTable = i;
            addressTable = a;
            identifierOffset = io;
        }
    }

    public enum Language
    {
        Unknown,
        English,
        German,
        Korean,
        Polish,
    }

    public class StringReader: IStringReader
    {
        IProcessMemoryReader reader;
        GameMemoryTable memory;

        static Dictionary<Language, Dictionary<ushort, string>> Cache = new Dictionary<Language, Dictionary<ushort, string>>();

        public Language Language { get; }

        public StringReader(IProcessMemoryReader reader, GameMemoryTable memory)
        {
            this.reader = reader;
            this.memory = memory;

            Language = DetectLanguage();
            if (!Cache.ContainsKey(Language))
                Cache[Language] = new Dictionary<ushort, string>();
        }

        private Language DetectLanguage()
        {
            switch (LookupStringTable(StringConstants.SinglePlayer))
            {
                case "SINGLE PLAYER": return Language.English;
                case "EINZELSPIELER": return Language.German;
                case "싱글 플레이어": return Language.Korean;
                case "JEDEN GRACZ": return Language.Polish;
                default: return Language.Unknown;
            }
        }

        public string GetString(ushort identifier)
        {
            // Check cache before reading from process.
            string identifierString;
            if (Cache[Language].TryGetValue(identifier, out identifierString))
                return identifierString;

            // Look up the string using the correct tables.
            identifierString = LookupStringTable(identifier);

            // Only cache valid strings.
            if (identifierString != null)
                Cache[Language][identifier] = identifierString;
            return identifierString;
        }

        // the lookup of strings is done in:
        // 1.14D:
        // - game.524930 lookup_string - lookup string by id in given table
        // - game.524A30 get_string_by_identifier - convenience function, used a lot (gets table by identifier and calls lookup_string)
        // - game.525770 the only other function calling lookup_string, which looks up the identifiers:
        //     14D0 => "K"
        //     14D1 => "M"
        //     14D2 => "B"
        //
        private string LookupStringTable(ushort identifier)
        {
            StringTable strTable = GetStringTableByIdentifier(identifier);
            identifier -= strTable.identifierOffset;

            IntPtr indexerTable = reader.ReadAddress32(strTable.indexTable);
            if (indexerTable == IntPtr.Zero) return null;

            IntPtr addressTable = reader.ReadAddress32(strTable.addressTable);
            if (addressTable == IntPtr.Zero) return null;

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
            // because this is also what happens in D2Lang.dll
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
            return reader.GetNullTerminatedString(stringAddress, 0x100, 0x4000, Encoding.Unicode);
        }

        private StringTable GetStringTableByIdentifier(ushort identifier)
        {
            // Handle expansion strings.
            if (identifier >= 0x4E20) // 20.000
            {
                return new StringTable(memory.ExpansionStringIndexerTable, memory.ExpansionStringAddressTable, 0x4E20);
            }

            // Handle patch strings.
            if (identifier >= 0x2710) // 10.000
            {
                return new StringTable(memory.PatchStringIndexerTable, memory.PatchStringAddressTable, 0x2710);
            }

            // Handle default strings.
            return new StringTable(memory.StringIndexerTable, memory.StringAddressTable);
        }

        /// <summary>
        /// Converts a C-format string (sprintf) to a C# format string.
        /// Does not handle precision formats or padding.
        /// Example: "Number: %d" -> "Number: {0}"
        /// </summary>
        /// <param name="input">The C-format string.</param>
        /// <param name="arguments">Outputs the argument count.</param>
        /// <returns>A C# format string.</returns>
        public string ConvertCFormatString(string input, out int arguments)
        {
            arguments = 0;
            if (input == null) return null;

            StringBuilder sb = new StringBuilder(input.Length + 20);

            bool handleArgument = false;
            foreach (char c in input.ToCharArray())
            {
                if (handleArgument)
                {
                    switch (c)
                    {
                        case 'd':
                        case 'f':
                        case 's':
                        case 'u':
                            // Format value.
                            sb.Append('{');
                            sb.Append(arguments);
                            sb.Append('}');

                            arguments += 1;
                            break;
                        case '%':
                            // Percent literal.
                            sb.Append(c);
                            break;
                        default: break;
                    }

                    handleArgument = false;
                }
                else
                {
                    handleArgument = c == '%';
                    if (!handleArgument)
                    {
                        sb.Append(c);
                    }
                }
            }


            // Output the C# format string.
            return sb.ToString();
        }
    }
}
