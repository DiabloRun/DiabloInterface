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
            Game game,
            Quests quests
        ) {
            Character = character;
            Game = game;
            Quests = quests;
        }

        public Character Character { get; }
        public Game Game { get; }
        public Quests Quests { get; }
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
        IStringLookupTable stringReader;
        ISkillReader skillReader;
        GameMemoryTable memory;

        bool wasInTitleScreen;

        private uint lastGameId = 0;
        private uint gameCount = 0;

        private uint charCount = 0;

        public Quests Quests { get; private set; } = new Quests();
        public GameDifficulty Difficulty { get; private set; }

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
        
        public Character ActiveCharacter { get; private set; }

        public Character CurrentCharacter { get; private set; }

        /// <summary>
        /// Gets or sets reader polling rate.
        /// </summary>
        public TimeSpan PollingRate { get; set; } = TimeSpan.FromMilliseconds(500);

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
                    memory = CreateGameMemoryTableForReader(reader);
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
                CreateReaders();
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

        void CreateReaders()
        {
            stringReader = new StringLookupTable(reader, memory);
            skillReader = new SkillReader(reader, memory);

            // note: readers need to be recreated everytime before read.. at least unitReader
            // unitReader fails sometimes... if this is not done .. why? not sure yet ~_~
            unitReader = new UnitReader(reader, memory, stringReader, skillReader);
            inventoryReader = new InventoryReader(reader, unitReader);
        }

        private IEnumerable<Item> GetInventoryItemsFiltered(D2Unit owner, Func<Item, bool> filter)
        {
            if (owner == null)
                return new List<Item>(0);

            var inventory = inventoryReader.EnumerateInventoryBackward(owner);
            return inventoryReader.Filter(inventory, filter);
        }

        Dictionary<BodyLocation, string> ReadEquippedItemStrings(D2Unit owner)
        {
            var itemStrings = new Dictionary<BodyLocation, string>();
            foreach (Item item in GetEquippedItems(owner))
            {
                // TODO: check why this get stats call is needed here
                List<D2Stat> itemStats = unitReader.GetStats(item.Unit);
                if (itemStats.Count == 0) continue;

                if (!itemStrings.ContainsKey(item.BodyLocation()))
                {
                    itemStrings.Add(item.BodyLocation(), ReadEquippedItemString(item, owner));
                }
            }
            return itemStrings;
        }

        private string ReadEquippedItemString(Item item, D2Unit owner)
        {
            StringBuilder s = new StringBuilder();
            s.Append(unitReader.GetFullItemName(item));
            s.Append(Environment.NewLine);

            List<string> magicalStrings = unitReader.GetMagicalStrings(item, owner, inventoryReader);
            foreach (string str in magicalStrings)
            {
                s.Append("    ");
                s.Append(str);
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }

        private List<int> ReadInventoryItemIds(D2Unit owner)
        {
            if (owner == null)
                return new List<int>(0);

            var baseItems = inventoryReader.EnumerateInventoryBackward(owner);
            var socketedItems = baseItems.SelectMany(item => unitReader.GetSocketedItems(item, inventoryReader));
            var allItems = baseItems.Concat(socketedItems);
            return (from item in allItems select item.Unit.eClass).ToList();
        }

        private IEnumerable<Item> GetEquippedItems(D2Unit owner)
        {
            return GetInventoryItemsFiltered(owner, (Item i) => i.IsEquipped());
        }

        private IEnumerable<Item> GetItemsEquippedInSlot(D2Unit owner, List<BodyLocation> slots)
        {
            return GetInventoryItemsFiltered(owner, (Item i) => slots.FindIndex(x => i.IsEquippedInSlot(x)) >= 0);
        }

        public void ItemSlotAction(List<BodyLocation> slots, Action<Item, D2Unit, UnitReader, IStringLookupTable, IInventoryReader> action)
        {
            if (!ValidateGameDataReaders()) return;

            var gameInfo = ReadGameInfo();
            if (gameInfo == null) return;

            try
            {
                foreach (Item item in GetItemsEquippedInSlot(gameInfo.Player, slots))
                {
                    action?.Invoke(item, gameInfo.Player, unitReader, stringReader, inventoryReader);
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

                // "Block" here until we have a valid reader.
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
                DataPointer unitAddress = game.UnitLists[0][client.UnitId & 0x7F];
                if (unitAddress.IsNull) return null;

                // Read player with player data.
                var player = reader.Read<D2Unit>(unitAddress);
                var playerData = player.UnitData.IsNull
                    ? null
                    : reader.Read<D2PlayerData>(player.UnitData);
                return new D2GameInfo(game, client, player, playerData);
            }
            catch (ProcessMemoryReadException)
            {
                return null;
            }
        }

        D2Game ReadActiveGameInstance()
        {
            uint gameId = reader.ReadUInt32(memory.GameId, AddressingMode.Relative);
            IntPtr worldPointer = reader.ReadAddress32(memory.World, AddressingMode.Relative);

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


            // TODO: move this out of this function
            // TODO: fix bug with not increasing gameCount (maybe use some more info from D2Game obj)
            // Note: gameId can be the same across D2 restarts
            // - launch d2
            // - start game (counter increases)
            // - close d2
            // - launch d2
            // - start game (counter doesnt increase)
            if (gameId != 0 && (lastGameId != gameId))
            {
                gameCount++;
                lastGameId = gameId;
            }

            return reader.Read<D2Game>(gamePointer);
        }

        void ProcessGameData()
        {
            CreateReaders();

            // Make sure the game is loaded.
            var gameInfo = ReadGameInfo();
            if (gameInfo == null)
            {
                wasInTitleScreen = true;
                return;
            }

            CurrentCharacter = ReadCharacterData(gameInfo);

            Quests = ReadQuests(gameInfo);
            var Game = ReadGameData(gameInfo);
            Difficulty = Game.Difficulty;
            OnDataRead(new DataReadEventArgs(
                CurrentCharacter,
                Game,
                Quests
            ));
        }

        Game ReadGameData(D2GameInfo gameInfo)
        {
            var g = new Game();
            g.Area = reader.ReadByte(memory.Area, AddressingMode.Relative);
            g.PlayersX = Math.Max(reader.ReadByte(memory.PlayersX, AddressingMode.Relative), (byte)1);
            g.Difficulty = (GameDifficulty)gameInfo.Game.Difficulty;
            g.GameCount = gameCount;
            g.CharCount = charCount;
            return g;
        }

        Quests ReadQuests(D2GameInfo gameInfo) => new Quests((
            from address in gameInfo.PlayerData.Quests
            where !address.IsNull
            select ReadQuestBuffer(address) into questBuffer
            select TransformToQuestList(questBuffer) into quests
            select quests).ToList());

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

        Character CharacterByName(string name)
        {
            if (!characters.ContainsKey(name))
                characters[name] = new Character { Name = name, Created = DateTime.Now };
            return characters[name];
        }

        Character ReadCharacterData(D2GameInfo gameInfo)
        {
            Character character = CharacterByName(gameInfo.PlayerData.PlayerName);

            if (wasInTitleScreen)
            {
                // A brand new character has been started.
                // The extra wasInTitleScreen check prevents DI from splitting
                // when it was started AFTER Diablo 2, but the char is still a new char
                if (Character.IsNewChar(gameInfo.Player, unitReader, inventoryReader, skillReader))
                {
                    // disable IsAutosplitChar from the other chars created so far
                    foreach (var pair in characters)
                        pair.Value.IsAutosplitChar = false;

                    Logger.Info($"A new chararacter was created: {character.Name}");
                    charCount++;
                    character.Deaths = 0;
                    character.IsAutosplitChar = true;
                    ActiveCharacter = character;
                    OnCharacterCreated(new CharacterCreatedEventArgs(character));
                }

                // Not in title screen anymore.
                wasInTitleScreen = false;
            }

            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.IsHardcore = gameInfo.Client.IsHardcore();
            character.IsExpansion = gameInfo.Client.IsExpansion();

            // Don't update stats and items while dead.
            if (!character.IsDead)
            {
                character.ParseStats(unitReader, gameInfo);
                character.EquippedItemStrings = ReadEquippedItemStrings(gameInfo.Player);
                character.InventoryItemIds = ReadInventoryItemIds(gameInfo.Player);
            }

            return character;
        }

        protected virtual void OnCharacterCreated(CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(this, e);

        protected virtual void OnDataRead(DataReadEventArgs e) =>
            DataRead?.Invoke(this, e);
    }
}
