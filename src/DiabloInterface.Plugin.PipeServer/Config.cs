namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class Config : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string PipeName { get { return GetString("PipeName"); } set { SetString("PipeName", value); } }

        public Config()
        {
            Enabled = true;
            PipeName = "DiabloInterfacePipe";
        }

        public Config(PluginConfig s) : this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                PipeName = s.GetString("PipeName");
            }
        }
    }

}
