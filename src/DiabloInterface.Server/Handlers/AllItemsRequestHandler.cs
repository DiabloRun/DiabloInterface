using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class AllItemsRequestHandler : IRequestHandler
    {
        D2DataReader dataReader;

        List<BodyLocation> AllItemLocations => new List<BodyLocation>()
        {
            BodyLocation.Head,
            BodyLocation.Amulet,
            BodyLocation.BodyArmor,
            BodyLocation.PrimaryLeft,
            BodyLocation.PrimaryRight,
            BodyLocation.RingLeft,
            BodyLocation.RingRight,
            BodyLocation.Belt,
            BodyLocation.Boots,
            BodyLocation.Gloves,
            BodyLocation.SecondaryLeft,
            BodyLocation.SecondaryRight
        };

        public AllItemsRequestHandler(D2DataReader dataReader)
        {
            if (dataReader == null)
                throw new ArgumentNullException(nameof(dataReader));

            this.dataReader = dataReader;
        }

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            var items = new List<ItemInfo>();
            dataReader.ItemSlotAction(AllItemLocations, (itemReader, item) => {
                items.Add(new ItemInfo()
                {
                    ItemName = itemReader.GetFullItemName(item),
                    Properties = itemReader.GetMagicalStrings(item),
                    Location = itemReader.GetItemData(item)?.BodyLoc ?? BodyLocation.None,
                });
            });

            return new QueryResponse()
            {
                Status = QueryStatus.Success,
                Payload = items,
            };
        }
    }
}
