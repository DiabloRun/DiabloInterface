using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Inventory;
using Zutatensuppe.D2Reader.Struct.Item;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zutatensuppe.D2Reader.Readers
{
    class InventoryReader
    {
        ProcessMemoryReader processReader;
        public ItemReader ItemReader { get; }

        public InventoryReader(ProcessMemoryReader reader, ItemReader itemReader)
        {
            processReader = reader;
            ItemReader = itemReader;
        }

        public IEnumerable<D2Unit> EnumerateInventory(D2Unit unit, Func<D2ItemData, bool> filter)
        {
            return from item in EnumerateInventory(unit)
                   let itemData = ItemReader.GetItemData(item)
                   where itemData != null && filter(itemData)
                   select item;
        }

        public IEnumerable<D2Unit> EnumerateInventory(D2Unit unit)
        {
            var inventory = processReader.Read<D2Inventory>(unit.pInventory.Address);
            if (inventory == null) yield break;

            // Traverse the linked list of inventory items.
            var item = GetUnit(inventory.pLastItem);
            for (; item != null; item = GetPreviousItem(item))
            {
                yield return item;
            }
        }

        private D2Unit GetUnit(DataPointer pointer)
        {
            if (pointer.IsNull) return null;
            return processReader.Read<D2Unit>(pointer.Address, AddressingMode.Absolute);
        }

        private D2Unit GetPreviousItem(D2Unit item)
        {
            var itemData = ItemReader.GetItemData(item);
            if (itemData == null) return null;

            return GetUnit(itemData.PreviousItem);
        }

        // TODO: not used, maybe remove?
        public void ResetCache()
        {
            ItemReader.ResetCache();
        }

        // TODO: not used, maybe remove?
        private string GetEquippedItemSlot(BodyLocation slot)
        {
            // Get all items at the target location.
            var itemsInSlot = from item in EnumerateInventory(ItemReader.GetPlayer())
                              let itemData = ItemReader.GetItemData(item)
                              where itemData != null && itemData.BodyLoc == slot
                              select item;

            // Make sure that there is an item in the inventory slot.
            var itemInSlot = itemsInSlot.SingleOrDefault();
            if (itemInSlot == null) return null;

            // Just return the full name of the item.
            return ItemReader.GetFullItemName(itemInSlot);
        }
    }
}
