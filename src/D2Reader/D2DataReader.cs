using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Unknown;
using Zutatensuppe.DiabloInterface.Core.Logging;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.D2Reader
{
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

        // set to true when d2 was running but no game is running
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

        public event EventHandler<DataReadEventArgs> DataRead;

        static TimeSpan POLLING_RATE_OUT_OF_GAME = TimeSpan.FromMilliseconds(50);
        static TimeSpan POLLING_RATE_INGAME = TimeSpan.FromMilliseconds(500);

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
                    reader = ProcessMemoryReader.Create(desc.ProcessName, desc.ModuleName, desc.SubModules);
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

        public void RunReadOperation()
        {
            while (!isDisposed)
            {
                if (Read())
                {
                    wasInTitleScreen = false;
                    Thread.Sleep(POLLING_RATE_INGAME);
                } else { 
                    wasInTitleScreen = true;
                    Thread.Sleep(POLLING_RATE_OUT_OF_GAME);
                }
            }
        }

        private bool Read()
        {
            // "Block" here until we have a valid reader.
            if (!ValidateGameDataReaders())
            {
                return false;
            }

            try
            {
                return ProcessGameData();
            }
            catch (ThreadAbortException)
            {
                Logger.Debug("ThreadAbortException");
                throw;
            }
            catch (Exception e)
            {
                Logger.Debug("Other Exception", e);
#if DEBUG
                // Print errors to console in debug builds.
                Console.WriteLine("Exception: {0}", e);
#endif
                // cleaning up readers, so they are recreated in next loop
                // otherwise data reading will stop here
                CleanUpDataReaders();
            }
            return false;
        }

        GameInfo ReadGameInfo()
        {
            try
            {
                // if game is still loading, dont read further
                var loading = reader.ReadInt32(memory.Loading);
                if (loading != 0)
                {
                    Logger.Info("Game still loading");
                    return null;
                }

                uint gameId = reader.ReadUInt32(memory.GameId);
                IntPtr worldPointer = reader.ReadAddress32(memory.World);

                // Get world if game is loaded.
                if (worldPointer == IntPtr.Zero) return null;
                D2World world = reader.Read<D2World>(worldPointer);

                // Find the game address.
                uint gameIndex = gameId & world.GameMask;
                uint gameOffset = (gameIndex * 0x0C) + 0x08;
                IntPtr gamePointer = reader.ReadAddress32(world.GameBuffer + gameOffset);

                // Check for invalid pointers, this value can actually be negative during transition
                // screens, so we need to reinterpret the pointer as a signed integer.
                if (unchecked((int)gamePointer.ToInt64()) < 0) return null;

                var game = reader.Read<D2Game>(gamePointer);
                if (game.Client.IsNull) return null;

                D2Client client = reader.Read<D2Client>(game.Client);
                // Make sure we are reading a player type.
                if (client.UnitType != 0) return null;

                // TODO: find out why the game handles the two cases differently
                //       for quests, the unit address from game.UnitLists works
                //       for inventory, the unit address from memory.PlayerUnit works
                //       the adresses may be same at the beginning but change if you make a rune word

                // player address for reading the actual player unit (for inventory, skills, etc.)
                IntPtr playerAddress = reader.ReadAddress32(memory.PlayerUnit);
                if ((long)playerAddress <= 0) return null;

                D2Unit player = reader.Read<D2Unit>(playerAddress);

                // player address for reading the player (name + quests)
                var tmpPlayer = UnitByTypeAndGuid(game, D2UnitType.Player, client.UnitId);
                var playerData = (tmpPlayer == null || tmpPlayer.UnitData.IsNull)
                    ? null
                    : reader.Read<D2PlayerData>(tmpPlayer.UnitData);
                return new GameInfo(game, gameId, client, player, playerData);
            }
            catch (ProcessMemoryReadException)
            {
                return null;
            }
        }

        // 1.14d: game.478F20
        // 1.13c: in D2Client.QueryInterface+F2B0
        private int GetPetGuid(D2Unit owner, int eClass, int unknown = 0)
        {
            IntPtr addr = reader.ReadAddress32(memory.Pets);
            if ((long)addr == 0) return -1;

            D2UnknownUnitStruct u;
            do
            {
                u = reader.Read<D2UnknownUnitStruct>(addr);
                if (u == null) return -1;

                if (
                    u.eClass == eClass
                    && u.OwnerGUID == owner.GUID
                    && (unknown != 0 || u.unknown_20 == unknown)
                )
                {
                    return u.GUID;
                }

                addr = u.pNext;
            } while ((long)addr != 0);
            return -1;
        }

        // get unit from the global unit list
        private D2Unit UnitByTypeAndGuid(D2UnitType type, int guid)
        {
            if (memory.Units114 != null)
            {
                // 1.14
                // for 1.14d see around game.463940
                int size = Marshal.SizeOf(typeof(D2GameUnitList));
                var addr = (IntPtr)memory.Units114 + (int)type * size;
                var list = reader.Read<D2GameUnitList>(addr);
                DataPointer unitAddress = list[guid & 0x7F];
                return UnitByGuid(unitAddress, guid);
            }

            if (memory.Units113 != null)
            {
                // 1.13
                // for 1.13d see function D2Client.dll+89CE0
                // for 1.13c see function around D2Client.QueryInterface+FB14
                var unitAddrPointer = (IntPtr)memory.Units113 + (int)(guid & 0x7F) * 4;
                var addr = reader.ReadAddress32(unitAddrPointer);
                return UnitByGuid(addr, guid);
            }

            return null;
        }

        // 1.14d: see game.552F60
        // get unit from the game unit list
        private D2Unit UnitByTypeAndGuid(D2Game game, D2UnitType type, int guid)
        {
            DataPointer unitAddress = game.UnitLists[(int)type][guid & 0x7F];
            return UnitByGuid(unitAddress, guid);
        }

        private D2Unit UnitByGuid(IntPtr unitAddress, int guid)
        {
            if ((long)unitAddress == 0) return null;

            var unit = reader.Read<D2Unit>(unitAddress);
            while (unit != null)
            {
                // note: d2 also checks the type of unit and if it doesnt
                //       match it goes to some error
                if (unit.GUID == guid)
                    return unit;

                if (unit.pPrevUnit.IsNull) return null;
                unit = reader.Read<D2Unit>(unit.pPrevUnit);
            };
            return null;
        }

        bool ProcessGameData()
        {
            // Make sure the game is loaded.
            var gameInfo = ReadGameInfo();
            if (gameInfo == null)
                return false;

            CreateReaders();

            // A brand new character has been started.
            // The extra wasInTitleScreen check prevents DI from splitting
            // when it was started AFTER Diablo 2, but the char is still a new char
            var isNewChar = false;
            try
            {
                isNewChar = wasInTitleScreen && Character.DetermineIfNewChar(
                    gameInfo.Player,
                    unitReader,
                    inventoryReader,
                    skillReader
                );
            } catch (Exception e)
            {
                Logger.Info($"Unable to determine if new char {e.Message}");
                return false;
            }

            var area = reader.ReadByte(memory.Area);

            // Make sure game is in a valid state.
            if (!IsValidState(isNewChar, gameInfo, area))
            {
                Logger.Info("Not in valid state");
                return false;
            }

            // TODO: fix bug with not increasing gameCount (maybe use some more info from D2Game obj)
            // Note: gameId can be the same across D2 restarts
            // - launch d2
            // - start game (counter increases)
            // - close d2
            // - launch d2
            // - start game (counter doesnt increase)
            if (gameInfo.GameId != 0 && (lastGameId != gameInfo.GameId))
            {
                gameCount++;
                lastGameId = gameInfo.GameId;
            }

            var character = ReadCharacterData(gameInfo, isNewChar);
            var quests = ReadQuests(gameInfo);
            Hireling hireling = null;
            try
            {
                hireling = ReadHirelingData(gameInfo);
            } catch (Exception e)
            {
                Logger.Error("Error reading hireling", e);
            }

            if (isNewChar)
            {
                charCount++;
                Logger.Info($"A new chararacter was created: {character.Name} (Char {charCount})");
            }

            var g = new Game();
            g.Area = area;
            g.InventoryTab = reader.ReadByte(memory.InventoryTab);
            g.PlayersX = Math.Max(reader.ReadByte(memory.PlayersX), (byte)1);
            g.Difficulty = (GameDifficulty)gameInfo.Game.Difficulty;
            g.Seed = gameInfo.Game.InitSeed;
            // todo: maybe improve the check, if needed...
            g.SeedIsArg = reader.CommandLineArgs.Contains("-seed");
            g.GameCount = gameCount;
            g.CharCount = charCount;
            g.Quests = quests;
            g.Character = character;
            g.Hireling = hireling;
            Game = g;

            OnDataRead(new DataReadEventArgs(Game));

            return true;
        }

        bool IsValidState(bool isNewChar, GameInfo gameInfo, byte area)
        {
            // we assume that for non new chars, game state is always valid for reading (for now)
            if (!isNewChar)
                return true;

            // When a new player is created, the game area is not updated immediately.
            // That means the game is in a kind of invalid state.
            switch (gameInfo.Player.actNo)
            {
                case 0: return area == (byte)Area.ROGUE_ENCAMPMENT;
                case 1: return area == (byte)Area.LUT_GHOLEIN;
                case 2: return area == (byte)Area.KURAST_DOCKTOWN; 
                case 3: return area == (byte)Area.PANDEMONIUM_FORTRESS; 
                case 4: return area == (byte)Area.HARROGATH; 
                default: return false;
            }
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
        
        Character ReadCharacterData(GameInfo gameInfo, bool isNewChar)
        {
            var name = gameInfo.PlayerData.PlayerName;

            if (isNewChar)
            {
                // disable IsNewChar from the other chars created so far
                foreach (var pair in characters)
                    pair.Value.IsNewChar = false;
            }

            if (isNewChar || !characters.ContainsKey(name))
            {
                characters[name] = new Character {
                    Name = name,
                    Created = DateTime.Now,
                    Guid = Guid.NewGuid().ToString(),
                    IsNewChar = isNewChar,
                };
            }

            Character character = characters[name];
            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.IsHardcore = gameInfo.Client.IsHardcore();
            character.IsExpansion = gameInfo.Client.IsExpansion();

            // Don't update stats and items while dead.
            if (!character.IsDead)
            {
                character.Parse(unitReader, gameInfo);
                character.InventoryItemIds = ReadInventoryItemIds(gameInfo.Player);
                character.Items = GetItemInfosByItems(GetEquippedItems(gameInfo.Player), gameInfo.Player);
            }

            return character;
        }

        Hireling ReadHirelingData(GameInfo gameInfo)
        {
            var guid = GetPetGuid(gameInfo.Player, (int)PetClass.HIRELING);
            if (guid < 0) return null;

            var unit = UnitByTypeAndGuid(D2UnitType.Monster, guid);
            if (unit == null) return null;

            var hireling = new Hireling();
            hireling.Parse(unit, unitReader, skillReader, reader, gameInfo);
            hireling.Items = GetItemInfosByItems(GetEquippedItems(unit), unit);
            return hireling;
        }

        private IEnumerable<Item> GetEquippedItems(D2Unit owner)
        {
            return GetInventoryItemsFiltered(owner, (Item i) => i.IsEquipped());
        }

        private List<ItemInfo> GetItemInfosByItems(IEnumerable<Item> items, D2Unit owner)
        {
            return items
                .Select(item => new ItemInfo(item, owner, unitReader, stringReader, inventoryReader))
                .ToList();
        }

        protected virtual void OnDataRead(DataReadEventArgs e) =>
            DataRead?.Invoke(this, e);
    }
}
