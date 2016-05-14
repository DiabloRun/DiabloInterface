using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using DiabloInterface.Properties;
using DiabloInterface.AutoSplit;

namespace DiabloInterface
{
    public partial class MainWindow : Form
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        string fileFolder = "txt";

        public SettingsHolder settings;

        const int MODE_DEATH = 0;
        const int MODE_NEUTRAL = 1;
        const int MODE_WALK = 2;
        const int MODE_RUN = 3;
        const int MODE_GET_HIT = 4;
        const int MODE_TOWN_NEUTRAL = 5;
        const int MODE_TOWN_WALK = 6;
        const int MODE_ATTACK_1 = 7;
        const int MODE_ATTACK_2 = 8;
        const int MODE_BLOCK = 9;
        const int MODE_CAST = 10;
        const int MODE_THROW = 11;
        const int MODE_KICK = 12;
        const int MODE_SKILL_1 = 13;
        const int MODE_SKILL_2 = 14;
        const int MODE_SKILL_3 = 15;
        const int MODE_SKILL_4 = 16;
        const int MODE_DEAD = 17;
        const int MODE_SEQUENCE = 18;
        const int MODE_KNOCK_BACK = 19;

        const int PENALTY_NORMAL = 0;
        const int PENALTY_NIGHTMARE = -40;
        const int PENALTY_HELL = -100;
        
        const int STR_IDX = 0;
        const int ENE_IDX = 1;
        const int DEX_IDX = 2;
        const int VIT_IDX = 3;

        const int CURRENT_LIFE_IDX = 6;
        const int MAX_LIFE_IDX = 7;

        const int CURRENT_MANA_IDX = 8;
        const int MAX_MANA_IDX = 9;

        const int CURRENT_STAMINA_IDX = 10;
        const int MAX_STAMINA_IDX = 11;

        const int LVL_IDX = 12;
        const int XP_IDX = 13;
        const int GOLD_BODY_IDX = 14;
        const int GOLD_STASH_IDX = 15;

        const int UNKNOWN1_IDX = 19;
        const int UNKNOWN2_IDX = 20;
        const int UNKNOWN3_IDX = 21;
        const int UNKNOWN4_IDX = 22;
        const int UNKNOWN5_IDX = 23;

        const int UNKNOWN6_IDX = 27;
        const int UNKNOWN7_IDX = 29;
        const int UNKNOWN8_IDX = 30;
        const int DEF_IDX = 31; //def (val + 1/4*dex)

        const int FIRE_RES_IDX = 39;
        const int FIRE_RES_ADD_IDX = 40;
        const int LIGHTNING_RES_IDX = 41;
        const int LIGHTNING_RES_ADD_IDX = 42;
        const int COLD_RES_IDX = 43;
        const int COLD_RES_ADD_IDX = 44;
        const int POISON_RES_IDX = 45;
        const int POISON_RES_ADD_IDX = 46;

        const int UNKNOWN10_IDX = 48;
        const int UNKNOWN11_IDX = 49;
        const int UNKNOWN12_IDX = 50;
        const int UNKNOWN13_IDX = 51;

        const int UNKNOWN14_IDX = 62;
        const int UNKNOWN15_IDX = 67;
        const int UNKNOWN16_IDX = 68;
        const int UNKNOWN17_IDX = 69;

        const int UNKNOWN18_IDX = 71;
        const int UNKNOWN19_IDX = 72;
        const int UNKNOWN20_IDX = 73;
        const int UNKNOWN21_IDX = 74;

        const int UNKNOWN22_IDX = 79;
        const int UNKNOWN23_IDX = 80;
        const int UNKNOWN24_IDX = 83;
        const int UNKNOWN25_IDX = 89;
        const int UNKNOWN26_IDX = 95;
        const int UNKNOWN27_IDX = 99;

        const int UNKNOWN28_IDX = 107;
        const int UNKNOWN29_IDX = 110;
        const int UNKNOWN30_IDX = 128;
        const int UNKNOWN31_IDX = 159;
        const int UNKNOWN32_IDX = 172;
        const int UNKNOWN33_IDX = 194;
        const int UNKNOWN34_IDX = 201;
        const int UNKNOWN35_IDX = 204;


        #region patch 1.14a adresses
        // const int ADDRESS_MODE = 0x44C658;
        // int[] OFFSETS_MODE = new int[] { 0x40, 0x210 };
        // const int ADDRESS_CHARACTER = 0x00498288;
        // int[] OFFSETS_CHARACTER = new int[] { 0x174, 0x5c, 0x48, 0x00 };

        // const int ADDRESS_DIFFICULTY = 0x42E1AC;
        // const int ADDRESS_NAME = 0x82E164;
        #endregion

        #region patch 1.14b adresses
        const int ADDRESS_MODE = 0x0039DEFC;
        int[] OFFSETS_MODE = new int[] { 0x10 };
        const int ADDRESS_CHARACTER = 0x0039DEFC;
        int[] OFFSETS_PLAYER_STATS = new int[] { 0x5c, 0x48, 0x00};
        int[] OFFSETS_INVENTORY = new int[] { 0x60 };
        
        const int ADDRESS_QUESTS = 0x003B8E54;
        int[] OFFSETS_QUESTS = new int[] { 0x264, 0x450, 0x20, 0x00 };

        const int ADDRESS_DIFFICULTY = 0x00398694;
        const int ADDRESS_NAME = 0x0039864C;
        const int ADDRESS_AREA = 0x0039B1C8;
        #endregion


        Dictionary<int, int> dataDict;

        int difficulty;
        int currentPenalty;
        bool dead;
        int deaths;
        int levelBefore = 0;

        int area;

        int bytesRead = 0;
        
        string name;
        Encoding enc;
        byte[] nameBytes;
        Thread robto;

        bool processRunning;
        Process D2Process;
        IntPtr D2ProcessHandle;
        IntPtr D2ProcessEntryAddress;
        
        int indexVal;
        int valOffset;
        int valBytes;


        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public List<AutoSplit.AutoSplit> getAutosplits()
        {
            return this.settings.autosplits;
        }
        public void addAutosplit(AutoSplit.AutoSplit autosplit)
        {
            this.settings.autosplits.Add(autosplit);
        }

        private byte reverseBits(byte b)
        {
            return (byte)(((b * 0x80200802ul) & 0x0884422110ul) * 0x0101010101ul >> 32);
        }
        private bool isNthBitSet(short c, int n)
        {
            int[] mask = { 128, 64, 32, 16, 8, 4, 2, 1 };
            if (n >= 8)
            {
                c = (short)((int)c >> 8);
                n -= 8;
            }
            return ((c & mask[n]) != 0);
        }

        public void updateAutosplits()
        {
            int y = 0;
            panel1.Controls.Clear();
            foreach (AutoSplit.AutoSplit autosplit in settings.autosplits)
            {
                Label lbl = new Label();
                lbl.SetBounds(0, y, panel1.Bounds.Width, 16);
                panel1.Controls.Add(lbl);
                autosplit.bindControl(lbl);
                y += 16;
            }
        }

        private void initialize()
        {
            settings = new SettingsHolder();
            settings.load();
            processRunning = false;
            enc = Encoding.GetEncoding("UTF-8");
            nameBytes = new byte[15];
            dead = false;
            deaths = 0;
            dataDict = new Dictionary<int, int>();

            // debugWindow = new DebugWindow();
            // debugWindow.Show();
            try
            {
                Process[] processes = Process.GetProcessesByName("game");
                if (processes.Length > 0)
                {
                    D2Process = processes[0];
                    D2ProcessHandle = OpenProcess(PROCESS_WM_READ, false, D2Process.Id);
                    ProcessModule processModule;
                    ProcessModuleCollection myProcessModuleCollection = D2Process.Modules;
                    for (int i = 0; i < myProcessModuleCollection.Count; i++)
                    {
                        processModule = myProcessModuleCollection[i];
                        if (processModule.ModuleName == "Game.exe")
                        {
                            D2ProcessEntryAddress = processModule.BaseAddress;
                            processRunning = true;
                            break;
                        }
                    }
                }
            } catch ( Exception e )
            {
                MessageBox.Show("Unable to initialize. Run with admin rights + make sure diablo 2 is running.");
            }
            
            applySettings();
            updateAutosplits();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            initialize();
            robto = new Thread(readGamedataThreadFunc);
            robto.Start();

        }
        public void readGamedataThreadFunc()
        {
            while (1 == 1)
            {
                Thread.Sleep(1000);

                if ( !processRunning ) { continue; }

                dataDict.Clear();

                // quests
                byte[] questBuffer = getBuffer(96, ADDRESS_QUESTS, OFFSETS_QUESTS, true);
                if (debugWindow != null)
                {
                    this.debugWindow.setQuestData(questBuffer);
                }

                try {
                    indexVal = 0;
                    // Start of player stuff (70 is just arbitrary number that is high enough to hold all the player info... )
                    for (int i = 0; i < 70; i++)
                    {
                        OFFSETS_PLAYER_STATS[OFFSETS_PLAYER_STATS.Length - 1] = 0x2 + i * 8;
                        if (i == 0)
                        {
                            indexVal = getByte(ADDRESS_CHARACTER, OFFSETS_PLAYER_STATS, true);
                        } else
                        {
                            int tmpVal = getByte(ADDRESS_CHARACTER, OFFSETS_PLAYER_STATS, true);
                            if ( indexVal > tmpVal )
                            {
                                break;
                            } else
                            {
                                indexVal = tmpVal;
                            }
                        }
                        valOffset = 0;
                        valBytes = 0;

                        switch (indexVal)
                        {
                            case STR_IDX:
                            case ENE_IDX:
                            case DEX_IDX:
                            case VIT_IDX:
                            case LVL_IDX:
                            case XP_IDX:
                            case GOLD_BODY_IDX:
                            case GOLD_STASH_IDX:

                            case DEF_IDX:

                            case FIRE_RES_IDX:
                            case FIRE_RES_ADD_IDX:
                            case LIGHTNING_RES_IDX:
                            case LIGHTNING_RES_ADD_IDX:
                            case COLD_RES_IDX:
                            case COLD_RES_ADD_IDX:
                            case POISON_RES_IDX:
                            case POISON_RES_ADD_IDX:
                                valOffset = 2;
                                valBytes = 4;
                                break;
                            case CURRENT_MANA_IDX:
                            case MAX_MANA_IDX:
                            case CURRENT_STAMINA_IDX:
                            case MAX_STAMINA_IDX:
                            case CURRENT_LIFE_IDX:
                            case MAX_LIFE_IDX:
                                // at offset 2 is a comma value. for us it is enough to know the int val
                                valOffset = 3;
                                valBytes = 4;
                                break;
                        }
                        if (valOffset > 0 && valBytes > 0)
                        {
                            OFFSETS_PLAYER_STATS[OFFSETS_PLAYER_STATS.Length - 1] = 0x2 + valOffset + i * 8;
                            
                            if (!dataDict.ContainsKey(indexVal))
                            {
                                dataDict.Add(indexVal, getInt(ADDRESS_CHARACTER, OFFSETS_PLAYER_STATS, true));
                            }
                        }
                        //Console.WriteLine(indexVal + ": " + getValueByIntAttribute(new IntAttribute(ADDRESS_CHARACTER, POINTERS_CHARACTER, 4, true)));
                    }


                    if (!dataDict.ContainsKey(STR_IDX)) dataDict.Add(STR_IDX, 0);
                    if (!dataDict.ContainsKey(ENE_IDX)) dataDict.Add(ENE_IDX, 0);
                    if (!dataDict.ContainsKey(DEX_IDX)) dataDict.Add(DEX_IDX, 0);
                    if (!dataDict.ContainsKey(VIT_IDX)) dataDict.Add(VIT_IDX, 0);
                    if (!dataDict.ContainsKey(LVL_IDX)) dataDict.Add(LVL_IDX, 0);
                    if (!dataDict.ContainsKey(XP_IDX)) dataDict.Add(XP_IDX, 0);
                    if (!dataDict.ContainsKey(GOLD_BODY_IDX)) dataDict.Add(GOLD_BODY_IDX, 0);
                    if (!dataDict.ContainsKey(GOLD_STASH_IDX)) dataDict.Add(GOLD_STASH_IDX, 0);
                    if (!dataDict.ContainsKey(DEF_IDX)) dataDict.Add(DEF_IDX, 0);

                    if (!dataDict.ContainsKey(FIRE_RES_IDX)) dataDict.Add(FIRE_RES_IDX, 0);
                    if (!dataDict.ContainsKey(FIRE_RES_ADD_IDX)) dataDict.Add(FIRE_RES_ADD_IDX, 0);
                    if (!dataDict.ContainsKey(LIGHTNING_RES_IDX)) dataDict.Add(LIGHTNING_RES_IDX, 0);
                    if (!dataDict.ContainsKey(LIGHTNING_RES_ADD_IDX)) dataDict.Add(LIGHTNING_RES_ADD_IDX, 0);
                    if (!dataDict.ContainsKey(COLD_RES_IDX)) dataDict.Add(COLD_RES_IDX, 0);
                    if (!dataDict.ContainsKey(COLD_RES_ADD_IDX)) dataDict.Add(COLD_RES_ADD_IDX, 0);
                    if (!dataDict.ContainsKey(POISON_RES_IDX)) dataDict.Add(POISON_RES_IDX, 0);
                    if (!dataDict.ContainsKey(POISON_RES_ADD_IDX)) dataDict.Add(POISON_RES_ADD_IDX, 0);

                    if (!dataDict.ContainsKey(CURRENT_LIFE_IDX)) dataDict.Add(CURRENT_LIFE_IDX, 0);
                    if (!dataDict.ContainsKey(MAX_LIFE_IDX)) dataDict.Add(MAX_LIFE_IDX, 0);
                    if (!dataDict.ContainsKey(CURRENT_MANA_IDX)) dataDict.Add(CURRENT_MANA_IDX, 0);
                    if (!dataDict.ContainsKey(MAX_MANA_IDX)) dataDict.Add(MAX_MANA_IDX, 0);
                    if (!dataDict.ContainsKey(CURRENT_STAMINA_IDX)) dataDict.Add(CURRENT_STAMINA_IDX, 0);
                    if (!dataDict.ContainsKey(MAX_STAMINA_IDX)) dataDict.Add(MAX_STAMINA_IDX, 0);

                    ReadProcessMemory((int)D2ProcessHandle, D2ProcessEntryAddress.ToInt32() + ADDRESS_NAME, nameBytes, nameBytes.Length, ref bytesRead);
                    string tmpName = enc.GetString(nameBytes);
                    if (tmpName != name && dataDict[LVL_IDX] == 1)
                    {
                        name = tmpName;
                        deaths = 0; // reset the deaths if name changed
                        foreach (AutoSplit.AutoSplit autosplit in this.settings.autosplits)
                        {
                            autosplit.reached = false;
                            switch (autosplit.type)
                            {
                                case AutoSplit.AutoSplit.TYPE_SPECIAL:
                                    if (autosplit.value == (int)AutoSplit.AutoSplit.Special.GAMESTART)
                                    {
                                        autosplit.reached = true;
                                        triggerAutosplit();
                                    }
                                    break;
                            }
                        }
                    }
                    
                    var mode = getInt(ADDRESS_MODE, OFFSETS_MODE, true);
                    levelBefore = dataDict[LVL_IDX];
                    if (dataDict[LVL_IDX] > 0 && (mode == MODE_DEAD || mode == MODE_DEATH) ) {
                        if (!dead) {
                            dead = true;
                            deaths++;
                        }
                    } else {
                        dead = false;
                    }


                    area = getByte(ADDRESS_AREA, null, true);
                    difficulty = getByte(ADDRESS_DIFFICULTY, null, true);
                    switch (difficulty)
                    {
                        case 0: currentPenalty = PENALTY_NORMAL; break;
                        case 1: currentPenalty = PENALTY_NIGHTMARE; break;
                        case 2: currentPenalty = PENALTY_HELL; break;
                    }
                    dataDict[FIRE_RES_IDX] += currentPenalty;
                    dataDict[COLD_RES_IDX] += currentPenalty;
                    dataDict[LIGHTNING_RES_IDX] += currentPenalty;
                    dataDict[POISON_RES_IDX] += currentPenalty;

                    dataDict[FIRE_RES_IDX] = Math.Min(dataDict[FIRE_RES_IDX], 75 + dataDict[FIRE_RES_ADD_IDX]);
                    dataDict[COLD_RES_IDX] = Math.Min(dataDict[COLD_RES_IDX], 75 + dataDict[COLD_RES_ADD_IDX]);
                    dataDict[LIGHTNING_RES_IDX] = Math.Min(dataDict[LIGHTNING_RES_IDX], 75 + dataDict[LIGHTNING_RES_ADD_IDX]);
                    dataDict[POISON_RES_IDX] = Math.Min(dataDict[POISON_RES_IDX], 75 + dataDict[POISON_RES_ADD_IDX]);

                    nameLabel.Invoke(new Action(delegate () { nameLabel.Text = name; })); // name
                    lvlLabel.Invoke(new Action(delegate () { lvlLabel.Text = "LVL: " + dataDict[LVL_IDX]; })); // level
                    strengthLabel.Invoke(new Action(delegate () { strengthLabel.Text = "STR: " + dataDict[STR_IDX]; }));
                    dexterityLabel.Invoke(new Action(delegate () { dexterityLabel.Text = "DEX: " + dataDict[DEX_IDX]; }));
                    vitalityLabel.Invoke(new Action(delegate () { vitalityLabel.Text = "VIT: " + dataDict[VIT_IDX]; }));
                    energyLabel.Invoke(new Action(delegate () { energyLabel.Text = "ENE: " + dataDict[ENE_IDX]; }));

                    fireResLabel.Invoke(new Action(delegate () { fireResLabel.Text = "FIRE: " + (dataDict[FIRE_RES_IDX]); }));
                    coldResLabel.Invoke(new Action(delegate () { coldResLabel.Text = "COLD: " + (dataDict[COLD_RES_IDX]); }));
                    lightningResLabel.Invoke(new Action(delegate () { lightningResLabel.Text = "LIGH: " + (dataDict[LIGHTNING_RES_IDX]); }));
                    poisonResLabel.Invoke(new Action(delegate () { poisonResLabel.Text = "POIS: " + (dataDict[POISON_RES_IDX]); }));

                    goldLabel.Invoke(new Action(delegate () { goldLabel.Text = "GOLD: " + (dataDict[GOLD_BODY_IDX] + dataDict[GOLD_STASH_IDX]); }));
                    deathsLabel.Invoke(new Action(delegate () { deathsLabel.Text = "DEATHS: " + deaths; }));

                    if (this.settings.createFiles)
                    {
                        if (!Directory.Exists(fileFolder))
                        {
                            Directory.CreateDirectory(fileFolder);
                        }
                        
                        File.WriteAllText(fileFolder + "/name.txt", name);
                        File.WriteAllText(fileFolder + "/level.txt", dataDict[LVL_IDX].ToString());
                        File.WriteAllText(fileFolder + "/strength.txt", dataDict[STR_IDX].ToString());
                        File.WriteAllText(fileFolder + "/dexterity.txt", dataDict[DEX_IDX].ToString());
                        File.WriteAllText(fileFolder + "/vitality.txt", dataDict[VIT_IDX].ToString());
                        File.WriteAllText(fileFolder + "/energy.txt", dataDict[ENE_IDX].ToString());
                        File.WriteAllText(fileFolder + "/fire_res.txt", (dataDict[FIRE_RES_IDX]).ToString());
                        File.WriteAllText(fileFolder + "/cold_res.txt", (dataDict[COLD_RES_IDX]).ToString());
                        File.WriteAllText(fileFolder + "/light_res.txt", (dataDict[LIGHTNING_RES_IDX]).ToString());
                        File.WriteAllText(fileFolder + "/poison_res.txt", (dataDict[POISON_RES_IDX]).ToString());
                        File.WriteAllText(fileFolder + "/gold.txt", (dataDict[GOLD_BODY_IDX] + dataDict[GOLD_STASH_IDX]).ToString());
                        File.WriteAllText(fileFolder + "/deaths.txt", deaths.ToString());
                    }

                    bool haveCharLevelCheckpoints = false;
                    bool haveEnterLevelCheckpoints = false;
                    bool haveItemCheckpoints = false;
                    bool haveQuestCheckpoints = false;


                    foreach (AutoSplit.AutoSplit autosplit in this.settings.autosplits)
                    {
                        if (autosplit.reached)
                        {
                            continue;
                        }
                        switch (autosplit.type)
                        {
                            case AutoSplit.AutoSplit.TYPE_CHAR_LEVEL:
                                haveCharLevelCheckpoints = true;
                                break;
                            case AutoSplit.AutoSplit.TYPE_AREA:
                                haveEnterLevelCheckpoints = true;
                                break;
                            case AutoSplit.AutoSplit.TYPE_ITEM:
                                haveItemCheckpoints = true;
                                break;
                            case AutoSplit.AutoSplit.TYPE_QUEST:
                                haveQuestCheckpoints = true;
                                break;
                        }
                    }
                    
                    List<int> itemsIds = new List<int>();
                    if (haveItemCheckpoints)
                    {

                        int inventoryAddr = getInt(ADDRESS_CHARACTER, OFFSETS_INVENTORY, true);
                        int itemType;
                        int itemAddress;
                        int unitDataAddress;

                        // cursor item
                        itemAddress = getInt(inventoryAddr + 0x20); //BitConverter.ToInt32(new byte[4] { inventory[0x20], inventory[0x21], inventory[0x22], inventory[0x23] }, 0);
                        if (itemAddress > 0)
                        {
                            itemType = getInt(itemAddress + 0x04);
                            itemsIds.Add(itemType);
                            Console.WriteLine("type cursor: " + itemType);
                        }

                        // all the other items
                        itemAddress = getInt(inventoryAddr + 0x10);// BitConverter.ToInt32(new byte[4] { inventory[0x10], inventory[0x11], inventory[0x12], inventory[0x13] }, 0);
                        if (itemAddress > 0)
                        {
                            itemType = getInt(itemAddress + 0x04);
                            itemsIds.Add(itemType);
                            Console.WriteLine("type last: " + itemType);

                            while (itemAddress > 0)
                            {
                                unitDataAddress = getInt(itemAddress + 0x14);
                                itemAddress = getInt(unitDataAddress + 0x60);
                                if (itemAddress > 0)
                                {

                                    itemType = getInt(itemAddress + 0x04);
                                    itemsIds.Add(itemType);
                                    Console.WriteLine("prev item type: " + itemType);

                                }
                            }
                        }
                        
                    }

                    foreach (AutoSplit.AutoSplit autosplit in this.settings.autosplits)
                    {
                        if (autosplit.reached)
                        {
                            continue;
                        }

                        switch (autosplit.type)
                        {
                            case AutoSplit.AutoSplit.TYPE_CHAR_LEVEL:
                                if (autosplit.value <= dataDict[LVL_IDX])
                                {
                                    autosplit.reached = true;
                                    triggerAutosplit();
                                }
                                break;
                            case AutoSplit.AutoSplit.TYPE_AREA:
                                if (autosplit.value == area)
                                {
                                    autosplit.reached = true;
                                    triggerAutosplit();
                                }
                                break;
                            case AutoSplit.AutoSplit.TYPE_ITEM:
                                if (itemsIds.Contains(autosplit.value))
                                {
                                    autosplit.reached = true;
                                    triggerAutosplit();
                                }
                                break;
                            case AutoSplit.AutoSplit.TYPE_QUEST:
                                short value = BitConverter.ToInt16(new byte[2] { reverseBits(questBuffer[autosplit.value]), reverseBits(questBuffer[autosplit.value + 1]), }, 0);
                                if (isNthBitSet(value, 0) || isNthBitSet(value, 1))
                                {
                                    // quest finished 
                                    autosplit.reached = true;
                                    triggerAutosplit();
                                }
                                break;
                        }
                    }

                } catch ( Exception ex )
                {
                    processRunning = false;
                }
            }
        }

        private void triggerAutosplit ()
        {
            if (settings.doAutosplit && settings.triggerKeys != "")
            {
                Console.WriteLine("{" + settings.triggerKeys + "}");
                SendKeys.SendWait("{"+ settings.triggerKeys + "}");
            }
        }

        public short getByte(int address, int[] offsets = null, bool relative = false)
        {
            byte[] bufferValue = getBuffer(0x01, address, offsets, relative);
            try
            {
                return bufferValue[0];

            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public short getShort(int address, int[] offsets = null, bool relative = false )
        {
            byte[] bufferValue = getBuffer(0x02, address, offsets, relative);
            try
            {
                return BitConverter.ToInt16(bufferValue, 0);

            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public int getInt(int address, int[] offsets = null, bool relative = false)
        {
            byte[] bufferValue = getBuffer(0x04, address, offsets, relative);
            try
            {
                return BitConverter.ToInt32(bufferValue, 0);

            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public byte[] getBuffer(int length, int address, int[] offsets = null, bool relative = false)
        {
            byte[] bufferAddress = new byte[4];
            byte[] bufferValue = new byte[length];
            int pointer = address;

            if (relative)
            {
                pointer = D2ProcessEntryAddress.ToInt32() + pointer;
            }
            if (offsets != null)
            {
                for (int i = 0; i < offsets.Length; i++)
                {
                    ReadProcessMemory((int)D2ProcessHandle, pointer, bufferAddress, bufferAddress.Length, ref bytesRead);
                    pointer = BitConverter.ToInt32(bufferAddress, 0) + offsets[i];
                }

            }

            ReadProcessMemory((int)D2ProcessHandle, pointer, bufferValue, bufferValue.Length, ref bytesRead);
            return bufferValue;
        }

        private void setFonts(string fontName, int size = 10, int sizeTitle = 18)
        {
            Font fBig = new Font(fontName, sizeTitle);
            Font fSmall = new Font(fontName, size);

            nameLabel.Font = fBig;
            lvlLabel.Font = fSmall;
            strengthLabel.Font = fSmall;
            dexterityLabel.Font = fSmall;
            vitalityLabel.Font = fSmall;
            energyLabel.Font = fSmall;
            fireResLabel.Font = fSmall;
            coldResLabel.Font = fSmall;
            lightningResLabel.Font = fSmall;
            poisonResLabel.Font = fSmall;
            goldLabel.Font = fSmall;
            deathsLabel.Font = fSmall;
        }

        public void applySettings()
        {
            setFonts(this.settings.fontName, this.settings.fontSize, this.settings.titleFontSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            robto.Abort();
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            initialize();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            robto.Abort();
            Application.Exit();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (settingsWindow == null || settingsWindow.IsDisposed) {
                settingsWindow = new SettingsWindow(this);
            }
            settingsWindow.Show();
            settingsWindow.Focus();
        }

    }
}
