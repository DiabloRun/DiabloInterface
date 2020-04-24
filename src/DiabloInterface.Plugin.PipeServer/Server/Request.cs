using Newtonsoft.Json.Linq;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class Request
    {
        public string Resource { get; set; }
        public JToken Payload { get; set; }
    }
}
