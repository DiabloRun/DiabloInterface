using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    internal class Config : IPluginConfig
    {
        public bool Enabled = false;
        public string Url = "";
        public string Headers = "";
    }
}
