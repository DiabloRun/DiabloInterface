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
        MainWindow main;

        bool disposed = false;

        const string DIABLO_PROCESS_NAME = "game";
        const string DIABLO_MODULE_NAME = "Game.exe";

        const int QUEST_BUFFER_DIFFICULTY_OFFSET = 128;
        const int QUEST_BUFFER_LENGTH = 96;

        ProcessMemoryReader reader;
        D2MemoryTable memory;
        D2MemoryTable nextMemoryTable;

        short difficulty;
        D2Data.Penalty currentPenalty;
        bool haveReset;
        string tmpName;

        int[] mask8BitSet = { 128, 64, 32, 16, 8, 4, 2, 1 };

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
            if (!memory.SupportsItemReading)
                return;

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

        private byte reverseBits(byte b)
        {
            return (byte)(((b * 0x80200802ul) & 0x0884422110ul) * 0x0101010101ul >> 32);
        }

        private bool isNthBitSet(short c, int n)
        {
            if (n >= 8)
            {
                c = (short)((int)c >> 8);
                n -= 8;
            }
            return ((c & mask8BitSet[n]) != 0);
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
                    readData();
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

        public void readData()
        {
            // Make sure there is a player before continuing.
            IntPtr characterUnitAddress = reader.ReadAddress32(memory.Address.PlayerUnit, AddressingMode.Relative);
            if (characterUnitAddress == IntPtr.Zero)
                return;

            var playerUnit = reader.Read<D2Unit>(characterUnitAddress);
            var playerData = reader.Read<D2PlayerData>(playerUnit.pUnitData);

            // get name
            tmpName = playerData.szPlayerName;
            if (tmpName != "" && tmpName != player.name)
            {
                player.name = tmpName;
                player.Deaths = 0; // reset the deaths if name changed
                foreach (AutoSplit autosplit in main.settings.autosplits)
                {
                    autosplit.reached = false;
                }
                haveReset = true;
                player.IsRecentlyStarted = false;
                return;
            }

            // todo: only read difficulty when it could possibly have changed
            difficulty = reader.ReadByte(memory.Address.Difficulty, AddressingMode.Relative);
            if (difficulty < 0 || difficulty > 2)
            {
                difficulty = 0;
            }
            switch (difficulty)
            {
                case 2: currentPenalty = D2Data.Penalty.HELL; break;
                case 1: currentPenalty = D2Data.Penalty.NIGHTMARE; break;
                case 0:
                default: currentPenalty = D2Data.Penalty.NORMAL; break;
            }
            var tmpPenalty = currentPenalty;

            // debug window - quests
            if (main.getDebugWindow() != null)
            {
                // read quests for debug window
                IntPtr questDataAddress = IntPtr.Zero;
                
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 0;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataNormal(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 1;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataNightmare(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 2;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataHell(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));

                // read items for debug window
                main.getDebugWindow().UpdateItemStats(reader, memory, playerUnit);
            }

            UnitReader unitReader = new UnitReader(reader, memory.Address);
            var statsMap = unitReader.GetStatsMap(playerUnit);

            player.ParseStats(statsMap, tmpPenalty);
            player.UpdateMode((D2Data.Mode)playerUnit.eMode);
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
                doAutoSplits();
            }
        }

        private void doAutoSplits()
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
                if (autosplit.reached || autosplit.difficulty != difficulty)
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
            byte[] questBuffer = new byte[] { };

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

            if (haveUnreachedQuestSplits)
            {
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * difficulty;
                
                var questBufferAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                questBuffer = reader.Read(questBufferAddress, QUEST_BUFFER_LENGTH);
            }

            foreach (AutoSplit autosplit in main.settings.autosplits)
            {
                if (autosplit.reached || autosplit.difficulty != difficulty)
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
                        if (questBuffer.Length > autosplit.value+1)
                        {
                            short value = BitConverter.ToInt16(new byte[2] { reverseBits(questBuffer[autosplit.value]), reverseBits(questBuffer[autosplit.value + 1]), }, 0);
                            if (isNthBitSet(value, 1) || isNthBitSet(value, 0))
                            {
                                // quest finished
                                autosplit.reached = true;
                                main.triggerAutosplit(player);
                            }
                        }
                        break;
                }
            }

        }
    }
}
