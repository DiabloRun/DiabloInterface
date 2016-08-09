using System;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class RequestHandlerInvalidException : Exception
    {
        public RequestHandlerInvalidException(string message)
            : base(message) { }
    }
}
