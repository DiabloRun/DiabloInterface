using System;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Core.Logging;
using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    class FileWriterPluginConfig : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string FileFolder { get { return GetString("FileFolder"); } set { SetString("FileFolder", value); } }

        public FileWriterPluginConfig()
        {
            Enabled = false;
            FileFolder = "Files";
        }

        public FileWriterPluginConfig(PluginConfig s): this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                FileFolder = s.GetString("FileFolder");
            }
        }
    }

    public class FileWriterPlugin : IPlugin
    {
        public string Name => "Filewriter";

        internal FileWriterPluginConfig config { get; private set; } = new FileWriterPluginConfig();

        public PluginConfig Config { get => config; set {
            config = new FileWriterPluginConfig(value);
            ApplyConfig();
        }}

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(FileWriterSettingsRenderer)},
        };

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
            Config = di.settings.CurrentSettings.PluginConf(Name);
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!config.Enabled) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, config.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
        }
        
        Dictionary<Type, IPluginRenderer> renderers = new Dictionary<Type, IPluginRenderer>();
        private void ApplyChanges()
        {
            foreach (var p in renderers)
                p.Value.ApplyChanges();
        }

        private void ApplyConfig()
        {
            foreach (var p in renderers)
                p.Value.ApplyConfig();
        }

        public T GetRenderer<T>() where T : IPluginRenderer
        {
            var type = typeof(T);
            if (!RendererMap.ContainsKey(type))
                return default(T);
            if (!renderers.ContainsKey(type))
                renderers[type] = (T)Activator.CreateInstance(RendererMap[type], this);
            return (T)renderers[type];
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
                Init();
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

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
            chkBox.Checked = p.config.Enabled;
        }

        public void ApplyChanges()
        {
        }

        public bool IsDirty()
        {
            return p.config.Enabled != chkBox.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new FileWriterPluginConfig();
            conf.Enabled = chkBox.Checked;
            return conf;
        }
    }
}
