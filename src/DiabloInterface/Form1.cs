using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace DiabloInterface
{
    public partial class Form1 : Form
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        string fileFolder = "txt";

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
        int[] OFFSETS_CHARACTER = new int[] { 0x5c, 0x48, 0x00};
        
        const int ADDRESS_DIFFICULTY = 0x00398694;
        const int ADDRESS_NAME = 0x0039864C;
        #endregion


        Dictionary<int, int> dataDict;

        int difficulty;
        int currentPenalty;
        bool dead;
        int deaths;

        int bytesRead = 0;
        
        string name;
        Encoding enc;
        byte[] nameBytes;
        Thread robto;

        bool processRunning;
        Process D2Process;
        IntPtr D2ProcessHandle;
        IntPtr D2ProcessEntryAddress;

        IntAttribute idx;
        IntAttribute val;
        int indexVal;
        int valOffset;
        int valBytes;

        public Form1()
        {
            InitializeComponent();
        }

        private void initialize()
        {
            processRunning = false;
            enc = Encoding.GetEncoding("UTF-8");
            nameBytes = new byte[15];
            dead = false;
            deaths = 0;
            dataDict = new Dictionary<int, int>();
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

                try {
                    indexVal = 0;
                    // Start of player stuff (70 is just arbitrary number that is high enough to hold all the player info... )
                    for (var i = 0; i < 70; i++)
                    {
                        OFFSETS_CHARACTER[OFFSETS_CHARACTER.Length - 1] = 0x2 + i * 8;
                        idx = new IntAttribute(ADDRESS_CHARACTER, OFFSETS_CHARACTER, 1, true);
                        if (i == 0)
                        {
                            indexVal = getValueByIntAttribute(idx);
                        } else
                        {
                            int tmpVal = getValueByIntAttribute(idx);
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
                            OFFSETS_CHARACTER[OFFSETS_CHARACTER.Length - 1] = 0x2 + valOffset + i * 8;

                            val = new IntAttribute(ADDRESS_CHARACTER, OFFSETS_CHARACTER, valBytes, true);
                            if (!dataDict.ContainsKey(indexVal))
                            {
                                dataDict.Add(indexVal, getValueByIntAttribute(val));
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
                    if (tmpName != name)
                    {
                        name = tmpName;
                        deaths = 0; // reset the deaths if name changed
                    }

                    var mode = new IntAttribute(ADDRESS_MODE, OFFSETS_MODE, 4, true);
                    var modeVal = getValueByIntAttribute(mode);
                    if (dataDict[LVL_IDX] > 0 && (modeVal == MODE_DEAD || modeVal == MODE_DEATH) ) {
                        if (!dead) {
                            dead = true;
                            deaths++;
                        }
                    } else {
                        dead = false;
                    }

                    var dfc = new IntAttribute(ADDRESS_DIFFICULTY, new int[] { }, 1, true);
                    difficulty = getValueByIntAttribute(dfc);
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

                    if (createFilesCheckbox.Checked)
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

                } catch ( Exception ex )
                {
                    processRunning = false;
                }
            }
        }
        
        public int getValueByIntAttribute(IntAttribute intAttr)
        {

            byte[] bufferAddress = new byte[4];
            byte[] bufferValue = new byte[intAttr.bytes];
            int pointer = intAttr.pointer;

            if (intAttr.pointerRelative)
            {
                pointer = D2ProcessEntryAddress.ToInt32() + pointer;
            }
            //
            for ( int i = 0; i < intAttr.offsets.Length; i++ )
            {
                ReadProcessMemory((int)D2ProcessHandle, pointer, bufferAddress, bufferAddress.Length, ref bytesRead);
                pointer = BitConverter.ToInt32(bufferAddress, 0) + intAttr.offsets[i];
            }

            ReadProcessMemory((int)D2ProcessHandle, pointer, bufferValue, bufferValue.Length, ref bytesRead);
            if (bufferValue.Length > 1) {
                try
                {
                intAttr.value = BitConverter.ToInt32(bufferValue, 0);

                } catch ( Exception e )
                {
                    intAttr.value = -1;
                }
            } else {
                intAttr.value = bufferValue[0];
            }
            return intAttr.value;
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
            FontDialog fontDialog1 = new FontDialog();
            // Show the dialog.
            DialogResult result = fontDialog1.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                nameLabel.Font = new Font (fontDialog1.Font.Name,18);
                lvlLabel.Font = new Font(fontDialog1.Font.Name, 10);
                strengthLabel.Font = new Font(fontDialog1.Font.Name, 10);
                dexterityLabel.Font = new Font(fontDialog1.Font.Name, 10);
                vitalityLabel.Font = new Font(fontDialog1.Font.Name, 10);
                energyLabel.Font = new Font(fontDialog1.Font.Name, 10);
                fireResLabel.Font = new Font(fontDialog1.Font.Name, 10);
                coldResLabel.Font = new Font(fontDialog1.Font.Name, 10);
                lightningResLabel.Font = new Font(fontDialog1.Font.Name, 10);
                poisonResLabel.Font = new Font(fontDialog1.Font.Name, 10);
                goldLabel.Font = new Font(fontDialog1.Font.Name, 10);
                deathsLabel.Font = new Font(fontDialog1.Font.Name, 10);
            }
        }
    }
}
