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

            // Add all items found in the slots.
            bool FilterSlots(D2ItemData data) => slots.FindIndex(x => x == data.BodyLoc && data.InvPage == InventoryPage.Equipped) >= 0;

            foreach (var item in unitReader.inventoryReader.EnumerateInventoryBackward(unitReader.GetPlayer(), FilterSlots))
            {
                action?.Invoke(unitReader.inventoryReader.ItemReader, item);
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
            var itemStrings = new Dictionary<BodyLocation, string>();

            // Build filter to get only equipped items.
            bool Filter(D2ItemData data) => data.BodyLoc != BodyLocation.None;

            foreach (D2Unit item in unitReader.inventoryReader.EnumerateInventoryBackward(unitReader.GetPlayer(), Filter))
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
            IReadOnlyCollection<D2Unit> baseItems = unitReader.inventoryReader.EnumerateInventoryBackward(unitReader.GetPlayer()).ToList();
            IEnumerable<D2Unit> socketedItems = baseItems.SelectMany(item => unitReader.inventoryReader.ItemReader.GetSocketedItems(item));
            IEnumerable<D2Unit> allItems = baseItems.Concat(socketedItems);
            return (from item in allItems select item.eClass).ToList();
        }

        bool IsAutosplitCharacter(Character character)
        {
            return ActiveCharacter == character;
        }

        bool HasStartingEquipment()
        {
            D2Unit p = unitReader.GetPlayer();

            int[] list = (
                from item
                in unitReader.inventoryReader.EnumerateInventoryForward(p)
                select item.eClass
            ).ToArray();

            // every class starts with:
            // 4 potions (0x24b)
            // 1 scroll ident (0x212)
            // 1 scroll tp (0x211)
            // and most also start with a 1 buckler (0x148) except necro and sorc
            // actually the order of the items in the inventory changes after restarting the game with the same char!
            // so we can be pretty sure if we found a new char or not.

            // but... sometimes, the char is not ready for being read and 0 items are returned...
            // not sure what to do about that.. todo: maybe read the list again after some ms if it is empty

            Dictionary<CharacterClass, int[]> startingItems = new Dictionary<CharacterClass, int[]>
            {
                // jav, buckler
                { CharacterClass.Amazon, new int[] { 0x2f, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // katar, buckler
                { CharacterClass.Assassin, new int[] { 0xaf, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // wand (no buckler)
                { CharacterClass.Necromancer, new int[] { 0xa, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // hand axe, buckler
                { CharacterClass.Barbarian, new int[] { 0x0, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // short sword, buckler
                { CharacterClass.Paladin, new int[] { 0x19, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // short staff (no buckler)
                { CharacterClass.Sorceress, new int[] { 0x3f, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
                // club, buckler
                { CharacterClass.Druid, new int[] { 0xe, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }}
            };

            if (!list.SequenceEqual(startingItems[(CharacterClass)p.eClass]))
                return false;
            ;

            return true;
        }

        Character ProcessCharacterData(D2GameInfo gameInfo)
        {
            string playerName = gameInfo.PlayerData.PlayerName;

            // Read character stats.
            int level = unitReader.GetStatValue(gameInfo.Player, StatIdentifier.Level) ?? 0;
            int experience = unitReader.GetStatValue(gameInfo.Player, StatIdentifier.Experience) ?? 0;

            // We encountered this character name before.
            if (characters.TryGetValue(playerName, out Character character))
            {
                // We were just in the title screen and came back to a new character.
                bool resetOnBeginning = experience == 0;

                // If we lost experience on level 1 we have a reset. Level 1 check is important or
                // this might think we reset when losing experience in nightmare or hell after dying.
                bool resetOnLvl1 = character.Level == 1 && experience < character.Experience;

                // When starting with -act5 switch, char level is 33 and xp is 7383752
                bool resetOnLvl33 = character.Level == 33 && experience == 7383752;

                // Check for reset with same character name.
                if (
                    (
                        resetOnBeginning
                        || resetOnLvl1
                        || resetOnLvl33
                        || level < character.Level
                    )
                    && wasInTitleScreen
                    && HasStartingEquipment()
                ) {
                    // Recreate character.
                    characters.Remove(playerName);
                    character = null;
                }
            }

            // If this character has not been read before, or if the character was reset
            // with the same name as a previous character.
            if (character == null)
            {
                character = new Character {Name = playerName};
                characters[playerName] = character;

                bool startedOnLvl1 = level == 1 && experience == 0;

                bool startedOnLvl33 = level == 33 && experience == 7383752;

                // A brand new character has been started.
                if ((startedOnLvl1 || startedOnLvl33) && HasStartingEquipment())
                {
                    ActiveCharacterTimestamp = DateTime.Now;
                    ActiveCharacter = character;
                    OnCharacterCreated(new CharacterCreatedEventArgs(character));
                }
            }

            // Not in title screen anymore.
            wasInTitleScreen = false;

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
