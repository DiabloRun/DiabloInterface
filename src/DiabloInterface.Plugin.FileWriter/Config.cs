using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    internal class Config : IPluginConfig
    {
        public bool Enabled = false;
        public string FileFolder = "Files";
    }
}
