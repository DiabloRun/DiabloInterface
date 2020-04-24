using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.DiabloInterface.Business.Plugin;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class CharacterStatFileWriterService : IPlugin
    {
        public string Name => "Filewriter";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler<IPlugin> Changed;

        public PluginData Data { get; } = new PluginData();

        public PluginConfig Cfg { get; } = new PluginConfig(new Dictionary<string, object>()
        {
            { "Enabled", false },
            { "FileFolder", "Files" },
        });

        private bool CreateFiles => Cfg.GetBool("Enabled");
        private string FileFolder => Cfg.GetString("FileFolder");

        public CharacterStatFileWriterService(ISettingsService settingsService)
        {
            Logger.Info("Creating character stat file writer service.");

            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                UpdateDataFromSettings(args.Settings);
            };
            UpdateDataFromSettings(settingsService.CurrentSettings);
        }

        void UpdateDataFromSettings(ApplicationSettings s)
        {
            Cfg.Apply(s.PluginConf("FileWriter"));
        }

        public void OnSettingsChanged()
        {
        }

        public void OnCharacterCreated(CharacterCreatedEventArgs e)
        {
        }

        public void OnDataRead(DataReadEventArgs e)
        {
            if (!CreateFiles) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }

        public void OnReset()
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
    }

    class FileWriterSettingsRenderer : IPluginSettingsRenderer
    {
        private CharacterStatFileWriterService p;

        private GroupBox pluginBox;
        private CheckBox chkBox;

        public FileWriterSettingsRenderer(CharacterStatFileWriterService p)
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

            pluginBox = new GroupBox();
            pluginBox.Controls.Add(chkBox);
        }

        public bool IsDirty()
        {
            return p.Cfg.GetBool("Enabled") != chkBox.Checked;
        }

        public PluginConfig Get()
        {
            return new PluginConfig(new Dictionary<string, object>()
            {
                {"Enabled", chkBox.Checked },
            });
        }

        public void Set(PluginConfig cfg)
        {
            chkBox.Checked = cfg.GetBool("Enabled");
        }

        public void ApplyChanges()
        {
        }
    }
}
