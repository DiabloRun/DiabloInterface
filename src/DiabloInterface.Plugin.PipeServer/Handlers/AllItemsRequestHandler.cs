using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    public class AllItemsRequestHandler : IRequestHandler
    {
        D2DataReader dataReader;

        public AllItemsRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = dataReader.Game?.Character?.Items,
            };
        }
    }
}
