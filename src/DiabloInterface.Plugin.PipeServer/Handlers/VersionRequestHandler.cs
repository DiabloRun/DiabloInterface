using System.Collections.Generic;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    public class VersionRequestHandler : IRequestHandler
    {
        Assembly assembly;

        public VersionRequestHandler(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            var version = assembly.GetName().Version;

            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = $"{version.Major}.{version.Minor}.{version.Build}"
            };
        }
    }
}
