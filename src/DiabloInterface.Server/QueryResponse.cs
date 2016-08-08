using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Server
{
    public enum QueryStatus
    {
        NotFound,
        Error,
        Success,
    }

    public class QueryResponse
    {
        public string Resource { get; set; }
        public QueryStatus Status { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public object Payload { get; set; }
    }
}
