using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Settings;
using Zutatensuppe.DiabloInterface.Services;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    class PipeServerPluginConfig : PluginConfig
    {
        public bool Enabled { get { return Is("Enabled"); } set { Set("Enabled", value); } }
        public string PipeName { get { return GetString("PipeName"); } set { Set("PipeName", value); } }

        public PipeServerPluginConfig()
        {
            Enabled = true;
            PipeName = "DiabloInterfacePipe";
        }

        public PipeServerPluginConfig(PluginConfig s): this()
        {
            if (s != null)
            {
                Enabled = s.Is("Enabled");
                PipeName = s.GetString("PipeName");
            }
        }
    }

    public class PipeServerPlugin : IPlugin
    {
        public string Name => "PipeServer";

        internal PipeServerPluginConfig Cfg { get; private set; } = new PipeServerPluginConfig();

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
            di.settings.Changed += Settings_Changed;
            Init(di.settings.CurrentSettings);
        }

        private void Settings_Changed(object sender, ApplicationSettingsEventArgs e)
        {
            Init(e.Settings);
        }

        private void Init(ApplicationSettings s)
        {
            Cfg = new PipeServerPluginConfig(s.PluginConf(Name));
            ReloadWithCurrentSettings(Cfg);

            Stop();
            
            if (Cfg.Enabled)
            {
                CreateServer(Cfg.PipeName);
            }
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

        public void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
            {
                s.Value.Stop();
            }
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

        PipeServerSettingsRenderer sr;
        public IPluginSettingsRenderer SettingsRenderer()
        {
            if (sr == null)
                sr = new PipeServerSettingsRenderer(this);
            return sr;
        }

        PipeServerDebugRenderer dr;
        public IPluginDebugRenderer DebugRenderer()
        {
            if (dr == null)
                dr = new PipeServerDebugRenderer(this);
            return dr;
        }

        private void ApplyChanges()
        {
            if (sr != null)
                sr.ApplyChanges();
            if (dr != null)
                dr.ApplyChanges();
        }

        private void ReloadWithCurrentSettings(PipeServerPluginConfig cfg)
        {
            if (sr != null)
                sr.Set(cfg);
        }

        internal string StatusTextMsg()
        {
            var txt = "";
            foreach (var s in Servers)
                txt += s.Key + ": " + (s.Value.Running ? "RUNNING" : "NOT RUNNING") + "\n";
            return txt;
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
            {
                Init();
            }
            return txtPipeServer;
        }

        private void Init()
        {
            txtPipeServer = new RichTextBox();
            txtPipeServer.Location = new Point(6, 19);
            txtPipeServer.Size = new Size(272, 62);
            txtPipeServer.TabIndex = 0;
            txtPipeServer.Text = "";

            ApplyChanges();
        }

        public void ApplyChanges()
        {
            if (txtPipeServer.InvokeRequired)
            {
                txtPipeServer.Invoke((Action)(() => ApplyChanges()));
                return;
            }

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
            {
                Init();
            }
            return pluginBox;
        }

        private void Init()
        {
            txtPipeServer = new RichTextBox();
            lblPipeServerStatus = new Label();
            chkPipeServerEnabled = new CheckBox();
            textBoxPipeName = new TextBox();
            labelPipeName = new Label();

            txtPipeServer.Location = new Point(68, 78);
            txtPipeServer.ReadOnly = true;
            txtPipeServer.Size = new Size(288, 34);
            txtPipeServer.Text = "";

            lblPipeServerStatus.AutoSize = true;
            lblPipeServerStatus.Location = new Point(5, 78);
            lblPipeServerStatus.Size = new Size(40, 13);
            lblPipeServerStatus.Text = "Status:";

            chkPipeServerEnabled.AutoSize = true;
            chkPipeServerEnabled.Location = new Point(68, 53);
            chkPipeServerEnabled.Size = new Size(59, 17);
            chkPipeServerEnabled.Text = "Enable";

            textBoxPipeName.Location = new Point(68, 24);
            textBoxPipeName.Margin = new Padding(2);
            textBoxPipeName.Size = new Size(288, 20);

            labelPipeName.AutoSize = true;
            labelPipeName.Location = new Point(3, 24);
            labelPipeName.Margin = new Padding(2, 0, 2, 0);
            labelPipeName.Size = new Size(62, 13);
            labelPipeName.Text = "Pipe Name:";

            pluginBox = new FlowLayoutPanel();
            pluginBox.Controls.Add(txtPipeServer);
            pluginBox.Controls.Add(lblPipeServerStatus);
            pluginBox.Controls.Add(chkPipeServerEnabled);
            pluginBox.Controls.Add(textBoxPipeName);
            pluginBox.Controls.Add(labelPipeName);
            pluginBox.Dock = DockStyle.Fill;

            Set(p.Cfg);
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.Cfg.PipeName != textBoxPipeName.Text
                || p.Cfg.Enabled != chkPipeServerEnabled.Checked;
        }

        public PluginConfig Get()
        {
            var conf = new PipeServerPluginConfig();
            conf.PipeName = textBoxPipeName.Text;
            conf.Enabled = chkPipeServerEnabled.Checked;
            return conf;
        }

        public void Set(PipeServerPluginConfig conf)
        {
            if (pluginBox.InvokeRequired)
            {
                pluginBox.Invoke((Action)(() => Set(conf)));
                return;
            }

            textBoxPipeName.Text = conf.PipeName;
            chkPipeServerEnabled.Checked = conf.Enabled;
        }

        public void ApplyChanges()
        {
            if (pluginBox.InvokeRequired)
            {
                pluginBox.Invoke((Action)(() => ApplyChanges()));
                return;
            }

            txtPipeServer.Text = p.StatusTextMsg();
        }
    }
}
