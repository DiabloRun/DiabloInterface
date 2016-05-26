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
        D2MemoryAddressTable memory;
        StringLookupTable stringReader;

        Dictionary<IntPtr, D2ItemData> cachedItemData;
        Dictionary<int, D2ItemDescription> cachedDescriptions;

        D2GlobalData globals;
        D2SafeArray descriptionTable;
        D2SafeArray lowQualityTable;
        ModifierTable magicModifiers;
        ModifierTable rareModifiers;

        public D2ItemReader(ProcessMemoryReader reader, D2MemoryAddressTable memory)
        {
            this.reader = reader;
            this.memory = memory;
            stringReader = new StringLookupTable(reader, memory);

            cachedItemData = new Dictionary<IntPtr, D2ItemData>();
            cachedDescriptions = new Dictionary<int, D2ItemDescription>();

            globals = reader.Read<D2GlobalData>(reader.ReadAddress32(memory.GlobalData, AddressingMode.Relative));
            lowQualityTable = reader.Read<D2SafeArray>(memory.LowQualityItems, AddressingMode.Relative);
            descriptionTable = reader.Read<D2SafeArray>(memory.ItemDescriptions, AddressingMode.Relative);
            magicModifiers = reader.Read<ModifierTable>(memory.MagicModifierTable, AddressingMode.Relative);
            rareModifiers = reader.Read<ModifierTable>(memory.RareModifierTable, AddressingMode.Relative);
        }

        public void ResetCache()
        {
            cachedDescriptions.Clear();
        }

        private T IndexIntoArray<T>(DataPointer array, int index, uint length) where T : class
        {
            // Index out of range.
            if (index >= length) return null;

            // Indexing is just taking the size of each element added to the base.
            int offset = index * Marshal.SizeOf<T>();
            return reader.Read<T>(array.Address + offset);
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

            return stringReader.GetString(description.NameHashCode);
        }

        private T LookupModifierTable<T>(ModifierTable table, ushort index) where T : class
        {
            // Handle invalid table data, an index of zero is also invalid.
            if (table == null || table.Memory.IsNull || index == 0)
                return null;

            // Read modifier with a zero based index.
            return IndexIntoArray<T>(table.Memory, index - 1, table.Length);
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

            return stringReader.GetString(prefixModifier.ModifierNameHash);
        }

        public string GetMagicSuffixName(D2Unit item)
        {
            // Name is always first suffix.
            var suffixModifier = GetMagicSuffixModifier(item, 0);
            if (suffixModifier == null) return null;

            return stringReader.GetString(suffixModifier.ModifierNameHash);
        }

        public string GetRarePrefixName(D2Unit item)
        {
            var prefixModifier = GetRarePrefixModifier(item);
            if (prefixModifier == null) return null;

            return stringReader.GetString(prefixModifier.ModifierNameHash);
        }

        public string GetRareSuffixName(D2Unit item)
        {
            var suffixModifier = GetRareSuffixModifier(item);
            if (suffixModifier == null) return null;

            return stringReader.GetString(suffixModifier.ModifierNameHash);
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
            var quality = GetItemQuality(item);
            if (quality != ItemQuality.Rare &&
                quality != ItemQuality.Crafted &&
                quality != ItemQuality.Tempered)
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

        public string GetItemUniqueName(D2Unit item)
        {
            var description = GetUniqueItemDescription(item);
            if (description == null) return null;

            return stringReader.GetString(description.StringIdentifier);
        }

        public string GetItemSetName(D2Unit item)
        {
            var description = GetSetItemDescription(item);
            if (description == null) return null;

            return stringReader.GetString(description.StringIdentifier);
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
            return stringReader.GetString(runewordHash);
        }

        public ItemQuality GetItemQuality(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return ItemQuality.Invalid;

            return itemData.Quality;
        }

        public string GetSuperiorItemName(D2Unit item)
        {
            if (!IsItemOfQuality(item, ItemQuality.Superior))
                return null;

            return stringReader.GetString(StringConstants.Superior);
        }

        public string GetLowQualityItemName(D2Unit item)
        {
            var description = GetLowQualityItemDescription(item);
            if (description == null) return null;

            return stringReader.GetString(description.StringIdentifier);
        }

        public string GetFullItemName(D2Unit item)
        {
            if (!IsValidItem(item)) return null;

            string name = null;
            switch (GetItemQuality(item))
            {
                case ItemQuality.Low:
                    name = GetLowQualityItemName(item);
                    name += " " + GetItemName(item);
                    break;
                case ItemQuality.Normal:
                    name = GetItemName(item);
                    break;
                case ItemQuality.Superior:
                    name = GetSuperiorItemName(item);
                    name += GetItemName(item);
                    break;
                case ItemQuality.Magic:
                    name = GetItemMagicName(item);
                    break;
                case ItemQuality.Set:
                    name = GetItemSetName(item);
                    name += " " + GetItemName(item);
                    break;
                case ItemQuality.Rare:
                case ItemQuality.Crafted:
                case ItemQuality.Tempered:
                    name = GetItemRareName(item);
                    break;
                case ItemQuality.Unique:
                    name = GetItemUniqueName(item);
                    name += " " + GetItemName(item);
                    break;
                default: return null;
            }

            // Runeword item name.
            string runeword = GetRunewordName(item);
            if (runeword != null)
            {
                name += ": " + runeword;
            }

            // Ethereal item name.
            if (ItemHasFlag(item, ItemFlag.Ethereal))
            {
                name = "Ethereal " + name;
            }

            return name;
        }

        public D2ItemDescription GetItemDescription(D2Unit item)
        {
            if (!IsValidItem(item)) return null;

            int itemIndex = item.eClass;

            // Early exit if memory already read.
            if (cachedDescriptions.ContainsKey(itemIndex))
                return cachedDescriptions[itemIndex];

            // Read item description from the description table.
            var description = IndexIntoArray<D2ItemDescription>(descriptionTable.Memory, item.eClass, descriptionTable.Length);

            // Cache the value to reduce reads.
            cachedDescriptions[itemIndex] = description;
            return description;
        }

        public D2LowQualityItemDescription GetLowQualityItemDescription(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;
            if (itemData.Quality != ItemQuality.Low) return null;

            // Read low quality item from array.
            var array = lowQualityTable.Memory;
            var index = itemData.FileIndex;
            var count = lowQualityTable.Length;
            return IndexIntoArray<D2LowQualityItemDescription>(array, index, count);
        }

        public D2UniqueItemDescription GetUniqueItemDescription(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;
            if (itemData.Quality != ItemQuality.Unique) return null;

            // Get unique item description from the unique description table.
            var array = globals.UniqueItemDescriptions;
            var index = itemData.FileIndex;
            var count = globals.UniqueItemDescriptionCount;
            return IndexIntoArray<D2UniqueItemDescription>(array, index, count);
        }

        public D2SetItemDescription GetSetItemDescription(D2Unit item)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return null;
            if (itemData.Quality != ItemQuality.Set) return null;

            // Get set item description from the set description table.
            var array = globals.SetItemDescriptions;
            var index = itemData.FileIndex;
            var count = globals.SetItemDescriptionCount;
            return IndexIntoArray<D2SetItemDescription>(array, index, count);
        }

        public bool IsItemInPage(D2Unit item, InventoryPage page)
        {
            var itemData = GetItemData(item);
            if (itemData == null) return false;

            return itemData.InvPage == page;
        }

        public D2ItemData GetItemData(D2Unit item)
        {
            if (!IsValidItem(item)) return null;
            if (item.pUnitData.IsNull) return null;

            D2ItemData itemData;
            if (cachedItemData.TryGetValue(item.pUnitData.Address, out itemData))
                return itemData;

            // Item data not cached, read from memory.
            itemData = reader.Read<D2ItemData>(item.pUnitData.Address);
            cachedItemData[item.pUnitData.Address] = itemData;
            return itemData;
        }
    }
}
