using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
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
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = ItemInfo.GetItemsByLocations(dataReader, AllItemLocations),
            };
        }
    }
}
