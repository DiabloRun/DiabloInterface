using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Server
{
    class ItemResponse
    {
        public string ItemName { get; set; }
        public List<string> Properties { get; set; }

        public ItemResponse()
        {
            Properties = new List<string>();
        }
    }

    class QueryResponse
    {
        public bool IsValid { get; set; }
        public bool Success { get; set; }
        public List<ItemResponse> Items { get; set; }

        public QueryResponse()
        {
            Items = new List<ItemResponse>();
        }
    }
}
