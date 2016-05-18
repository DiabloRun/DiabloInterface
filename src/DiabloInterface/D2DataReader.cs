using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DiabloInterface
{
    public class D2DataReader
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        MainWindow main;

        const string d2ProcessName = "game";
        const string d2ModuleName = "Game.exe";

        const int QUEST_BUFFER_DIFFICULTY_OFFSET = 128;
        const int QUEST_BUFFER_LENGTH = 96;

        bool processRunning;
        Process D2Process;
        IntPtr D2ProcessHandle;
        IntPtr D2ProcessEntryAddress;

        // buffer read stuff
        byte[] buffer;
        int nullIndex;
        int bytesRead = 0;

        Encoding enc;
        short difficulty;
        D2Data.Penalty currentPenalty;
        bool haveReset;
        string tmpName;
        

        int[] mask8BitSet = { 128, 64, 32, 16, 8, 4, 2, 1 };

        private D2Player player;

        int ADDRESS_CHARACTER;
        int[] OFFSETS_MODE;
        int[] OFFSETS_PLAYER_STATS;
        int[] OFFSETS_INVENTORY;
        int[] OFFSETS_NAME;

        int ADDRESS_QUESTS;
        int[] OFFSETS_QUESTS;

        int ADDRESS_DIFFICULTY;
        int ADDRESS_AREA;

        #region patch 1.14a adresses
        // const int ADDRESS_MODE = 0x44C658;
        // int[] OFFSETS_MODE = new int[] { 0x40, 0x210 };
        // const int ADDRESS_CHARACTER = 0x00498288;
        // int[] OFFSETS_CHARACTER = new int[] { 0x174, 0x5c, 0x48, 0x00 };

        // const int ADDRESS_DIFFICULTY = 0x42E1AC;
        // const int ADDRESS_NAME = 0x82E164;
        #endregion

        #region patch 1.14b adresses
        //int ADDRESS_CHARACTER = 0x0039DEFC;
        //int[] OFFSETS_MODE = new int[] { 0x10 };
        //int[] OFFSETS_PLAYER_STATS = new int[] { 0x5c, 0x48, 0x00 };
        //int[] OFFSETS_INVENTORY = new int[] { 0x60 };
        //int[] OFFSETS_NAME = new int[] { 0x14, 0x00 };

        //int ADDRESS_QUESTS = 0x003B8E54;
        //int[] OFFSETS_QUESTS = new int[] { 0x264, 0x450, 0x20, 0x00 };

        //int ADDRESS_DIFFICULTY = 0x00398694;
        //int ADDRESS_AREA = 0x0039B1C8;
        #endregion

        #region patch 1.14c adresses
        //int ADDRESS_CHARACTER = 0x0039CEFC;
        //int[] OFFSETS_MODE = new int[] { 0x10 };
        //int[] OFFSETS_PLAYER_STATS = new int[] { 0x5c, 0x48, 0x00 };
        //int[] OFFSETS_INVENTORY = new int[] { 0x60 };
        //int[] OFFSETS_NAME = new int[] { 0x14, 0x00 };

        //int ADDRESS_QUESTS = 0x003B7E54;
        //int[] OFFSETS_QUESTS = new int[] { 0x264, 0x450, 0x20, 0x00 };

        //int ADDRESS_DIFFICULTY = 0x00397694;
        //int ADDRESS_AREA = 0x0039A1C8;
        #endregion

        public D2DataReader (MainWindow main)
        {
            this.main = main;
            enc = Encoding.GetEncoding("UTF-8");
            player = new D2Player();
            setD2Version();
        }

        public void setD2Version ( string version = "1.14c" )
        {
            switch (version)
            {
                case "1.14b":
                    ADDRESS_CHARACTER = 0x0039DEFC;
                    OFFSETS_MODE = new int[] { 0x10 };
                    OFFSETS_PLAYER_STATS = new int[] { 0x5c, 0x48, 0x00 };
                    OFFSETS_INVENTORY = new int[] { 0x60 };
                    OFFSETS_NAME = new int[] { 0x14, 0x00 };

                    ADDRESS_QUESTS = 0x003B8E54;
                    OFFSETS_QUESTS = new int[] { 0x264, 0x450, 0x20, 0x00 };

                    ADDRESS_DIFFICULTY = 0x00398694;
                    ADDRESS_AREA = 0x0039B1C8;
                    break;
                case "1.14c":
                default:
                    ADDRESS_CHARACTER = 0x0039CEFC;
                    OFFSETS_MODE = new int[] { 0x10 };
                    OFFSETS_PLAYER_STATS = new int[] { 0x5c, 0x48, 0x00 };
                    OFFSETS_INVENTORY = new int[] { 0x60 };
                    OFFSETS_NAME = new int[] { 0x14, 0x00 };

                    ADDRESS_QUESTS = 0x003B7E54;
                    OFFSETS_QUESTS = new int[] { 0x264, 0x450, 0x20, 0x00 };

                    ADDRESS_DIFFICULTY = 0x00397694;
                    ADDRESS_AREA = 0x0039A1C8;
                    break;
            }
        }
        public bool checkIfD2Running()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(d2ProcessName);
                if (processes.Length == 0)
                {
                    processRunning = false;
                    return processRunning;
                }

                D2Process = processes[0];
                D2ProcessHandle = OpenProcess(PROCESS_WM_READ, false, D2Process.Id);
                for (int i = 0; i < D2Process.Modules.Count; i++)
                {
                    if (D2Process.Modules[i].ModuleName == d2ModuleName)
                    {
                        D2ProcessEntryAddress = D2Process.Modules[i].BaseAddress;
                        processRunning = true;
                        break;
                    }
                }
                return processRunning;

            }
            catch
            {
                processRunning = false;
                return processRunning;
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
            while (1 == 1)
            {
                Thread.Sleep(500);

                if (!processRunning) {
                    checkIfD2Running();
                    continue;
                }

                try
                {
                    readData();
                }
                catch
                {
                    processRunning = false;
                }
            }
        }

        public void readData()
        {

            // get name 
            tmpName = readString(15, ADDRESS_CHARACTER, OFFSETS_NAME, true);
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
            difficulty = readByte(ADDRESS_DIFFICULTY, null, true);
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
                OFFSETS_QUESTS[OFFSETS_QUESTS.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 0;
                main.getDebugWindow().setQuestDataNormal(readBuffer(QUEST_BUFFER_LENGTH, ADDRESS_QUESTS, OFFSETS_QUESTS, true));
                OFFSETS_QUESTS[OFFSETS_QUESTS.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 1;
                main.getDebugWindow().setQuestDataNightmare(readBuffer(QUEST_BUFFER_LENGTH, ADDRESS_QUESTS, OFFSETS_QUESTS, true));
                OFFSETS_QUESTS[OFFSETS_QUESTS.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * 2;
                main.getDebugWindow().setQuestDataHell(readBuffer(QUEST_BUFFER_LENGTH, ADDRESS_QUESTS, OFFSETS_QUESTS, true));
            }

            player.fill(readDataDict(), currentPenalty);
            player.mode = (D2Data.Mode)readShort(ADDRESS_CHARACTER, OFFSETS_MODE, true);
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

        private Dictionary<int, int> readDataDict()
        {

            // 70/71 = arbitrary number to get all the player data, could probably be set to lower number.

            // stats
            Dictionary<int, int> dataDict = new Dictionary<int, int>();
            byte[] statsBuffer = readBuffer(71 * 8, ADDRESS_CHARACTER, OFFSETS_PLAYER_STATS, true);
            int indexVal = 0;
            int valOffset;
            int off;

            for (int i = 0; i < 70; i++)
            {
                off = 2 + i * 8;
                if (i == 0 || indexVal <= statsBuffer[off])
                {
                    indexVal = statsBuffer[off];
                }
                else
                {
                    break;
                }

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

                int inventoryAddr = readInt(ADDRESS_CHARACTER, OFFSETS_INVENTORY, true);
                int itemAddress;
                int unitDataAddress;

                // cursor item (item that is "floating" below cursor when dragging or picking up with open inventory)
                itemAddress = readInt(inventoryAddr + 0x20); //BitConverter.ToInt32(new byte[4] { inventory[0x20], inventory[0x21], inventory[0x22], inventory[0x23] }, 0);
                if (itemAddress > 0)
                {
                    itemsIds.Add(readInt(itemAddress + 0x04));
                    //Console.WriteLine("type cursor: " + itemType);
                }

                // all the other items
                // 0x10: last item in inventory
                itemAddress = readInt(inventoryAddr + 0x10);// BitConverter.ToInt32(new byte[4] { inventory[0x10], inventory[0x11], inventory[0x12], inventory[0x13] }, 0);
                while (itemAddress > 0)
                {
                    itemsIds.Add(readInt(itemAddress + 0x04));

                    // 0x14: unit data of prev item
                    unitDataAddress = readInt(itemAddress + 0x14);

                    // 0x60: pointer to prev item in inventory
                    itemAddress = readInt(unitDataAddress + 0x60);

                }

            }

            if (haveUnreachedAreaSplits)
            {
                area = readByte(ADDRESS_AREA, null, true);
            }

            if (haveUnreachedQuestSplits)
            {
                OFFSETS_QUESTS[OFFSETS_QUESTS.Length - 1] = QUEST_BUFFER_DIFFICULTY_OFFSET * difficulty;
                questBuffer = readBuffer(QUEST_BUFFER_LENGTH, ADDRESS_QUESTS, OFFSETS_QUESTS, true);
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

        private string readString(int length, int address, int[] offsets = null, bool relative = false)
        {
            buffer = readBuffer(length, address, offsets, relative);
            nullIndex = Array.IndexOf(buffer, (byte)0);
            return enc.GetString(buffer, 0, (nullIndex == -1) ? buffer.Length : nullIndex);
        }

        private short readByte(int address, int[] offsets = null, bool relative = false)
        {
            try
            {
                return readBuffer(0x01, address, offsets, relative)[0];
            }
            catch
            {
                return -1;
            }
        }

        private short readShort(int address, int[] offsets = null, bool relative = false)
        {
            try
            {
                return BitConverter.ToInt16(readBuffer(0x02, address, offsets, relative), 0);
            }
            catch
            {
                return -1;
            }
        }

        private int readInt(int address, int[] offsets = null, bool relative = false)
        {
            try
            {
                return BitConverter.ToInt32(readBuffer(0x04, address, offsets, relative), 0);
            }
            catch
            {
                return -1;
            }
        }

        private byte[] readBuffer(int length, int pointer, int[] offsets = null, bool relative = false)
        {
            if (relative)
            {
                pointer = D2ProcessEntryAddress.ToInt32() + pointer;
            }
            if (offsets != null)
            {
                buffer = new byte[4];
                for (int i = 0; i < offsets.Length; i++)
                {
                    ReadProcessMemory((int)D2ProcessHandle, pointer, buffer, buffer.Length, ref bytesRead);
                    pointer = BitConverter.ToInt32(buffer, 0) + offsets[i];
                }

            }

            buffer = new byte[length];
            ReadProcessMemory((int)D2ProcessHandle, pointer, buffer, buffer.Length, ref bytesRead);
            return buffer;
        }
    }
}
