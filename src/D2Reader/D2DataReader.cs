using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.D2Reader.Struct.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Zutatensuppe.D2Reader
{
    public delegate void NewCharacterCreatedEventHandler(object sender, NewCharacterEventArgs e);
    public delegate void DataReadEventHandler(object sender, DataReadEventArgs e);
    public delegate void DataReaderEventHandler(object sender, DataReaderEventArgs e);

    public class DataReaderEventArgs : System.EventArgs
    {
        public enum MsgType{
            DISPOSED,
        };

        public MsgType type;
        public DataReaderEventArgs(MsgType type)
        {
            this.type = type;
        }
    }
    public class NewCharacterEventArgs : System.EventArgs
    {
        public Character Character;

        // Provide one or more constructors, as well as fields and
        // accessors for the arguments.
        public NewCharacterEventArgs(Character character)
        {
            Character = character;
        }
    }
    public class DataReadEventArgs : System.EventArgs
    {
        public Dictionary<int, int> ItemClassMap;
        public Character Character;
        public bool IsAutosplitCharacter;
        
        public DataReadEventArgs(Character character, Dictionary<int, int> itemClassMap, bool isAutosplitCharacter)
        {
            ItemClassMap = itemClassMap;
            Character = character;
            IsAutosplitCharacter = isAutosplitCharacter;
        }
    }

    public class D2DataReader : IDisposable
    {

        public event NewCharacterCreatedEventHandler NewCharacter;

        public event DataReadEventHandler DataRead;

        public event DataReaderEventHandler DataReader;

        const string DIABLO_PROCESS_NAME = "game";
        const string DIABLO_MODULE_NAME = "Game.exe";

        bool disposed = false;

        D2GameInfo gameInfo;
        ProcessMemoryReader reader;
        UnitReader unitReader;
        InventoryReader inventoryReader;
        D2MemoryTable memory;
        D2MemoryTable nextMemoryTable;
        
        bool wasInTitleScreen = false;
        Character activeCharacter = null;
        Dictionary<string, Character> characters;

        public D2DataReader( string version)
        {
            this.memory = GetVersionMemoryTable(version);

            characters = new Dictionary<string, Character>();
        }

        public void SetD2Version(string version)
        {
            this.memory = GetVersionMemoryTable(version);
        }


        private D2MemoryTable GetVersionMemoryTable(string version)
        {
            D2MemoryTable memoryTable = new D2MemoryTable();

            // Offsets are the same for all versions so far.
            memoryTable.Offset.Quests = new int[] { 0x264, 0x450, 0x20, 0x00 };

            switch (version)
            {
                case "1.14c":
                    memoryTable.Address.World = new IntPtr(0x0047ACC0);
                    memoryTable.Address.GameId = new IntPtr(0x00479C94);
                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039CEFC);//(0x39DAF8);
                    memoryTable.Address.Area = new IntPtr(0x0039A1C8);

                    memoryTable.Address.GlobalData = new IntPtr(0x33FD78);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x563BE0);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x5639E0);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x563A04);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x563A28);

                    memoryTable.Address.StringIndexerTable = new IntPtr(0x479A3C);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x479A40);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x479A58);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x479A44);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x479A5C);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x479A48);

                    break;
                case "1.14d":
                default:

                    // some global things: 

                    // 0x6C1164: a list of int (addresses), length is probably 22 .. there is a switch case reading from it when casting a spell


                    // possible statlist flags? 
                    // they are referenced directly, for example:
                    // => 0x6CE264 => 00 00 00 00
                    // => 0x6CE268 => 01 00 00 00
                    // => 0x6CE26C => 02 00 00 00
                    // => 0x6CE270 => 04 00 00 00 
                    // ... 

                    // some global stuff found in asm:

                    // 0x745774 => ??? contains Address to something? 
                    // 0x7457AC => ??? (some number or const or type id?) 

                    // 0x7A0610 
                    // 0x7A0650 
                    // 0x7A0654 
                    // 0x7A2788 => some number?
                    // 0x7A27C0 => some base address to an array
                    // 0x7A2808 => ???
                    // 0x724AC0 => base address for D2CharacterClassSkillIconStruct, length is 7*0x22 (classes: 0-6)
                    // 0x7A6A70 => contains address of player unit 

                    // 0x7A6F96 => some array ?
                    // 0x7A740A => end address of that array?

                    // 0x7c07fc => probably some value (0x03e60000 returned when getting some char class related info from skill)
                    // 0x7c8cb0 => some global number read at beginning of "gameLoop/renderUI?" (maybe some kind of game state?

                    // 0x96c8a8 => an address to int array of the length 7? and all 7 values seem to be 0x63
                    // 0x9D4180 => something in "soSomethingWithSkill2" func

                    // somehing to do with the screen resolution???
                    // 0x71146C => ??? some number ... values seen: 0x320 (= 800)
                    // 0x711470 => ??? some number ... values seen: 0x258 (= 600)
                    // 0x7A6AB0 => ??? some number ... values seen: 0x190 (= 400)
                    // 0x7A6AAC => ??? some number ... values seen: 0x12C (= 300)


                    memoryTable.Address.World = new IntPtr(0x00483D38);
                    memoryTable.Address.GameId = new IntPtr(0x00482D0C);
                    memoryTable.Address.PlayerUnit = new IntPtr(0x003A5E74);
                    memoryTable.Address.Area = new IntPtr(0x003A3140);

                    memoryTable.Address.GlobalData = new IntPtr(0x344304); // =>  points to 0x96bc30 (0x56bc30)
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);

                    memoryTable.Address.StringIndexerTable = new IntPtr(0x4829B4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x4829B8);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x4829BC);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x4829C0);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x4829D0);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x4829D4);

                    break;
            }

            return memoryTable;
        }

        #region Disposable

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
            if (disposing)
            {
                OnDataReader(new DataReaderEventArgs(DataReaderEventArgs.MsgType.DISPOSED));

                this.disposed = true;

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
        }

        #endregion

        public Byte GetDifficulty()
        {
            D2GameInfo gameInfo = GetGameInfo();
            return gameInfo.Game.Difficulty;
        }
        public List<int> GetItemIds()
        {
            return (from item in inventoryReader.EnumerateInventory()
                        select item.eClass).ToList();
        }
        public byte GetAreaId()
        {
            return reader.ReadByte(memory.Address.Area, AddressingMode.Relative);
        }

        public bool checkIfD2Running()
        {
            // If a reader already exists but the process is closed, dispose of the reader.
            if (reader != null && !reader.IsValid)
            {
                reader.Dispose();
                reader = null;
            }

            // A reader exists already.
            if (reader != null)
                return true;

            try
            {
                reader = new ProcessMemoryReader(DIABLO_PROCESS_NAME, DIABLO_MODULE_NAME);
                unitReader = new UnitReader(reader, memory);
                inventoryReader = new InventoryReader(reader, memory);

                // test stuff
                //int idx = D2StatArray.GetArrayIndexByStatCostClassId(reader, new IntPtr(0x49C6048), 0x00070000);
                //idx = D2StatArray.GetArrayIndexByStatCostClassId(reader, new IntPtr(0x06FF5024), 0x00ac0000);


                // Process opened successfully.
                return true;
            }
            catch (ProcessNotFoundException)
            {
                // Failed to open process.
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
                if (action != null)
                {
                    action(inventoryReader.ItemReader, item);
                }
            }
        }

        public void readDataThreadFunc()
        {
            while (!disposed)
            {
                Thread.Sleep(500);

                // Block here until we have a valid reader.
                if (!checkIfD2Running())
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
            Character character = GetCurrentCharacter(gameInfo);

            IntPtr playerAddress = reader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);


            var statsMap = unitReader.GetStatsMap(gameInfo.Player);
            var itemStatsMap = unitReader.GetItemStatsMap(gameInfo.Player);
            var skillsMap = unitReader.GetSkillMap(gameInfo.Player);
            Dictionary<int, int> itemClassMap = unitReader.GetItemClassMap(gameInfo.Player);

            // Update character data.
            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.ParseStats(statsMap, itemStatsMap, gameInfo);
            int count0 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(0));
            int count1 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(1));
            int count2 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(2));
            character.CompletedQuestCounts[0] = count0;
            character.CompletedQuestCounts[1] = count1;
            character.CompletedQuestCounts[2] = count2;

            // New data was read, update anyone interested:
            OnDataRead(new DataReadEventArgs(character, itemClassMap, IsAutosplitCharacter(character)));
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
                character.name = playerName;
                characters[playerName] = character;

                // A brand new character has been started.
                if (experience == 0 && level == 1)
                {
                    activeCharacter = character;
                    OnNewCharacter(new NewCharacterEventArgs(character));
                }
            }

            // Not in title screen anymore.
            wasInTitleScreen = false;

            return character;
        }

        public D2ItemData GetItemData(D2Unit item)
        {
            D2ItemData itemData = reader.Read<D2ItemData>(item.UnitData);
            return itemData;
        }
        public IEnumerable<D2Unit> GetInventoryItems(Func<D2ItemData, bool> filter)
        {
            return inventoryReader.EnumerateInventory(filter);
        }
        public List<D2Stat> GetStats(D2Unit item)
        {
            return unitReader.GetStats(item);
        }
        public string GetFullItemName(D2Unit item)
        {
            return inventoryReader.ItemReader.GetFullItemName(item);
        }
        public List<string> GetMagicalItemStrings(D2Unit item)
        {
            return inventoryReader.ItemReader.GetMagicalStrings(item);
        }

        public ushort[] GetQuestBuffer(int difficulty)
        {
            if (gameInfo == null)
                return null;
            if (difficulty < 0 || difficulty >= gameInfo.PlayerData.Quests.Length)
                return null;
            if (gameInfo.PlayerData.Quests[difficulty].IsNull)
                return null;

            // Read quest array as an array of 16 bit values.
            D2QuestArray questArray = reader.Read<D2QuestArray>(gameInfo.PlayerData.Quests[difficulty]);
            byte[] questBytes = reader.Read(questArray.Buffer, questArray.Length);
            ushort[] questBuffer = new ushort[(questBytes.Length + 1) / 2];
            Buffer.BlockCopy(questBytes, 0, questBuffer, 0, questBytes.Length);

            return questBuffer;
        }
        
        protected virtual void OnNewCharacter(NewCharacterEventArgs e)
        {
            NewCharacter?.Invoke(this, e);
        }

        protected virtual void OnDataRead(DataReadEventArgs e)
        {
            DataRead?.Invoke(this, e);
        }

        protected virtual void OnDataReader(DataReaderEventArgs e)
        {
            DataReader?.Invoke(this, e);
        }
    }
}
