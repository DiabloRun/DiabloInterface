using Zutatensuppe.D2Reader.Struct;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.D2Reader.Struct.Item.Modifier;
using Zutatensuppe.D2Reader.Struct.Stat;
using Zutatensuppe.D2Reader.Struct.Skill;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Monster;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.D2Reader.Readers
{
    public class UnitReader
    {
        static D2ItemStatCost[] ItemStatCost = null;

        Dictionary<IntPtr, D2ItemData> cachedItemData;
        Dictionary<int, D2ItemDescription> cachedDescriptions;

        D2GlobalData globals;
        D2SafeArray descriptionTable;
        D2SafeArray lowQualityTable;
        readonly ModifierTable magicModifiers;
        readonly ModifierTable rareModifiers;

        private IProcessMemoryReader reader;
        private IStringReader stringReader;
        private ISkillReader skillReader;

        readonly ushort[] opNestings;

        public UnitReader(
            IProcessMemoryReader reader,
            GameMemoryTable memory,
            IStringReader stringReader,
            ISkillReader skillReader
        ) {
            this.reader = reader;
            this.stringReader = stringReader;
            this.skillReader = skillReader;

            cachedItemData = new Dictionary<IntPtr, D2ItemData>();
            cachedDescriptions = new Dictionary<int, D2ItemDescription>();

            globals = reader.Read<D2GlobalData>(reader.ReadAddress32(memory.GlobalData));
            lowQualityTable = reader.Read<D2SafeArray>(memory.LowQualityItems);
            descriptionTable = reader.Read<D2SafeArray>(memory.ItemDescriptions);
            magicModifiers = reader.Read<ModifierTable>(memory.MagicModifierTable);
            rareModifiers = reader.Read<ModifierTable>(memory.RareModifierTable);
            if (globals != null)
            {
                opNestings = reader.ReadArray<ushort>(globals.OpStatNesting, (int)globals.OpStatNestingCount);

                if (ItemStatCost == null && !globals.ItemStatCost.IsNull)
                {
                    ItemStatCost = reader.ReadArray<D2ItemStatCost>(globals.ItemStatCost, (int)globals.ItemStatCostCount);
                }
            }
        }

        public void ResetCache()
        {
            cachedItemData.Clear();
            cachedDescriptions.Clear();
        }

        public List<D2Stat> GetStats(D2Unit unit)
        {
            if (unit == null || unit.StatListNode.IsNull)
                return new List<D2Stat>(0);

            var statsListNode = reader.Read<D2StatListEx>(unit.StatListNode);

            // Get the best available stat array.
            D2StatArray statArray = statsListNode.BestStatsArray();

            // Return the array data and return as list.
            var stats = reader.ReadArray<D2Stat>(statArray.Address, statArray.Length);
            return new List<D2Stat>(stats);
        }

        public Dictionary<StatIdentifier, D2Stat> GetStatsMap(D2Unit unit)
        {
            var tmp = (from stat in GetStats(unit)
                       where stat.HasValidLoStatIdentifier()
                       group stat by (StatIdentifier)stat.LoStatID into g
                       select g);
            return tmp.ToDictionary(x => x.Key, x => x.First());
        }

        public int? GetStatValue(D2Unit unit, ushort statId)
        {
            foreach (D2Stat stat in GetStats(unit))
                if (stat.LoStatID == statId)
                    return stat.Value;

            return null;
        }

        public int? GetStatValue(D2Unit unit, StatIdentifier statId)
        {
            return GetStatValue(unit, (ushort)statId);
        }

        public string GetItemName(Item item)
        {
            // The hash code for the item name lies in the description table.
            var description = GetItemDescription(item.Unit);
            if (description == null) return null;
            return GetString(description.NameHashCode);
        }

        public D2ItemData GetItemData(D2Unit unit)
        {
            if (!unit.IsItem() || unit.UnitData.IsNull) return null;

            // todo: determine if the cache here is actually useful
            //       maybe creates more problems than it solves, pointer changes,
            //       so reading data without clearing cache can result in wrong data
            if (cachedItemData.TryGetValue(unit.UnitData, out D2ItemData itemData))
                return itemData;

            // Item data not cached, read from memory.
            itemData = reader.Read<D2ItemData>(unit.UnitData);

            cachedItemData[unit.UnitData] = itemData;
            return itemData;
        }

        public D2MonsterData GetMonsterData(D2Unit unit)
        {
            if (!unit.IsMonster() || unit.UnitData.IsNull) return null;
            return reader.Read<D2MonsterData>(unit.UnitData);
        }

        public D2ItemDescription GetItemDescription(D2Unit item)
        {
            if (item.eType != D2UnitType.Item)
                throw new Exception("Wrong D2Unit type");
            int eClass = item.eClass;

            // Early exit if memory already read.
            if (cachedDescriptions.ContainsKey(eClass))
                return cachedDescriptions[eClass];

            // Read item description from the description table.
            var description = reader.IndexIntoArray<D2ItemDescription>(
                descriptionTable.Memory,
                eClass,
                descriptionTable.Length
            );

            cachedDescriptions[eClass] = description;
            return description;
        }

        public string GetFullItemName(Item item)
        {
            var n = GetGrammaticalName(GetItemName(item));
            string baseName = n.Item1;
            string grammarCase = n.Item2;

            // Non identified items just get the base item name.
            if (!item.IsIdentified())
                return baseName;

            List<string> words = new List<string>();

            string fullName;
            switch (item.ItemQuality())
            {
                case ItemQuality.Low:
                    // fullName: quality + name
                    fullName = ExtractGrammaticalCase(GetLowQualityItemName(item), grammarCase);
                    fullName += " " + baseName;
                    break;
                case ItemQuality.Normal:
                    fullName = baseName;
                    break;
                case ItemQuality.Superior:
                    // fullName: quality + name
                    fullName = ExtractGrammaticalCase(GetSuperiorItemName(item), grammarCase);
                    fullName = " " + baseName;
                    break;
                case ItemQuality.Magic:
                    // fullName: prefix + name + suffix
                    string magicPrefix = ExtractGrammaticalCase(GetMagicPrefixName(item), grammarCase);
                    string magicSuffix = ExtractGrammaticalCase(GetMagicSuffixName(item), grammarCase);
                    words.Add(magicPrefix);
                    words.Add(baseName);
                    words.Add(magicSuffix);
                    fullName = string.Join(" ", words.Where(s => !string.IsNullOrEmpty(s)));

                    break;
                case ItemQuality.Set:
                    // fullName: prefix + name
                    fullName = ExtractGrammaticalCase(GetItemSetName(item), grammarCase);
                    fullName += " " + baseName;
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
                        words.Add(baseName);
                    }
                    else
                    {
                        words.Add(rareSuffix);
                        words.Add(rarePrefix);
                        words.Add(baseName);
                    }
                    fullName = string.Join(" ", words.Where(s => !string.IsNullOrEmpty(s)));
                    break;
                case ItemQuality.Unique:
                    // Ignore prefixes for quest items.
                    if (Enum.IsDefined(typeof(D2Data.QuestItemId), item.Unit.eClass))
                    {
                        fullName = baseName;
                    }
                    else
                    {
                        // fullName: prefix + name
                        fullName = ExtractGrammaticalCase(GetItemUniqueName(item), grammarCase);
                        fullName += " " + baseName;
                    }
                    break;
                default: return null;
            }

            // Runeword item name.
            string runeword = GetRunewordName(item);
            if (runeword != null)
            {
                fullName = baseName + " [" + runeword + "]";
            }

            // Ethereal item name.
            if (item.IsEthereal())
            {
                // TODO: use correct language (from the game) instead of hardcoded "Ethereal"
                fullName = "Ethereal " + fullName;
            }

            return fullName;
        }
        
        public List<string> GetMagicalStrings(Item item, D2Unit owner, IInventoryReader inventoryReader)
        {
            List<D2Stat> magicalStats = GetMagicalStats(item, inventoryReader);
            if (magicalStats.Count == 0) return new List<string>(0);

            // Perform special handling for some stats such as damage ranges.
            // Example: "1-80 lightning damage" comes from 2 stats.
            var rangeData = new RangeStatData(stringReader, magicalStats);

            string getDescription(D2Stat stat)
            {
                if (rangeData.TryHandleStat(stat, out string description))
                    return description;

                // If not handled specially, do default handling.
                return GetStatPropertyDescription(magicalStats, stat, owner);
            }

            // Only get stat descriptions for stat identifiers contained in the opNestings array.
            // This also orders stats into the same order that the game displays the stats.
            var properties = (from op in opNestings
                              from stat in magicalStats
                              where stat.LoStatID == op
                              let description = getDescription(stat)
                              where description != null
                              // Get the property list in reverse (d2 builds them backwards).
                              select description).Reverse().ToList();

            // Check for sockets.
            int? socketCount = GetStatValue(item.Unit, StatIdentifier.SocketCount);
            if (socketCount.HasValue)
            {
                // TODO: use correct language (from the game) instead of hardcoded "Socketed"
                string sockets = string.Format("Socketed ({0})", socketCount.Value);
                properties.Add(sockets);
            }

            return properties;
        }

        public IEnumerable<Item> GetSocketedItems(Item item, IInventoryReader inventoryReader)
        {
            return inventoryReader.EnumerateInventoryForward(item.Unit);
        }

        private T LookupModifierTable<T>(ModifierTable table, ushort index) where T : class
        {
            // Handle invalid table data, an index of zero is also invalid.
            if (table == null || table.Memory.IsNull || index == 0)
                return null;

            // Read modifier with a zero based index.
            return reader.IndexIntoArray<T>(table.Memory, index - 1, table.Length);
        }

        private MagicModifier LookupMagicModifier(ushort index)
        {
            return LookupModifierTable<MagicModifier>(magicModifiers, index);
        }

        private RareModifier LookupRareModifier(ushort index)
        {
            return LookupModifierTable<RareModifier>(rareModifiers, index);
        }

        private MagicModifier GetMagicPrefixModifier(Item item, int index)
        {
            if (index >= item.ItemData.MagicPrefix.Length) return null;
            return LookupMagicModifier(item.ItemData.MagicPrefix[index]);
        }

        private MagicModifier GetMagicSuffixModifier(Item item, int index)
        {
            if (index >= item.ItemData.MagicSuffix.Length) return null;
            return LookupMagicModifier(item.ItemData.MagicSuffix[index]);
        }

        private RareModifier GetRarePrefixModifier(Item item)
        {
            return LookupRareModifier(item.ItemData.RarePrefix);
        }

        private RareModifier GetRareSuffixModifier(Item item)
        {
            return LookupRareModifier(item.ItemData.RareSuffix);
        }

        private string GetString(ushort hash)
        {
            return stringReader.GetString(hash);
        }

        private string ConvertCFormatString(string input, out int arguments)
        {
            return stringReader.ConvertCFormatString(input, out arguments);
        }

        private string GetMagicPrefixName(Item item)
        {
            // Name is always first prefix.
            var prefixModifier = GetMagicPrefixModifier(item, 0);
            if (prefixModifier == null) return null;
            return GetString(prefixModifier.ModifierNameHash);
        }

        private string GetMagicSuffixName(Item item)
        {
            // Name is always first suffix.
            var suffixModifier = GetMagicSuffixModifier(item, 0);
            if (suffixModifier == null) return null;
            return GetString(suffixModifier.ModifierNameHash);
        }

        private string GetRarePrefixName(Item item)
        {
            var prefixModifier = GetRarePrefixModifier(item);
            if (prefixModifier == null) return null;
            return GetString(prefixModifier.ModifierNameHash);
        }

        private string GetRareSuffixName(Item item)
        {
            var suffixModifier = GetRareSuffixModifier(item);
            if (suffixModifier == null) return null;
            return GetString(suffixModifier.ModifierNameHash);
        }

        private string GetItemUniqueName(Item item)
        {
            var description = GetUniqueItemDescription(item);
            if (description == null) return null;
            return GetString(description.StringIdentifier);
        }

        private string GetItemSetName(Item item)
        {
            var description = GetSetItemDescription(item);
            if (description == null) return null;
            return GetString(description.StringIdentifier);
        }

        private string GetRunewordName(Item item)
        {
            // Only if runeword flag is set.
            if (!item.HasRuneWord()) return null;

            // When the runeword flag is set, magic prefix 0 is the hash code.
            ushort runewordHash = item.ItemData.MagicPrefix[0];
            return GetString(runewordHash);
        }

        private string GetSuperiorItemName(Item item)
        {
            if (item.ItemQuality() != ItemQuality.Superior) return null;
            return GetString(StringConstants.Superior);
        }

        private string GetLowQualityItemName(Item item)
        {
            var description = GetLowQualityItemDescription(item);
            if (description == null) return null;
            return GetString(description.StringIdentifier);
        }

        public Tuple<string, string> GetGrammaticalName(string name)
        {
            if (name == null)
                return new Tuple<string, string>(name, null);

            var grammarMatch = Regex.Match(name, @"^(\[[a-z]+\])");
            if (grammarMatch.Success)
                return new Tuple<string, string>(
                    name.Substring(grammarMatch.Length),
                    grammarMatch.Value
                );

            return new Tuple<string, string>(name, null);
        }

        private string ExtractGrammaticalCase(string value, string grammarCase)
        {
            // null value, so just return null
            if (value == null)
                return value;
            // No grammar case on english version of the game.
            if (grammarCase == null)
                return value;

            string[] cases = Regex.Split(value, @"(\[[a-z]+\])");
            int caseIndex = Array.IndexOf(cases, grammarCase);
            if (caseIndex >= 0 && caseIndex < cases.Length - 1)
                return cases[caseIndex + 1];

            // Something went wrong, just return it.
            return value;
        }

        private D2LowQualityItemDescription GetLowQualityItemDescription(Item item)
        {
            if (item.ItemQuality() != ItemQuality.Low) return null;

            // Read low quality item from array.
            var array = lowQualityTable.Memory;
            var index = item.ItemData.FileIndex;
            var count = lowQualityTable.Length;
            return reader.IndexIntoArray<D2LowQualityItemDescription>(array, index, count);
        }

        private D2UniqueItemDescription GetUniqueItemDescription(Item item)
        {
            if (item.ItemQuality() != ItemQuality.Unique) return null;

            // Get unique item description from the unique description table.
            var array = globals.UniqueItemDescriptions;
            var index = item.ItemData.FileIndex;
            var count = globals.UniqueItemDescriptionCount;
            return reader.IndexIntoArray<D2UniqueItemDescription>(array, index, count);
        }

        private D2SetItemDescription GetSetItemDescription(Item item)
        {
            if (item.ItemQuality() != ItemQuality.Set) return null;

            // Get set item description from the set description table.
            var array = globals.SetItemDescriptions;
            var index = item.ItemData.FileIndex;
            var count = globals.SetItemDescriptionCount;
            return reader.IndexIntoArray<D2SetItemDescription>(array, index, count);
        }

        private D2CharacterStats GetCharacterData(ushort characterIdentifier)
        {
            return reader.IndexIntoArray<D2CharacterStats>(globals.Characters, characterIdentifier, globals.CharacterCount);
        }

        private int EvaluateStat(D2Stat stat, D2ItemStatCost statCost, D2Unit unit)
        {
            if (statCost.Op < 2 || statCost.Op > 5)
                return stat.Value >> statCost.ValShift;

            if (statCost.OpBase < 0 || statCost.OpBase >= ItemStatCost.Length)
                return 0;

            if (unit == null)
                return 0;

            // Get the unit stat.
            int unitStat = GetStatValue(unit, statCost.OpBase) ?? 0;
            unitStat >>= ItemStatCost[statCost.OpBase].ValShift;

            // Evaluate based on the unit stat (e.g. player level)
            int value = (stat.Value * unitStat) >> statCost.OpParam;
            return value >> statCost.ValShift;
        }

        private string GetStatPropertyDescription(List<D2Stat> stats, D2Stat stat, D2Unit unit)
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

            // Gets the evaluated stat value, takes care of stats per unit (player) level,
            // value shifts, etc...
            int value = EvaluateStat(stat, statCost, unit);

            int arguments;
            string format = null;

            switch (printFunction)
            {
                case 0x00: return null;
                case 0x01: case 0x06: case 0x0C:
                    if (printPosition == 1)
                        format = "{0:+#;-#;0} {1}";
                    else if (printPosition == 2)
                        format = "{1} {0:+#;-#;0}";
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
                    else return GetString(printStringId);
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
                        format = GetString(StringConstants.RepairsDurability);
                        format = ConvertCFormatString(format, out arguments);
                        return string.Format(format, value);
                    }
                    else
                    {
                        int duration = (quotient + 12) / 25;
                        format = GetString(StringConstants.RepairsDurabilityN);
                        format = ConvertCFormatString(format, out arguments);
                        return string.Format(format, 1, duration);
                    }
                case 0x0D:
                    {
                        var characterId = stat.HiStatID;
                        var characterData = GetCharacterData(characterId);
                        if (characterData == null) return null;
                        if (value == 0) value = 1;

                        string allSkills = GetString(characterData.AllSkillsStringId);
                        if (allSkills == null) return null;

                        return string.Format("+{0} {1}", value, allSkills);
                    }
                case 0x0E:
                    {
                        var characterId = (ushort)(stat.HiStatID >> 3);
                        var characterData = GetCharacterData(characterId);
                        if (characterData == null) return null;

                        ushort skillTab = (ushort)(stat.HiStatID & 0x7);
                        if (skillTab > 2) return null;

                        ushort skillTabStringId = characterData.SkillTabStrings[skillTab];
                        format = GetString(skillTabStringId);
                        format = ConvertCFormatString(format, out arguments);
                        if (arguments != 1) return null;

                        string classRestriction = GetString(characterData.StrClassOnly);
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
                        string skillName = skillReader.GetSkillName(skillIdentifier);
                        if (skillName == null) return null;

                        format = GetString(printStringId);
                        format = ConvertCFormatString(format, out arguments);
                        if (format == null || arguments != 3) return null;

                        // Example: "10% Chance to cast level 3 Charged Bolt when struck".
                        return string.Format(format, value, skillLevel, skillName);
                    }
                case 0x10:
                    {
                        string skillName = skillReader.GetSkillName(stat.HiStatID);
                        if (skillName == null) return null;

                        format = GetString(printStringId);
                        format = ConvertCFormatString(format, out arguments);
                        if (arguments != 2) return null;

                        return string.Format(format, value, skillName);
                    }
                case 0x11: case 0x12:
                    // Increased stat during time of day, this is not actually used on any ingame items,
                    // so I'm gonna spare myself some work and ignore it. :)
                    // Example: "+24% Cold Absorb (Increases during Nighttime)"
                    return null;
                case 0x13:
                    format = GetString(printStringId);
                    format = ConvertCFormatString(format, out arguments);
                    if (arguments != 1) return null;

                    return string.Format(format, stat.Value);
                case 0x16:
                    {
                        format = GetString(printStringId);
                        // This is most likely unused in the real game.
                        ushort monsterPropIndex = stat.HiStatID;
                        if (stat.HiStatID >= globals.MonsterPropCount)
                            monsterPropIndex = 0;
                        IntPtr monsterTypeAddress = globals.MonsterTypes.Address + monsterPropIndex * 0xC;
                        ushort monsterNameId = reader.ReadUInt16(monsterTypeAddress + 0xA);
                        string monsterName = GetString(monsterNameId);

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
                        sb.Append(GetString(StringConstants.ItemSkillLevel));
                        sb.Append(" ");
                        sb.Append(skillLevel);
                        sb.Append(" ");

                        string skillName = skillReader.GetSkillName(skillIdentifier);
                        if (skillName == null) return null;

                        // Example: "Level 3 Charged Bolt"
                        sb.Append(skillName);
                        sb.Append(" ");

                        // Get skill charges.
                        format = GetString(printStringId);
                        format = ConvertCFormatString(format, out arguments);
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
                        sb.Append(GetString(StringConstants.BonusTo));
                        sb.Append(' ');

                        string skillName = skillReader.GetSkillName(stat.HiStatID);
                        if (skillName == null) return null;
                        sb.Append(skillName);

                        D2SkillData skillData = skillReader.GetSkillData(stat.HiStatID);
                        if (skillData == null) return null;

                        // TODO: remove the magic 7. (without further checking: 0-6 refers to the character classes)
                        if (skillData.ClassId < 7)
                        {
                            var characterId = (ushort)skillData.ClassId;
                            var characterData = GetCharacterData(characterId);
                            if (characterData != null)
                            {
                                sb.Append(' ');
                                sb.Append(GetString(characterData.StrClassOnly));
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
                        sb.Append(GetString(StringConstants.BonusTo));
                        sb.Append(' ');
                        string skillName = skillReader.GetSkillName(stat.HiStatID);

                        if (skillName == null) return null;
                        sb.Append(skillName);

                        return sb.ToString();
                    }
                default: break;
            }

            if (format == null) return null;

            string description = string.Format(format, value, GetString(printStringId));

            if (printFunction >= 0x6 && printFunction < 0xA || printFunction == 0x15)
            {
                string extraDescription;
                if (printDescription == 0x1506)
                    extraDescription = GetString(0x2B53);
                else
                    extraDescription = GetString(printDescription);

                // Example: ... + "(Increases with Character Level)"
                description += " " + extraDescription;
            }

            return description;
        }

        private List<D2Stat> GetMagicalStats(Item item, IInventoryReader inventoryReader)
        {
            // check if item has any stats, otherwise the other stuff doesnt work
            List<D2Stat> itemStats = GetStats(item.Unit);
            if (itemStats.Count == 0) return itemStats;

            // Combine stats in different states.
            // TODO: why find out why 16 is hardcoded here
            List<D2Stat> stats = new List<D2Stat>(16);
            CombineNodeStats(stats, FindStatListNode(item.Unit, 0x00));
            CombineNodeStats(stats, FindStatListNode(item.Unit, 0xAB));

            foreach(Item socketedItem in GetSocketedItems(item, inventoryReader))
            {
                CombineNodeStats(stats, FindStatListNode(socketedItem.Unit, 0x00));
            }

            return stats;
        }

        private D2StatList FindStatListNode(D2Unit unit, uint state)
        {
            if (unit.StatListNode.IsNull)
                return null;

            var statNodeEx = reader.Read<D2StatListEx>(unit.StatListNode);

            // Get the appropriate stat node.
            DataPointer statsPointer = statNodeEx.pMyStats;
            if (statNodeEx.ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                statsPointer = statNodeEx.pMyLastList;

            if (statsPointer.IsNull) return null;

            // Get previous node in the linked list (belonging to this list).
            D2StatList getPreviousNode(D2StatList x)
            {
                if (x.PreviousList.IsNull) return null;
                return reader.Read<D2StatList>(x.PreviousList);
            }

            // Iterate stat nodes until we find the node we're looking for.
            D2StatList statNode = reader.Read<D2StatList>(statsPointer);
            for (; statNode != null; statNode = getPreviousNode(statNode))
            {
                if (statNode.State != state)
                    continue;
                if (statNode.Flags.HasFlag(StatListFlag.HasProperties))
                    break;
            }

            return statNode;
        }

        private void CombineNodeStats(List<D2Stat> stats, D2StatList node)
        {
            if (node == null || node.Stats.Address.IsNull) return;
            D2Stat[] nodeStats = reader.ReadArray<D2Stat>(node.Stats.Address, node.Stats.Length);
            foreach (D2Stat nodeStat in nodeStats)
            {
                int index = stats.FindIndex(x =>
                    x.HiStatID == nodeStat.HiStatID &&
                    x.LoStatID == nodeStat.LoStatID);
                if (index >= 0)
                {
                    // Already have the stat, increase value.
                    stats[index].Value += nodeStat.Value;
                }
                else
                {
                    // Stat not found, add to list.
                    stats.Add(nodeStat);
                }
            }
        }

        public bool IsUnitSelectable(D2Unit unit)
        {
            return UnitClassHasFlag(unit.eClass, UnitClassFlag.IS_SELECTABLE);
        }

        private bool UnitClassHasFlag(int eClass, UnitClassFlag flag)
        {
            // actually game reads this differently, not
            // starting from globals, but actually from position in dll
            // 1.13C: see D2Game.dll + 0x13b0
            // 1.14D: see Game.BNGatewayAccess::CurGateway+35213
            var desc = reader.IndexIntoArray<D2ClassDescription>(
                globals.ClassDescriptions,
                eClass,
                globals.ClassCount
            );
            if (desc == null) return false;

            // __unknown_index_18 is some index in the unknown array of unknown structs
            // TODO: the struct could be created as a class... instead of hardcoding the size here
            if (desc.__unknown_index_18 < 0) return false;
            if (desc.__unknown_index_18 >= globals.UnknownCount_0A98) return false;
            var unknownStructAddr = globals.UnknownPointer_0A90 + 0x134 * desc.__unknown_index_18;
            if (unknownStructAddr.IsNull) return false;

            int offset = ((byte)flag >> 3) + 0x04;
            var value = (int) reader.ReadByte(unknownStructAddr + offset);
            int flagIdx = ((byte)flag) & 0x07;
            return HasFlagByIndex(value, flagIdx);
        }

        private bool HasFlagByIndex(int value, int flagIdx)
        {
            // we could read this flag index to bits array from the
            // game, but it is probably overkill. likely no mod is changing
            // up the flags in the game at that location. locations are:
            // 1.14D: Game.BNGatewayAccess::UpdateGatewaysFromIni+1B5A18
            // 1.13C: ["d2game.dll"] + 0xF8BE0
            var flagBits = new uint[]
            {
                0x00000001,
                0x00000002,
                0x00000004,
                0x00000008,
                0x00000010,
                0x00000020,
                0x00000040,
                0x00000080,
                0x00000100,
                0x00000200,
                0x00000400,
                0x00000800,
                0x00001000,
                0x00002000,
                0x00004000,
                0x00008000,
                0x00010000,
                0x00020000,
                0x00040000,
                0x00080000,
                0x00100000,
                0x00200000,
                0x00400000,
                0x00800000,
                0x01000000,
                0x02000000,
                0x04000000,
                0x08000000,
                0x10000000,
                0x20000000,
                0x40000000,
                0x80000000,
            };
            return (flagBits[flagIdx] & value) > 0;
        }
    }
}
