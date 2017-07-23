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
        GameMemoryTable memory;
        ProcessMemoryReader processReader;
        ItemReader itemReader;

        public ItemReader ItemReader { get { return itemReader; } }

        public InventoryReader(ProcessMemoryReader reader, GameMemoryTable memory)
        {
            processReader = reader;
            this.memory = memory;
            itemReader = new ItemReader(processReader, memory);
        }

        public IEnumerable<D2Unit> EnumerateInventory(Func<D2ItemData, bool> filter)
        {
            return from item in EnumerateInventory()
                   let itemData = itemReader.GetItemData(item)
                   where itemData != null && filter(itemData)
                   select item;
        }

        public IEnumerable<D2Unit> EnumerateInventory()
        {
            var inventory = GetPlayerInventory();
            if (inventory == null) yield break;

            // Traverse the linked list of inventory items.
            var item = GetUnit(inventory.pLastItem);
            for (; item != null; item = GetPreviousItem(item))
            {
                yield return item;
            }
        }

        private D2Inventory GetPlayerInventory()
        {
            var playerAddress = processReader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);
            if (playerAddress == IntPtr.Zero) return null;

            var playerUnit = processReader.Read<D2Unit>(playerAddress);
            if (playerUnit == null) return null;

            return processReader.Read<D2Inventory>(playerUnit.pInventory.Address);
        }

        private D2Unit GetUnit(DataPointer pointer)
        {
            if (pointer.IsNull) return null;
            return processReader.Read<D2Unit>(pointer.Address, AddressingMode.Absolute);
        }

        private D2Unit GetPreviousItem(D2Unit item)
        {
            var itemData = itemReader.GetItemData(item);
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
            var itemsInSlot = from item in EnumerateInventory()
                              let itemData = itemReader.GetItemData(item)
                              where itemData != null && itemData.BodyLoc == slot
                              select item;

            // Make sure that there is an item in the inventory slot.
            var itemInSlot = itemsInSlot.SingleOrDefault();
            if (itemInSlot == null) return null;

            // Just return the full name of the item.
            return itemReader.GetFullItemName(itemInSlot);
        }
    }
}
