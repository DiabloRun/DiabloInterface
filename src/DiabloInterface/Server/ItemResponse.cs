using System.Collections.Generic;

namespace DiabloInterface.Server
{
    class ItemResponseData
    {
        public string ItemName { get; set; }
        public List<string> Properties { get; set; }

        public ItemResponseData()
        {
            Properties = new List<string>();
        }
    }

    class ItemResponse
    {
        public bool Success { get; set; }
        public List<ItemResponseData> Items { get; set; }

        public ItemResponse()
        {
            Items = new List<ItemResponseData>();
        }
    }
}
