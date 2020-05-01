namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    class Config : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string Url { get { return GetString("Url"); } set { SetString("Url", value); } }
        public string Headers { get { return GetString("Headers"); } set { SetString("Headers", value); } }

        public Config()
        {
            Enabled = false;
            Url = "";
            Headers = "";
        }

        public Config(PluginConfig s) : this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                Url = s.GetString("Url");
                Headers = s.GetString("Headers");
            }
        }
    }

}
