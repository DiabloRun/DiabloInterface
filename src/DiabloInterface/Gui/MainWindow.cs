using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace DiabloInterface
{
    public partial class MainWindow : Form
    {
        public SettingsHolder settings;

        Thread dataReaderThread;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        D2DataReader dataReader;

        public MainWindow()
        {
            // We want to dispose our handles once the window is disposed.
            Disposed += OnWindowDisposed;

            InitializeComponent();

            initialize();
        }

        private void OnWindowDisposed(object sender, EventArgs e)
        {
            // Make sure the process handles close correctly.
            if (dataReader != null)
            {
                dataReader.Dispose();
                dataReader = null;
            }
        }

        public DebugWindow getDebugWindow()
        {
            return this.debugWindow;
        }

        public List<AutoSplit> getAutosplits()
        {
            return this.settings.autosplits;
        }
        public void addAutosplit(AutoSplit autosplit)
        {
            this.settings.autosplits.Add(autosplit);
        }

        private D2MemoryTable GetVersionMemoryTable(string version)
        {
            D2MemoryTable memoryTable = new D2MemoryTable();
            memoryTable.SupportsItemReading = false;

            // Offsets are the same for both versions so far.
            memoryTable.Offset.Quests = new int[] { 0x264, 0x450, 0x20, 0x00 };

            switch (version)
            {
                case "1.14b":
                    memoryTable.Address.Character   = new IntPtr(0x0039DEFC);
                    memoryTable.Address.Quests      = new IntPtr(0x003B8E54);
                    memoryTable.Address.Difficulty  = new IntPtr(0x00398694);
                    memoryTable.Address.Area        = new IntPtr(0x0039B1C8);
                    break;
                case "1.14c":
                default:
                    memoryTable.Address.Character   = new IntPtr(0x0039CEFC);
                    memoryTable.Address.Quests      = new IntPtr(0x003B7E54);
                    memoryTable.Address.Difficulty  = new IntPtr(0x00397694);
                    memoryTable.Address.Area        = new IntPtr(0x0039A1C8);

                    memoryTable.SupportsItemReading = true;
                    memoryTable.Address.ItemDescriptions            = new IntPtr(0x5639E0);
                    memoryTable.Address.MagicModifierTable          = new IntPtr(0x563A04);
                    memoryTable.Address.RareModifierTable           = new IntPtr(0x563A28);
                    memoryTable.Address.StringIndexerTable          = new IntPtr(0x479A3C);
                    memoryTable.Address.StringAddressTable          = new IntPtr(0x479A40);
                    memoryTable.Address.PatchStringIndexerTable     = new IntPtr(0x479A58);
                    memoryTable.Address.PatchStringAddressTable     = new IntPtr(0x479A44);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x479A5C);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x479A48);
                    break;
            }

            return memoryTable;
        }

        private void initialize()
        {
            settings = new SettingsHolder();
            settings.load();

            if (dataReader == null)
            {
                var memoryTable = GetVersionMemoryTable(settings.d2Version);
                dataReader = new D2DataReader(this, memoryTable);
            }
            if (!dataReader.checkIfD2Running())
            {
                MessageBox.Show("Unable to initialize. Run with admin rights + make sure diablo 2 is running.");
            }
            if (dataReaderThread == null)
            {
                dataReaderThread = new Thread(dataReader.readDataThreadFunc);
                dataReaderThread.Start();
            }

            applySettings();
            updateAutosplits();
        }

        public void updateAutosplits()
        {
            if (debugWindow != null)
            {
                debugWindow.updateAutosplits(settings.autosplits);
            }
        }

        public void updateLabels ( D2Player player )
        {
            nameLabel.Invoke(new Action(delegate () { nameLabel.Text = player.name; })); // name
            lvlLabel.Invoke(new Action(delegate () { lvlLabel.Text = "LVL: " + player.lvl ; })); // level
            strengthLabel.Invoke(new Action(delegate () { strengthLabel.Text = "STR: " + player.strength; }));
            dexterityLabel.Invoke(new Action(delegate () { dexterityLabel.Text = "DEX: " + player.dexterity; }));
            vitalityLabel.Invoke(new Action(delegate () { vitalityLabel.Text = "VIT: " + player.vitality; }));
            energyLabel.Invoke(new Action(delegate () { energyLabel.Text = "ENE: " + player.energy; }));
            fireResLabel.Invoke(new Action(delegate () { fireResLabel.Text = "FIRE: " + player.calculatedFireRes; }));
            coldResLabel.Invoke(new Action(delegate () { coldResLabel.Text = "COLD: " + player.calculatedColdRes; }));
            lightningResLabel.Invoke(new Action(delegate () { lightningResLabel.Text = "LIGH: " + player.calculatedLightningRes; }));
            poisonResLabel.Invoke(new Action(delegate () { poisonResLabel.Text = "POIS: " + player.calculatedPoisonRes; }));
            goldLabel.Invoke(new Action(delegate () { goldLabel.Text = "GOLD: " + (player.goldBody + player.goldStash); }));
            deathsLabel.Invoke(new Action(delegate () { deathsLabel.Text = "DEATHS: " + player.deaths; }));
        }

        public void triggerAutosplit(D2Player player)
        {
            if (player.newlyStarted && settings.doAutosplit && settings.triggerKeys != "")
            {
                KeyManager.sendKeys(settings.triggerKeys);
            }
        }

        public void writeFiles(D2Player player)
        {

            // todo: only write files if content changed
            if (!settings.createFiles)
            {
                return;
            }

            if (!Directory.Exists(settings.fileFolder))
            {
                Directory.CreateDirectory(settings.fileFolder);
            }

            File.WriteAllText(settings.fileFolder + "/name.txt", player.name);
            File.WriteAllText(settings.fileFolder + "/level.txt", player.lvl.ToString());
            File.WriteAllText(settings.fileFolder + "/strength.txt", player.strength.ToString());
            File.WriteAllText(settings.fileFolder + "/dexterity.txt", player.dexterity.ToString());
            File.WriteAllText(settings.fileFolder + "/vitality.txt", player.vitality.ToString());
            File.WriteAllText(settings.fileFolder + "/energy.txt", player.energy.ToString());
            File.WriteAllText(settings.fileFolder + "/fire_res.txt", player.calculatedFireRes.ToString());
            File.WriteAllText(settings.fileFolder + "/cold_res.txt", player.calculatedColdRes.ToString());
            File.WriteAllText(settings.fileFolder + "/light_res.txt", player.calculatedLightningRes.ToString());
            File.WriteAllText(settings.fileFolder + "/poison_res.txt", player.calculatedPoisonRes.ToString());
            File.WriteAllText(settings.fileFolder + "/gold.txt", (player.goldBody + player.goldStash).ToString());
            File.WriteAllText(settings.fileFolder + "/deaths.txt", player.deaths.ToString());

        }

        public void applySettings()
        {
            Font fBig = new Font(this.settings.fontName, this.settings.titleFontSize);
            Font fSmall = new Font(this.settings.fontName, this.settings.fontSize);

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

            if ( settings.showDebug )
            {
                if (debugWindow == null || debugWindow.IsDisposed)
                {
                    debugWindow = new DebugWindow();
                }
                debugWindow.Show();
            } else
            {
                if (debugWindow != null && !debugWindow.IsDisposed)
                {
                    debugWindow.Hide();
                }
            }

            var memoryTable = GetVersionMemoryTable(settings.d2Version);
            dataReader.SetNextMemoryTable(memoryTable);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataReaderThread.Abort();
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
            dataReaderThread.Abort();
            Application.Exit();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (settingsWindow == null || settingsWindow.IsDisposed) {
                settingsWindow = new SettingsWindow(this);
            }
            settingsWindow.ShowDialog();
            //settingsWindow.Focus();
        }

    }
}
