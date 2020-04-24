using System;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class RequestHandlerNullResponseException : Exception
    {
        public string Resource { get; }
        public IRequestHandler Handler { get; }

        public RequestHandlerNullResponseException(string resource, IRequestHandler handler) :
            base($"Request handler '{handler.GetType().Name}' for resource '{resource}' returned null.")
        {
            Resource = resource;
            Handler = handler;
        }
    }
}
