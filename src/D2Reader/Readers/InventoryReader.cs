using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Inventory;
using Zutatensuppe.D2Reader.Struct.Item;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zutatensuppe.D2Reader.Readers
{
    public class InventoryReader: IInventoryReader
    {
        IProcessMemoryReader processReader;
        public ItemReader ItemReader { get; }

        public InventoryReader(IProcessMemoryReader reader, ItemReader itemReader)
        {
            processReader = reader;
            ItemReader = itemReader;
        }

        public IEnumerable<D2Unit> EnumerateInventoryBackward(D2Unit unit, Func<D2ItemData, bool> filter)
        {
            return from item in EnumerateInventoryBackward(unit)
                   let itemData = ItemReader.GetItemData(item)
                   where itemData != null && filter(itemData)
                   select item;
        }

        public IEnumerable<D2Unit> EnumerateInventoryBackward(D2Unit unit)
        {
            if (unit.pInventory.IsNull)
                yield break;

            var inventory = processReader.Read<D2Inventory>(unit.pInventory);
            if (inventory.pLastItem.IsNull)
                yield break;

            var item = GetUnit(inventory.pLastItem);
            for (; item != null; item = GetPreviousItem(item))
            {
                yield return item;
            }
        }

        public IEnumerable<D2Unit> EnumerateInventoryForward(D2Unit unit)
        {
            if (unit.pInventory.IsNull)
                yield break;

            var inventory = processReader.Read<D2Inventory>(unit.pInventory);
            if (inventory.pFirstItem.IsNull)
                yield break;

            var item = GetUnit(inventory.pFirstItem);
            for (; item != null; item = GetNextItem(item))
            {
                yield return item;
            }
        }

        private D2Unit GetPreviousItem(D2Unit item)
        {
            var itemData = ItemReader.GetItemData(item);
            if (itemData == null) return null;

            return GetUnit(itemData.PreviousItem);
        }

        private D2Unit GetNextItem(D2Unit item)
        {
            var itemData = ItemReader.GetItemData(item);
            if (itemData == null) return null;

            return GetUnit(itemData.NextItem);
        }

        private D2Unit GetUnit(DataPointer pointer)
        {
            if (pointer.IsNull) return null;
            return processReader.Read<D2Unit>(pointer);
        }
    }
}
