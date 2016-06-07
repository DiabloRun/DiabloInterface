using DiabloInterface.D2.Struct;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace DiabloInterface.D2.Readers
{
    public class ItemReader : UnitReader
    {
        static D2ItemStatCost[] ItemStatCost = null;

        Dictionary<IntPtr, D2ItemData> cachedItemData;
        Dictionary<int, D2ItemDescription> cachedDescriptions;

        D2GlobalData globals;
        D2SafeArray descriptionTable;
        D2SafeArray lowQualityTable;
        ModifierTable magicModifiers;
        ModifierTable rareModifiers;

        ushort[] opNestings;

        public ItemReader(ProcessMemoryReader reader, D2MemoryAddressTable memory) : base(reader, memory)
        {
            cachedItemData = new Dictionary<IntPtr, D2ItemData>();
            cachedDescriptions = new Dictionary<int, D2ItemDescription>();

            globals = reader.Read<D2GlobalData>(reader.ReadAddress32(memory.GlobalData, AddressingMode.Relative));
            lowQualityTable = reader.Read<D2SafeArray>(memory.LowQualityItems, AddressingMode.Relative);
            descriptionTable = reader.Read<D2SafeArray>(memory.ItemDescriptions, AddressingMode.Relative);
            magicModifiers = reader.Read<ModifierTable>(memory.MagicModifierTable, AddressingMode.Relative);
            rareModifiers = reader.Read<ModifierTable>(memory.RareModifierTable, AddressingMode.Relative);

            opNestings = reader.ReadArray<ushort>(globals.OpStatNesting, (int)globals.OpStatNestingCount);

            if (ItemStatCost == null && !globals.ItemStatCost.IsNull)
            {
                ItemStatCost = reader.ReadArray<D2ItemStatCost>(globals.ItemStatCost, (int)globals.ItemStatCostCount);
            }
        }

        public override void ResetCache()
        {
            base.ResetCache();
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

        string GetGrammaticalName(string name, out string grammarCase)
        {
            var grammarMatch = Regex.Match(name, @"^(\[[a-z]+\])");
            if (grammarMatch.Success)
            {
                grammarCase = grammarMatch.Value;
                return name.Substring(grammarMatch.Length);
            }
            else
            {
                grammarCase = null;
                return name;
            }
        }

        string ExtractGrammaticalCase(string value, string grammarCase)
        {
            // No grammar case on english version of the game.
            if (grammarCase == null)
                return value;

            string[] cases = Regex.Split(value, @"(\[[a-z]+\])");
            int caseIndex = Array.IndexOf(cases, grammarCase);
            if (caseIndex >= 0 && caseIndex < cases.Length - 1)
            {
                return cases[caseIndex + 1];
            }
            // Something went wrong, just return it.
            else return value;
        }

        public string GetFullItemName(D2Unit item)
        {
            if (!IsValidItem(item))
                return null;

            string fullName;
            string grammarCase;
            string name = GetGrammaticalName(GetItemName(item), out grammarCase);
            List<string> words = new List<string>();

            // Non identified items just get the base item name.
            if (!ItemHasFlag(item, ItemFlag.Identified))
                return name;

            ItemQuality quality = GetItemQuality(item);
            switch (quality)
            {
                case ItemQuality.Low:
                    // fullName: quality + name
                    fullName = ExtractGrammaticalCase(GetLowQualityItemName(item), grammarCase);
                    fullName += " " + name;
                    break;
                case ItemQuality.Normal:
                    fullName = name;
                    break;
                case ItemQuality.Superior:
                    // fullName: quality + name
                    fullName = ExtractGrammaticalCase(GetSuperiorItemName(item), grammarCase);
                    fullName = " " + name;
                    break;
                case ItemQuality.Magic:
                    // fullName: prefix + name + suffix
                    string magicPrefix = ExtractGrammaticalCase(GetMagicPrefixName(item), grammarCase);
                    string magicSuffix = ExtractGrammaticalCase(GetMagicSuffixName(item), grammarCase);
                    words.Add(magicPrefix);
                    words.Add(name);
                    words.Add(magicSuffix);
                    fullName = string.Join(" ", words.Where(s => !string.IsNullOrEmpty(s)));

                    break;
                case ItemQuality.Set:
                    // fullName: prefix + name
                    fullName = ExtractGrammaticalCase(GetItemSetName(item), grammarCase);
                    fullName += " " + name;
                    break;
                case ItemQuality.Rare:
                case ItemQuality.Crafted:
                case ItemQuality.Tempered:
                    // fullName: prefix + suffix + name
                    string rarePrefix = ExtractGrammaticalCase(GetRarePrefixName(item), grammarCase);
                    string rareSuffix = ExtractGrammaticalCase(GetRareSuffixName(item), grammarCase);
                    if (grammarCase == null)
                    {
                        words.Add(rarePrefix);
                        words.Add(rareSuffix);
                        words.Add(name);
                    }
                    else
                    {
                        words.Add(rareSuffix);
                        words.Add(rarePrefix);
                        words.Add(name);
                    }
                    fullName = string.Join(" ", words.Where(s => !string.IsNullOrEmpty(s)));
                    break;
                case ItemQuality.Unique:
                    // Ignore prefixes for quest items.
                    if (Enum.IsDefined(typeof(D2Data.QuestItemId), item.eClass))
                    {
                        fullName = name;
                    }
                    else
                    {
                        // fullName: prefix + name
                        fullName = ExtractGrammaticalCase(GetItemUniqueName(item), grammarCase);
                        fullName += " " + name;
                    }
                    break;
                default: return null;
            }

            // Runeword item name.
            string runeword = GetRunewordName(item);
            if (runeword != null)
            {
                fullName = name + " [" + runeword + "]";
            }

            // Ethereal item name.
            if (ItemHasFlag(item, ItemFlag.Ethereal))
            {
                fullName = "Ethereal " + fullName;
            }

            return fullName;
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
            if (cachedItemData.TryGetValue(item.pUnitData, out itemData))
                return itemData;

            // Item data not cached, read from memory.
            itemData = reader.Read<D2ItemData>(item.pUnitData);
            cachedItemData[item.pUnitData] = itemData;
            return itemData;
        }

        public string GetDefenseString(D2Unit item)
        {
            int? defense = GetStatValue(item, StatIdentifier.Defense);
            if (defense == null) return null;

            var sb = new StringBuilder(stringReader.GetString(StringConstants.Defense));
            sb.Append(' ');
            sb.Append(defense.Value.ToString());

            return sb.ToString();
        }

        private D2SkillData GetSkillData(ushort skillIdentifier)
        {
            // Skills.txt
            return IndexIntoArray<D2SkillData>(globals.Skills, skillIdentifier, globals.SkillCount);
        }

        private D2CharacterStats GetCharacterData(ushort characterIdentifier)
        {
            return IndexIntoArray<D2CharacterStats>(globals.Characters, characterIdentifier, globals.CharacterCount);
        }

        private string GetSkillName(ushort skillIdentifier)
        {
            D2SkillData skillData = GetSkillData(skillIdentifier);
            if (skillData == null) return null;

            // Skill description table (unknown).
            if (skillData.SkillDesc == 0 || skillData.SkillDesc >= globals.SkillDescriptionCount)
                return null;
            IntPtr skillDescriptionAddress = globals.SkillDescriptions.Address + (int)skillData.SkillDesc * 0x120;
            UInt16 skillNameID = reader.ReadUInt16(skillDescriptionAddress + 8);

            if (skillNameID == 0) return null;
            return stringReader.GetString(skillNameID);
        }

        private int EvaluateStat(D2Stat stat, D2ItemStatCost statCost)
        {
            int value = stat.Value;
            if (statCost.Op >= 2 && statCost.Op <= 5)
            {
                if (statCost.OpBase < 0 || statCost.OpBase >= ItemStatCost.Length)
                    return 0;
                D2ItemStatCost baseStatCost = ItemStatCost[statCost.OpBase];

                // Get the player stat.
                D2Unit player = GetPlayer();
                int playerStat = GetStatValue(player, statCost.OpBase) ?? 0;
                playerStat >>= baseStatCost.ValShift;

                // Evaluate based on the player stat (e.g. player level)
                value = (value * playerStat) >> statCost.OpParam;
            }

            return value >> statCost.ValShift;
        }

        private string GetStatPropertyDescription(List<D2Stat> stats, D2Stat stat)
        {
            if (stat.LoStatID >= ItemStatCost.Length)
                return null;
            var statCost = ItemStatCost[stat.LoStatID];

            byte printFunction = statCost.DescFunc;
            byte printPosition = statCost.DescVal;
            ushort printDescription = statCost.DescStr2;
            ushort printStringId = statCost.DescStrPos;

            // Check if the group of the value is used.
            // Grouping of stats is for stats such as resistances and attribute bonuses.
            //      E.g: "+6 light res, +6 fire res, +6 cold res, +6 psn res" becomes: "+6 All Res"
            if (statCost.DGrp != 0)
            {
                // Check if all stats in the group have the same value.
                // Only print first in the group if the group has the same values.
                // If the values in the group different, print them individually.
                bool lastOfGroup = false;
                bool groupIsSame = true;
                foreach (var groupStatCost in ItemStatCost)
                {
                    if (groupStatCost.DGrp != statCost.DGrp)
                        continue;
                    int index = stats.FindIndex(x => x.LoStatID == groupStatCost.StatId);
                    lastOfGroup = groupStatCost.StatId == stat.LoStatID;
                    if (index < 0 || stats[index].Value != stat.Value)
                    {
                        groupIsSame = false;
                        break;
                    }
                }

                if (groupIsSame)
                {
                    // Only print the last of equal groups.
                    if (!lastOfGroup) return null;

                    printFunction = statCost.DGrpFunc;
                    printPosition = statCost.DGrpVal;
                    printStringId = statCost.DGrpStrPos;
                    printDescription = statCost.DGrpStr2;
                }
            }

            // Gets the evaluated stat value, takes care of stats per player level,
            // value shifts, etc...
            int value = EvaluateStat(stat, statCost);

            int arguments;
            string format = null;

            switch (printFunction)
            {
                case 0x00: return null;
                case 0x01: case 0x06: case 0x0C:
                    if (printPosition == 1)
                        format = "+{0} {1}";
                    else if (printPosition == 2)
                        format = "{1} +{0}";
                    else return null;
                    break;
                case 0x02: case 0x07:
                    if (printPosition == 1)
                        format = "{0}% {1}";
                    else if (printPosition == 2)
                        format = "{1} {0}%";
                    break;
                case 0x03: case 0x09:
                    if (printPosition == 1)
                        format = "{0} {1}";
                    else if (printPosition == 2)
                        format = "{1} {0}";
                    else return stringReader.GetString(printStringId);
                    break;
                case 0x04: case 0x08:
                    if (printPosition == 1)
                        format = "+{0}% {1}";
                    else if (printPosition == 2)
                        format = "{1} +{0}%";
                    break;
                case 0x05: case 0x0A:
                    value = (value * 0x64) >> 7;
                    if (printPosition == 1)
                        format = "{0}% {1}";
                    else if (printPosition == 2)
                        format = "{1} {0}%";
                    break;
                case 0x0B:
                    int quotient = 2500 / value;
                    if (quotient <= 30)
                    {
                        format = stringReader.GetString(StringConstants.RepairsDurability);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        return string.Format(format, value);
                    }
                    else
                    {
                        int duration = (quotient + 12) / 25;
                        format = stringReader.GetString(StringConstants.RepairsDurabilityN);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        return string.Format(format, 1, duration);
                    }
                case 0x0D:
                    {
                        var characterData = GetCharacterData(stat.HiStatID);
                        if (characterData == null) return null;
                        if (value == 0) value = 1;

                        string allSkills = stringReader.GetString(characterData.AllSkillsStringId);
                        if (allSkills == null) return null;

                        return string.Format("+{0} {1}", value, allSkills);
                    }
                case 0x0E:
                    {
                        ushort characterId = (ushort)(stat.HiStatID >> 3);
                        var characterData = GetCharacterData(characterId);
                        if (characterData == null) return null;

                        ushort skillTab = (ushort)(stat.HiStatID & 0x7);
                        if (skillTab > 2) return null;

                        ushort skillTabStringId = characterData.SkillTabStrings[skillTab];
                        format = stringReader.GetString(skillTabStringId);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        if (arguments != 1) return null;

                        string classRestriction = stringReader.GetString(characterData.StrClassOnly);
                        return string.Format(format, value) + " " + classRestriction;
                    }
                case 0x14: case 0x15:
                    if (printPosition == 1)
                        format = "-{0}% {1}";
                    else if (printPosition == 2)
                        format = "{1} -{0}%";
                    break;
                case 0x0F:
                    {
                        ushort skillIdentifier = (ushort)(stat.HiStatID >> (byte)globals.ItemSkillShift);
                        uint skillLevel = stat.HiStatID & globals.ItemSkillMask;
                        string skillName = GetSkillName(skillIdentifier);
                        if (skillName == null) return null;

                        format = stringReader.GetString(printStringId);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        if (format == null || arguments != 3) return null;

                        // Example: "10% Chance to cast level 3 Charged Bolt when struck".
                        return string.Format(format, value, skillLevel, skillName);
                    }
                case 0x10:
                    {
                        string skillName = GetSkillName(stat.HiStatID);
                        if (skillName == null) return null;

                        format = stringReader.GetString(printStringId);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        if (arguments != 2) return null;

                        return string.Format(format, value, skillName);
                    }
                case 0x11: case 0x12:
                    // Increased stat during time of day, this is not actually used on any ingame items,
                    // so I'm gonna spare myself some work and ignore it. :)
                    // Example: "+24% Cold Absorb (Increases during Nighttime)"
                    return null;
                case 0x13:
                    format = stringReader.GetString(printStringId);
                    format = stringReader.ConvertCFormatString(format, out arguments);
                    if (arguments != 1) return null;

                    return string.Format(format, stat.Value);
                case 0x16:
                    {
                        format = stringReader.GetString(printStringId);
                        // This is most likely unused in the real game.
                        ushort monsterPropIndex = stat.HiStatID;
                        if (stat.HiStatID >= globals.MonsterPropCount)
                            monsterPropIndex = 0;
                        IntPtr monsterTypeAddress = globals.MonsterTypes.Address + monsterPropIndex * 0xC;
                        ushort monsterNameId = reader.ReadUInt16(monsterTypeAddress + 0xA);
                        string monsterName = stringReader.GetString(monsterNameId);

                        if (printPosition == 1)
                            return string.Format("+{0}% {1}: {2}", value, format, monsterName);
                        else if (printPosition == 2)
                            return string.Format("{1} +{0}%: {2}", value, format, monsterName);
                        else return null;
                    }
                case 0x17:
                    // This is for affixes with bonus stats against specific monsters.
                    // Assumed output: "+10% Attack Rating (versus:|against|vs) Skeletons"
                    // This is actually not used ingame and we thus ignore it.
                    // Note: HiStatID is the monster type id.
                    return null;
                case 0x18:
                    {
                        StringBuilder sb = new StringBuilder();

                        ushort skillIdentifier = (ushort)(stat.HiStatID >> (byte)globals.ItemSkillShift);
                        uint skillLevel = stat.HiStatID & globals.ItemSkillMask;

                        // Example: "Level 3"
                        sb.Append(stringReader.GetString(StringConstants.ItemSkillLevel));
                        sb.Append(" ");
                        sb.Append(skillLevel);
                        sb.Append(" ");

                        string skillName = GetSkillName(skillIdentifier);
                        if (skillName == null) return null;

                        // Example: "Level 3 Charged Bolt"
                        sb.Append(skillName);
                        sb.Append(" ");

                        // Get skill charges.
                        format = stringReader.GetString(printStringId);
                        format = stringReader.ConvertCFormatString(format, out arguments);
                        int maximumCharges = (value >> 8) & 0xFF;
                        int currentCharges = (value & 0x0FF);

                        // Example: "Level 3 Charged Bolt (27/27 Charges)"
                        sb.AppendFormat(format, currentCharges, maximumCharges);
                        return sb.ToString();
                    }
                case 0x19: case 0x1A:
                    // Probably unused ingame, but logic is pretty simple.
                    value *= -1;
                    if (value > 0) goto case 0x01;
                    if (printPosition == 1)
                        format = "{0} {1}";
                    else if (printPosition == 2)
                        format = "{1} {0}";
                    else return null;
                    break;
                case 0x1B:
                    {
                        StringBuilder sb = new StringBuilder(0x100);
                        sb.Append('+');
                        sb.Append(value);
                        sb.Append(' ');
                        sb.Append(stringReader.GetString(StringConstants.BonusTo));
                        sb.Append(' ');

                        string skillName = GetSkillName(stat.HiStatID);
                        if (skillName == null) return null;
                        sb.Append(skillName);

                        D2SkillData skillData = GetSkillData(stat.HiStatID);
                        if (skillData == null) return null;

                        if (skillData.ClassId < 7)
                        {
                            var characterData = GetCharacterData((ushort)skillData.ClassId);
                            if (characterData != null)
                            {
                                sb.Append(' ');
                                sb.Append(stringReader.GetString(characterData.StrClassOnly));
                            }
                        }

                        return sb.ToString();
                    }
                case 0x1C:
                    {
                        // Note: Some unknown things are happening in this case,
                        //       but hopefully it should still work.
                        StringBuilder sb = new StringBuilder(0x100);
                        sb.Append('+');
                        sb.Append(value);
                        sb.Append(' ');
                        sb.Append(stringReader.GetString(StringConstants.BonusTo));
                        sb.Append(' ');
                        string skillName = GetSkillName(stat.HiStatID);

                        if (skillName == null) return null;
                        sb.Append(skillName);

                        return sb.ToString();
                    }
                default: break;
            }

            if (format != null)
            {
                string description = stringReader.GetString(printStringId);
                description = string.Format(format, value, description);

                if (printFunction >= 0x6 && printFunction < 0xA || printFunction == 0x15)
                {
                    string extraDescription;
                    if (printDescription == 0x1506)
                        extraDescription = stringReader.GetString(0x2B53);
                    else extraDescription = stringReader.GetString(printDescription);

                    // Example: ... + "(Increases with Character Level)"
                    description += " " + extraDescription;
                }

                return description;
            }
            return null;
        }

        public List<string> GetMagicalStrings(D2Unit item)
        {
            if (item == null) return null;

            // Combine stats in different states.
            List<D2Stat> stats = new List<D2Stat>(16);
            CombineNodeStats(stats, FindStatListNode(item, 0x00));
            CombineNodeStats(stats, FindStatListNode(item, 0xAB));

            // Helper for iterating item inventory.
            Func<D2Unit, D2Unit> getNextInventoryItem = subItem => {
                var subItemData = GetItemData(subItem);
                if (subItemData == null) return null;
                if (subItemData.NextItem.IsNull) return null;
                return reader.Read<D2Unit>(subItemData.NextItem);
            };

            // Combine with socketed item stats (runes, gems).
            // This is also done for runewords and such, without this an "Ancient's Pledge" runeword
            // would be missing a few resistances...
            if (!item.pInventory.IsNull)
            {
                var inventory = reader.Read<D2Inventory>(item.pInventory);
                if (!inventory.pFirstItem.IsNull)
                {
                    D2Unit subItem = reader.Read<D2Unit>(inventory.pFirstItem);
                    for (; subItem != null; subItem = getNextInventoryItem(subItem))
                    {
                        var node = FindStatListNode(subItem, 0x00);
                        CombineNodeStats(stats, node);
                    }
                }
            }

            // Perform special handling for some stats such as damage ranges.
            // Example: "1-80 lightning damage" comes from 2 stats.
            var rangeData = new RangeStatData(stringReader, stats);
            Func<D2Stat, string> getDescription = stat => {
                string description;
                if (rangeData.TryHandleStat(stat, out description))
                    return description;

                // If not handled specially, do default handling.
                return GetStatPropertyDescription(stats, stat);
            };

            // Only get stat descriptions for stat identifiers contained in the opNestings array.
            var printFilter = new HashSet<ushort>(opNestings);
            return (from stat in stats
                    where printFilter.Contains(stat.LoStatID)
                    let description = getDescription(stat)
                    where description != null
                    select description).ToList();
        }
    }
}
