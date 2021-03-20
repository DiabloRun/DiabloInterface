using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Inventory;
using Zutatensuppe.D2Reader.Struct.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using Zutatensuppe.D2Reader.Models;

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

        public IEnumerable<Item> Filter(
            IEnumerable<Item> enumerable,
            Func<Item, bool> filter = null
        ) {
            return from item in enumerable where filter(item) select item;
        }

        public IEnumerable<Item> EnumerateInventoryBackward(D2Unit unit)
        {
            return EnumerateInventory(
                unit,
                (D2Inventory i) => i.pLastItem,
                (D2ItemData i) => i.PreviousItem
            );
        }

        public IEnumerable<Item> EnumerateInventoryForward(D2Unit unit)
        {
            return EnumerateInventory(
                unit,
                (D2Inventory i) => i.pFirstItem,
                (D2ItemData i) => i.NextItem
            );
        }

        private IEnumerable<Item> EnumerateInventory(
            D2Unit unit,
            Func<D2Inventory, DataPointer> starter,
            Func<D2ItemData, DataPointer> advancer
        ) {
            if (unit == null || unit.pInventory.IsNull)
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
                    yield break;

                yield return new Item { ItemData = itemData, Unit = item };

                item = GetUnit(advancer(itemData));
            }
        }

        private D2Unit GetUnit(DataPointer pointer)
        {
            return pointer.IsNull ? null : processReader.Read<D2Unit>(pointer);
        }

        private List<Item> ReadItemContainer(D2ItemContainerInfo container, ItemContainer name)
        {
            DataPointer[] itemArray = processReader.ReadArray<DataPointer>(container.pArray, container.Width * container.Height);
            var addedItems = new List<DataPointer>();
            var items = new List<Item>();

            for (int i=0; i<itemArray.Length; i++)
            {
                DataPointer ptr = itemArray[i];
                if (ptr.IsNull || addedItems.Contains(ptr))
                    continue;
                D2Unit unit = processReader.Read<D2Unit>(ptr);
                D2ItemData itemData = unitReader.GetItemData(unit);
                D2ItemDescription itemDesc = unitReader.GetItemDescription(unit);
                Item item = new Item {
                    Unit = unit,
                    ItemData = itemData,
                    Location = new ItemLocation {
                        X = i % container.Width,
                        Y = i / container.Width,
                        Width = itemDesc.invWidth,
                        Height = itemDesc.invHeight,
                        BodyLocation = name == ItemContainer.Equipment ? (BodyLocation)i : BodyLocation.None,
                        Container = name
                    }
                };
                items.Add(item);
                addedItems.Add(ptr);
            }
            return items;
        }

        public List<Item> GetAllItems(D2Unit owner)
        {
            var inventory = processReader.Read<D2Inventory>(owner.pInventory);
            if (inventory == null || inventory.pInvInfo.IsNull)
                return new List<Item>(0);
            D2InventoryInfo invInfo = processReader.Read<D2InventoryInfo>(inventory.pInvInfo);

            var items = new List<Item>();
            items.AddRange(ReadItemContainer(invInfo.Equipment, ItemContainer.Equipment));
            items.AddRange(ReadItemContainer(invInfo.Belt,      ItemContainer.Belt));
            items.AddRange(ReadItemContainer(invInfo.Inventory, ItemContainer.Inventory));
            items.AddRange(ReadItemContainer(invInfo.Cube,      ItemContainer.Cube));
            items.AddRange(ReadItemContainer(invInfo.Stash,     ItemContainer.Stash));
            return items;
        }
    }
}
