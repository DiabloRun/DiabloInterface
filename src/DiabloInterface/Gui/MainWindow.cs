using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using DiabloInterface.Server;
using DiabloInterface.Logging;
using System.Text;
using DiabloInterface.Gui.Controls;
using DiabloInterface.D2;

namespace DiabloInterface.Gui
{
    public partial class MainWindow : Form
    {
        private const string ItemServerPipeName = "DiabloInterfaceItems";
        private const string WindowTitleFormat = "Diablo Interface v{0}"; // {0} => Application.ProductVersion

        public ApplicationSettings Settings { get; private set; }

        Thread dataReaderThread;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        D2DataReader dataReader;
        ItemServer itemServer;

        IEnumerable<Label> infoLabels;

        public MainWindow()
        {
            // We want to dispose our handles once the window is disposed.
            Disposed += OnWindowDisposed;

            InitializeLogger();
            WriteLogHeader();

            InitializeComponent();
            InitializeLabels();

            // Display current version along with the application name.
            Text = string.Format(WindowTitleFormat, Application.ProductVersion);
        }

        void InitializeLabels()
        {
            infoLabels = new[]
            {
                deathsLabel,
                goldLabel, lvlLabel,
                strLabel, dexLabel, vitLabel, eneLabel,
                frwLabel, fhrLabel, fcrLabel, iasLabel,
                fireLabel, coldLabel, lighLabel, poisLabel,
                labelPoisonResVal, labelFireResVal, labelLightResVal, labelColdResVal,
                labelFhrVal, labelFcrVal, labelFrwVal, labelIasVal,
                labelStrVal, labelDexVal, labelVitVal, labelEneVal, 
                labelNormPerc, labelNmPerc, labelHellPerc,
                normLabel, nmLabel, hellLabel,
                normLabelVal, nmLabelVal, hellLabelVal,
            };
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
                autosplit.IsReached = false;
            }
            if (panelRuneDisplay.Controls.Count > 0)
            {
                foreach (RuneDisplayElement c in panelRuneDisplay.Controls)
                {
                    c.SetHaveRune(false);
                }
            }
            if (panelRuneDisplay2.Controls.Count > 0)
            {
                foreach (RuneDisplayElement c in panelRuneDisplay2.Controls)
                {
                    c.SetHaveRune(false);
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
                    memoryTable.Address.World       = new IntPtr(0x00483D38);
                    memoryTable.Address.GameId      = new IntPtr(0x00482D0C);
                    memoryTable.Address.PlayerUnit  = new IntPtr(0x003A5E74);
                    memoryTable.Address.Area        = new IntPtr(0x003A3140);

                    memoryTable.Address.GlobalData = new IntPtr(0x344304);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);
                    
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x4829B4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x4829B8);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x4829BC);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x4829C0);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x4829D0);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x4829D4);

                    break;
            }

            return memoryTable;
        }

        ApplicationSettings LoadSettings()
        {
            var persistence = new SettingsPersistence();
            var settings = persistence.Load();

            if (settings == null)
            {
                Logger.Instance.WriteLine("Loaded default settings.");

                // Return default settings.
                return new ApplicationSettings();
            }

            return settings;
        }

        private void initialize()
        {
            try
            {
                Settings = LoadSettings();
            }
            catch (Exception e)
            {
                // Log error and show error message.
                Logger.Instance.WriteLine("Unhandled Settings Error:\n{0}", e.ToString());
                MessageBox.Show("An unhandled exception was caught trying to load the settings.\nPlease report the error and include the log found in the log folder.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Rethrow current exception.
                throw;
            }

            if (dataReader == null)
            {
                var memoryTable = GetVersionMemoryTable(Settings.D2Version);
                dataReader = new D2DataReader(this, memoryTable);
                itemServer = new ItemServer(dataReader, ItemServerPipeName);
            }

            if (dataReaderThread == null)
            {
                dataReaderThread = new Thread(dataReader.readDataThreadFunc);
                dataReaderThread.IsBackground = true;
                dataReaderThread.Start();
            }

            if (Settings.CheckUpdates)
            {
                VersionChecker.CheckForUpdate(false);
            }

            ApplySettings(Settings);
        }

        private void UpdateMinWidth(Label[] labels)
        {
            int w = 0;
            foreach (Label label in labels)
            {
                var measuredSize = TextRenderer.MeasureText(label.Text, label.Font, Size.Empty, TextFormatFlags.SingleLine);
                if (measuredSize.Width > w) w = measuredSize.Width;
            }

            foreach (Label label in labels)
            {
                label.MinimumSize = new Size(w, 0);
            }
        }

        public void UpdateLabels(Character player, Dictionary<int, int> itemClassMap)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateLabels(player, itemClassMap)));
                return;
            }

            nameLabel.Text = player.name;
            lvlLabel.Text = "LVL: " + player.Level;
            goldLabel.Text = "GOLD: " + (player.Gold + player.GoldStash);
            deathsLabel.Text = "DEATHS: " + player.Deaths;
            
            labelStrVal.Text = "" + player.Strength;
            labelDexVal.Text = "" + player.Dexterity;
            labelVitVal.Text = "" + player.Vitality;
            labelEneVal.Text = "" + player.Energy;
            UpdateMinWidth(new Label[] { labelStrVal, labelDexVal, labelVitVal, labelEneVal });
            
            labelFrwVal.Text = "" + player.FasterRunWalk;
            labelFcrVal.Text = "" + player.FasterCastRate;
            labelFhrVal.Text = "" + player.FasterHitRecovery;
            labelIasVal.Text = "" + player.IncreasedAttackSpeed;
            UpdateMinWidth(new Label[] { labelFrwVal, labelFcrVal, labelFhrVal, labelIasVal });
            
            labelFireResVal.Text = "" + player.FireResist;
            labelColdResVal.Text = "" + player.ColdResist;
            labelLightResVal.Text = "" + player.LightningResist;
            labelPoisonResVal.Text = "" + player.PoisonResist;
            UpdateMinWidth(new Label[] { labelFireResVal, labelColdResVal, labelLightResVal, labelPoisonResVal });

            int perc0 = (int)(100.0 * player.CompletedQuestCounts[0] / (float)D2QuestHelper.Quests.Count + .5);
            int perc1 = (int)(100.0 * player.CompletedQuestCounts[1] / (float)D2QuestHelper.Quests.Count + .5);
            int perc2 = (int)(100.0 * player.CompletedQuestCounts[2] / (float)D2QuestHelper.Quests.Count + .5);

            normLabelVal.Text = perc0 + "%";
            nmLabelVal.Text = perc1 + "%";
            hellLabelVal.Text = perc2 + "%";
            UpdateMinWidth(new Label[] { normLabelVal, nmLabelVal, hellLabelVal });

            labelNormPerc.Text = "NO: " + perc0 + "%";
            labelNmPerc.Text = "NM: " + perc1 + "%";
            labelHellPerc.Text = "HE: " + perc2 + "%";

            if (panelRuneDisplay.Controls.Count > 0)
            {

                Dictionary<int, int> dict = new Dictionary<int, int>(itemClassMap);
                foreach (RuneDisplayElement c in panelRuneDisplay.Controls)
                {
                    int eClass = (int)c.getRune() + 610;
                    if (dict.ContainsKey(eClass) && dict[eClass] > 0)
                    {
                        dict[eClass]--;
                        c.SetHaveRune(true);
                    }
                }
            }
            if (panelRuneDisplay2.Controls.Count > 0)
            {

                Dictionary<int, int> dict = new Dictionary<int, int>(itemClassMap);
                foreach (RuneDisplayElement c in panelRuneDisplay2.Controls)
                {
                    int eClass = (int)c.getRune() + 610;
                    if (dict.ContainsKey(eClass) && dict[eClass] > 0)
                    {
                        dict[eClass]--;
                        c.SetHaveRune(true);
                    }
                }
            }
        }

        public void triggerAutosplit(Character player)
        {
            if (Settings.DoAutosplit && Settings.AutosplitHotkey != Keys.None)
            {
                KeyManager.TriggerHotkey(Settings.AutosplitHotkey);
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
            File.WriteAllText(Settings.FileFolder + "/fcr.txt", player.FasterCastRate.ToString());
            File.WriteAllText(Settings.FileFolder + "/frw.txt", player.FasterRunWalk.ToString());
            File.WriteAllText(Settings.FileFolder + "/fhr.txt", player.FasterHitRecovery.ToString());
            File.WriteAllText(Settings.FileFolder + "/ias.txt", player.IncreasedAttackSpeed.ToString());

        }

        private void ChangeVisibility(Control c, bool visible)
        {
            if (visible)
            {
                c.Show();
            } else
            {
                c.Hide();
            }
        }
        public void ApplySettings(ApplicationSettings settings)
        {
            Settings = settings;

            ApplyVersionSettings();
            ApplyLabelSettings();
            ApplyRuneSettings();

            UpdateLayout();

            // Update debug window.
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }

            LogAutoSplits();
        }

        void ApplyVersionSettings()
        {
            var memoryTable = GetVersionMemoryTable(Settings.D2Version);
            dataReader.SetNextMemoryTable(memoryTable);
        }

        void ApplyLabelSettings()
        {
            nameLabel.Font = new Font(Settings.FontName, Settings.FontSizeTitle);
            Font infoFont = new Font(Settings.FontName, Settings.FontSize);
            foreach (Label label in infoLabels)
                label.Font = infoFont;

            // Hide/show labels wanted labels.
            ChangeVisibility(nameLabel, Settings.DisplayName);

            ChangeVisibility(goldLabel, Settings.DisplayGold);
            ChangeVisibility(panelSimpleStats, Settings.DisplayGold);

            ChangeVisibility(deathsLabel, Settings.DisplayDeathCounter);
            ChangeVisibility(lvlLabel, Settings.DisplayLevel);
            ChangeVisibility(panelDeathsLvl, Settings.DisplayDeathCounter || Settings.DisplayLevel);
            
            ChangeVisibility(panelResistances, Settings.DisplayResistances);
            ChangeVisibility(panelBaseStats, Settings.DisplayBaseStats);
            ChangeVisibility(panelAdvancedStats, Settings.DisplayAdvancedStats);

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            ChangeVisibility(panelDiffPercentages, count < 3 && Settings.DisplayDifficultyPercentages);
            ChangeVisibility(panelDiffPercentages2, count >= 3 && Settings.DisplayDifficultyPercentages);

            ChangeVisibility(panelStats, 
                Settings.DisplayResistances 
                || Settings.DisplayBaseStats 
                || Settings.DisplayAdvancedStats
                || Settings.DisplayDifficultyPercentages
            );
            
        }

        void ApplyRuneSettings()
        {
            panelRuneDisplay.Controls.Clear();
            if (Settings.Runes.Count > 0)
            {
                foreach (int r in Settings.Runes)
                {
                    RuneDisplayElement element = new RuneDisplayElement((Rune)r, null, this);
                    element.SetRuneSprite(Settings.DisplayRunesHighContrast);
                    element.SetRemovable(false);
                    element.SetHaveRune(false);
                    panelRuneDisplay.Controls.Add(element);
                }
            }

            ChangeVisibility(panelRuneDisplay, Settings.DisplayRunes && Settings.DisplayRunesHorizontal && Settings.Runes.Count > 0);
            
            panelRuneDisplay2.Controls.Clear();
            if (Settings.Runes.Count > 0)
            {
                foreach (int r in Settings.Runes)
                {
                    RuneDisplayElement element = new RuneDisplayElement((Rune)r, null, this);
                    element.SetRuneSprite(Settings.DisplayRunesHighContrast);
                    element.SetRemovable(false);
                    element.SetHaveRune(false);
                    panelRuneDisplay2.Controls.Add(element);
                }
            }

            ChangeVisibility(panelRuneDisplay2, Settings.DisplayRunes && !Settings.DisplayRunesHorizontal && Settings.Runes.Count > 0);
        }

        void LogAutoSplits()
        {
            var logMessage = new StringBuilder();
            logMessage.Append("Configured autosplits:");

            for (int i = 0; i < Settings.Autosplits.Count; ++i)
            {
                var split = Settings.Autosplits[i];

                logMessage.AppendLine();
                logMessage.Append("  ");
                logMessage.AppendFormat("#{0} [{2}, {3}, {4}] \"{1}\"", i,
                    split.Name, split.Type, split.Value, split.Difficulty);
            }

            Logger.Instance.WriteLine(logMessage.ToString());
        }

        public void UpdateLayout()
        {

            int padding = 0;
            // Calculate maximum sizes that the labels can possible get.
            Size nameSize = TextRenderer.MeasureText(new string('W', 15), nameLabel.Font, Size.Empty, TextFormatFlags.SingleLine);

            // base stats have 3 char label (STR, VIT, ect.) and realistically a max value < 500 (lvl 99*5 + alkor quest... items can increase this tho)
            // we will assume the "longest" string is DEX: 499 (most likely dex or ene will be longest str.)
            padding = (panelAdvancedStats.Visible || panelResistances.Visible) ? 8 : 0;
            Size statSize = TextRenderer.MeasureText("DEX: 499", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size basePanelSize = new Size(statSize.Width + padding, statSize.Height * 4);

            // advanced stats have 3 char label (FCR, FRW, etc.) and realistically a max value < 100
            // we will assume the "longest" string is FRW: 99
            padding = panelResistances.Visible ? 8 : 0;
            Size advancedStatSize = TextRenderer.MeasureText("FRW: 99", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size advancedStatPanelSize = new Size(advancedStatSize.Width + padding, advancedStatSize.Height * 4);

            // Panel size for resistances can be negative, so max number of chars are 10 (LABL: -VAL)
            // resistances never go below -100 (longest possible string for the label) and never go above 95
            // we will assume the "longest" string is COLD: -100
            Size resStatSize = TextRenderer.MeasureText("COLD: -100", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size resPanelSize = new Size(resStatSize.Width, resStatSize.Height * 4);
            
            // we will assume the "longest" string is NORM: 100%
            Size diffPercStatSize = TextRenderer.MeasureText("NORM: 100%", strLabel.Font, Size.Empty, TextFormatFlags.SingleLine);
            Size diffPercPanelSize = new Size(diffPercStatSize.Width, diffPercStatSize.Height * 4);

            int count = 0;
            if (panelResistances.Visible) count++;
            if (panelBaseStats.Visible) count++;
            if (panelAdvancedStats.Visible) count++;

            // Recalculate panel size if the title is wider than all panels combined.
            int statsWidth = (panelResistances.Visible ? resPanelSize.Width : 0)
                + (panelBaseStats.Visible ? basePanelSize.Width : 0)
                + (panelAdvancedStats.Visible ? advancedStatPanelSize.Width : 0)
                + ((count < 3 && panelDiffPercentages.Visible) ? panelDiffPercentages.Width : 0)
            ;

            float ratio = (float)nameSize.Width / (float)statsWidth;

            if (ratio > 1.0f)
            {
                resPanelSize.Width = (int)(resPanelSize.Width * ratio + .5f);
                basePanelSize.Width = (int)(basePanelSize.Width * ratio + .5f);
                advancedStatPanelSize.Width = (int)(advancedStatPanelSize.Width * ratio + .5f);
                if (count < 3 && panelDiffPercentages.Visible)
                {
                    panelDiffPercentages.Width = (int)(diffPercPanelSize.Width * ratio + .5f);
                }
            }

            nameLabel.Size = nameSize;
            panelBaseStats.Size = basePanelSize;
            panelAdvancedStats.Size = advancedStatPanelSize;
            panelResistances.Size = resPanelSize;
            panelDiffPercentages.Size = diffPercPanelSize;

            UpdateRuneLayout();
            Invalidate(true);
        }

        void UpdateRuneLayout()
        {
            int y;
            int x;
            int height;
            int scroll;
            
            if (Settings.DisplayRunesHorizontal)
            {
                y = 0;
                x = 0;
                height = -1;
                scroll = panelRuneDisplay.VerticalScroll.Value;
                foreach (Control c in panelRuneDisplay.Controls)
                {
                    if (c is RuneDisplayElement && c.Visible)
                    {
                        if (height == -1)
                        {
                            height = c.Height;
                        }
                        if (x + c.Width > panelRuneDisplay.Width && panelRuneDisplay.Width >= c.Width)
                        {
                            y += c.Height + 4;
                            x = 0;
                            height = y + c.Height;
                        }
                        c.Location = new Point(x, -scroll + y);
                        x += c.Width + 4; // 4 padding for runes
                    }
                }

                panelRuneDisplay.Height = height == -1 ? 0 : height;
            }


            if (!Settings.DisplayRunesHorizontal)
            {
                y = 0;
                x = 0;
                height = -1;
                scroll = panelRuneDisplay2.VerticalScroll.Value;
                foreach (Control c in panelRuneDisplay2.Controls)
                {
                    if (c is RuneDisplayElement && c.Visible)
                    {
                        if (height == -1)
                        {
                            height = c.Height;
                        }
                        if (x + c.Width > panelRuneDisplay2.Width && panelRuneDisplay2.Width >= c.Width)
                        {
                            y += c.Height + 4;
                            x = 0;
                            height = y + c.Height;
                        }
                        c.Location = new Point(x, -scroll + y);
                        x += c.Width;
                    }
                }

                panelRuneDisplay2.Height = height == -1 ? 0 : height;
                panelRuneDisplay2.Width = 28;
            }

        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void resetMenuItem_Click(object sender, EventArgs e)
        {
            initialize();
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsWindow == null || settingsWindow.IsDisposed)
            {
                settingsWindow = new SettingsWindow(Settings);
                settingsWindow.SettingsUpdated += (settings) => ApplySettings(settings);
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
