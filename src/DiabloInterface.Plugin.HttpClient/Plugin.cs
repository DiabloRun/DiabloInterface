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
using Zutatensuppe.DiabloInterface.Lib.Plugin;
using Zutatensuppe.DiabloInterface.Lib;

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
        
        public override void SetConfig(IPluginConfig c)
        {
            Config = c == null ? new Config() : c as Config;
            ApplyConfig();

            if (Config.Enabled)
                Logger.Info($"HTTP client enabled: {Config.Url}");
            else
                Logger.Info("HTTP client disabled");
        }

        public override void Initialize(IDiabloInterface di)
        {
            SetConfig(di.configService.CurrentConfig.PluginConf(Name));
            di.game.DataRead += Game_DataRead;
        }

        private class RequestBody
        {
            public static readonly List<string> AutocompareProps = new List<string> {
                "Area",
                "InventoryTab",
                "Difficulty",
                "PlayersX",
                "Seed",
                "SeedIsArg",
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
                "Life",
                "LifeMax",
                "Mana",
                "ManaMax",
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
            };

            public string Headers { get; set; }
            public int? Area { get; set; }
            public byte? InventoryTab { get; set; }
            public GameDifficulty? Difficulty { get; set; }
            public int? PlayersX { get; set; }
            public uint? Seed { get; set; }
            public bool? SeedIsArg { get; set; }
            public uint? GameCount { get; set; }
            public uint? CharCount { get; set; }
            public bool? NewCharacter { get; set; }
            public string Name { get; set; }
            public string Guid { get; set; }
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
            public int? Life { get; set; }
            public int? LifeMax { get; set; }
            public int? Mana { get; set; }
            public int? ManaMax { get; set; }
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

            public HirelingDiff Hireling { get; set; }
        }

        private class HirelingDiff
        {
            public static readonly List<string> AutocompareProps = new List<string> {
                "Name",
                "Class",
                "Level",
                "Experience",
                "Strength",
                "Dexterity",
                "FireResist",
                "ColdResist",
                "LightningResist",
                "PoisonResist"
            };

            public string Name { get;  set; }
            public int? Class { get;  set; }
            public int? Level { get; set; }
            public int? Experience { get; set; }
            public int? Strength { get; set; }
            public int? Dexterity { get; set; }
            public int? FireResist { get; set; }
            public int? ColdResist { get; set; }
            public int? LightningResist { get; set; }
            public int? PoisonResist { get; set; }
            public List<ItemInfo> Items { get; set; }
            public List<ItemInfo> AddedItems { get; set; }
            public List<BodyLocation> RemovedItems { get; set; }
            public List<uint> SkillIds { get; set; }
        }

        private RequestBody GetDiff(RequestBody newVal, RequestBody prevVal)
        {
            var diff = new RequestBody()
            {
                Headers = Config.Headers,
                Name = newVal.Name,
                Guid = newVal.Guid,
            };

            if (!newVal.CharCount.Equals(prevVal.CharCount))
            {
                diff.NewCharacter = true;
                prevVal = new RequestBody();
            }

            if (!newVal.GameCount.Equals(prevVal.GameCount))
            {
                prevVal = new RequestBody();
            }

            var hasDiff = false;
            foreach (string propertyName in RequestBody.AutocompareProps)
            {
                var property = typeof(RequestBody).GetProperty(propertyName);
                var prevValue = property.GetValue(prevVal);
                var newValue = property.GetValue(newVal);
                if (!ObjectsEqual(prevValue, newValue))
                {
                    hasDiff = true;
                    property.SetValue(diff, newValue);
                }
            }

            var itemDiff = ItemsDiff(newVal.Items, prevVal.Items);
            diff.AddedItems = itemDiff.Item1;
            diff.RemovedItems = itemDiff.Item2;

            diff.CompletedQuests = BuildCompletedQuestsDiff(
                newVal.CompletedQuests,
                prevVal.CompletedQuests
            );

            diff.Hireling = BuildHirelingDiff(
                newVal.Hireling,
                prevVal.Hireling
            );

            hasDiff = hasDiff
                || diff.AddedItems != null
                || diff.RemovedItems != null
                || diff.CompletedQuests != null
                || diff.Hireling != null;

            return hasDiff ? diff : null;
        }

        Dictionary<GameDifficulty, List<QuestId>> BuildCompletedQuestsDiff(
            Dictionary<GameDifficulty, List<QuestId>> newVal,
            Dictionary<GameDifficulty, List<QuestId>> prevVal
        )
        {
            if (prevVal == null && newVal == null)
                return null;

            if (prevVal == null || newVal == null)
                return newVal;

            var diff = new Dictionary<GameDifficulty, List<QuestId>>();
            var hasDiff = false;

            foreach (var pair in Quests.DefaultCompleteQuestIds)
            {
                var completed = newVal[pair.Key].FindAll(id => !prevVal[pair.Key].Contains(id));

                if (completed.Count() > 0)
                {
                    hasDiff = true;
                    diff.Add(pair.Key, completed);
                }
            }
            return hasDiff ? diff : null;
        }

        HirelingDiff BuildHirelingDiff(HirelingDiff newVal, HirelingDiff prevVal)
        {
            if (prevVal == null && newVal == null)
                return null;

            if (prevVal == null || newVal == null)
                return newVal;

            var diff = new HirelingDiff();
            var hasDiff = false;

            var hirelingItemDiff = ItemsDiff(newVal.Items, prevVal.Items);
            diff.AddedItems = hirelingItemDiff.Item1;
            diff.RemovedItems = hirelingItemDiff.Item2;

            if (!ListsEqual(prevVal.SkillIds, newVal.SkillIds))
                diff.SkillIds = newVal.SkillIds;

            foreach (string propertyName in HirelingDiff.AutocompareProps)
            {
                var property = typeof(HirelingDiff).GetProperty(propertyName);
                var prevValue = property.GetValue(prevVal);
                var newValue = property.GetValue(newVal);
                if (!ObjectsEqual(prevValue, newValue))
                {
                    hasDiff = true;
                    property.SetValue(diff, newValue);
                }
            }

            hasDiff = hasDiff
                || diff.AddedItems != null
                || diff.RemovedItems != null
                || diff.SkillIds != null;

            return hasDiff ? diff : null;
        }

        private bool ListsEqual<T>(List<T> listA, List<T> listB)
        {
            if (listA == null && listB == null)
                return true;
            if (listA == null || listB == null)
                return false;
            return listA.SequenceEqual(listB);
        }

        private bool ObjectsEqual(object objA, object objB)
        {
            if (objA == null && objB == null)
                return true;
            if (objA == null || objB == null)
                return false;
            return objA.Equals(objB);
        }

        private Tuple<List<ItemInfo>, List<BodyLocation>> ItemsDiff(
            List<ItemInfo> newItems,
            List<ItemInfo> prevItems
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
                SeedIsArg = e.Game.SeedIsArg,
                GameCount = e.Game.GameCount,
                CharCount = e.Game.CharCount,
                Name = e.Character.Name,
                Guid = e.Character.Guid,
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
                Life = e.Character.Life,
                LifeMax = e.Character.LifeMax,
                Mana = e.Character.Mana,
                ManaMax = e.Character.ManaMax,
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
                Hireling = new HirelingDiff
                {
                    Name = e.Game.Hireling?.Name,
                    Class = e.Game.Hireling?.Class,
                    SkillIds = e.Game.Hireling?.SkillIds,
                    Level = e.Game.Hireling?.Level,
                    Experience = e.Game.Hireling?.Experience,
                    Strength = e.Game.Hireling?.Strength,
                    Dexterity = e.Game.Hireling?.Dexterity,
                    FireResist = e.Game.Hireling?.FireResist,
                    ColdResist = e.Game.Hireling?.ColdResist,
                    LightningResist = e.Game.Hireling?.LightningResist,
                    PoisonResist = e.Game.Hireling?.PoisonResist,
                    Items = e.Game.Hireling?.Items
                },
            };

            var diff = GetDiff(newData, PrevData);

            if (diff != null)
            {
                var json = JsonConvert.SerializeObject(
                    diff,
                    Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                );
                PostJson(json);
            }

            PrevData = newData;
        }
    }
}
