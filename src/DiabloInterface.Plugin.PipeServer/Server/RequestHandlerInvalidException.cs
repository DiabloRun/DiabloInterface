using System;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class RequestHandlerInvalidException : Exception
    {
        public RequestHandlerInvalidException(string message)
            : base(message) { }
    }
}
