namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class LegacyRequest
    {
        public string EquipmentSlot { get; set; }

        public Request AsRequest()
        {
            var request = new Request();
            request.Resource = $@"items/{EquipmentSlot}";
            return request;
        }
    }
}
