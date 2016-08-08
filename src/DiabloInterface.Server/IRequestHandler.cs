using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Server
{
    public interface IRequestHandler
    {
        QueryResponse HandleRequest(QueryRequest request, IList<string> arguments);
    }
}
