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

        D2Unit player = null;

        public UnitReader(ProcessMemoryReader reader, D2MemoryAddressTable memory)
        {
            this.reader = reader;
            this.memory = memory;
            stringReader = new StringLookupTable(reader, memory);
        }

        public virtual void ResetCache()
        {
            player = null;
        }

        protected D2Unit GetPlayer()
        {
            if (player == null)
            {
                IntPtr playerAddress = reader.ReadAddress32(memory.PlayerUnit, AddressingMode.Relative);
                player = reader.Read<D2Unit>(playerAddress);
            }

            return player;
        }

        public List<D2Stat> GetStats(D2Unit unit)
        {
            if (unit == null) return null;
            if (unit.StatListNode.IsNull)
                return null;

            var node = reader.Read<D2StatListEx>(unit.StatListNode);

            // Get the best available stat array.
            D2StatArray statArray = node.BaseStats;
            if (node.ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                statArray = node.FullStats;

            // Empty list.
            if (node.FullStats.Length == 0)
                return new List<D2Stat>();

            // Return the array data and return as list.
            var stats = reader.ReadArray<D2Stat>(statArray.Address, statArray.Length);
            return new List<D2Stat>(stats);
        }

        public Dictionary<StatIdentifier, D2Stat> GetStatsMap(D2Unit unit)
        {
            List<D2Stat> stats = GetStats(unit);
            if (stats == null) return null;

            return (from stat in stats
                    where Enum.IsDefined(typeof(StatIdentifier), stat.LoStatID)
                    group stat by (StatIdentifier)stat.LoStatID into g
                    select g).ToDictionary(x => x.Key, x => x.Single());
        }

        public int? GetStatValue(D2Unit unit, ushort statId)
        {
            List<D2Stat> stats = GetStats(unit);
            if (stats == null) return null;

            foreach (D2Stat stat in stats)
            {
                if (stat.LoStatID == statId)
                    return stat.Value;
            }

            return null;
        }

        public int? GetStatValue(D2Unit unit, StatIdentifier statId)
        {
            return GetStatValue(unit, (ushort)statId);
        }

        public D2StatList FindStatListNode(D2Unit item, uint state)
        {
            if (item.StatListNode.IsNull)
                return null;

            var statNodeEx = reader.Read<D2StatListEx>(item.StatListNode);

            // Get the appropriate stat node.
            DataPointer statsPointer = statNodeEx.pMyStats;
            if (statNodeEx.ListFlags.HasFlag(StatListFlag.HasCompleteStats))
                statsPointer = statNodeEx.pMyLastList;

            if (statsPointer.IsNull) return null;

            // Get previous node in the linked list (belonging to this list).
            Func<D2StatList, D2StatList> getPreviousNode = x => {
                if (x.PreviousList.IsNull) return null;
                return reader.Read<D2StatList>(x.PreviousList);
            };

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

        public void CombineNodeStats(List<D2Stat> stats, D2StatList node)
        {
            if (node == null || node.Stats.Address.IsNull) return;
            D2Stat[] nodeStats = reader.ReadArray<D2Stat>(node.Stats.Address, node.Stats.Length);
            foreach (D2Stat nodeStat in nodeStats)
            {
                int index = stats.FindIndex(x =>
                    x.HiStatID == nodeStat.HiStatID &&
                    x.LoStatID == nodeStat.LoStatID);
                // Already have the stat, increase value.
                if (index >= 0)
                {
                    stats[index].Value += nodeStat.Value;
                }
                // Stat not found, add to list.
                else stats.Add(nodeStat);
            }
        }
    }
}
