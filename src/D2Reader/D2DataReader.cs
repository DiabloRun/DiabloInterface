namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Readers;
    using Struct;
    using Struct.Item;
    using Struct.Stat;
    using DiabloInterface.Core.Logging;

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
        ItemClassMap = 1 << 5
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
        public Dictionary<int, int> ItemClassMap;
        public Dictionary<int, ushort[]> QuestBuffers;
        public Character Character;
        public bool IsAutosplitCharacter;
        public int CurrentArea;
        public byte CurrentDifficulty;
        public List<int> ItemIds;
        public Dictionary<BodyLocation, string> ItemStrings;

        public DataReadEventArgs(Character character, Dictionary<BodyLocation, string> itemStrings, int currentArea, byte currentDifficulty, List<int> itemIds, Dictionary<int, int> itemClassMap, bool isAutosplitCharacter, Dictionary<int, ushort[]> questBuffers)
        {
            ItemClassMap = itemClassMap;
            Character = character;
            IsAutosplitCharacter = isAutosplitCharacter;
            QuestBuffers = questBuffers;
            CurrentArea = currentArea;
            CurrentDifficulty = currentDifficulty;
            ItemIds = itemIds;
            ItemStrings = itemStrings;
        }
    }

    public class D2DataReader : IDisposable
    {
        const string DiabloProcessName = "game";
        const string DiabloModuleName = "Game.exe";

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        bool isDisposed;

        D2GameInfo gameInfo;
        ProcessMemoryReader reader;
        UnitReader unitReader;
        InventoryReader inventoryReader;
        D2MemoryTable memory;
        D2MemoryTable nextMemoryTable;

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
        Dictionary<int, int> itemClassMap;

        public DataReaderEnableFlags ReadFlags { get; set; } =
              DataReaderEnableFlags.CurrentArea
            | DataReaderEnableFlags.CurrentDifficulty
            | DataReaderEnableFlags.ItemClassMap
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

        public D2DataReader(string version)
        {
            memory = GetVersionMemoryTable(version);

            characters = new Dictionary<string, Character>();
            questBuffers = new Dictionary<int, ushort[]>();
            itemStrings = new Dictionary<BodyLocation, string>();
            inventoryItemIds = new List<int>();
            itemClassMap = new Dictionary<int, int>();
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
            nextMemoryTable = GetVersionMemoryTable(version);
        }

        D2MemoryTable GetVersionMemoryTable(string version)
        {
            var memoryTable = new D2MemoryTable();

            // Offsets are the same for all versions so far.
            memoryTable.Offset.Quests = new int[] { 0x264, 0x450, 0x20, 0x00 };

            switch (version)
            {
                // how to find the addresses in cheat engine, for example after new patch without too big changes
                // note: cheat engine wants the addresses with the 400000 offset. 
                // so when copying to CE, add 400000, when copying from CE, substract 400000

                // World
                // ...
                // - launch diablo
                // - scan for 0
                // - start a game 
                // - scan for changed value
                // - relaunch diablo and start a game
                // - scan for unchanged value
                // - leave game
                // - scan for 0
                // - do that until about 200 addresses are left, while in game, some addresses change,
                //   but when clicking outside of game, they are unchanged again. remove those addresses
                // - remove all addresses that are not green
                // - remove all addresses where the values do not look like addresses
                // - there should be about 11 remaining addresses, browse memory of those
                // - the one you are looking for has:
                //   - lots of 0000000 before it
                //   - another address close afterwards
                //   - a counter of some sort a bit after that (or loop variable or something)
                //   - a value field whose value is changing every some seconds
                // - to verify, go to that found address +24 and check if there is a int (= 3) there


                // GameId
                // - launch diablo
                // - scan for int 0
                // - start a game 
                // - scan for changed value
                // -- (repeat until satisfied)
                // - exit game
                // - scan for unchanged value
                // - enter new game
                // - scan for increased value by 1
                // -- (repeat above if neccessary)
                // - there is one green address left, that is the game id



                // PlayerUnit
                // tricky.. might not always work
                // - go to town and stand still
                // - scan for int 5
                // - start running
                // - scan for int 3
                // - start walking
                // - scan for int 6
                // - stand still
                // - scan for int 5
                // - there should be some addresses (eg 05DF8B10 and 05E02710)
                // - (unpause the game if paused)
                // - watch for both addresses + 34 (05DF8B44 and 05E02744). discard the address where the value stays 0 
                // - note that address - 44 (eg 05DF8B00)
                // - make a scan for that address
                // - it is the first green address

                // Area
                // - go to an area (eg. rouge camp)
                // - scan for unknown value
                // - wait a bit
                // -- (repeat from here if needed)
                // - scan for unchanged value
                // - go to another area (eg. cold plains)
                // - scan for changed value
                // - wait a bit 
                // - scan for unchanged value
                // -- (repeat above if needed)
                // - there should be some addresses. browse all
                // - it should be the one address that has lots of 000000 before and lots of FFFFFFF afterwards

                // GlobalData
                // - go ingame
                // - search 0000170300001703 (hex, 8bytes)
                // - that address - 44
                // - make a pointer scan for that address
                // - chose the one address that has 0 offset. it is that address

                // LowQualityItems
                // - search '43 72 75 64 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 AE 51 43 72 61 63 6B 65 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 BD 06 44 61 6D 61 67 65 64'
                //   (value type: array of byte)
                // - note the address (eg 066FC2F0)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the address, should be only one green (eg 0096CC5C)
                // - that address - 4

                // ItemDescriptions
                // - tricky.. might not work!
                // - search '66 6C 70 68 61 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 75 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 75'
                // - note the first address (eg 0566D00C)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 0566D00C0566D00C)
                // - the found address - 4

                // MagicModifierTable
                // - search '6F 66 20 48 65 61 6C 74 68 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 07 00 00 03 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 01 00 FF 00 07 00 00 00 01 00 00 00 00 00 00 00 01 05 FF FF 00 00 03 00 25 00 33'
                // - note the address (eg 0599CE8C)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 0599CE8C0599CE8C)
                // - the found address - 4

                // RareModifierTable
                // - search '00 00 00 00 00 00 00 00 00 00 00 00 15 09 00 00 1E 00 20 00 21 00 22 00 1C 00 43 00 00 00 00 00 00 00 00 00 00 00 62 69 74 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 09 00 00 1E 00 20 00 21 00 22 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 73 63 72 61 74 63 68'
                // - note the address (eg 066364BC)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 066364BC066364BC)
                // - the found address - 4



                // the following depend on the language!! the resulting address should however be the same in the end!

                // StringIndexerTable
                // - see StringAddressTable
                // - that address - 4

                // StringAddressTable
                // - open a game
                // - for german:
                //   - search 'Kampfstab' and go back to the [ms]. use that address
                // - for english:
                //   - search 'Quarterstaff' and use that address
                // - note the address (eg 00C56024)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 05A03C24)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable

                // PatchStringIndexerTable
                // - see PatchStringAddressTable
                // - that address + 14

                // PatchStringAddressTable
                // - open a game
                // - for german:
                //   - search 'Party-Schaden' unicode string
                // - for english:
                //   - search 'Party Damage' unicode string
                // - look before that address, there should be some 20 00 00 00. the first of those is the address. (it should be the found address -14)
                // - note the address (eg 05A14A8C)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 059990E4)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable

                // ExpansionStringIndexerTable
                // - see ExpansionStringAddressTable
                // - that address + 14

                // ExpansionStringAddressTable
                // - open a game
                // - for german or english:
                //   - search 'Trang' unicode string
                // - there are many addresses. the one we are looking for is at the beginning of a block and before it is a block of ????????
                // - note the address (eg 0599CE8C)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 05A09074)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable

                case "1.14b":
                    memoryTable.Address.GlobalData = new IntPtr(0x00340D78);

                    memoryTable.Address.World = new IntPtr(0x0047BD78);
                    memoryTable.Address.GameId = new IntPtr(0x0047AD4C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x564C98);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x564A98);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x564ABC);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x564AE0);
                    
                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039DEFC);//(0x39DAF8);
                    memoryTable.Address.Area = new IntPtr(0x0039B1C8);
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x47AAF4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x47AAF8);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x47AB10);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x47AAFC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x47AB14);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x47AB00);

                    break;
                case "1.14c":
                    memoryTable.Address.World = new IntPtr(0x0047ACC0);
                    memoryTable.Address.GameId = new IntPtr(0x00479C94);
                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039CEFC);
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
                   
                    memoryTable.Address.GlobalData = new IntPtr(0x00344304); // =>  points to 0x96bc30 (0x56bc30)

                    memoryTable.Address.World = new IntPtr(0x00483D38); // points to 0x7A0690 (0x3A0690)
                    memoryTable.Address.GameId = new IntPtr(0x00482D0C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58); // count. pointer is that address + 4
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x003A5E74); // points to the player struct address
                    memoryTable.Address.Area = new IntPtr(0x003A3140); // integer that is the current area of the player

                    /*GOT*/memoryTable.Address.StringIndexerTable = new IntPtr(0x4829B4); // => points to 0x6684C7C
                    memoryTable.Address.StringAddressTable = new IntPtr(0x4829B8); // => points to 0x5A03c24

                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x4829D0);
                    /*GOT*/memoryTable.Address.PatchStringAddressTable = new IntPtr(0x4829BC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x4829D4);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x4829C0);

                    break;
            }

            return memoryTable;
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
                gameInfo
            );

            if (ReadFlags.HasFlag(DataReaderEnableFlags.ItemClassMap))
                itemClassMap = unitReader.GetItemClassMap(gameInfo.Player);
            else
                itemClassMap.Clear();

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
                inventoryItemIds = (from item in inventoryReader.EnumerateInventory()
                                     select item.eClass).ToList();
            else
                inventoryItemIds.Clear();

            // New data was read, update anyone interested:
            OnDataRead(CreateDataReadEventArgs());
        }

        DataReadEventArgs CreateDataReadEventArgs()
        {
            return new DataReadEventArgs(
                character,
                itemStrings,
                currentArea,
                currentDifficulty,
                inventoryItemIds,
                itemClassMap,
                IsAutosplitCharacter(character),
                questBuffers
            );
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
