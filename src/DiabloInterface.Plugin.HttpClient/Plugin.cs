using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Business.Plugin;
using Zutatensuppe.DiabloInterface.Business.Services;
using Zutatensuppe.DiabloInterface.Business.Settings;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace DiabloInterface.Plugin.HttpClient
{
    public class HttpClientPlugin : IPlugin
    {
        public string Name => "HttpClient";

        public event EventHandler<IPlugin> Changed;
        public PluginData Data { get; } = new PluginData(new Dictionary<string, object>() {
            { "content", "" },
        });

        public PluginConfig Cfg { get; } = new PluginConfig(new Dictionary<string, object>()
        {
            { "Enabled", false },
            { "Url", "" },
            { "Headers", "" },
        });

        private bool HttpClientEnabled => Cfg.GetBool("Enabled");
        private string HttpClientUrl => Cfg.GetString("Url");
        private string HttpClientHeaders => Cfg.GetString("Headers");

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();
        private bool SendingData = false;
        private string Url;
        private string Headers;

        private D2DataReader dataReader;

        private RequestBody PrevData = new RequestBody()
        {
            Items = new List<ItemInfo>(),
            Quests = new Dictionary<GameDifficulty, List<QuestId>>() {
                { GameDifficulty.Normal, new List<QuestId>() },
                { GameDifficulty.Nightmare, new List<QuestId>() },
                { GameDifficulty.Hell, new List<QuestId>() }
            }
        };
        private JsonSerializerSettings RequestBodyJsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        List<GameDifficulty> AllGameDifficulties => new List<GameDifficulty>()
        {
            GameDifficulty.Normal,
            GameDifficulty.Nightmare,
            GameDifficulty.Hell
        };

        List<BodyLocation> AllItemLocations => new List<BodyLocation>()
        {
            BodyLocation.Head,
            BodyLocation.Amulet,
            BodyLocation.BodyArmor,
            BodyLocation.PrimaryLeft,
            BodyLocation.PrimaryRight,
            BodyLocation.RingLeft,
            BodyLocation.RingRight,
            BodyLocation.Belt,
            BodyLocation.Boots,
            BodyLocation.Gloves,
            BodyLocation.SecondaryLeft,
            BodyLocation.SecondaryRight
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

        public HttpClientPlugin(GameService gameService, SettingsService settingsService)
        {
            dataReader = gameService.DataReader;

            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                Init(args.Settings);
            };

            Init(settingsService.CurrentSettings);
        }

        private void Init(ApplicationSettings s)
        {
            Cfg.Apply(s.PluginConf("HttpClient"));

            Logger.Info(HttpClientUrl);

            Stop();
            if (HttpClientEnabled)
            {
                CreateServer(HttpClientUrl, HttpClientHeaders);
            }
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
            public Nullable<int> Area { get; set; }
            public Nullable<GameDifficulty> Difficulty { get; set; }
            public Nullable<int> PlayersX { get; set; }
            public Nullable<uint> GameCount { get; set; }
            public Nullable<uint> CharCount { get; set; }
            public Nullable<bool> NewCharacter { get; set; }
            public string Name { get; set; }
            public Nullable<CharacterClass> CharClass { get; set; }
            public Nullable<bool> IsHardcore { get; set; }
            public Nullable<bool> IsExpansion { get; set; }
            public Nullable<bool> IsDead { get; set; }
            public Nullable<short> Deaths { get; set; }
            public Nullable<int> Level { get; set; }
            public Nullable<int> Experience { get; set; }
            public Nullable<int> Strength { get; set; }
            public Nullable<int> Dexterity { get; set; }
            public Nullable<int> Vitality { get; set; }
            public Nullable<int> Energy { get; set; }
            public Nullable<int> FireResist { get; set; }
            public Nullable<int> ColdResist { get; set; }
            public Nullable<int> LightningResist { get; set; }
            public Nullable<int> PoisonResist { get; set; }
            public Nullable<int> Gold { get; set; }
            public Nullable<int> GoldStash { get; set; }
            public Nullable<int> FasterCastRate { get; set; }
            public Nullable<int> FasterHitRecovery { get; set; }
            public Nullable<int> FasterRunWalk { get; set; }
            public Nullable<int> IncreasedAttackSpeed { get; set; }
            public Nullable<int> MagicFind { get; set; }
            public List<ItemInfo> Items { get; set; }
            public List<ItemInfo> AddedItems { get; set; }
            public List<BodyLocation> RemovedItems { get; set; }
            public Dictionary<GameDifficulty, List<QuestId>> Quests { get; set; }
            public Dictionary<GameDifficulty, List<QuestId>> CompletedQuests { get; set; }
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

            foreach (var difficulty in AllGameDifficulties)
            {
                var completed = newData.Quests[difficulty].FindAll(id => !PrevData.Quests[difficulty].Contains(id));

                if (completed.Count() > 0)
                {
                    noChanges = false;
                    diff.CompletedQuests.Add(difficulty, completed);
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
                var content = await response.Content.ReadAsStringAsync();

                Data.Set("content", content);
                Changed?.Invoke(this, this);
            }
            catch (HttpRequestException)
            {
                Data.Set("content", "Request failed");
                Changed?.Invoke(this, this);
            }
            finally
            {
                SendingData = false;
            }
        }

        void OnDataRead(object sender, DataReadEventArgs e)
        {
            if (SendingData)
            {
                return;
            }

            var game = dataReader.Game;
            var character = dataReader.CurrentCharacter;
            var items = ItemInfo.GetItemsByLocations(dataReader, AllItemLocations);
            var quests = new Dictionary<GameDifficulty, List<QuestId>>();

            foreach (var difficulty in AllGameDifficulties)
            {
                quests.Add(difficulty, dataReader.Quests.CompletedQuestIdsByDifficulty(difficulty));
            }

            var newData = new RequestBody()
            {
                Area = game.Area,
                Difficulty = game.Difficulty,
                PlayersX = game.PlayersX,
                GameCount = game.GameCount,
                CharCount = game.CharCount,
                Name = character.Name,
                CharClass = character.CharClass,
                IsHardcore = character.IsHardcore,
                IsExpansion = character.IsExpansion,
                IsDead = character.IsDead,
                Deaths = character.Deaths,
                Level = character.Level,
                Experience = character.Experience,
                Strength = character.Strength,
                Dexterity = character.Dexterity,
                Vitality = character.Vitality,
                Energy = character.Energy,
                FireResist = character.FireResist,
                ColdResist = character.ColdResist,
                LightningResist = character.LightningResist,
                PoisonResist = character.PoisonResist,
                Gold = character.Gold,
                GoldStash = character.GoldStash,
                FasterCastRate = character.FasterCastRate,
                FasterHitRecovery = character.FasterHitRecovery,
                FasterRunWalk = character.FasterRunWalk,
                IncreasedAttackSpeed = character.IncreasedAttackSpeed,
                MagicFind = character.MagicFind,
                Items = items,
                Quests = quests
            };

            var diff = GetDiff(newData);

            if (diff != null)
            {
                string json = JsonConvert.SerializeObject(diff, Formatting.Indented, RequestBodyJsonSettings);
                PostJson(json);
            }

            PrevData = newData;
        }

        public void Stop()
        {
            Logger.Info("Stopping HTTP client");
        }

        public void OnSettingsChanged()
        {
        }

        public void OnCharacterCreated(CharacterCreatedEventArgs e)
        {
        }

        public void OnDataRead(DataReadEventArgs e)
        {
            OnDataRead(null, e);
        }

        public void OnReset()
        {
        }


        public void Dispose()
        {
        }

        HttpClientSettingsRenderer r;
        public IPluginSettingsRenderer SettingsRenderer()
        {
            if (r == null)
                r = new HttpClientSettingsRenderer(this);
            return r;
        }

        public IPluginDebugRenderer DebugRenderer()
        {
            return null;
        }
    }

    class HttpClientSettingsRenderer : IPluginSettingsRenderer
    {
        private HttpClientPlugin p;

        private GroupBox pluginBox;
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
            {
                Init();
            }
            return pluginBox;
        }

        private void Init()
        {
            txtHttpClientHeaders = new RichTextBox();
            txtHttpClientHeaders.Location = new Point(68, 80);
            txtHttpClientHeaders.Size = new Size(288, 69);
            txtHttpClientHeaders.Text = "";

            label6 = new Label();
            label6.AutoSize = true;
            label6.Location = new Point(5, 80);
            label6.Size = new Size(50, 13);
            label6.Text = "Headers:";

            txtHttpClientStatus = new RichTextBox();
            txtHttpClientStatus.Location = new Point(68, 155);
            txtHttpClientStatus.ReadOnly = true;
            txtHttpClientStatus.Size = new Size(288, 34);
            txtHttpClientStatus.Text = "";

            label4 = new Label();
            label4.AutoSize = true;
            label4.Location = new Point(5, 155);
            label4.Size = new Size(40, 13);
            label4.Text = "Status:";

            chkHttpClientEnabled = new CheckBox();
            chkHttpClientEnabled.AutoSize = true;
            chkHttpClientEnabled.Location = new Point(68, 53);
            chkHttpClientEnabled.Size = new Size(59, 17);
            chkHttpClientEnabled.Text = "Enable";

            textBoxHttpClientUrl = new TextBox();
            textBoxHttpClientUrl.Location = new Point(68, 24);
            textBoxHttpClientUrl.Margin = new Padding(2);
            textBoxHttpClientUrl.Size = new Size(288, 20);
            textBoxHttpClientUrl.TabIndex = 1;

            label5 = new Label();
            label5.AutoSize = true;
            label5.Location = new Point(3, 24);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Size = new Size(32, 13);
            label5.Text = "URL:";

            pluginBox = new GroupBox();
            pluginBox.Controls.Add(txtHttpClientHeaders);
            pluginBox.Controls.Add(label6);
            pluginBox.Controls.Add(txtHttpClientStatus);
            pluginBox.Controls.Add(label4);
            pluginBox.Controls.Add(chkHttpClientEnabled);
            pluginBox.Controls.Add(textBoxHttpClientUrl);
            pluginBox.Controls.Add(label5);
            pluginBox.Location = new Point(5, 277);
            pluginBox.Margin = new Padding(2);
            pluginBox.Padding = new Padding(2);
            pluginBox.Size = new Size(361, 194);
            pluginBox.TabStop = false;
            pluginBox.Text = "HTTP Client";
        }

        public bool IsDirty()
        {
            return p.Cfg.GetString("Url") != textBoxHttpClientUrl.Text
                || p.Cfg.GetString("Headers") != txtHttpClientHeaders.Text
                || p.Cfg.GetBool("Enabled") != chkHttpClientEnabled.Checked;
        }

        public PluginConfig Get()
        {
            return new PluginConfig(new Dictionary<string, object>()
            {
                {"Url", textBoxHttpClientUrl.Text},
                {"Enabled", chkHttpClientEnabled.Checked },
                {"Headers", txtHttpClientHeaders.Text},
            });
        }

        public void Set(PluginConfig cfg)
        {
            textBoxHttpClientUrl.Text = cfg.GetString("Url");
            chkHttpClientEnabled.Checked = cfg.GetBool("Enabled");
            txtHttpClientHeaders.Text = cfg.GetString("Headers");
        }

        public void ApplyChanges()
        {
            txtHttpClientStatus.Text = p.Data.GetString("content");
        }
    }
}
