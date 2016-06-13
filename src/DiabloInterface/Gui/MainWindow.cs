using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using DiabloInterface.Server;
using DiabloInterface.Gui;
using DiabloInterface.Logging;

namespace DiabloInterface
{
    public partial class MainWindow : Form
    {

        private const int OriginalHeight = 200;
        private const string ItemServerPipeName = "DiabloInterfaceItems";
        private const string WindowTitleFormat = "Diablo Interface v{0}"; // {0} => Application.ProductVersion

        public SettingsHolder Settings;

        Thread dataReaderThread;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        D2DataReader dataReader;
        ItemServer itemServer;

        public MainWindow()
        {
            // We want to dispose our handles once the window is disposed.
            Disposed += OnWindowDisposed;

            InitializeLogger();
            WriteLogHeader();

            InitializeComponent();

            // Display current version along with the application name.
            Text = string.Format(WindowTitleFormat, Application.ProductVersion);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            itemServer.Stop();
            base.OnFormClosing(e);
        }

        void InitializeLogger()
        {
            List<ILogWriter> logWriters = new List<ILogWriter>();

            // Attatch log file writer.
            string logFile = Path.Combine("Logs", FileLogWriter.TimedLogFilename());
            logWriters.Add(new FileLogWriter(logFile));

#if DEBUG
            // Use a console logger on debug versions.
            logWriters.Add(new ConsoleLogWriter());
#endif

            // Create and use the new logger.
            Logger.Instance = new Logger(logWriters);
        }

        void WriteLogHeader()
        {
            Logger.Instance.WriteLineRaw("Diablo Interface Version {0}", Application.ProductVersion);
            Logger.Instance.WriteLineRaw("Operating system: {0}", Environment.OSVersion);
            Logger.Instance.WriteLineRaw(new string('-', 40));
        }

        public void Reset()
        {
            foreach (AutoSplit autosplit in Settings.Autosplits)
            {
                autosplit.Reached = false;
            }
            if (runeDisplayPanel.Controls.Count > 0)
            {
                foreach (RuneDisplayElement c in runeDisplayPanel.Controls)
                {
                    c.setHaveRune(false);
                }
            }
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
            return debugWindow;
        }

        public List<AutoSplit> getAutosplits()
        {
            return Settings.Autosplits;
        }
        public void addAutosplit(AutoSplit autosplit)
        {
            Settings.Autosplits.Add(autosplit);
        }

        private D2MemoryTable GetVersionMemoryTable(string version)
        {
            D2MemoryTable memoryTable = new D2MemoryTable();

            // Offsets are the same for all versions so far.
            memoryTable.Offset.Quests = new int[] { 0x264, 0x450, 0x20, 0x00 };

            switch (version)
            {
                case "1.14c":
                    memoryTable.Address.World       = new IntPtr(0x0047ACC0);
                    memoryTable.Address.GameId      = new IntPtr(0x00479C94);
                    memoryTable.Address.PlayerUnit  = new IntPtr(0x0039CEFC);//(0x39DAF8);
                    memoryTable.Address.Area        = new IntPtr(0x0039A1C8);

                    memoryTable.Address.GlobalData                  = new IntPtr(0x33FD78);
                    memoryTable.Address.LowQualityItems             = new IntPtr(0x563BE0);
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
                case "1.14d":
                default:
                    var off = 0x8f78;

                    memoryTable.Address.World       = new IntPtr(0x00483D38);
                    memoryTable.Address.GameId      = new IntPtr(0x00482D0C);
                    memoryTable.Address.PlayerUnit  = new IntPtr(0x0039CEFC + off);
                    memoryTable.Address.Area        = new IntPtr(0x0039A1C8 + off);

                    memoryTable.Address.GlobalData = new IntPtr(0x344304);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);

                    off = 0x4829b4 - 0x479A3C; // 0x4829b4 = new adress
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x479A3C+ off);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x479A40 + off);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x479A44 + off);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x479A48 + off);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x479A58 + off);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x479A5C + off);

                    break;
            }

            return memoryTable;
        }

        private void initialize()
        {
            Settings = new SettingsHolder();
            Settings.load();

            if (dataReader == null)
            {
                var memoryTable = GetVersionMemoryTable(Settings.D2Version);
                dataReader = new D2DataReader(this, memoryTable);
                itemServer = new ItemServer(dataReader, ItemServerPipeName);
            }

            if (dataReaderThread == null)
            {
                dataReaderThread = new Thread(dataReader.readDataThreadFunc);
                dataReaderThread.Start();
            }

            if (Settings.CheckUpdates)
            {
                VersionChecker.CheckForUpdate(false);
            }

            ApplySettings();
        }

        public void updateLabels ( Character player, Dictionary<int, int> itemClassMap )
        {
            nameLabel.Invoke(new Action(delegate () { nameLabel.Text = player.name; })); // name
            lvlLabel.Invoke(new Action(delegate () { lvlLabel.Text = "LVL: " + player.Level ; })); // level

            strLabel.Invoke(new Action(delegate () { strLabel.Text = "STR: " + player.Strength; }));
            dexLabel.Invoke(new Action(delegate () { dexLabel.Text = "DEX: " + player.Dexterity; }));
            vitLabel.Invoke(new Action(delegate () { vitLabel.Text = "VIT: " + player.Vitality; }));
            eneLabel.Invoke(new Action(delegate () { eneLabel.Text = "ENE: " + player.Energy; }));

            frwLabel.Invoke(new Action(delegate () { frwLabel.Text = "FRW: " + player.FasterRunWalk; }));
            fcrLabel.Invoke(new Action(delegate () { fcrLabel.Text = "FCR: " + player.FasterCastRate; }));
            fhrLabel.Invoke(new Action(delegate () { fhrLabel.Text = "FHR: " + player.FasterHitRecovery; }));
            iasLabel.Invoke(new Action(delegate () { iasLabel.Text = "IAS: " + player.IncreasedAttackSpeed; }));

            fireLabel.Invoke(new Action(delegate () { fireLabel.Text = "FIRE: " + player.FireResist; }));
            coldLabel.Invoke(new Action(delegate () { coldLabel.Text = "COLD: " + player.ColdResist; }));
            lighLabel.Invoke(new Action(delegate () { lighLabel.Text = "LIGH: " + player.LightningResist; }));
            poisLabel.Invoke(new Action(delegate () { poisLabel.Text = "POIS: " + player.PoisonResist; }));

            goldLabel.Invoke(new Action(delegate () { goldLabel.Text = "GOLD: " + (player.Gold + player.GoldStash); }));

            deathsLabel.Invoke(new Action(delegate () { deathsLabel.Text = "DEATHS: " + player.Deaths; }));

            if (runeDisplayPanel.Controls.Count > 0)
            {

                Dictionary<int, int> dict = new Dictionary<int, int>(itemClassMap);
                foreach (RuneDisplayElement c in runeDisplayPanel.Controls)
                {
                    int eClass = (int)c.getRune() + 610;
                    if (dict.ContainsKey(eClass) && dict[eClass] > 0)
                    {
                        dict[eClass]--;
                        c.setHaveRune(true);
                    }
                }
            }
        }

        public void triggerAutosplit(Character player)
        {
            if (Settings.DoAutosplit && Settings.TriggerKeys != "")
            {
                KeyManager.sendKeys(Settings.TriggerKeys);
            }
        }

        public void writeFiles(Character player)
        {

            // todo: only write files if content changed
            if (!Settings.CreateFiles)
            {
                return;
            }

            if (!Directory.Exists(Settings.FileFolder))
            {
                Directory.CreateDirectory(Settings.FileFolder);
            }

            File.WriteAllText(Settings.FileFolder + "/name.txt", player.name);
            File.WriteAllText(Settings.FileFolder + "/level.txt", player.Level.ToString());
            File.WriteAllText(Settings.FileFolder + "/strength.txt", player.Strength.ToString());
            File.WriteAllText(Settings.FileFolder + "/dexterity.txt", player.Dexterity.ToString());
            File.WriteAllText(Settings.FileFolder + "/vitality.txt", player.Vitality.ToString());
            File.WriteAllText(Settings.FileFolder + "/energy.txt", player.Energy.ToString());
            File.WriteAllText(Settings.FileFolder + "/fire_res.txt", player.FireResist.ToString());
            File.WriteAllText(Settings.FileFolder + "/cold_res.txt", player.ColdResist.ToString());
            File.WriteAllText(Settings.FileFolder + "/light_res.txt", player.LightningResist.ToString());
            File.WriteAllText(Settings.FileFolder + "/poison_res.txt", player.PoisonResist.ToString());
            File.WriteAllText(Settings.FileFolder + "/gold.txt", (player.Gold + player.GoldStash).ToString());
            File.WriteAllText(Settings.FileFolder + "/deaths.txt", player.Deaths.ToString());

        }

        public void ApplySettings()
        {
            Font fBig = new Font(this.Settings.FontName, this.Settings.TitleFontSize);
            Font fSmall = new Font(this.Settings.FontName, this.Settings.FontSize);

            nameLabel.Font = fBig;
            lvlLabel.Font = fSmall;
            strLabel.Font = fSmall;
            dexLabel.Font = fSmall;
            vitLabel.Font = fSmall;
            eneLabel.Font = fSmall;

            fcrLabel.Font = fSmall;
            frwLabel.Font = fSmall;
            fhrLabel.Font = fSmall;
            iasLabel.Font = fSmall;

            fireLabel.Font = fSmall;
            coldLabel.Font = fSmall;
            lighLabel.Font = fSmall;
            poisLabel.Font = fSmall;
            goldLabel.Font = fSmall;
            deathsLabel.Font = fSmall;

            var memoryTable = GetVersionMemoryTable(Settings.D2Version);
            dataReader.SetNextMemoryTable(memoryTable);

            runeDisplayPanel.Controls.Clear();
            if ( Settings.Runes.Count > 0 )
            {
                int extraheight = 0;
                foreach (int r in Settings.Runes)
                {
                    RuneDisplayElement element = new RuneDisplayElement((Rune)r, null, this);
                    element.setRemovable(false);
                    element.setHaveRune(false);
                    runeDisplayPanel.Controls.Add(element);
                    extraheight = relayout();
                }

                Height = OriginalHeight + extraheight + 8;
            } else
            {
                Height = OriginalHeight;
            }

            // Update debug window.
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }
        }

        public int relayout(bool checkVisible = true)
        {
            int y = 0;
            int x = 0;
            int height = -1;
            int scroll = runeDisplayPanel.VerticalScroll.Value;
            foreach (Control c in runeDisplayPanel.Controls)
            {
                if (c is RuneDisplayElement && (!checkVisible || c.Visible))
                {
                    if(height == -1)
                    {
                        height = c.Height;
                    }
                    if (x + c.Width > runeDisplayPanel.Width && runeDisplayPanel.Width >= c.Width)
                    {
                        y += c.Height;
                        x = 0;
                        height = y += c.Height;
                    }
                    c.Location = new Point(x, -scroll + y);
                    x += c.Width;
                }
            }

            runeDisplayPanel.Height = height == -1 ? 0 : height;
            return height;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataReaderThread.Abort();
            Application.Exit();
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
            if (settingsWindow == null || settingsWindow.IsDisposed)
            {
                settingsWindow = new SettingsWindow(Settings);
                settingsWindow.SettingsUpdated += (settings) => ApplySettings();
            }

            settingsWindow.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            initialize();
        }

        private void debugMenuItem_Click(object sender, EventArgs e)
        {
            if (debugWindow == null || debugWindow.IsDisposed)
            {
                debugWindow = new DebugWindow();
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }

            debugWindow.Show();
        }
    }
}
