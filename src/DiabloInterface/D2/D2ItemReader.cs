using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiabloInterface.D2
{
    class D2ItemReader
    {
        ProcessMemoryReader reader;
        D2MemoryAdressTable memory;

        Dictionary<int, D2ItemDescription> cachedDescriptions;

        ModifierTable magicModifiers;
        ModifierTable rareModifiers;

        public D2ItemReader(ProcessMemoryReader reader, D2MemoryAdressTable memory)
        {
            this.reader = reader;
            this.memory = memory;

            cachedDescriptions = new Dictionary<int, D2ItemDescription>();

            magicModifiers = reader.Read<ModifierTable>(memory.MagicModifierTable, AddressingMode.Relative);
            rareModifiers = reader.Read<ModifierTable>(memory.RareModifierTable, AddressingMode.Relative);
        }

        public void ResetCache()
        {
            cachedDescriptions.Clear();
        }

        private string LookupStringIdentifierTable(ushort identifier, IntPtr indexerTable, IntPtr addressTable)
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

        public string LookupStringIdentifier(ushort identifier)
        {
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
            return LookupStringIdentifierTable(identifier, indexerTable, addressTable);
        }

        public bool IsValidItem(D2Unit item)
        {
            return (item != null && item.eType == D2UnitType.Item);
        }

        public string GetItemName(D2Unit item)
        {
            if (!IsValidItem(item)) return null;

            // The hash code for the item name lies in the description table.
            var description = GetItemDescription(item);
            if (description == null) return null;

            return LookupStringIdentifier(description.NameHashCode);
        }

        private T LookupModifierTable<T>(ModifierTable table, ushort index) where T : class
        {
            // Handle invalid table data.
            if (table == null || table.Memory.IsNull)
                return null;
            // Handle index out of range.
            if (index == 0 || index > table.Length)
                return null;

            // Byte offset into array.
            int offset = (index - 1) * Marshal.SizeOf<T>();
            IntPtr modifierAddress = table.Memory.Address + offset;

            // Read modifier.
            return reader.Read<T>(modifierAddress, AddressingMode.Absolute);
        }

        public MagicModifier LookupMagicModifier(ushort index)
        {
            return LookupModifierTable<MagicModifier>(magicModifiers, index);
        }

        public RareModifier LookupRareModifier(ushort index)
        {
            return LookupModifierTable<RareModifier>(rareModifiers, index);
        }

        public MagicModifier GetMagicPrefixModifier(D2Unit item, int index)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;
            if (index >= itemData.MagicPrefix.Length)
                return null;

            return LookupMagicModifier(itemData.MagicPrefix[index]);
        }

        public MagicModifier GetMagicSuffixModifier(D2Unit item, int index)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;
            if (index >= itemData.MagicSuffix.Length)
                return null;

            return LookupMagicModifier(itemData.MagicSuffix[index]);
        }

        public RareModifier GetRarePrefixModifier(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;

            return LookupRareModifier(itemData.RarePrefix);
        }

        public RareModifier GetRareSuffixModifier(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;

            return LookupRareModifier(itemData.RareSuffix);
        }

        public string GetMagicPrefixName(D2Unit item)
        {
            // Name is always first prefix.
            var prefixModifier = GetMagicPrefixModifier(item, 0);
            if (prefixModifier == null) return null;

            return LookupStringIdentifier(prefixModifier.ModifierNameHash);
        }

        public string GetMagicSuffixName(D2Unit item)
        {
            // Name is always first suffix.
            var suffixModifier = GetMagicSuffixModifier(item, 0);
            if (suffixModifier == null) return null;

            return LookupStringIdentifier(suffixModifier.ModifierNameHash);
        }

        public string GetRarePrefixName(D2Unit item)
        {
            var prefixModifier = GetRarePrefixModifier(item);
            if (prefixModifier == null) return null;

            return LookupStringIdentifier(prefixModifier.ModifierNameHash);
        }

        public string GetRareSuffixName(D2Unit item)
        {
            var suffixModifier = GetRareSuffixModifier(item);
            if (suffixModifier == null) return null;

            return LookupStringIdentifier(suffixModifier.ModifierNameHash);
        }

        public bool IsItemOfQuality(D2Unit item, ItemQuality quality)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return false;
            return itemData.Quality == quality;
        }

        public string GetItemMagicName(D2Unit item)
        {
            if (!IsItemOfQuality(item, ItemQuality.Magic))
                return null;

            var itemName = GetItemName(item);
            var prefixName = GetMagicPrefixName(item);
            var suffixName = GetMagicSuffixName(item);

            // Build item name if modifiers exist.
            StringBuilder nameBuilder = new StringBuilder();
            if (prefixName != null)
            {
                nameBuilder.Append(prefixName);
                nameBuilder.Append(' ');
            }
            nameBuilder.Append(itemName);
            if (suffixName != null)
            {
                nameBuilder.Append(' ');
                nameBuilder.Append(suffixName);
            }

            return nameBuilder.ToString();
        }

        public string GetItemRareName(D2Unit item)
        {
            if (!IsItemOfQuality(item, ItemQuality.Rare))
                return null;

            var itemName = GetItemName(item);
            var prefix = GetRarePrefixName(item);
            var suffix = GetRareSuffixName(item);

            var nameBuilder = new StringBuilder();
            if (prefix != null)
            {
                nameBuilder.Append(prefix);
                nameBuilder.Append(' ');
            }
            if (suffix != null)
            {
                nameBuilder.Append(suffix);
                nameBuilder.Append(' ');
            }
            nameBuilder.Append(itemName);
            return nameBuilder.ToString();
        }

        public bool ItemHasFlag(D2Unit item, ItemFlag flag)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return false;

            return itemData.ItemFlags.HasFlag(flag);
        }

        public string GetRunewordName(D2Unit item)
        {
            if (!IsValidItem(item)) return null;
            var itemData = GetItemData(item);

            // Only if runeword flag is set.
            if (itemData == null || !itemData.ItemFlags.HasFlag(ItemFlag.Runeword))
                return null;

            // When the runeword flag is set, magic prefix 0 is the hash code.
            ushort runewordHash = itemData.MagicPrefix[0];
            return LookupStringIdentifier(runewordHash);
        }

        public string GetItemQualityString(D2Unit item)
        {
            if (!IsValidItem(item)) return null;
            var itemData = GetItemData(item);
            if (itemData == null) return null;

            switch (itemData.Quality)
            {
                case ItemQuality.Superior:
                    return LookupStringIdentifier(0x06BF);
                default: return null;
            }
        }

        public D2ItemDescription GetItemDescription(D2Unit item)
        {
            if (!IsValidItem(item)) return null;

            int itemIndex = item.eClass;

            // Early exit if memory already read.
            if (cachedDescriptions.ContainsKey(itemIndex))
                return cachedDescriptions[itemIndex];

            var descriptionTable = reader.Read<D2ItemDescriptionVector>(memory.ItemDescriptions, AddressingMode.Relative);

            // Must be in range of table.
            if (itemIndex >= descriptionTable.Length)
                return null;

            // Get offset into table.
            int offset = itemIndex * Marshal.SizeOf<D2ItemDescription>();

            // Get description address.
            IntPtr descriptionAddress = descriptionTable.Memory.Address + offset;

            // Read, cache and return.
            var description = reader.Read<D2ItemDescription>(descriptionAddress, AddressingMode.Absolute);
            cachedDescriptions[itemIndex] = description;
            return description;
        }

        public D2ItemData GetItemData(D2Unit item)
        {
            if (!IsValidItem(item)) return null;
            if (item.pUnitData.IsNull) return null;

            return reader.Read<D2ItemData>(item.pUnitData.Address, AddressingMode.Absolute);
        }

        public D2Unit GetUnit(DataPointer pointer)
        {
            if (pointer.IsNull) return null;
            return reader.Read<D2Unit>(pointer.Address, AddressingMode.Absolute);
        }

        public D2Unit GetPreviousItem(D2Unit item)
        {
            if (!IsValidItem(item)) return null;
            var itemData = GetItemData(item);
            if (itemData == null) return null;

            return GetUnit(itemData.PreviousItem);
        }
    }
}
