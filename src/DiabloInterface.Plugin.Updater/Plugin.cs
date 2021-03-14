using System;
using System.IO;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    public class Plugin : BasePlugin
    {
        public override string Name => "Updater";

        private readonly string VersionFile = "last_found_version.txt";

        private readonly VersionChecker versionChecker = new VersionChecker();

        internal Config Config { get; private set; } = new Config();

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        private IDiabloInterface di;

        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
        }

        public override void Initialize(IDiabloInterface di)
        {
            this.di = di;
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
            AutomaticallyCheckVersion();
        }

        private string LastFoundVersionUrl
        {
            get => File.Exists(VersionFile) ? File.ReadAllText(VersionFile) : null;
            set { if (value != null) { File.WriteAllText(VersionFile, value); } }
        }

        internal void ManuallyCheckVersion()
        {
            var r = versionChecker.CheckForUpdate(di.appInfo.Version, LastFoundVersionUrl, true);
            LastFoundVersionUrl = r.updateUrl;
            Ask(r);
        }

        private void AutomaticallyCheckVersion()
        {
            if (!Config.Enabled) return;

            var r = versionChecker.CheckForUpdate(di.appInfo.Version, LastFoundVersionUrl, false);
            LastFoundVersionUrl = r.updateUrl;
            Ask(r);
        }

        private void Ask(VersionCheckerResult r)
        {
            if (r.target == null || r.question == null || r.title == null) return;

            if (MessageBox.Show(r.question, r.title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(r.target);
                } catch
                {
                    MessageBox.Show(
                        $"Unable to open browser, please go to {r.target} manually.",
                        "Error"
                    );
                }
            }
        }
    }
}
