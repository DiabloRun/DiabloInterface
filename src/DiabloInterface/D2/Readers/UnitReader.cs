using DiabloInterface.D2.Struct;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DiabloInterface.D2.Readers
{
    public class UnitReader
    {
        protected ProcessMemoryReader reader;
        protected D2MemoryAddressTable memory;
        protected StringLookupTable stringReader;

        public UnitReader(ProcessMemoryReader reader, D2MemoryAddressTable memory)
        {
            this.reader = reader;
            this.memory = memory;
            stringReader = new StringLookupTable(reader, memory);
        }

        public int GetStatsCount(D2Unit unit)
        {
            if (unit.StatListNode.IsNull)
                return 0;

            var node = reader.Read<D2StatListEx>(unit.StatListNode);
            if (!node.ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                return 0;

            return node.FullStats.Length;
        }

        public D2Stat[] GetStats(D2Unit unit)
        {
            if (unit == null) return null;
            if (unit.StatListNode.IsNull)
                return null;

            var node = reader.Read<D2StatListEx>(unit.StatListNode);
            if (!node.ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                return new D2Stat[] { };
            if (node.FullStats.Length == 0)
                return new D2Stat[] { };

            return reader.ReadArray<D2Stat>(node.FullStats.Array, node.FullStats.Length);
        }

        public Dictionary<StatIdentifier, D2Stat> GetStatsMap(D2Unit unit)
        {
            D2Stat[] stats = GetStats(unit);
            if (stats == null) return null;

            return (from stat in stats
                    where Enum.IsDefined(typeof(StatIdentifier), stat.LoStatID)
                    group stat by (StatIdentifier)stat.LoStatID into g
                    select g).ToDictionary(x => x.Key, x => x.Single());
        }

        public int? GetStatValue(D2Unit unit, StatIdentifier statID)
        {
            D2Stat[] stats = GetStats(unit);
            if (stats == null) return null;

            foreach (D2Stat stat in stats)
            {
                if (stat.IsOfType(statID))
                    return stat.Value;
            }

            return null;
        }
    }
}
