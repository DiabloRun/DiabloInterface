using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class PipeServerPluginConfig : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string PipeName { get { return GetString("PipeName"); } set { SetString("PipeName", value); } }

        public PipeServerPluginConfig()
        {
            Enabled = true;
            PipeName = "DiabloInterfacePipe";
        }

        public PipeServerPluginConfig(PluginConfig s): this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                PipeName = s.GetString("PipeName");
            }
        }
    }

    public class PipeServerPlugin : IPlugin
    {
        public string Name => "PipeServer";

        internal PipeServerPluginConfig config { get; private set; } = new PipeServerPluginConfig();

        public PluginConfig Config { get => config; set {
            config = new PipeServerPluginConfig(value);
            ApplyConfig();
            
            Stop();

            if (config.Enabled)
                CreateServer(config.PipeName);
        }}

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(PipeServerSettingsRenderer)},
            {typeof(IPluginDebugRenderer), typeof(PipeServerDebugRenderer)},
        };

        private ILogger Logger;

        private DiabloInterface di;

        private Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        public PipeServerPlugin(DiabloInterface di)
        {
            Logger = di.Logger(this);
            this.di = di;
        }

        public void Initialize()
        {
            Config = di.settings.CurrentSettings.PluginConf(Name);
        }
        
        private void CreateServer(string pipeName)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var pipeServer = new DiabloInterfaceServer(pipeName);
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"game", () => new GameRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(di.game.DataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(di.game.DataReader));
            pipeServer.Start();
            Servers.Add(pipeName, pipeServer);

            ApplyChanges();
        }

        private void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
                s.Value.Stop();

            Servers.Clear();

            ApplyChanges();
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
            Stop();
        }

        internal string StatusTextMsg()
        {
            var txt = "";
            foreach (var s in Servers)
                txt += s.Key + ": " + (s.Value.Running ? "RUNNING" : "NOT RUNNING") + "\n";
            return txt;
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

    class PipeServerDebugRenderer : IPluginDebugRenderer
    {
        PipeServerPlugin s;
        public PipeServerDebugRenderer(PipeServerPlugin s)
        {
            this.s = s;
        }

        private RichTextBox txtPipeServer;
        public Control Render()
        {
            if (txtPipeServer == null || txtPipeServer.IsDisposed)
                Init();
            return txtPipeServer;
        }

        private void Init()
        {
            txtPipeServer = new RichTextBox();
            txtPipeServer.Location = new Point(6, 19);
            txtPipeServer.Size = new Size(272, 62);
            txtPipeServer.TabIndex = 0;
            txtPipeServer.Text = "";

            ApplyConfig();
            ApplyChanges();
        }

        public void ApplyConfig()
        {
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = s.StatusTextMsg();
        }
    }

    class PipeServerSettingsRenderer : IPluginSettingsRenderer
    {
        private TextBox textBoxPipeName;
        private Label labelPipeName;
        private CheckBox chkPipeServerEnabled;
        private RichTextBox txtPipeServer;
        private Label lblPipeServerStatus;

        private FlowLayoutPanel pluginBox;

        private PipeServerPlugin p;
        public PipeServerSettingsRenderer(PipeServerPlugin s)
        {
            this.p = s;
        }

        public Control Render()
        {
            if (pluginBox == null || pluginBox.IsDisposed)
                Init();
            return pluginBox;
        }

        private void Init()
        {
            labelPipeName = new Label();
            labelPipeName.AutoSize = true;
            labelPipeName.Margin = new Padding(2);
            labelPipeName.Size = new Size(288, 20);
            labelPipeName.Text = "Pipe Name:";

            textBoxPipeName = new TextBox();
            textBoxPipeName.Margin = new Padding(2);
            textBoxPipeName.Size = new Size(288, 20);

            chkPipeServerEnabled = new CheckBox();
            chkPipeServerEnabled.AutoSize = true;
            chkPipeServerEnabled.Size = new Size(288, 20);
            chkPipeServerEnabled.Text = "Enable";

            lblPipeServerStatus = new Label();
            lblPipeServerStatus.AutoSize = true;
            lblPipeServerStatus.Size = new Size(288, 20);
            lblPipeServerStatus.Text = "Status:";

            txtPipeServer = new RichTextBox();
            txtPipeServer.ReadOnly = true;
            txtPipeServer.Size = new Size(288, 34);
            txtPipeServer.Text = "";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(labelPipeName);
            pluginBox.Controls.Add(textBoxPipeName);
            pluginBox.Controls.Add(chkPipeServerEnabled);
            pluginBox.Controls.Add(lblPipeServerStatus);
            pluginBox.Controls.Add(txtPipeServer);
            pluginBox.Dock = DockStyle.Fill;

            ApplyConfig();
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.config.PipeName != textBoxPipeName.Text
                || p.config.Enabled != chkPipeServerEnabled.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new PipeServerPluginConfig();
            conf.PipeName = textBoxPipeName.Text;
            conf.Enabled = chkPipeServerEnabled.Checked;
            return conf;
        }

        public void ApplyConfig()
        {
            textBoxPipeName.Text = p.config.PipeName;
            chkPipeServerEnabled.Checked = p.config.Enabled;
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = p.StatusTextMsg();
        }
    }
}
