using Zutatensuppe.DiabloInterface.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    class Config : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string FileFolder { get { return GetString("FileFolder"); } set { SetString("FileFolder", value); } }

        public Config()
        {
            Enabled = false;
            FileFolder = "Files";
        }

        public Config(PluginConfig s) : this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                FileFolder = s.GetString("FileFolder");
            }
        }
    }

}
