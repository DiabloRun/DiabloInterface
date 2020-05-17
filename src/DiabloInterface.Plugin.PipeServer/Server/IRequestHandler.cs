using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public interface IRequestHandler
    {
        Response HandleRequest(Request request, IList<string> arguments);
    }
}
