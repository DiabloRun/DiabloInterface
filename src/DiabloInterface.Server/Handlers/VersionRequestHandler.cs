using System.Collections.Generic;
using System.Reflection;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class VersionRequestHandler : IRequestHandler
    {
        Assembly assembly;

        public VersionRequestHandler(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            var version = assembly.GetName().Version;

            return new QueryResponse()
            {
                Status = QueryStatus.Success,
                Payload = $"{version.Major}.{version.Minor}.{version.Build}"
            };
        }
    }
}
