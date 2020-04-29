using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Plugin;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Newtonsoft.Json;

namespace DiabloInterface.Plugin.HttpClient
{
    class HttpClientPluginConfig : PluginConfig
    {
        public bool Enabled { get { return GetBool("Enabled"); } set { SetBool("Enabled", value); } }
        public string Url { get { return GetString("Url"); } set { SetString("Url", value); } }
        public string Headers { get { return GetString("Headers"); } set { SetString("Headers", value); } }

        public HttpClientPluginConfig()
        {
            Enabled = false;
            Url = "";
            Headers = "";
        }

        public HttpClientPluginConfig(PluginConfig s): this()
        {
            if (s != null)
            {
                Enabled = s.GetBool("Enabled");
                Url = s.GetString("Url");
                Headers = s.GetString("Headers");
            }
        }
    }

    public class HttpClientPlugin : IPlugin
    {
        public string Name => "HttpClient";

        internal HttpClientPluginConfig config { get; private set; } = new HttpClientPluginConfig();

        public PluginConfig Config { get => config; set {
            config = new HttpClientPluginConfig(value);
            ApplyConfig();

            Logger.Info(config.Url);

            Stop();
            if (config.Enabled)
                CreateServer(config.Url, config.Headers);
        }}

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(HttpClientSettingsRenderer)},
        };

        private ILogger Logger;

        private Zutatensuppe.DiabloInterface.DiabloInterface di;

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();
        private bool SendingData = false;
        private string Url;
        private string Headers;

        internal string content;

        private RequestBody PrevData = new RequestBody()
        {
            Items = new List<ItemInfo>(),
            Quests = Quests.DefaultCompleteQuestIds,
        };
        
        private List<string> BodyProperties = new List<String>()
        {
            "Area",
            "Difficulty",
            "PlayersX",
            "Name",
            "CharClass",
            "IsHardcore",
            "IsExpansion",
            "IsDead",
            "Deaths",
            "Level",
            "Experience",
            "Strength",
            "Dexterity",
            "Vitality",
            "Energy",
            "FireResist",
            "ColdResist",
            "LightningResist",
            "PoisonResist",
            "Gold",
            "GoldStash",
            "FasterCastRate",
            "FasterHitRecovery",
            "FasterRunWalk",
            "IncreasedAttackSpeed",
            "MagicFind"
        };

        public HttpClientPlugin(Zutatensuppe.DiabloInterface.DiabloInterface di)
        {
            Logger = di.Logger(this);
            this.di = di;
        }

        public void Initialize()
        {
            di.game.DataRead += Game_DataRead;
            Config = di.settings.CurrentSettings.PluginConf(Name);
        }

        private void CreateServer(string url, string headers)
        {
            Logger.Info($"Creating HTTP CLIENT: {url}");
            Url = url;
            Headers = headers;
        }

        private class RequestBody
        {
            public string Headers { get; set; }
            public int? Area { get; set; }
            public GameDifficulty? Difficulty { get; set; }
            public int? PlayersX { get; set; }
            public uint? GameCount { get; set; }
            public uint? CharCount { get; set; }
            public bool? NewCharacter { get; set; }
            public string Name { get; set; }
            public CharacterClass? CharClass { get; set; }
            public bool? IsHardcore { get; set; }
            public bool? IsExpansion { get; set; }
            public bool? IsDead { get; set; }
            public short? Deaths { get; set; }
            public int? Level { get; set; }
            public int? Experience { get; set; }
            public int? Strength { get; set; }
            public int? Dexterity { get; set; }
            public int? Vitality { get; set; }
            public int? Energy { get; set; }
            public int? FireResist { get; set; }
            public int? ColdResist { get; set; }
            public int? LightningResist { get; set; }
            public int? PoisonResist { get; set; }
            public int? Gold { get; set; }
            public int? GoldStash { get; set; }
            public int? FasterCastRate { get; set; }
            public int? FasterHitRecovery { get; set; }
            public int? FasterRunWalk { get; set; }
            public int? IncreasedAttackSpeed { get; set; }
            public int? MagicFind { get; set; }
            public List<ItemInfo> Items { get; set; }
            public List<ItemInfo> AddedItems { get; set; }
            public List<BodyLocation> RemovedItems { get; set; }
            public Dictionary<GameDifficulty, List<QuestId>> Quests { get; set; }
            public Dictionary<GameDifficulty, List<QuestId>> CompletedQuests { get; set; }

            internal string ToJson()
            {
                return JsonConvert.SerializeObject(
                    this,
                    Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                );
            }
        }

        private bool IsSameItem(ItemInfo itemA, ItemInfo itemB)
        {
            if (itemA == null && itemB == null)
            {
                return true;
            }

            if (itemA == null || itemB == null)
            {
                return false;
            }

            return itemA.Class == itemB.Class
                && itemA.ItemName == itemB.ItemName
                && itemA.ItemBaseName == itemB.ItemBaseName
                && itemA.QualityColor == itemB.QualityColor
                && itemA.Location == itemB.Location
                && itemA.Properties.SequenceEqual(itemB.Properties);
        }

        private RequestBody GetDiff(RequestBody newData)
        {
            var noChanges = true;
            var diff = new RequestBody()
            {
                Headers = Headers,
                Name = newData.Name,
                RemovedItems = new List<BodyLocation>(),
                AddedItems = new List<ItemInfo>(),
                CompletedQuests = new Dictionary<GameDifficulty, List<QuestId>>()
            };

            if (!newData.CharCount.Equals(PrevData.CharCount))
            {
                diff.NewCharacter = true;
            }

            if (!newData.GameCount.Equals(PrevData.GameCount))
            {
                PrevData = new RequestBody()
                {
                    Items = new List<ItemInfo>(),
                    Quests = new Dictionary<GameDifficulty, List<QuestId>>() {
                        { GameDifficulty.Normal, new List<QuestId>() },
                        { GameDifficulty.Nightmare, new List<QuestId>() },
                        { GameDifficulty.Hell, new List<QuestId>() }
                    }
                };
            }

            foreach (string propertyName in BodyProperties)
            {
                var property = typeof(RequestBody).GetProperty(propertyName);
                var prevValue = property.GetValue(PrevData);
                var newValue = property.GetValue(newData);

                if (!newValue.Equals(prevValue))
                {
                    noChanges = false;
                    property.SetValue(diff, newValue);
                }
            }

            foreach (var prevItem in PrevData.Items)
            {
                if (!newData.Items.Any(newItem => IsSameItem(prevItem, newItem)))
                {
                    noChanges = false;
                    diff.RemovedItems.Add(prevItem.Location);
                }
            }

            foreach (var newItem in newData.Items)
            {
                if (!PrevData.Items.Any(prevItem => IsSameItem(prevItem, newItem)))
                {
                    noChanges = false;
                    diff.AddedItems.Add(newItem);
                }
            }

            foreach (var pair in Quests.DefaultCompleteQuestIds)
            {
                var completed = newData.Quests[pair.Key].FindAll(id => !PrevData.Quests[pair.Key].Contains(id));

                if (completed.Count() > 0)
                {
                    noChanges = false;
                    diff.CompletedQuests.Add(pair.Key, completed);
                }
            }

            return noChanges ? null : diff;
        }

        async private Task PostJson(string json)
        {
            SendingData = true;

            try
            {
                var response = await Client.PostAsync(Url, new StringContent(json, Encoding.UTF8, "application/json"));
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                content = "Request failed";
            }
            finally
            {
                ApplyChanges();
                SendingData = false;
            }
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (SendingData)
            {
                return;
            }

            var newData = new RequestBody()
            {
                Area = e.Game.Area,
                Difficulty = e.Game.Difficulty,
                PlayersX = e.Game.PlayersX,
                GameCount = e.Game.GameCount,
                CharCount = e.Game.CharCount,
                Name = e.Character.Name,
                CharClass = e.Character.CharClass,
                IsHardcore = e.Character.IsHardcore,
                IsExpansion = e.Character.IsExpansion,
                IsDead = e.Character.IsDead,
                Deaths = e.Character.Deaths,
                Level = e.Character.Level,
                Experience = e.Character.Experience,
                Strength = e.Character.Strength,
                Dexterity = e.Character.Dexterity,
                Vitality = e.Character.Vitality,
                Energy = e.Character.Energy,
                FireResist = e.Character.FireResist,
                ColdResist = e.Character.ColdResist,
                LightningResist = e.Character.LightningResist,
                PoisonResist = e.Character.PoisonResist,
                Gold = e.Character.Gold,
                GoldStash = e.Character.GoldStash,
                FasterCastRate = e.Character.FasterCastRate,
                FasterHitRecovery = e.Character.FasterHitRecovery,
                FasterRunWalk = e.Character.FasterRunWalk,
                IncreasedAttackSpeed = e.Character.IncreasedAttackSpeed,
                MagicFind = e.Character.MagicFind,
                Items = e.Character.Items,
                Quests = e.Quests.CompletedQuestIds,
            };

            var diff = GetDiff(newData);

            if (diff != null)
            {
                PostJson(diff.ToJson());
            }

            PrevData = newData;
        }

        public void Stop()
        {
            Logger.Info("Stopping HTTP client");
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

    class HttpClientSettingsRenderer : IPluginSettingsRenderer
    {
        private HttpClientPlugin p;

        private FlowLayoutPanel pluginBox;
        private RichTextBox txtHttpClientHeaders;
        private Label label6;
        private RichTextBox txtHttpClientStatus;
        private Label label4;
        private CheckBox chkHttpClientEnabled;
        private TextBox textBoxHttpClientUrl;
        private Label label5;

        public HttpClientSettingsRenderer(HttpClientPlugin p)
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
            label6 = new Label();
            label6.AutoSize = true;
            label6.Size = new Size(50, 13);
            label6.Text = "Headers:";

            txtHttpClientHeaders = new RichTextBox();
            txtHttpClientHeaders.Size = new Size(288, 69);
            txtHttpClientHeaders.Text = "";

            label5 = new Label();
            label5.AutoSize = true;
            label5.Size = new Size(288, 13);
            label5.Text = "URL:";

            textBoxHttpClientUrl = new TextBox();
            textBoxHttpClientUrl.Size = new Size(288, 20);
            textBoxHttpClientUrl.TabIndex = 1;

            chkHttpClientEnabled = new CheckBox();
            chkHttpClientEnabled.AutoSize = true;
            chkHttpClientEnabled.Size = new Size(288, 17);
            chkHttpClientEnabled.Text = "Enable";

            label4 = new Label();
            label4.AutoSize = true;
            label4.Size = new Size(288, 13);
            label4.Text = "Status:";

            txtHttpClientStatus = new RichTextBox();
            txtHttpClientStatus.ReadOnly = true;
            txtHttpClientStatus.Size = new Size(288, 100);
            txtHttpClientStatus.Text = "";

            pluginBox = new FlowLayoutPanel();
            pluginBox.FlowDirection = FlowDirection.TopDown;
            pluginBox.Controls.Add(label5);
            pluginBox.Controls.Add(textBoxHttpClientUrl);
            pluginBox.Controls.Add(label6);
            pluginBox.Controls.Add(txtHttpClientHeaders);
            pluginBox.Controls.Add(chkHttpClientEnabled);
            pluginBox.Controls.Add(label4);
            pluginBox.Controls.Add(txtHttpClientStatus);
            pluginBox.Dock = DockStyle.Fill;

            ApplyConfig();
            ApplyChanges();
        }

        public bool IsDirty()
        {
            return p.config.Url != textBoxHttpClientUrl.Text
                || p.config.Headers != txtHttpClientHeaders.Text
                || p.config.Enabled != chkHttpClientEnabled.Checked;
        }

        public PluginConfig GetEditedConfig()
        {
            var conf = new HttpClientPluginConfig();
            conf.Enabled = chkHttpClientEnabled.Checked;
            conf.Url = textBoxHttpClientUrl.Text;
            conf.Headers = txtHttpClientHeaders.Text;
            return conf;
        }

        public void ApplyConfig()
        {
            textBoxHttpClientUrl.Text = p.config.Url;
            chkHttpClientEnabled.Checked = p.config.Enabled;
            txtHttpClientHeaders.Text = p.config.Headers;
        }

        public void ApplyChanges()
        {
            txtHttpClientStatus.Text = p.content;
        }
    }
}
