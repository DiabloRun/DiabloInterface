namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

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
            byte currentDifficulty,
            List<int> itemIds,
            bool isAutosplitCharacter,
            Dictionary<int, ushort[]> questBuffers)
        {
            Character = character;
            IsAutosplitCharacter = isAutosplitCharacter;
            QuestBuffers = questBuffers;
            CurrentArea = currentArea;
            CurrentDifficulty = currentDifficulty;
            ItemIds = itemIds;
            ItemStrings = itemStrings;
        }

        public Dictionary<int, ushort[]> QuestBuffers { get; }
        public Character Character { get; }
        public bool IsAutosplitCharacter { get; }
        public int CurrentArea { get; }
        public byte CurrentDifficulty { get; }
        public List<int> ItemIds { get; }
        public Dictionary<BodyLocation, string> ItemStrings { get; }
    }

    public class D2DataReader : IDisposable
    {
        const string DiabloProcessName = "game";
        const string DiabloModuleName = "Game.exe";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IGameMemoryTableFactory memoryTableFactory;

        bool isDisposed;

        D2GameInfo gameInfo;
        ProcessMemoryReader reader;
        UnitReader unitReader;
        InventoryReader inventoryReader;
        GameMemoryTable memory;
        GameMemoryTable nextMemoryTable;

        int currentArea;
        byte currentDifficulty;
        bool wasInTitleScreen = false;
        DateTime activeCharacterTimestamp;
        Character activeCharacter = null;
        Character character = null;
        Dictionary<string, Character> characters;
        Dictionary<int, ushort[]> questBuffers;
        Dictionary<BodyLocation, string> itemStrings;
        List<int> inventoryItemIds;

        public DataReaderEnableFlags ReadFlags { get; set; } =
              DataReaderEnableFlags.CurrentArea
            | DataReaderEnableFlags.CurrentDifficulty
            | DataReaderEnableFlags.QuestBuffers
            | DataReaderEnableFlags.InventoryItemIds;

        public DateTime ActiveCharacterTimestamp => activeCharacterTimestamp;
        public Character ActiveCharacter => activeCharacter;
        public Character CurrentCharacter => character;

        /// <summary>
        /// Polling rate for data reads.
        /// </summary>
        public TimeSpan PollingRate { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Gets the most recent quest status buffer for the current difficulty.
        /// </summary>
        /// <returns></returns>
        public ushort[] CurrentQuestBuffer
        {
            get
            {
                if (questBuffers == null)
                    return null;

                ushort[] statusBuffer;
                if (questBuffers.TryGetValue(currentDifficulty, out statusBuffer))
                    return statusBuffer;
                else return null;
            }
        }

        public D2DataReader(IGameMemoryTableFactory memoryTableFactory, string gameVersion)
        {
            if (memoryTableFactory == null) throw new ArgumentNullException(nameof(memoryTableFactory));

            this.memoryTableFactory = memoryTableFactory;

            memory = CreateGameMemoryTableForVersion(gameVersion);

            characters = new Dictionary<string, Character>();
            questBuffers = new Dictionary<int, ushort[]>();
            itemStrings = new Dictionary<BodyLocation, string>();
            inventoryItemIds = new List<int>();
        }

        public event EventHandler<CharacterCreatedEventArgs> CharacterCreated;

        public event EventHandler<DataReadEventArgs> DataRead;

        #region IDisposable Implementation

        ~D2DataReader()
        {
            Dispose(false);
        }

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

        public void SetD2Version(string version)
        {
            nextMemoryTable = CreateGameMemoryTableForVersion(version);
        }

        GameMemoryTable CreateGameMemoryTableForVersion(string gameVersion)
        {
            return memoryTableFactory.CreateForVersion(gameVersion);
        }

        public bool InitializeReaders()
        {
            // If a reader already exists but the process is closed, dispose of the reader.
            if (reader != null && !reader.IsValid)
            {
                Logger.Warn("Disposing old Diablo II process.");

                reader.Dispose();
                reader = null;
            }

            // A reader exists already.
            if (reader != null)
                return true;

            try
            {
                reader = new ProcessMemoryReader(DiabloProcessName, DiabloModuleName);
                unitReader = new UnitReader(reader, memory);
                inventoryReader = new InventoryReader(reader, memory);

                // test stuff
                //int idx = D2StatArray.GetArrayIndexByStatCostClassId(reader, new IntPtr(0x49C6048), 0x00070000);
                //idx = D2StatArray.GetArrayIndexByStatCostClassId(reader, new IntPtr(0x06FF5024), 0x00ac0000);

                Logger.Info("Found a Diablo II process.");

                // Process opened successfully.
                return true;
            }
            catch (ProcessNotFoundException)
            {
                // Failed to open process.
                return false;
            }
            catch (ProcessMemoryReadException)
            {
                // Failed to read memory.
                return false;
            }
        }

        public void ItemSlotAction(List<BodyLocation> slots, Action<ItemReader, D2Unit> action)
        {
            if (reader == null) return;

            // Add all items found in the slots.
            Func<D2ItemData, bool> filterSlots = data => slots.FindIndex(x => x == data.BodyLoc) >= 0;
            foreach (var item in inventoryReader.EnumerateInventory(filterSlots))
            {
                action?.Invoke(inventoryReader.ItemReader, item);
            }
        }

        public void RunReadOperation()
        {
            while (!isDisposed)
            {
                Thread.Sleep(PollingRate);

                // Block here until we have a valid reader.
                if (!InitializeReaders())
                    continue;

                // Memory table change.
                if (nextMemoryTable != null)
                {
                    memory = nextMemoryTable;
                    nextMemoryTable = null;
                }

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

        D2GameInfo GetGameInfo()
        {
            uint gameId = reader.ReadUInt32(memory.Address.GameId, AddressingMode.Relative);
            IntPtr worldPointer = reader.ReadAddress32(memory.Address.World, AddressingMode.Relative);

            // Get world if game is loaded.
            if (worldPointer == IntPtr.Zero) return null;
            D2World world = reader.Read<D2World>(worldPointer);

            // Find the game address.
            uint gameIndex = gameId & world.GameMask;
            uint gameOffset = gameIndex * 0x0C + 0x08;
            IntPtr gamePointer = reader.ReadAddress32(world.GameBuffer + gameOffset);

            // Check for invalid pointers, this value can actually be negative during transition
            // screens, so we need to reinterpret the pointer as a signed integer.
            if (unchecked((int)gamePointer.ToInt64()) < 0)
                return null;

            try
            {
                D2Game game = reader.Read<D2Game>(gamePointer);
                if (game.Client.IsNull) return null;
                D2Client client = reader.Read<D2Client>(game.Client);

                // Make sure we are reading a player type.
                if (client.UnitType != 0) return null;

                // Get the player address from the list of units.
                DataPointer unitAddress = game.UnitLists[0][client.UnitId & 0x7F];
                if (unitAddress.IsNull) return null;

                // Read player with player data.
                var player = reader.Read<D2Unit>(unitAddress);
                var playerData = player.UnitData.IsNull ?
                    null : reader.Read<D2PlayerData>(player.UnitData);

                return new D2GameInfo(game, player, playerData);
            }
            catch (ProcessMemoryReadException)
            {
                return null;
            }
        }

        public void ProcessGameData()
        {
            // Make sure the game is loaded.
            gameInfo = GetGameInfo();
            if (gameInfo == null)
            {
                wasInTitleScreen = true;
                return;
            }
            unitReader = new UnitReader(reader, memory);
            inventoryReader = new InventoryReader(reader, memory);

            // Get associated character data.
            character = GetCurrentCharacter(gameInfo);

            IntPtr playerAddress = reader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);

            // var skillsMap = unitReader.GetSkillMap(gameInfo.Player);

            // Update character data.
            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.ParseStats(
                unitReader.GetStatsMap(gameInfo.Player),
                unitReader.GetItemStatsMap(gameInfo.Player),
                gameInfo);

            // get quest buffers
            questBuffers.Clear();
            if (ReadFlags.HasFlag(DataReaderEnableFlags.QuestBuffers))
            {
                for (int i = 0; i < gameInfo.PlayerData.Quests.Length; i++)
                {
                    if (gameInfo.PlayerData.Quests[i].IsNull)
                        continue;

                    // Read quest array as an array of 16 bit values.
                    D2QuestArray questArray = reader.Read<D2QuestArray>(gameInfo.PlayerData.Quests[i]);
                    byte[] questBytes = reader.Read(questArray.Buffer, questArray.Length);
                    ushort[] questBuffer = new ushort[(questBytes.Length + 1) / 2];
                    Buffer.BlockCopy(questBytes, 0, questBuffer, 0, questBytes.Length);

                    character.CompletedQuestCounts[i] = D2QuestHelper.GetReallyCompletedQuestCount(questBuffer);

                    questBuffers.Add(i, questBuffer);
                }
            }

            if (ReadFlags.HasFlag(DataReaderEnableFlags.CurrentArea))
                currentArea = reader.ReadByte(memory.Address.Area, AddressingMode.Relative);
            else
                currentArea = -1;

            if (ReadFlags.HasFlag(DataReaderEnableFlags.CurrentDifficulty))
                currentDifficulty = gameInfo.Game.Difficulty;
            else
                currentDifficulty = 0;

            // Build filter to get only equipped items.
            itemStrings.Clear();
            if (ReadFlags.HasFlag(DataReaderEnableFlags.EquippedItemStrings))
            {
                Func<D2ItemData, bool> filter = data => data.BodyLoc != BodyLocation.None;
                foreach (D2Unit item in inventoryReader.EnumerateInventory(filter))
                {
                    List<D2Stat> itemStats = unitReader.GetStats(item);
                    if (itemStats == null)
                    {
                        continue;
                    }

                    StringBuilder statBuilder = new StringBuilder();
                    statBuilder.Append(inventoryReader.ItemReader.GetFullItemName(item));

                    statBuilder.Append(Environment.NewLine);
                    List<string> magicalStrings = inventoryReader.ItemReader.GetMagicalStrings(item);
                    foreach (string str in magicalStrings)
                    {
                        statBuilder.Append("    ");
                        statBuilder.Append(str);
                        statBuilder.Append(Environment.NewLine);
                    }
                    D2ItemData itemData = reader.Read<D2ItemData>(item.UnitData);
                    if ( !itemStrings.ContainsKey(itemData.BodyLoc) )
                    {
                        itemStrings.Add(itemData.BodyLoc, statBuilder.ToString());
                    }
                }

            }

            if (ReadFlags.HasFlag(DataReaderEnableFlags.InventoryItemIds))
                inventoryItemIds = ReadInventoryItemIds(inventoryReader);
            else inventoryItemIds.Clear();

            // New data was read, update anyone interested:
            OnDataRead(CreateDataReadEventArgs());
        }

        List<int> ReadInventoryItemIds(InventoryReader inventoryReader1)
        {
            IEnumerable<D2Unit> baseItems = inventoryReader.EnumerateInventory();
            IEnumerable<D2Unit> socketedItems = baseItems.SelectMany(item => inventoryReader.ItemReader.GetSocketedItems(item));
            IEnumerable<D2Unit> allItems = baseItems.Concat(socketedItems);
            return (from item in allItems select item.eClass).ToList();
        }

        DataReadEventArgs CreateDataReadEventArgs()
        {
            return new DataReadEventArgs(
                character,
                itemStrings,
                currentArea,
                currentDifficulty,
                inventoryItemIds,
                IsAutosplitCharacter(character),
                questBuffers);
        }

        bool IsAutosplitCharacter(Character character)
        {
            return activeCharacter == character;
        }

        Character GetCurrentCharacter(D2GameInfo gameInfo)
        {
            string playerName = gameInfo.PlayerData.PlayerName;

            // Read character stats.
            int level = unitReader.GetStatValue(gameInfo.Player, StatIdentifier.Level) ?? 0;
            int experience = unitReader.GetStatValue(gameInfo.Player, StatIdentifier.Experience) ?? 0;

            // We encountered this character name before.
            Character character = null;
            if (characters.TryGetValue(playerName, out character))
            {
                // We were just in the title screen and came back to a new character.
                bool ResetOnBeginning = wasInTitleScreen && experience == 0;

                // If we lost experience on level 1 we have a reset. Level 1 check is important or
                // this might think we reset when losing experience in nightmare or hell after dying.
                bool ResetOnLevelOne = character.Level == 1 && experience < character.Experience;

                // Check for reset with same character name.
                if (ResetOnBeginning || ResetOnLevelOne || level < character.Level)
                {
                    // Recreate character.
                    characters.Remove(playerName);
                    character = null;
                }
            }

            // If this character has not been read before, or if the character was reset
            // with the same name as a previous character.
            if (character == null)
            {
                character = new Character();
                character.Name = playerName;
                characters[playerName] = character;

                // A brand new character has been started.
                if (experience == 0 && level == 1)
                {
                    activeCharacterTimestamp = DateTime.Now;
                    activeCharacter = character;
                    OnCharacterCreated(new CharacterCreatedEventArgs(character));
                }
            }

            // Not in title screen anymore.
            wasInTitleScreen = false;

            return character;
        }

        protected virtual void OnCharacterCreated(CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(this, e);

        protected virtual void OnDataRead(DataReadEventArgs e) =>
            DataRead?.Invoke(this, e);
    }
}
