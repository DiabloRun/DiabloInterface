using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Newtonsoft.Json;
using Zutatensuppe.DiabloInterface.Core.Logging;
using System.Reflection;

namespace Zutatensuppe.DiabloInterface.Plugin.HttpClient
{
    public class Plugin : BasePlugin
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "HttpClient";

        protected override Type ConfigEditRendererType => typeof(ConfigEditRenderer);

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();

        private bool SendingData = false;

        internal string content;

        internal Config Config { get; private set; } = new Config();

        private RequestBody PrevData = new RequestBody()
        {
            Items = new List<ItemInfo>(),
            Quests = Quests.DefaultCompleteQuestIds,
        };
        
        private readonly List<string> BodyProperties = new List<string>()
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

        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
            ApplyConfig();

            if (Config.Enabled)
                Logger.Info($"HTTP client enabled: {Config.Url}");
            else
                Logger.Info("HTTP client disabled");
        }

        public override void Initialize(DiabloInterface di)
        {
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
            di.game.DataRead += Game_DataRead;
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
                Headers = Config.Headers,
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
                var response = await Client.PostAsync(Config.Url, new StringContent(json, Encoding.UTF8, "application/json"));
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
            if (!Config.Enabled) return;

            if (SendingData) return;

            var newData = new RequestBody()
            {
                Area = (int)e.Game.Area,
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
    }
}
