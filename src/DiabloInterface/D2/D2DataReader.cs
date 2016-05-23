using DiabloInterface.D2;
using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;
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

        private D2Unit pl;
        private D2PlayerData plUnitData;
        private D2StatListEx plStats;

#if DEBUG
        bool didPrintInventory = false;
#endif

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

#if DEBUG
                    if (!didPrintInventory && memory.SupportsItemReading)
                    {
                        didPrintInventory = true;
                        PrintInventoryItems();
                    }
#endif
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
            IntPtr characterUnitAddress = reader.ReadAddress32(memory.Address.Character, AddressingMode.Relative);
            pl = reader.Read<D2Unit>(characterUnitAddress);
            plUnitData = reader.Read<D2PlayerData>(pl.pUnitData.Address);

            // get name
            tmpName = plUnitData.szPlayerName;
            if (tmpName != "" && tmpName != player.name)
            {
                player.name = tmpName;
                player.deaths = 0; // reset the deaths if name changed
                foreach (AutoSplit autosplit in main.settings.autosplits)
                {
                    autosplit.reached = false;
                }
                haveReset = true;
                player.newlyStarted = false;
                return;
            }

            // todo: only read difficulty when it could possibly have changed
            difficulty = reader.ReadByte(memory.Address.Difficulty, AddressingMode.Relative);
            if (difficulty < 0 || difficulty > 2 )
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

            // debug window - quests
            if (main.getDebugWindow() != null)
            {
                IntPtr questDataAddress = IntPtr.Zero;

                // Well this is becoming a mess to clean up...
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 0;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataNormal(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 1;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataNightmare(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));
                memory.Offset.Quests[memory.Offset.Quests.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 2;
                questDataAddress = reader.ResolveAddressPath(memory.Address.Quests, memory.Offset.Quests, AddressingMode.Relative);
                main.getDebugWindow().setQuestDataHell(reader.Read(questDataAddress, QUEST_BUFFER_LENGTH));

                main.getDebugWindow().updateItemStats(reader, pl);
            }

            plStats = reader.Read<D2StatListEx>(new IntPtr(pl.pStatListEx));

            byte[] statsBuffer = reader.Read(new IntPtr(plStats.FullStats), plStats.FullStatsCount * 8);

            player.fill(readDataDict(statsBuffer), currentPenalty);
            player.mode = (D2Data.Mode)pl.eMode;
            player.handleDeath();
            if (haveReset)
            {
                player.newlyStarted = (player.xp == 0 && player.lvl == 1);
                haveReset = false;
            }

            main.updateLabels(player);
            main.writeFiles(player);

            // autosplits only if newly started player
            if (player.newlyStarted)
            {
                doAutoSplits();
            }
        }

        private Dictionary<int, int> readDataDict(byte[] statsBuffer)
        {

            // 70/71 = arbitrary number to get all the player data, could probably be set to lower number.

            // stats
            Dictionary<int, int> dataDict = new Dictionary<int, int>();
            int indexVal = 0;
            int valOffset;
            int off;

            for (int i = 0; i < statsBuffer.Length/8; i++)
            {
                off = 2 + i * 8;
                indexVal = statsBuffer[off];

                if (dataDict.ContainsKey(indexVal))
                {
                    continue;
                }

                valOffset = 0;

                switch (indexVal)
                {
                    case D2Data.CHAR_STR_IDX:
                    case D2Data.CHAR_ENE_IDX:
                    case D2Data.CHAR_DEX_IDX:
                    case D2Data.CHAR_VIT_IDX:
                    case D2Data.CHAR_LVL_IDX:
                    case D2Data.CHAR_XP_IDX:
                    case D2Data.CHAR_GOLD_BODY_IDX:
                    case D2Data.CHAR_GOLD_STASH_IDX:
                    case D2Data.CHAR_DEF_IDX:
                    case D2Data.CHAR_FIRE_RES_IDX:
                    case D2Data.CHAR_FIRE_RES_ADD_IDX:
                    case D2Data.CHAR_LIGHTNING_RES_IDX:
                    case D2Data.CHAR_LIGHTNING_RES_ADD_IDX:
                    case D2Data.CHAR_COLD_RES_IDX:
                    case D2Data.CHAR_COLD_RES_ADD_IDX:
                    case D2Data.CHAR_POISON_RES_IDX:
                    case D2Data.CHAR_POISON_RES_ADD_IDX:
                        valOffset = 2;
                        break;
                    case D2Data.CHAR_CURRENT_MANA_IDX:
                    case D2Data.CHAR_MAX_MANA_IDX:
                    case D2Data.CHAR_CURRENT_STAMINA_IDX:
                    case D2Data.CHAR_MAX_STAMINA_IDX:
                    case D2Data.CHAR_CURRENT_LIFE_IDX:
                    case D2Data.CHAR_MAX_LIFE_IDX:
                        // at offset 2 is a comma value. for us it is enough to know the int val
                        valOffset = 3;
                        break;
                }
                if (valOffset > 0)
                {
                    dataDict.Add(indexVal, BitConverter.ToInt32(new byte[] {
                        statsBuffer[off + valOffset],
                        statsBuffer[off + valOffset + 1],
                        statsBuffer[off + valOffset + 2],
                        statsBuffer[off + valOffset + 3]
                    }, 0));
                }
            }
            return dataDict;
        }

        void PrintInventoryItems()
        {
            var playerAddress = reader.ReadAddress32(memory.Address.Character, AddressingMode.Relative);
            var playerUnit = reader.Read<D2Unit>(playerAddress);
            var inventory = reader.Read<D2Inventory>(playerUnit.pInventory.Address);

            var decoder = new D2ItemReader(reader, memory.Address);
            var item = decoder.GetUnit(inventory.pLastItem);

            // Print all items not in stash.
            //	Note: What if cube is in stash?
            for (; item != null; item = decoder.GetPreviousItem(item))
            {
                var itemData = decoder.GetItemData(item);
                if (itemData.InvPage != InventoryPage.Stash)
                    PrintItemName(decoder, item);
            }
        }

        void PrintItemName(D2ItemReader decoder, D2Unit item)
        {
            if (!decoder.IsValidItem(item)) return;
            if (!decoder.ItemHasFlag(item, ItemFlag.Identified))
                return;

            if (decoder.IsItemOfQuality(item, ItemQuality.Magic))
            {
                var name = decoder.GetItemMagicName(item);
                Console.WriteLine("Name: {0}", name);
                return;
            }

            if (decoder.IsItemOfQuality(item, ItemQuality.Rare))
            {
                var name = decoder.GetItemRareName(item);
                Console.WriteLine("Name: {0}", name);
                return;
            }

            string fullName = decoder.GetItemName(item);
            string quality = decoder.GetItemQualityString(item);
            if (quality != null)
            {
                fullName = string.Format("{0} {1}", quality, fullName);
            }

            string runeword = decoder.GetRunewordName(item);
            if (runeword != null)
            {
                fullName += ": " + runeword;
            }

            Console.WriteLine("Name: {0}", fullName);
        }

        private void doAutoSplits()
        {

            foreach (AutoSplit autosplit in main.settings.autosplits)
            {
                if (!autosplit.reached && autosplit.type == AutoSplit.TYPE_SPECIAL && autosplit.value == (int)AutoSplit.Special.GAMESTART)
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
                    case AutoSplit.TYPE_CHAR_LEVEL:
                        haveUnreachedCharLevelSplits = true;
                        break;
                    case AutoSplit.TYPE_AREA:
                        haveUnreachedAreaSplits = true;
                        break;
                    case AutoSplit.TYPE_ITEM:
                        haveUnreachedItemSplits = true;
                        break;
                    case AutoSplit.TYPE_QUEST:
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
                D2Inventory inventory = reader.Read<D2Inventory>(pl.pInventory.Address);
                // int inventoryAddr = readInt(ADDRESS_CHARACTER, OFFSETS_INVENTORY, true);


                // cursor item (item that is "floating" below cursor when dragging or picking up with open inventory)
                //itemAddress = inventory.pInvOwnerItem; // readInt(inventoryAddr + 0x20); //BitConverter.ToInt32(new byte[4] { inventory[0x20], inventory[0x21], inventory[0x22], inventory[0x23] }, 0);
                if (inventory.pInvOwnerItem > 0)
                {
                    itemsIds.Add(reader.ReadInt32(new IntPtr(inventory.pInvOwnerItem + 0x04)));
                    //Console.WriteLine("type cursor: " + itemType);
                }

                // all the other items
                // 0x10: last item in inventory
                if (!inventory.pLastItem.IsNull)
                {
                    IntPtr itemAddress = inventory.pLastItem.Address;
                    D2Unit item;
                    D2ItemData itemData;
                    do
                    {
                        item = reader.Read<D2Unit>(itemAddress);
                        itemsIds.Add(item.eClass);
                        itemData = reader.Read<D2ItemData>(item.pUnitData.Address);
                        itemAddress = itemData.PreviousItem.Address;
                    } while (itemAddress != IntPtr.Zero);
                }

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
                    case AutoSplit.TYPE_CHAR_LEVEL:
                        if (autosplit.value <= player.lvl)
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.TYPE_AREA:
                        if (autosplit.value == area)
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.TYPE_ITEM:
                        if (itemsIds.Contains(autosplit.value))
                        {
                            autosplit.reached = true;
                            main.triggerAutosplit(player);
                        }
                        break;
                    case AutoSplit.TYPE_QUEST:
                        short value = BitConverter.ToInt16(new byte[2] { reverseBits(questBuffer[autosplit.value]), reverseBits(questBuffer[autosplit.value + 1]), }, 0);
                        if (isNthBitSet(value, 1) || isNthBitSet(value, 0))
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
