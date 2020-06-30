namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class Request
    {
        public string Resource { get; set; }
        public object Payload { get; set; }

        internal static string NormalizeResource(string res)
        {
            return res.Trim().ToLowerInvariant();
        }
    }
}
