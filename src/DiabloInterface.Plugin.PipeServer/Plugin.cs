using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Business.Plugin;
using Zutatensuppe.DiabloInterface.Business.Settings;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer
{
    public class PipeServerPlugin : IPlugin
    {
        public string Name => "PipeServer";

        public event EventHandler<IPlugin> Changed;
        public PluginData Data { get; } = new PluginData(new Dictionary<string, object>()
        {
            { "statuses", new Dictionary<string, bool>() },
        });

        public PluginConfig Cfg { get; } = new PluginConfig(new Dictionary<string, object>()
        {
            { "Enabled", true },
            { "PipeName", "DiabloInterfacePipe"},
        });

        private bool PipeServerEnabled => Cfg.GetBool("Enabled");
        private string PipeName => Cfg.GetString("PipeName");

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, DiabloInterfaceServer> Servers = new Dictionary<string, DiabloInterfaceServer>();

        private D2DataReader dataReader;

        public Dictionary<string, bool> ServerStatuses => Servers.ToDictionary(s => s.Key, s => s.Value.Running);

        public PipeServerPlugin(Business.Services.GameService gameService, Business.Services.SettingsService settingsService)
        {
            dataReader = gameService.DataReader;
            settingsService.SettingsChanged += (object sender, Business.Services.ApplicationSettingsEventArgs args) =>
            {
                Init(args.Settings);
            };

            Init(settingsService.CurrentSettings);
        }

        private void Init(ApplicationSettings s)
        {
            Cfg.Apply(s.PluginConf("HttpClient"));

            Stop();
            if (PipeServerEnabled)
            {
                CreateServer(PipeName);
            }
        }

        private void CreateServer(string pipeName)
        {
            Logger.Info($"Creating Server: {pipeName}");
            var pipeServer = new DiabloInterfaceServer(pipeName);
            // todo: get d2interface version
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"game", () => new GameRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/completed", () => new CompletedQuestsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(dataReader));
            pipeServer.Start();
            Servers.Add(pipeName, pipeServer);

            Data.Set("statuses", ServerStatuses);
            Changed?.Invoke(this, this);
        }

        public void Stop()
        {
            Logger.Info("Stopping all Servers");
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in Servers)
            {
                s.Value.Stop();
            }
            Servers.Clear();

            Data.Set("statuses", ServerStatuses);
            Changed?.Invoke(this, this);
        }

        public void OnSettingsChanged()
        {
        }

        public void OnCharacterCreated(CharacterCreatedEventArgs e)
        {
        }

        public void OnDataRead(DataReadEventArgs e)
        {
        }

        public void OnReset()
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

        internal string StatusTextMsg()
        {
            var txt = "";
            foreach (KeyValuePair<string, bool> s in (Dictionary<string, bool>)Data.Get("statuses"))
            {
                txt += s.Key + ": " + (s.Value ? "RUNNING" : "NOT RUNNING") + "\n";
            }
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

        private GroupBox grpPipeServer;
        private RichTextBox txtPipeServer;
        public Control Render()
        {
            if (grpPipeServer == null || grpPipeServer.IsDisposed)
            {
                Init();
            }
            return grpPipeServer;
        }

        private void Init()
        {
            txtPipeServer = new RichTextBox();
            txtPipeServer.Location = new Point(6, 19);
            txtPipeServer.Size = new Size(272, 62);
            txtPipeServer.TabIndex = 0;
            txtPipeServer.Text = "";

            grpPipeServer = new GroupBox();
            grpPipeServer.Controls.Add(this.txtPipeServer);
            grpPipeServer.Location = new Point(579, 471);
            grpPipeServer.Size = new Size(284, 88);
            grpPipeServer.TabIndex = 15;
            grpPipeServer.TabStop = false;
            grpPipeServer.Text = "Pipe Server";
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

        private GroupBox pluginBox;

        private PipeServerPlugin s;
        public PipeServerSettingsRenderer(PipeServerPlugin s)
        {
            this.s = s;
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

            pluginBox = new GroupBox();
            pluginBox.Controls.Add(txtPipeServer);
            pluginBox.Controls.Add(lblPipeServerStatus);
            pluginBox.Controls.Add(chkPipeServerEnabled);
            pluginBox.Controls.Add(textBoxPipeName);
            pluginBox.Controls.Add(labelPipeName);
            pluginBox.Location = new Point(5, 140);
            pluginBox.Margin = new Padding(2);
            pluginBox.Padding = new Padding(2);
            pluginBox.Size = new Size(361, 123);
            pluginBox.TabStop = false;
            pluginBox.Text = "Pipe Server";
        }

        public bool IsDirty()
        {
            return s.Cfg.GetString("PipeName") != textBoxPipeName.Text
                || s.Cfg.GetBool("Enabled") != chkPipeServerEnabled.Checked;
        }

        public PluginConfig Get()
        {
            return new PluginConfig(new Dictionary<string, object>()
            {
                {"PipeName", textBoxPipeName.Text },
                {"Enabled", chkPipeServerEnabled.Checked },
            });
        }

        public void Set(PluginConfig cfg)
        {
            textBoxPipeName.Text = cfg.GetString("PipeName");
            chkPipeServerEnabled.Checked = cfg.GetBool("Enabled");
        }

        public void ApplyChanges()
        {
            txtPipeServer.Text = s.StatusTextMsg();
        }
    }
}
