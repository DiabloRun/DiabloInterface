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
            "InventoryTab",
            "Difficulty",
            "PlayersX",
            "Seed",
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
            "MagicFind",

            "HirelingName",
            "HirelingClass",
            "HirelingLevel",
            "HirelingExperience",
            "HirelingStrength",
            "HirelingDexterity",
            "HirelingFireResist",
            "HirelingColdResist",
            "HirelingLightningResist",
            "HirelingPoisonResist",
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
            public byte? InventoryTab { get; set; }
            public GameDifficulty? Difficulty { get; set; }
            public int? PlayersX { get; set; }
            public uint? Seed { get; set; }
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

            public string HirelingName { get; set; }
            public int? HirelingClass { get; set; }
            public int? HirelingLevel { get; set; }
            public int? HirelingExperience { get; set; }
            public int? HirelingStrength { get; set; }
            public int? HirelingDexterity { get; set; }
            public int? HirelingFireResist { get; set; }
            public int? HirelingColdResist { get; set; }
            public int? HirelingLightningResist { get; set; }
            public int? HirelingPoisonResist { get; set; }
            public List<ItemInfo> HirelingItems { get; set; }
            public List<ItemInfo> HirelingAddedItems { get; set; }
            public List<BodyLocation> HirelingRemovedItems { get; set; }

            internal string ToJson()
            {
                return JsonConvert.SerializeObject(
                    this,
                    Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                );
            }
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
                if (prevValue == null && newValue == null)
                {
                    continue;
                }

                if (prevValue == null || newValue == null)
                {
                    noChanges = false;
                    property.SetValue(diff, newValue);
                }

                if (!newValue.Equals(prevValue))
                {
                    noChanges = false;
                    property.SetValue(diff, newValue);
                }
            }

            var itemDiff = ItemsDiff(PrevData.Items, newData.Items);
            diff.AddedItems = itemDiff.Item1;
            diff.RemovedItems = itemDiff.Item2;

            var hirelingItemDiff = ItemsDiff(PrevData.HirelingItems, newData.HirelingItems);
            diff.HirelingAddedItems = hirelingItemDiff.Item1;
            diff.HirelingRemovedItems = hirelingItemDiff.Item2;

            if (
                diff.AddedItems != null
                || diff.RemovedItems != null
                || diff.HirelingAddedItems != null
                || diff.HirelingRemovedItems != null
            )
            {
                noChanges = false;
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

        private Tuple<List<ItemInfo>, List<BodyLocation>> ItemsDiff(
            List<ItemInfo> prevItems,
            List<ItemInfo> newItems
        )
        {
            List<ItemInfo> addedItems = new List<ItemInfo>();
            List<BodyLocation> removedItems = new List<BodyLocation>();

            if (newItems != null)
            {
                foreach (var newItem in newItems)
                {
                    if (prevItems == null || !prevItems.Any(prevItem => ItemInfo.AreEqual(prevItem, newItem)))
                    {
                        addedItems.Add(newItem);
                    }
                }
            }

            if (prevItems != null)
            {
                foreach (var prevItem in prevItems)
                {
                    if (newItems == null || !newItems.Any(newItem => ItemInfo.AreEqual(prevItem, newItem)))
                    {
                        removedItems.Add(prevItem.Location);
                    }
                }
            }

            return new Tuple<List<ItemInfo>, List<BodyLocation>>(
                addedItems.Count > 0 ? addedItems : null,
                removedItems.Count > 0 ? removedItems : null
            );
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
                Area = e.Game.Area,
                InventoryTab = e.Game.InventoryTab,
                Difficulty = e.Game.Difficulty,
                PlayersX = e.Game.PlayersX,
                Seed = e.Game.Seed,
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

                HirelingName = e.Game.Hireling?.Name,
                HirelingClass = e.Game.Hireling?.Class,
                HirelingLevel = e.Game.Hireling?.Level,
                HirelingExperience = e.Game.Hireling?.Experience,
                HirelingStrength = e.Game.Hireling?.Strength,
                HirelingDexterity = e.Game.Hireling?.Dexterity,
                HirelingFireResist = e.Game.Hireling?.FireResist,
                HirelingColdResist = e.Game.Hireling?.ColdResist,
                HirelingLightningResist = e.Game.Hireling?.LightningResist,
                HirelingPoisonResist = e.Game.Hireling?.PoisonResist,
                HirelingItems = e.Game.Hireling?.Items,
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
