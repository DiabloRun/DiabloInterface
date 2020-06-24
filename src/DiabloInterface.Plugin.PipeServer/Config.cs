using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    internal class Config : IPluginConfig
    {
        public bool Enabled = false;
        public string PipeName = "DiabloInterfacePipe";
        public uint CacheMs = 2000;
    }
}
