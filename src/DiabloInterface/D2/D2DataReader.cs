using DiabloInterface.D2;
using DiabloInterface.D2.Readers;
using DiabloInterface.D2.Struct;
using DiabloInterface.Gui;
using DiabloInterface.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DiabloInterface
{
    public class D2DataReader : IDisposable
    {
        const string DIABLO_PROCESS_NAME = "game";
        const string DIABLO_MODULE_NAME = "Game.exe";

        class GameInfo
        {
            public D2Game Game { get; private set; }
            public D2Unit Player { get; private set; }
            public D2PlayerData PlayerData { get; private set; }

            public GameInfo(D2Game game, D2Unit player, D2PlayerData playerData)
            {
                Game = game;
                Player = player;
                PlayerData = playerData;
            }
        }

        MainWindow main;

        bool disposed = false;

        ProcessMemoryReader reader;
        D2MemoryTable memory;
        D2MemoryTable nextMemoryTable;

        bool wasInTitleScreen = false;
        Character activeCharacter = null;
        Dictionary<string, Character> characters;

        public D2DataReader(MainWindow main, D2MemoryTable memory)
        {
            this.main = main;
            this.memory = memory;

            characters = new Dictionary<string, Character>();
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
                Logger.Instance.WriteLine("Data reader disposed.");

                this.disposed = true;

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
        }

        #endregion

        public void SetNextMemoryTable(D2MemoryTable table)
        {
            nextMemoryTable = table;
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
            var inventoryReader = new InventoryReader(reader, memory);

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

        GameInfo GetGameInfo()
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

                return new GameInfo(game, player, playerData);
            }
            catch (ProcessMemoryReadException)
            {
                return null;
            }
        }

        public void ProcessGameData()
        {
            // Make sure the game is loaded.
            var gameInfo = GetGameInfo();
            if (gameInfo == null)
            {
                wasInTitleScreen = true;
                return;
            }

            UpdateDebugWindow(gameInfo);

            // Get associated character data.
            Character character = GetCurrentCharacter(gameInfo);
            UnitReader unitReader = new UnitReader(reader, memory);
            var statsMap = unitReader.GetStatsMap(gameInfo.Player);
            var itemStatsMap = unitReader.GetItemStatsMap(gameInfo.Player);
            Dictionary<int, int> itemClassMap = unitReader.GetItemClassMap(gameInfo.Player);

            // Update character data.
            character.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            character.ParseStats(statsMap, itemStatsMap, gameInfo.Game.Difficulty);
            int count0 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(gameInfo.PlayerData, 0));
            int count1 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(gameInfo.PlayerData, 1));
            int count2 = D2QuestHelper.GetReallyCompletedQuestCount(GetQuestBuffer(gameInfo.PlayerData, 2));
            character.CompletedQuestCounts[0] = count0;
            character.CompletedQuestCounts[1] = count1;
            character.CompletedQuestCounts[2] = count2;

            // Update UI.
            main.UpdateLabels(character, itemClassMap);
            main.writeFiles(character);

#if DEBUG
            Console.WriteLine("Normal:    " + character.CompletedQuestCounts[0] + "/" + D2QuestHelper.Quests.Count + " (" + (character.CompletedQuestCounts[0] * 100.0f / D2QuestHelper.Quests.Count) + "%)" );
            Console.WriteLine("Nightmare: " + character.CompletedQuestCounts[1] + "/" + D2QuestHelper.Quests.Count + " (" + (character.CompletedQuestCounts[1] * 100.0f / D2QuestHelper.Quests.Count) + "%)");
            Console.WriteLine("Hell:      " + character.CompletedQuestCounts[2] + "/" + D2QuestHelper.Quests.Count + " (" + (character.CompletedQuestCounts[2] * 100.0f / D2QuestHelper.Quests.Count) + "%)");
#endif

            // Update autosplits only if enabled and the character was a freshly started character.
            if (IsAutosplitCharacter(character) && main.Settings.DoAutosplit)
            {
                UpdateAutoSplits(gameInfo, character);
            }
        }

        bool IsAutosplitCharacter(Character character)
        {
            return activeCharacter == character;
        }

        Character GetCurrentCharacter(GameInfo gameInfo)
        {
            string playerName = gameInfo.PlayerData.PlayerName;

            // Read character stats.
            UnitReader unitReader = new UnitReader(reader, memory);
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
                    Logger.Instance.WriteLine("Enabled autosplits for character: {0}", character.name);

                    activeCharacter = character;
                    main.Reset();
                }
            }

            // Not in title screen anymore.
            wasInTitleScreen = false;

            return character;
        }

        void UpdateDebugWindow(GameInfo gameInfo)
        {
            var debugWindow = main.getDebugWindow();
            if (debugWindow == null) return;

            // Fill in quest data.
            for (int difficulty = 0; difficulty < gameInfo.PlayerData.Quests.Length; ++difficulty)
            {
                ushort[] questBuffer = GetQuestBuffer(gameInfo.PlayerData, difficulty);
                if (questBuffer != null)
                {
                    debugWindow.UpdateQuestData(questBuffer, difficulty);
                }
            }

            debugWindow.UpdateItemStats(reader, memory, gameInfo.Player);
        }

        ushort[] GetQuestBuffer(D2PlayerData playerData, int difficulty)
        {
            if (difficulty < 0 || difficulty >= playerData.Quests.Length)
                return null;
            if (playerData.Quests[difficulty].IsNull)
                return null;

            // Read quest array as an array of 16 bit values.
            D2QuestArray questArray = reader.Read<D2QuestArray>(playerData.Quests[difficulty]);
            byte[] questBytes = reader.Read(questArray.Buffer, questArray.Length);
            ushort[] questBuffer = new ushort[(questBytes.Length + 1) / 2];
            Buffer.BlockCopy(questBytes, 0, questBuffer, 0, questBytes.Length);

            return questBuffer;
        }

        private void UpdateAutoSplits(GameInfo gameInfo, Character character)
        {
            foreach (AutoSplit autosplit in main.Settings.Autosplits)
            {
                if (autosplit.IsReached) {
                    continue;
                }
                if (autosplit.Type != AutoSplit.SplitType.Special)
                {
                    continue;
                }
                if (autosplit.Value == (int)AutoSplit.Special.GameStart)
                {
                    CompleteAutoSplit(autosplit, character);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100Percent 
                    && character.CompletedQuestCounts[gameInfo.Game.Difficulty] == D2QuestHelper.Quests.Count 
                    && autosplit.MatchesDifficulty(gameInfo.Game.Difficulty))
                {
                    CompleteAutoSplit(autosplit, character);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100PercentAllDifficulties
                    && character.CompletedQuestCounts[0] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[1] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[2] == D2QuestHelper.Quests.Count)
                {
                    CompleteAutoSplit(autosplit, character);
                }
            }

            bool haveUnreachedCharLevelSplits = false;
            bool haveUnreachedAreaSplits = false;
            bool haveUnreachedItemSplits = false;
            bool haveUnreachedQuestSplits = false;

            foreach (AutoSplit autosplit in main.Settings.Autosplits)
            {
                if (autosplit.IsReached || !autosplit.MatchesDifficulty(gameInfo.Game.Difficulty))
                {
                    continue;
                }
                switch (autosplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        haveUnreachedCharLevelSplits = true;
                        break;
                    case AutoSplit.SplitType.Area:
                        haveUnreachedAreaSplits = true;
                        break;
                    case AutoSplit.SplitType.Item:
                        haveUnreachedItemSplits = true;
                        break;
                    case AutoSplit.SplitType.Quest:
                        haveUnreachedQuestSplits = true;
                        break;
                }
            }

            // if no unreached splits, return
            if (!(haveUnreachedCharLevelSplits || haveUnreachedAreaSplits || haveUnreachedItemSplits || haveUnreachedQuestSplits))
            {
                return;
            }

            List<int> itemsIds = new List<int>();
            int area = -1;

            if (haveUnreachedItemSplits)
            {
                // Get all item IDs.
                var inventoryReader = new InventoryReader(reader, memory);
                itemsIds = (from item in inventoryReader.EnumerateInventory()
                          select item.eClass).ToList();
            }

            if (haveUnreachedAreaSplits)
            {
                area = reader.ReadByte(memory.Address.Area, AddressingMode.Relative);
            }

            ushort[] questBuffer = null;

            if (haveUnreachedQuestSplits)
            {
                questBuffer = GetQuestBuffer(gameInfo.PlayerData, gameInfo.Game.Difficulty);
            }

            foreach (AutoSplit autosplit in main.Settings.Autosplits)
            {
                if (autosplit.IsReached || !autosplit.MatchesDifficulty(gameInfo.Game.Difficulty))
                {
                    continue;
                }

                switch (autosplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        if (autosplit.Value <= character.Level)
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Area:
                        if (autosplit.Value == area)
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Item:
                        if (itemsIds.Contains(autosplit.Value))
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Quest:
                        if ( D2QuestHelper.IsQuestComplete((D2QuestHelper.Quest)autosplit.Value, questBuffer) ) {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                }
            }
        }

        void CompleteAutoSplit(AutoSplit autosplit, Character character)
        {
            // Autosplit already reached.
            if (autosplit.IsReached)
            {
                return;
            }

            autosplit.IsReached = true;
            main.triggerAutosplit(character);

            int autoSplitIndex = main.Settings.Autosplits.IndexOf(autosplit);
            Logger.Instance.WriteLine("AutoSplit: #{0} ({1}, {2}) Reached.",
                autoSplitIndex, autosplit.Name, autosplit.Difficulty);
        }
    }
}
