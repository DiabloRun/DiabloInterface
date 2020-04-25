using System;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Settings;
using Zutatensuppe.DiabloInterface.Services;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{

    class FileWriterPluginConfig : PluginConfig
    {
        public bool Enabled { get { return Is("Enabled"); } set { Set("Enabled", value); } }
        public string FileFolder { get { return GetString("FileFolder"); } set { Set("FileFolder", value); } }

        public FileWriterPluginConfig()
        {
            Enabled = false;
            FileFolder = "Files";
        }

        public FileWriterPluginConfig(PluginConfig s): this()
        {
            if (s != null)
            {
                Enabled = s.Is("Enabled");
                FileFolder = s.GetString("FileFolder");
            }
        }
    }

    public class FileWriterPlugin : IPlugin
    {
        public string Name => "Filewriter";

        internal FileWriterPluginConfig Cfg { get; private set; } = new FileWriterPluginConfig();

        private ILogger Logger;

        DiabloInterface di;

        public FileWriterPlugin(DiabloInterface di)
        {
            Logger = di.Logger(this);
            Logger.Info("Creating character stat file writer service.");
            this.di = di;
        }

        public void Initialize()
        {
            di.game.DataRead += Game_DataRead;
            di.settings.Changed += Settings_Changed;
            Init(di.settings.CurrentSettings);
        }

        private void Settings_Changed(object sender, ApplicationSettingsEventArgs e)
        {
            Init(e.Settings);
        }

        private void Init(ApplicationSettings s)
        {
            Cfg = new FileWriterPluginConfig(s.PluginConf(Name));
            ReloadWithCurrentSettings(Cfg);
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Cfg.Enabled) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, Cfg.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
        }

        FileWriterSettingsRenderer r;
        public IPluginSettingsRenderer SettingsRenderer()
        {
            if (r == null)
                r = new FileWriterSettingsRenderer(this);
            return r;
        }

        public IPluginDebugRenderer DebugRenderer()
        {
            return null;
        }

        private void ReloadWithCurrentSettings(FileWriterPluginConfig cfg)
        {
            if (r != null)
                r.Set(cfg);
        }
    }

    class FileWriterSettingsRenderer : IPluginSettingsRenderer
    {
        private FileWriterPlugin p;

        private FlowLayoutPanel pluginBox;
        private CheckBox chkBox;

        public FileWriterSettingsRenderer(FileWriterPlugin p)
        {
            this.p = p;
        }

        public Control Render()
        {
            if (pluginBox == null || pluginBox.IsDisposed)
            {
                Init();
            }
            return pluginBox;
        }

        private void Init()
        {
            chkBox = new CheckBox();
            chkBox.AutoSize = true;
            chkBox.Location = new System.Drawing.Point(10, 19);
            chkBox.Size = new System.Drawing.Size(78, 17);
            chkBox.Text = "Create files";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(chkBox);

            Set(p.Cfg);
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.Cfg.Enabled != chkBox.Checked;
        }

        public PluginConfig Get()
        {
            var conf = new FileWriterPluginConfig();
            conf.Enabled = chkBox.Checked;
            return conf;
        }

        public void Set(FileWriterPluginConfig conf)
        {
            if (chkBox.InvokeRequired)
            {
                chkBox.Invoke((Action)(() => Set(conf)));
                return;
            }

            chkBox.Checked = conf.Enabled;
        }

        public void ApplyChanges()
        {
        }
    }
}
