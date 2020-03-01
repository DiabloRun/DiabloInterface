using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class ItemInfo
    {
        public string ItemName { get; set; }
        public List<string> Properties { get; set; }
        public BodyLocation Location { get; set; }

        // backwards compatibility with D2ID
        public string BaseItem { get; set; }
        public string Quality { get; set; }

        public static List<ItemInfo> GetItemsByLocations(D2DataReader dataReader, List<BodyLocation> locations)
        {
            List<ItemInfo> Items = new List<ItemInfo>();
            dataReader.ItemSlotAction(locations, (itemReader, item) => {
                Items.Add(new ItemInfo(itemReader, item));
            });
            return Items;
        }

        private ItemInfo(ItemReader itemReader, D2Unit item)
        {
            ItemName = itemReader.GetFullItemName(item);
            Properties = itemReader.GetMagicalStrings(item);
            Location = itemReader.GetItemData(item)?.BodyLoc ?? BodyLocation.None;

            // backwards compatibility with D2ID
            BaseItem = itemReader.GetGrammaticalName(itemReader.GetItemName(item), out string grammerCase);
            Quality = QualityColor(itemReader.GetItemQuality(item));
        }

        private string QualityColor(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.Low:
                case ItemQuality.Normal:
                case ItemQuality.Superior:
                    return "WHITE";
                case ItemQuality.Magic:
                    return "BLUE";
                case ItemQuality.Rare:
                    return "YELLOW";
                case ItemQuality.Crafted:
                case ItemQuality.Tempered:
                    return "ORANGE";
                case ItemQuality.Unique:
                    return "GOLD";
                case ItemQuality.Set:
                    return "GREEN";
                default:
                    return "";
            }
        }
    }

    public class ItemResponsePayload
    {
        public bool IsValidSlot { get; set; }
        public List<ItemInfo> Items { get; set; }
    }

    public class ItemRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public ItemRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = BuildPayload(GetItemLocations(arguments[0])),
            };
        }

        private ItemResponsePayload BuildPayload(List<BodyLocation> locations)
        {
            if (locations.Count == 0)
            {
                return new ItemResponsePayload() { IsValidSlot = false };
            }

            return new ItemResponsePayload()
            {
                IsValidSlot = true,
                Items = ItemInfo.GetItemsByLocations(dataReader, locations)
            };
        }

        private static List<BodyLocation> GetItemLocations(string itemSlot)
        {
            var locations = new List<BodyLocation>();
            if (string.IsNullOrEmpty(itemSlot))
                return locations;

            switch (itemSlot.Trim().ToLowerInvariant())
            {
                case "helm":
                case "head":
                    locations.Add(BodyLocation.Head);
                    break;
                case "armor":
                case "body":
                case "torso":
                    locations.Add(BodyLocation.BodyArmor);
                    break;
                case "amulet":
                    locations.Add(BodyLocation.Amulet);
                    break;
                case "ring":
                case "rings":
                    locations.Add(BodyLocation.RingLeft);
                    locations.Add(BodyLocation.RingRight);
                    break;
                case "belt":
                    locations.Add(BodyLocation.Belt);
                    break;
                case "glove":
                case "gloves":
                case "hand":
                    locations.Add(BodyLocation.Gloves);
                    break;
                case "boot":
                case "boots":
                case "foot":
                case "feet":
                    locations.Add(BodyLocation.Boots);
                    break;
                case "primary":
                case "weapon":
                    locations.Add(BodyLocation.PrimaryLeft);
                    break;
                case "offhand":
                case "shield":
                    locations.Add(BodyLocation.PrimaryRight);
                    break;
                case "weapon2":
                case "secondary":
                    locations.Add(BodyLocation.SecondaryLeft);
                    break;
                case "secondaryshield":
                case "secondaryoffhand":
                case "shield2":
                    locations.Add(BodyLocation.SecondaryRight);
                    break;
            }

            return locations;
        }
    }
}
