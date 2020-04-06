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
            IList<QuestCollection> quests,
            uint gameCounter)
        {
            Character = character;
            ItemStrings = itemStrings;
            CurrentArea = currentArea;
            CurrentDifficulty = currentDifficulty;
            CurrentPlayersX = currentPlayersX;
            ItemIds = itemIds;
            IsAutosplitCharacter = isAutosplitCharacter;
            Quests = quests;
            GameCounter = gameCounter;
        }

        public Character Character { get; }
        public Dictionary<BodyLocation, string> ItemStrings { get; }
        public int CurrentArea { get; }
        public int CurrentPlayersX { get; }
        public GameDifficulty CurrentDifficulty { get; }
        public List<int> ItemIds { get; }
        public bool IsAutosplitCharacter { get; }
        public IList<QuestCollection> Quests { get; }
        public uint GameCounter { get;  }
    }

    public class ProcessDescription
    {
        public string ProcessName;
        public string ModuleName;
        public string[] SubModules;
    }

    public class D2DataReader : IDisposable
    {
        readonly ProcessDescription[] processDescriptions;

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IGameMemoryTableFactory memoryTableFactory;

        readonly Dictionary<string, Character> characters = new Dictionary<string, Character>();

        bool isDisposed;

        IProcessMemoryReader reader;
        IInventoryReader inventoryReader;
        UnitReader unitReader;
        ISkillReader skillReader;
        GameMemoryTable memory;

        GameDifficulty currentDifficulty;
        bool wasInTitleScreen;
        IList<QuestCollection> gameQuests = new List<QuestCollection>();

        private uint firstGameId = 0;

        public D2DataReader(
            IGameMemoryTableFactory memoryTableFactory,
            ProcessDescription[] processDescriptions
        )
        {
            this.memoryTableFactory = memoryTableFactory ?? throw new ArgumentNullException(nameof(memoryTableFactory));
            this.processDescriptions = processDescriptions;
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

        public uint GameCounter { get; private set; }

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

        GameMemoryTable CreateGameMemoryTableForReader(IProcessMemoryReader reader)
        {
            var memoryTable = memoryTableFactory.CreateForReader(reader);
            Logger.Debug($"Memory table created.");
            return memoryTable;
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
            foreach (var desc in this.processDescriptions)
            {
                try
                {
                    reader = new ProcessMemoryReader(desc.ProcessName, desc.ModuleName, desc.SubModules);
                }
                catch (ProcessNotFoundException)
                {
                    CleanUpDataReaders();
                }
                if (reader != null)
                {
                    break;
                }
            }

            if (reader == null)
            {
                return false;
            }

            Logger.Info($"Diablo II process found, version: {reader.FileVersion}");

            try
            {
                memory = CreateGameMemoryTableForReader(reader);
                skillReader = new SkillReader(reader, memory);
                var stringReader = new StringLookupTable(reader, memory.Address);
                unitReader = new UnitReader(reader, memory, stringReader, skillReader);
                inventoryReader = new InventoryReader(reader, unitReader);
                return true;
            }
            catch (ModuleNotLoadedException e)
            {
                Logger.Error($"Try launching a game. Module not loaded: {e.ModuleName}");
                CleanUpDataReaders();

                return false;
            }
            catch (GameVersionUnsupportedException e)
            {
                Logger.Error($"Version not supported: {e.GameVersion}");
                CleanUpDataReaders();

                return false;
            }
            catch (ProcessMemoryReadException)
            {
                Logger.Error("Failed to read memory");
                CleanUpDataReaders();

                return false;
            }
        }

        void CleanUpDataReaders()
        {
            reader?.Dispose();
            reader = null;
            memory = null;
            inventoryReader = null;
            unitReader = null;
            skillReader = null;
        }
        
        public void ItemSlotAction(List<BodyLocation> slots, Action<D2Unit, D2Unit, UnitReader, IInventoryReader> action)
        {
            if (!ValidateGameDataReaders()) return;

            var gameInfo= ReadGameInfo();
            if (gameInfo == null) return;

            // Add all items found in the slots.
            bool Filter(D2ItemData d, D2Unit u) => slots.FindIndex(x => d.IsEquippedInSlot(x)) >= 0;

            try
            {
                foreach (D2Unit item in inventoryReader.EnumerateInventoryBackward(gameInfo.Player, Filter))
                {
                    action?.Invoke(item, gameInfo.Player, unitReader, inventoryReader);
                }
            } catch (Exception e)
            {
                string s = String.Join(",", from slot in slots select slot.ToString());
                Logger.Error($"Unable to execute ItemSlotAction. {s}", e);
                
                unitReader.ResetCache();
            }
        }

        public void RunReadOperation()
        {
            while (!isDisposed)
            {
                Thread.Sleep(PollingRate);

                // Block here until we have a valid reader.
                if (!ValidateGameDataReaders())
                {
                    continue;
                }

                try
                {
                    ProcessGameData();
                }
                catch (ThreadAbortException)
                {
                    Logger.Debug("ThreadAbortException");
                    throw;
                }
                catch (Exception e)
                {
                    Logger.Debug("Other Exception");
#if DEBUG
                    // Print errors to console in debug builds.
                    Console.WriteLine("Exception: {0}", e);
#endif
                    // cleaning up readers, so they are recreated in next loop
                    // otherwise data reading will stop here
                    CleanUpDataReaders();
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
                // note: previously used way to get the player was
                // if ((long)memory.Address.PlayerUnit > 0) {
                //     IntPtr playerAddress = reader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);
                //     if ((long)playerAddress > 0) {
                //         player = reader.Read<D2Unit>(playerAddress)
                //     }
                // }
                IntPtr playerAddress = reader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);
                DataPointer unitAddress = game.UnitLists[0][client.UnitId & 0x7F];
                Logger.Debug($"read address of player: {playerAddress} VS {unitAddress.Address}");
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

            if (firstGameId == 0 && gameId != 0)
            {
                firstGameId = gameId;
            }
            GameCounter = gameId - firstGameId + 1;
            // Check for invalid pointers, this value can actually be negative during transition
            // screens, so we need to reinterpret the pointer as a signed integer.
            if (unchecked((int)gamePointer.ToInt64()) < 0)
                return null;

            return reader.Read<D2Game>(gamePointer);
        }

        void ProcessGameData()
        {
            Logger.Debug("ProcessGameData");
            // Make sure the game is loaded.
            var gameInfo = ReadGameInfo();
            if (gameInfo == null)
            {
                Logger.Debug("gameInfo is null");
                wasInTitleScreen = true;
                return;
            }

            Logger.Debug("ProcessCharacterData");
            CurrentCharacter = ProcessCharacterData(gameInfo);

            Logger.Debug("ProcessQuests");
            gameQuests = ProcessQuests(gameInfo);

            Logger.Debug("ProcessCurrentDifficulty");
            currentDifficulty = ProcessCurrentDifficulty(gameInfo);

            Logger.Debug("OnDataRead");
            OnDataRead(new DataReadEventArgs(
                CurrentCharacter,
                ProcessEquippedItemStrings(gameInfo.Player),
                ProcessCurrentArea(),
                currentDifficulty,
                ProcessCurrentPlayersX(),
                ProcessInventoryItemIds(gameInfo.Player),
                IsAutosplitCharacter(CurrentCharacter),
                gameQuests,
                GameCounter
            ));

            Logger.Debug("Done");
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

        Dictionary<BodyLocation, string> ProcessEquippedItemStrings(D2Unit player)
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.EquippedItemStrings)
                ? ReadEquippedItemStrings(player)
                : new Dictionary<BodyLocation, string>();
        }

        Dictionary<BodyLocation, string> ReadEquippedItemStrings(D2Unit player)
        {
            if (player == null)
                return new Dictionary<BodyLocation, string>();

            // Build filter to get only equipped items.
            bool Filter(D2ItemData d, D2Unit u) => d.IsEquipped();

            var itemStrings = new Dictionary<BodyLocation, string>();
            foreach (D2Unit item in inventoryReader.EnumerateInventoryBackward(player, Filter))
            {
                // TODO: check why this get stats call is needed here
                List<D2Stat> itemStats = unitReader.GetStats(item);
                if (itemStats.Count == 0) continue;

                StringBuilder statBuilder = new StringBuilder();
                statBuilder.Append(unitReader.GetFullItemName(item));
                statBuilder.Append(Environment.NewLine);

                List<string> magicalStrings = unitReader.GetMagicalStrings(item, player, inventoryReader);
                foreach (string str in magicalStrings)
                {
                    statBuilder.Append("    ");
                    statBuilder.Append(str);
                    statBuilder.Append(Environment.NewLine);
                }

                // TODO: not read this again, it is already read in EnumerateInventory
                D2ItemData itemData = reader.Read<D2ItemData>(item.UnitData);
                if (!itemStrings.ContainsKey(itemData.BodyLoc))
                {
                    itemStrings.Add(itemData.BodyLoc, statBuilder.ToString());
                }
            }
            return itemStrings;
        }

        List<int> ProcessInventoryItemIds(D2Unit player)
        {
            return ReadFlags.HasFlag(DataReaderEnableFlags.InventoryItemIds)
                ? ReadInventoryItemIds(player)
                : new List<int>();
        }

        List<int> ReadInventoryItemIds(D2Unit player)
        {
            if (player == null)
                return new List<int>();

            IReadOnlyCollection<D2Unit> baseItems = inventoryReader.EnumerateInventoryBackward(player).ToList(); 
            IEnumerable<D2Unit> socketedItems = baseItems.SelectMany(item => unitReader.GetSocketedItems(item, inventoryReader));
            IEnumerable<D2Unit> allItems = baseItems.Concat(socketedItems);
            return (from item in allItems select item.eClass).ToList();
        }

        bool IsAutosplitCharacter(Character character)
        {
            return ActiveCharacter == character;
        }

        Character CharacterByNameCached(string playerName)
        {
            if (!characters.TryGetValue(playerName, out Character character))
            {
                character = new Character { Name = playerName };
                characters[playerName] = character;
            }
            return character;
        }

        Character ProcessCharacterData(D2GameInfo gameInfo)
        {
            Character character = CharacterByNameCached(gameInfo.PlayerData.PlayerName);

            if (wasInTitleScreen)
            {
                // A brand new character has been started.
                // The extra wasInTitleScreen check prevents DI from splitting
                // when it was started AFTER Diablo 2, but the char is still a new char
                if (Character.IsNewChar(gameInfo.Player, unitReader, inventoryReader, skillReader))
                {
                    Logger.Info($"A new chararacter was created: {character.Name}");
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
