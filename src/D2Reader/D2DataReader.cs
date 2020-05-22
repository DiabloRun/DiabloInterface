using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Core.Logging;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.D2Reader
{
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
        public DataReadEventArgs(Game game) {
            Character = game.Character;
            Game = game;
            Quests = game.Quests;
        }

        public Game Game { get; }
        public Character Character { get; }
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
        IStringReader stringReader;
        ISkillReader skillReader;
        GameMemoryTable memory;

        bool wasInTitleScreen;

        private uint lastGameId = 0;
        private uint gameCount = 0;

        private uint charCount = 0;

        public Game Game { get; private set; }

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
                catch (ModuleNotLoadedException)
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
            stringReader = new StringReader(reader, memory);
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

        internal void ItemSlotAction(List<BodyLocation> slots, Action<Item, D2Unit, UnitReader, IStringReader, IInventoryReader> action)
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

        internal void ItemAction(IEnumerable<Item> items, Action<Item, D2Unit, UnitReader, IStringReader, IInventoryReader> action)
        {
            if (!ValidateGameDataReaders()) return;

            var gameInfo = ReadGameInfo();
            if (gameInfo == null) return;

            try
            {
                foreach (Item item in items)
                {
                    action?.Invoke(item, gameInfo.Player, unitReader, stringReader, inventoryReader);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Unable to execute ItemAction.", e);

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

        GameInfo ReadGameInfo()
        {
            try
            {
                D2Game game = ReadActiveGameInstance();
                if (game == null || game.Client.IsNull) return null;
                D2Client client = reader.Read<D2Client>(game.Client);
                // Make sure we are reading a player type.
                if (client.UnitType != 0) return null;

                // TODO: find out why the game handles the two cases differently
                //       for quests, the unit address from game.UnitLists works
                //       for inventory, the unit address from memory.PlayerUnit works
                //       the adresses may be same at the beginning but change if you make a rune word

                // player address for reading the actual player unit (for inventory, skills, etc.)
                if ((long)memory.PlayerUnit <= 0) return null;
                IntPtr playerAddress = reader.ReadAddress32(memory.PlayerUnit, AddressingMode.Relative);
                if ((long)playerAddress <= 0) return null;
                D2Unit player = reader.Read<D2Unit>(playerAddress);

                // player address for reading the player (name + quests)
                DataPointer unitAddress = game.UnitLists[0][client.UnitId & 0x7F];
                if (unitAddress.IsNull) return null;
                // Read player with player data.
                var tmpPlayer = reader.Read<D2Unit>(unitAddress);
                var playerData = tmpPlayer.UnitData.IsNull
                    ? null
                    : reader.Read<D2PlayerData>(tmpPlayer.UnitData);

                return new GameInfo(game, client, player, playerData);
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
            // Make sure the game is loaded.
            var gameInfo = ReadGameInfo();
            if (gameInfo == null)
            {
                wasInTitleScreen = true;
                return;
            }

            CreateReaders();

            var isNewChar = wasInTitleScreen && Character.DetermineIfNewChar(
                gameInfo.Player,
                unitReader,
                inventoryReader,
                skillReader
            );

            var area = reader.ReadByte(memory.Area, AddressingMode.Relative);
            var character = ReadCharacterData(gameInfo);

            // A brand new character has been started.
            // The extra wasInTitleScreen check prevents DI from splitting
            // when it was started AFTER Diablo 2, but the char is still a new char
            if (isNewChar)
            {
                // disable IsNewChar from the other chars created so far
                foreach (var pair in characters)
                    pair.Value.IsNewChar = false;

                character.Deaths = 0;
                character.IsNewChar = true;
                Logger.Info($"A new chararacter was created: {character.Name}");
                charCount++;
                OnCharacterCreated(new CharacterCreatedEventArgs(character));

                // When a new player is created, the game area is not updated immediately.
                // That means the game is in a kind of invalid state.
                // Instead of waiting for the next loop, we use the area that we know
                // the char starts in and set it manually, no matter what the game tells us
                switch (gameInfo.Player.actNo)
                {
                    case 0: area = (byte)Area.ROGUE_ENCAMPMENT; break;
                    case 1: area = (byte)Area.LUT_GHOLEIN; break;
                    case 2: area = (byte)Area.KURAST_DOCKTOWN; break;
                    case 3: area = (byte)Area.PANDEMONIUM_FORTRESS; break;
                    case 4: area = (byte)Area.HARROGATH; break;
                    default: break;
                }
            }

            var g = new Game();
            g.Area = area;
            g.InventoryTab = reader.ReadByte(memory.InventoryTab, AddressingMode.Relative);
            g.PlayersX = Math.Max(reader.ReadByte(memory.PlayersX, AddressingMode.Relative), (byte)1);
            g.Difficulty = (GameDifficulty)gameInfo.Game.Difficulty;
            g.Seed = gameInfo.Game.InitSeed;
            g.GameCount = gameCount;
            g.CharCount = charCount;
            g.Quests = ReadQuests(gameInfo);
            g.Character = character;
            Game = g;

            OnDataRead(new DataReadEventArgs(Game));

            wasInTitleScreen = false;
        }

        Quests ReadQuests(GameInfo gameInfo) => new Quests((
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

        Character ReadCharacterData(GameInfo gameInfo)
        {
            Character character = CharacterByName(gameInfo.PlayerData.PlayerName);

            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.IsHardcore = gameInfo.Client.IsHardcore();
            character.IsExpansion = gameInfo.Client.IsExpansion();

            // Don't update stats and items while dead.
            if (!character.IsDead)
            {
                character.ParseStats(unitReader, gameInfo);
                character.InventoryItemIds = ReadInventoryItemIds(gameInfo.Player);
                character.Items = ItemInfo.GetItemsByItems(this, GetEquippedItems(gameInfo.Player));
            }

            return character;
        }

        protected virtual void OnCharacterCreated(CharacterCreatedEventArgs e) =>
            CharacterCreated?.Invoke(this, e);

        protected virtual void OnDataRead(DataReadEventArgs e) =>
            DataRead?.Invoke(this, e);
    }
}
