using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Inventory;
using Zutatensuppe.D2Reader.Struct.Item;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zutatensuppe.D2Reader.Readers
{
    public class InventoryReader : IInventoryReader
    {
        private IProcessMemoryReader processReader;

        private UnitReader unitReader { get; }

        public InventoryReader(IProcessMemoryReader reader, UnitReader unitReader)
        {
            processReader = reader;
            this.unitReader = unitReader;
        }

        public IEnumerable<D2Unit> EnumerateInventoryBackward(
            D2Unit unit,
            Func<D2ItemData, D2Unit, bool> filter = null
        ) {
            return EnumerateInventory(
                unit,
                (D2Inventory i) => i.pLastItem,
                (D2ItemData i) => i.PreviousItem,
                filter
            );
        }

        public IEnumerable<D2Unit> EnumerateInventoryForward(
            D2Unit unit,
            Func<D2ItemData, D2Unit, bool> filter = null
        ) {
            return EnumerateInventory(
                unit,
                (D2Inventory i) => i.pFirstItem,
                (D2ItemData i) => i.NextItem,
                filter
            );
        }

        private IEnumerable<D2Unit> EnumerateInventory(
            D2Unit unit,
            Func<D2Inventory, DataPointer> starter,
            Func<D2ItemData, DataPointer> advancer,
            Func<D2ItemData, D2Unit, bool> filter = null
        ) {
            if (unit.pInventory.IsNull)
                yield break;

            var inventory = processReader.Read<D2Inventory>(unit.pInventory);
            if (inventory == null)
                yield break;

            // prevent endless loop that sometimes happens by storing the found guids
            // seems to happen sometimes when characters are changed
            // probably the incoming unit addresses are not the ones from the game at
            // that point anymore
            var guids = new Dictionary<int, bool>();

            var item = GetUnit(starter(inventory));
            while (item != null && !guids.ContainsKey(item.GUID))
            {
                guids[item.GUID] = true;

                var itemData = unitReader.GetItemData(item);
                if (itemData == null)
                    break;

                if (filter == null || filter(itemData, item))
                    yield return item;

                item = GetUnit(advancer(itemData));
            }
        }

        private D2Unit GetUnit(DataPointer pointer)
        {
            return pointer.IsNull ? null : processReader.Read<D2Unit>(pointer);
        }
    }
}
