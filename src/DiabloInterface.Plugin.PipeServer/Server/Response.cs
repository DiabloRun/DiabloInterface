using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public enum ResponseStatus
    {
        NotFound,
        Error,
        Success,
    }

    public class Response
    {
        public string Resource { get; set; }
        public ResponseStatus Status { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public object Payload { get; set; }
    }
}
