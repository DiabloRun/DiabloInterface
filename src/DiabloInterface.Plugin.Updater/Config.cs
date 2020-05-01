namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    class Config : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }

        public Config()
        {
            Enabled = true;
        }

        public Config(PluginConfig s) : this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
            }
        }
    }
}
