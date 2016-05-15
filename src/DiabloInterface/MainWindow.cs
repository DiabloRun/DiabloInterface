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

namespace DiabloInterface
{
    public partial class MainWindow : Form
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        
        public SettingsHolder settings;

        Thread dataReaderThread;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        D2DataReader dataReader;

        public MainWindow()
        {
            InitializeComponent();

            initialize();
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

        private void initialize()
        {
            settings = new SettingsHolder();
            settings.load();

            if (dataReader == null)
            {
                dataReader = new D2DataReader(this);
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
            int y = 0;
            panel1.Controls.Clear();
            foreach (AutoSplit autosplit in settings.autosplits)
            {
                Label lbl = new Label();
                lbl.SetBounds(0, y, panel1.Bounds.Width, 16);
                panel1.Controls.Add(lbl);
                autosplit.bindControl(lbl);
                y += 16;
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
                Console.WriteLine("{" + settings.triggerKeys + "}");
                SendKeys.SendWait("{" + settings.triggerKeys + "}");
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
