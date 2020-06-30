using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    internal class Config : IPluginConfig
    {
        public bool Enabled = false;
        public string PipeName = "DiabloInterfacePipe";
        public uint CacheMs = 2000;

        internal bool Equals(Config other)
        {
            return Enabled == other.Enabled
                && PipeName == other.PipeName
                && CacheMs == other.CacheMs;
        }
    }
}
