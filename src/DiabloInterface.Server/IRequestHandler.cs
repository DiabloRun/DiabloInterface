using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Server
{
    public interface IRequestHandler
    {
        Response HandleRequest(Request request, IList<string> arguments);
    }
}
