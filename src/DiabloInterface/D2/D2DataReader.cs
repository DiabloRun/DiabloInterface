using DiabloInterface.D2.Readers;
using DiabloInterface.D2.Struct;
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

        bool haveReset;

        private D2Player player;

        public D2DataReader(MainWindow main, D2MemoryTable memory)
        {
            this.main = main;
            this.memory = memory;

            player = new D2Player();
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
            if (gameInfo == null) return;

            // Check if the player has changed.
            if (gameInfo.PlayerData.PlayerName != player.name)
            {
                player.name = gameInfo.PlayerData.PlayerName;
                player.Deaths = 0; // reset the deaths if name changed
                foreach (AutoSplit autosplit in main.settings.autosplits)
                {
                    autosplit.reached = false;
                }
                haveReset = true;
                player.IsRecentlyStarted = false;
                return;
            }

            UpdateDebugWindow(gameInfo);

            UnitReader unitReader = new UnitReader(reader, memory.Address);
            var statsMap = unitReader.GetStatsMap(gameInfo.Player);

            player.UpdateMode((D2Data.Mode)gameInfo.Player.eMode);
            player.ParseStats(statsMap, gameInfo.Game.Difficulty);
            if (haveReset)
            {
                player.IsRecentlyStarted = (player.Experience == 0 && player.Level == 1);
                haveReset = false;
            }

            main.updateLabels(player);
            main.writeFiles(player);

            // autosplits only if newly started player
            if (player.IsRecentlyStarted)
            {
                UpdateAutoSplits(gameInfo);
            }
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

        bool IsQuestCompleted(GameInfo gameInfo, int questId)
        {
            ushort[] questBuffer = GetQuestBuffer(gameInfo.PlayerData, gameInfo.Game.Difficulty);
            if (questBuffer == null) return false;

            if (questId < 0 || questId >= questBuffer.Length)
                return false;

            // Make sure one of the completions bits are set.
            ushort questCompletionBits = (1 << 0) | (1 << 1);
            return (questBuffer[questId] & questCompletionBits) != 0;
        }

        private void UpdateAutoSplits(GameInfo gameInfo)
        {
            foreach (AutoSplit autosplit in main.settings.autosplits)
            {
                if (!autosplit.reached && autosplit.type == AutoSplit.Type.Special && autosplit.value == (int)AutoSplit.Special.GAMESTART)
                {
                    autosplit.reached = true;
                    main.triggerAutosplit(player);
                }
            }

            bool haveUnreachedCharLevelSplits = false;
            bool haveUnreachedAreaSplits = false;
            bool haveUnreachedItemSplits = false;
            bool haveUnreachedQuestSplits = false;

            foreach (AutoSplit autosplit in main.settings.autosplits)
            {
                if (autosplit.reached || autosplit.difficulty != gameInfo.Game.Difficulty)
                {
                    continue;
                }
                switch (autosplit.type)
                {
                    case AutoSplit.Type.CharLevel:
                        haveUnreachedCharLevelSplits = true;
                        break;
                    case AutoSplit.Type.Area:
                        haveUnreachedAreaSplits = true;
                        break;
                    case AutoSplit.Type.Item:
                        haveUnreachedItemSplits = true;
                        break;
                    case AutoSplit.Type.Quest:
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

            foreach (AutoSplit autosplit in main.settings.autosplits)
            {
                if (autosplit.reached || autosplit.difficulty != gameInfo.Game.Difficulty)
                {
                    continue;
                }

                switch (autosplit.type)
                {
                    case AutoSplit.Type.CharLevel:
                        if (autosplit.value <= player.Level)
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.Type.Area:
                        if (autosplit.value == area)
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.Type.Item:
                        if (itemsIds.Contains(autosplit.value))
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.Type.Quest:
                        if (IsQuestCompleted(gameInfo, autosplit.value >> 1))
                        {
                            // quest finished
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                }
            }
        }
    }
}
