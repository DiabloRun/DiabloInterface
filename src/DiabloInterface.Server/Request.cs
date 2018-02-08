using Newtonsoft.Json.Linq;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class Request
    {
        public string Resource { get; set; }
        public JToken Payload { get; set; }
    }
}
