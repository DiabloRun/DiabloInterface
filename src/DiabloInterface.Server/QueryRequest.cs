using Newtonsoft.Json.Linq;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class QueryRequest
    {
        public string Resource { get; set; }
        public JToken Payload { get; set; }
    }
}
