using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class ItemInfo
    {
        public string ItemName { get; set; }
        public List<string> Properties { get; set; }
        public BodyLocation Location { get; set; }

        public ItemInfo()
        {
            Properties = new List<string>();
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

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            List<BodyLocation> equipmentLocations = GetItemLocations(arguments[0]);
            if (equipmentLocations.Count == 0)
                return BuildResponse(new ItemResponsePayload() { IsValidSlot = false });

            var responsePayload = new ItemResponsePayload()
            {
                IsValidSlot = true,
                Items = new List<ItemInfo>()
            };

            dataReader.ItemSlotAction(equipmentLocations, (itemReader, item) => {
                responsePayload.Items.Add(new ItemInfo()
                {
                    ItemName = itemReader.GetFullItemName(item),
                    Properties = itemReader.GetMagicalStrings(item),
                    Location = itemReader.GetItemData(item)?.BodyLoc ?? BodyLocation.None,
                });
            });

            return BuildResponse(responsePayload);
        }

        static QueryResponse BuildResponse(ItemResponsePayload payload)
        {
            return new QueryResponse()
            {
                Status = QueryStatus.Success,
                Payload = payload,
            };
        }

        static List<BodyLocation> GetItemLocations(string itemSlot)
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
