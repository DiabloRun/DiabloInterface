namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.D2Reader.Readers;
    using Zutatensuppe.D2Reader.Struct;
    using Zutatensuppe.D2Reader.Struct.Item;
    using Zutatensuppe.D2Reader.Struct.Skill;
    using Zutatensuppe.D2Reader.Struct.Stat;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    /// <summary>
    ///     Describes what type of data the data reader should read.
    /// </summary>
    [Flags]
    public enum DataReaderEnableFlags
    {
        CurrentArea = 1 << 0,
        CurrentDifficulty = 1 << 1,
        QuestBuffers = 1 << 2,
        InventoryItemIds = 1 << 3,
        EquippedItemStrings = 1 << 4,
        CurrentPlayersX = 1 << 5,
    }

    public class CharacterCreatedEventArgs : EventArgs
    {
        public CharacterCreatedEventArgs(Character character)
        {
            Character = character;
        }

        public Character Character { get; }
    }

    public class DataReadEventArgs : EventArgs
    {
        public DataReadEventArgs(
            Character character,
            Dictionary<BodyLocation, string> itemStrings,
            int currentArea,
            GameDifficulty currentDifficulty,
            int currentPlayersX,
            List<int> itemIds,
            bool isAutosplitCharacter,
            IList<QuestCollection> quests)
        {
            Character = character;
            ItemStrings = itemStrings;
            CurrentArea = currentArea;
            CurrentDifficulty = currentDifficulty;
            CurrentPlayersX = currentPlayersX;
            ItemIds = itemIds;
            IsAutosplitCharacter = isAutosplitCharacter;
            Quests = quests;
        }

        public Character Character { get; }
        public Dictionary<BodyLocation, string> ItemStrings { get; }
        public int CurrentArea { get; }
        public int CurrentPlayersX { get; }
        public GameDifficulty CurrentDifficulty { get; }
        public List<int> ItemIds { get; }
        public bool IsAutosplitCharacter { get; }
        public IList<QuestCollection> Quests { get; }
    }

    public class D2DataReader : IDisposable
    {
        const string DiabloProcessName = "game";
        const string DiabloModuleName = "Game.exe";
        readonly string[] DiabloSubModules = {
            "D2Common.dll",
            "D2Launch.dll",
            "D2Lang.dll",
            "D2Net.dll",
            "D2Game.dll",
            "D2Client.dll"
        };
        const string D2SEProcessName = "d2se";
        const string D2SEModuleName = "D2SE.exe";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IGameMemoryTableFactory memoryTableFactory;

        readonly Dictionary<string, Character> characters = new Dictionary<string, Character>();

        bool isDisposed;

        ProcessMemoryReader reader;
        UnitReader unitReader;
        GameMemoryTable memory;

        GameDifficulty currentDifficulty;
        bool wasInTitleScreen;
        IList<QuestCollection> gameQuests = new List<QuestCollection>();

        public D2DataReader(IGameMemoryTableFactory memoryTableFactory)
        {
            this.memoryTableFactory = memoryTableFactory ?? throw new ArgumentNullException(nameof(memoryTableFactory));
        }

        ~D2DataReader()
        {
            Dispose(false);
        }

        public event EventHandler<CharacterCreatedEventArgs> CharacterCreated;

        public event EventHandler<DataReadEventArgs> DataRead;

        public DataReaderEnableFlags ReadFlags { get; set; } =
            DataReaderEnableFlags.CurrentArea
            | DataReaderEnableFlags.CurrentDifficulty
            | DataReaderEnableFlags.CurrentPlayersX
            | DataReaderEnableFlags.QuestBuffers
            | DataReaderEnableFlags.InventoryItemIds;

        public DateTime ActiveCharacterTimestamp { get; private set; }

        public Character ActiveCharacter { get; private set; }

        public Character CurrentCharacter { get; private set; }

        /// <summary>
        /// Gets or sets reader polling rate.
        /// </summary>
        public TimeSpan PollingRate { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Gets the most recent quest status buffer for the current difficulty.
        /// </summary>
        /// <returns></returns>
        public QuestCollection CurrentQuests =>
            gameQuests.ElementAtOrDefault((int)currentDifficulty);

        bool IsProcessReaderTerminated => reader != null && !reader.IsValid;

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            Logger.Info("Data reader disposed.");

            if (disposing && reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            isDisposed = true;
        }

        #endregion

        GameMemoryTable CreateGameMemoryTableForReader(ProcessMemoryReader reader)
        {
            string fileVersion = reader.FileVersion;
            if (reader.ModuleName == D2SEModuleName)
            {
                string version = reader.ReadNullTerminatedString(new IntPtr(0x1A049), 5, Encoding.ASCII, AddressingMode.Relative);
                if (version == "1.13c")
                {
                    fileVersion = GameVersion.Version_1_13_C;
                }
            }
            return memoryTableFactory.CreateForVersion(fileVersion, reader.ModuleBaseAddresses);
        }

        bool ValidateGameDataReaders()
        {
            DisposeProcessReaderIfProcessTerminated();
            return reader != null || InitializeGameDataReaders();
        }

        void DisposeProcessReaderIfProcessTerminated()
        {
            if (!IsProcessReaderTerminated) return;

            Logger.Warn("Disposing process reader for terminated Diablo II process.");

            reader.Dispose();
            reader = null;
        }

        bool InitializeGameDataReaders()
        {
            try
            {
                reader = new ProcessMemoryReader(DiabloProcessName, DiabloModuleName, DiabloSubModules);
            }
            catch (ProcessNotFoundException)
            {
                CleanUpDataReaders();
            }
            if (reader == null)
            {
                try
                {
                    reader = new ProcessMemoryReader(D2SEProcessName, D2SEModuleName, DiabloSubModules);
                }
                catch (ProcessNotFoundException)
                {
                    CleanUpDataReaders();
                }
            }

            if (reader == null)
            {
                return false;
            }

            try
            {
                memory = CreateGameMemoryTableForReader(reader);

                CreateUnitReader();

                Logger.Info($"Found the Diablo II process (version {reader.FileVersion}).");

                return true;
            }
            catch (ModuleNotLoadedException e)
            {
                Logger.Error($"Diablo II Process was found, but the module {e.ModuleName} was not yet loaded. Try launching a game.");
                CleanUpDataReaders();

                return false;
            }
            catch (GameVersionUnsupportedException e)
            {
                Logger.Error($"Diablo II Process was found, but the version {e.GameVersion} is not supported.");
                CleanUpDataReaders();

                return false;
            }
            catch (ProcessMemoryReadException)
            {
                Logger.Error("Diablo II Process was found but failed to read memory.");
                CleanUpDataReaders();

                return false;
            }

            void CleanUpDataReaders()
            {
                reader?.Dispose();
                reader = null;
                memory = null;
                unitReader = null;
            }
        }

        public void ItemSlotAction(List<BodyLocation> slots, Action<ItemReader, D2Unit> action)
        {
            if (reader == null) return;

            if (unitReader == null) return;

            var player = unitReader.GetPlayer();
            if (player == null) return;

            // Add all items found in the slots.
            bool FilterSlots(D2ItemData data) => slots.FindIndex(
                x => x == data.BodyLoc
                && data.InvPage == InventoryPage.Equipped
            ) >= 0;

            try
            {
                foreach (var item in unitReader.inventoryReader.EnumerateInventoryBackward(player, FilterSlots))
                {
                    action?.Invoke(unitReader.inventoryReader.ItemReader, item);
                }
            } catch (Exception e)
            {
                Logger.Error($"Unable to execute ItemSlotAction. {e.Message}");
            }
        }

        public void RunReadOperation()
        {
            while (!isDisposed)
            {
                Thread.Sleep(PollingRate);

                // Block here until we have a valid reader.
                if (!ValidateGameDataReaders())
                    continue;

                try
                {
                    ProcessGameData();
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception e)
                {
#if DEBUG
                    // Print errors to console in debug builds.
                    Console.WriteLine("Exception: {0}", e);
#endif
                }
            }
        }

        D2GameInfo ReadGameInfo()
        {
            try
            {
                D2Game game = ReadActiveGameInstance();
                if (game == null || game.Client.IsNull) return null;

                D2Client client = reader.Read<D2Client>(game.Client);

                // Make sure we are reading a player type.
                if (client.UnitType != 0) return null;

                // Get the player address from the list of units.
                DataPointer unitAddress = game.UnitLists[0][client.UnitId & 0x7F];
                if (unitAddress.IsNull) return null;

                // Read player with player data.
                var player = reader.Read<D2Unit>(unitAddress);
                var playerData = player.UnitData.IsNull
                    ? null
                    : reader.Read<D2PlayerData>(player.UnitData);

                return new D2GameInfo(game, player, playerData);
            }
            catch (ProcessMemoryReadException)
            {
                return null;
            }
        }

        D2Game ReadActiveGameInstance()
        {
            uint gameId = reader.ReadUInt32(memory.Address.GameId, AddressingMode.Relative);
            IntPtr worldPointer = reader.ReadAddress32(memory.Address.World, AddressingMode.Relative);

            // Get world if game is loaded.
            if (worldPointer == IntPtr.Zero) return null;
            D2World world = reader.Read<D2World>(worldPointer);

            // Find the game address.
            uint gameIndex = gameId & world.GameMask;
            uint gameOffset = (gameIndex * 0x0C) + 0x08;
            IntPtr gamePointer = reader.ReadAddress32(world.GameBuffer + gameOffset);

            // Check for invalid pointers, this value can actually be negative during transition
            // screens, so we need to reinterpret the pointer as a signed integer.
            if (unchecked((int)gamePointer.ToInt64()) < 0)
                return null;

            return reader.Read<D2Game>(gamePointer);
        }

        void ProcessGameData()
        {
            // Make sure the game is loaded.
            var gameInfo = ReadGameInfo();
            if (gameInfo == null)
            {
                wasInTitleScreen = true;
                return;
            }

            CreateUnitReader();

            CurrentCharacter = ProcessCharacterData(gameInfo);
            gameQuests = ProcessQuests(gameInfo);
            currentDifficulty = ProcessCurrentDifficulty(gameInfo);

            OnDataRead(new DataReadEventArgs(
                CurrentCharacter,
                ProcessEquippedItemStrings(),
                ProcessCurrentArea(),
                currentDifficulty,
                ProcessCurrentPlayersX(),
                ProcessInventoryItemIds(),
                IsAutosplitCharacter(CurrentCharacter),
                gameQuests
            ));
        }

        void CreateUnitReader()
        {
            unitReader = new UnitReader(reader, memory, new StringLookupTable(reader, memory.Address));
        }

        List<QuestCollection> ProcessQuests(D2GameInfo gameInfo)
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.QuestBuffers)
                ? ReadQuests(gameInfo)
                : new List<QuestCollection>();
        }

        List<QuestCollection> ReadQuests(D2GameInfo gameInfo) => (
            from address in gameInfo.PlayerData.Quests
            where !address.IsNull
            select ReadQuestBuffer(address) into questBuffer
            select TransformToQuestList(questBuffer) into quests
            select new QuestCollection(quests)).ToList();

        ushort[] ReadQuestBuffer(DataPointer address)
        {
            // Read quest array as an array of 16 bit values.
            var questArray = reader.Read<D2QuestArray>(address);
            byte[] questBytes = reader.Read(questArray.Buffer, questArray.Length);

            var questBuffer = new ushort[(questBytes.Length + 1) / 2];
            Buffer.BlockCopy(questBytes, 0, questBuffer, 0, questBytes.Length);
            return questBuffer;
        }

        static List<Quest> TransformToQuestList(IEnumerable<ushort> questBuffer) => questBuffer
            .Select((data, index) => QuestFactory.CreateFromBufferIndex(index, data))
            .Where(quest => quest != null).ToList();

        int ProcessCurrentArea()
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.CurrentArea)
                ? reader.ReadByte(memory.Address.Area, AddressingMode.Relative)
                : -1;
        }

        int ProcessCurrentPlayersX()
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.CurrentPlayersX)
                ? Math.Max(reader.ReadByte(memory.Address.PlayersX, AddressingMode.Relative), (byte)1)
                : -1;
        }

        GameDifficulty ProcessCurrentDifficulty(D2GameInfo gameInfo)
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.CurrentDifficulty)
                ? (GameDifficulty)gameInfo.Game.Difficulty
                : GameDifficulty.Normal;
        }

        Dictionary<BodyLocation, string> ProcessEquippedItemStrings()
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.EquippedItemStrings)
                ? ReadEquippedItemStrings()
                : new Dictionary<BodyLocation, string>();
        }

        Dictionary<BodyLocation, string> ReadEquippedItemStrings()
        {
            var player = unitReader.GetPlayer();
            if (player == null)
                return new Dictionary<BodyLocation, string>();

            // Build filter to get only equipped items.
            bool Filter(D2ItemData data) => data.BodyLoc != BodyLocation.None;

            var itemStrings = new Dictionary<BodyLocation, string>();
            foreach (D2Unit item in unitReader.inventoryReader.EnumerateInventoryBackward(player, Filter))
            {
                List<D2Stat> itemStats = unitReader.GetStats(item);
                if (itemStats == null) continue;

                StringBuilder statBuilder = new StringBuilder();
                statBuilder.Append(unitReader.inventoryReader.ItemReader.GetFullItemName(item));

                statBuilder.Append(Environment.NewLine);
                List<string> magicalStrings = unitReader.inventoryReader.ItemReader.GetMagicalStrings(item);
                foreach (string str in magicalStrings)
                {
                    statBuilder.Append("    ");
                    statBuilder.Append(str);
                    statBuilder.Append(Environment.NewLine);
                }

                D2ItemData itemData = reader.Read<D2ItemData>(item.UnitData);
                if (!itemStrings.ContainsKey(itemData.BodyLoc))
                {
                    itemStrings.Add(itemData.BodyLoc, statBuilder.ToString());
                }
            }
            return itemStrings;
        }

        List<int> ProcessInventoryItemIds()
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.InventoryItemIds)
                ? ReadInventoryItemIds()
                : new List<int>();
        }

        List<int> ReadInventoryItemIds()
        {
            var player = unitReader.GetPlayer();
            if (player == null)
                return new List<int>();

            IReadOnlyCollection<D2Unit> baseItems = unitReader.inventoryReader.EnumerateInventoryBackward(player).ToList();
            IEnumerable<D2Unit> socketedItems = baseItems.SelectMany(item => unitReader.inventoryReader.ItemReader.GetSocketedItems(item));
            IEnumerable<D2Unit> allItems = baseItems.Concat(socketedItems);
            return (from item in allItems select item.eClass).ToList();
        }

        bool IsAutosplitCharacter(Character character)
        {
            return ActiveCharacter == character;
        }

        bool IsNewChar()
        {
            D2Unit p = unitReader.GetPlayer();
            if (p == null)
                return false;

            return MatchesStartingProps(p)
                && MatchesStartingItems(p)
                && MatchesStartingSkills(p);
        }
        private bool MatchesStartingProps(D2Unit p)
        {
            // check -act2/3/4/5 level|xp
            int level = unitReader.GetStatValue(p, StatIdentifier.Level) ?? 0;
            int experience = unitReader.GetStatValue(p, StatIdentifier.Experience) ?? 0;

            // first we will check the level and XP
            // act should be set to the act we are currently in
            return 
                (level == 1 && experience == 0 && p.actNo == 0)
                || (level == 16 && experience == 220165 && p.actNo == 1)
                || (level == 21 && experience == 839864 && p.actNo == 2)
                || (level == 27 && experience == 2563061 && p.actNo == 3)
                || (level == 33 && experience == 7383752 && p.actNo == 4);
        }

        private bool MatchesStartingItems(D2Unit p)
        {
            int[] list = (
                from item
                in unitReader.inventoryReader.EnumerateInventoryForward(p)
                select item.eClass
            ).ToArray();

            return list.SequenceEqual(Character.StartingItems[(CharacterClass)p.eClass]);
        }

        private bool MatchesStartingSkills(D2Unit p)
        {
            int skillCount = 0;
            foreach (D2Skill skill in unitReader.skillReader.EnumerateSkills(p))
            {
                var skillData = unitReader.skillReader.ReadSkillData(skill);
                Skill skillId = (Skill)skillData.SkillId;
                if (!Character.StartingSkills[(CharacterClass)p.eClass].ContainsKey(skillId))
                {
                    return false;
                }

                if (Character.StartingSkills[(CharacterClass)p.eClass][skillId] != unitReader.skillReader.GetTotalNumberOfSkillPoints(skill))
                {
                    return false;
                }
                skillCount++;
            }

            return skillCount == Character.StartingSkills[(CharacterClass)p.eClass].Count;
        }

        Character ProcessCharacterData(D2GameInfo gameInfo)
        {
            string playerName = gameInfo.PlayerData.PlayerName;
            Character character;

            if (wasInTitleScreen || !characters.TryGetValue(playerName, out character))
            {
                character = new Character { Name = playerName };
                characters[playerName] = character;

                // A brand new character has been started.
                // The extra wasInTitleScreen check prevents DI from splitting
                // when it was started AFTER Diablo 2, but the char is still a new char
                if (wasInTitleScreen && IsNewChar())
                {
                    ActiveCharacterTimestamp = DateTime.Now;
                    ActiveCharacter = character;
                    OnCharacterCreated(new CharacterCreatedEventArgs(character));
                }

                // Not in title screen anymore.
                wasInTitleScreen = false;
            }

            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);

            // Don't update stats while dead.
            if (!character.IsDead)
            {
                character.ParseStats(unitReader, gameInfo);
            }
            return character;
        }

        protected virtual void OnCharacterCreated(CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(this, e);

        protected virtual void OnDataRead(DataReadEventArgs e) =>
            DataRead?.Invoke(this, e);
    }
}
