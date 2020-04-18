using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Business.Settings;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Business.Services
{
    public class HttpClientService
    {
        private static readonly HttpClient Client = new HttpClient();
        private bool SendingData = false;
        private string Url;
        private string Headers;

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private GameService gameService;
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

        public event EventHandler<string> ResponseReceived;

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

        public HttpClientService(GameService gameService, SettingsService settingsService)
        {
            this.gameService = gameService;
            dataReader = gameService.DataReader;

            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                Init(args.Settings);
            };

            Init(settingsService.CurrentSettings);
        }

        private void Init(ApplicationSettings s)
        {
            Stop();
            if (s.HttpClientEnabled)
            {
                CreateServer(s.HttpClientUrl, s.HttpClientHeaders);
            }
        }

        private void CreateServer(string url, string headers)
        {
            Logger.Info($"Creating HTTP CLIENT: {url}");
            Url = url;
            Headers = headers;
            gameService.DataRead += OnDataRead;
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

                ResponseReceived?.Invoke(this, content);
            }
            catch (HttpRequestException e)
            {
                ResponseReceived?.Invoke(this, "Request failed");
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
            dataReader.DataRead -= OnDataRead;
        }
    }
}
