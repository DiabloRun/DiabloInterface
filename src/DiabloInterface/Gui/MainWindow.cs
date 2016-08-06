using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Server;
using Zutatensuppe.DiabloInterface.Logging;
using System.Text;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using Zutatensuppe.DiabloInterface.Autosplit;
using Zutatensuppe.DiabloInterface.Settings;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public partial class MainWindow : WsExCompositedForm
    {

        private const string ItemServerPipeName = "DiabloInterfaceItems";
        private const string WindowTitleFormat = "Diablo Interface v{0}"; // {0} => Application.ProductVersion

        public ApplicationSettings Settings { get; private set; }

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
                autosplit.IsReached = false;
            }
            horizontalLayout1.ResetText();
            verticalLayout2.ResetText();
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

        ApplicationSettings LoadSettings(string fileName)
        {
            var persistence = new SettingsPersistence();

            ApplicationSettings settings = null;

            if (fileName == String.Empty)
                settings = persistence.Load();
            else
                settings = persistence.Load(fileName);

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
                Logger.Instance.WriteLine("Unhandled Settings Error:{0}{1}", Environment.NewLine, e.ToString());
                MessageBox.Show(
                    "An unhandled exception was caught trying to load the settings."+ Environment.NewLine + 
                    "Please report the error and include the log found in the log folder.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Rethrow current exception.
                throw;
            }

            if (dataReader == null)
            {
                dataReader = new D2DataReader(Settings.D2Version);
                dataReader.NewCharacter += new NewCharacterCreatedEventHandler(this.d2Reader_NewCharacter);
                dataReader.DataRead += new DataReadEventHandler(this.d2Reader_DataRead);
                dataReader.DataReader += new DataReaderEventHandler(this.d2Reader_DataReader);

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

        private void d2Reader_DataReader(object sender, DataReaderEventArgs e)
        {
            Logger.Instance.WriteLine("Generic Data reader event: "+ e.type.ToString());
        }

        private void d2Reader_DataRead(object sender, DataReadEventArgs e)
        {
            this.UpdateLabels(e.Character, e.ItemClassMap);
            this.writeFiles(e.Character);

            UpdateDebugWindow((D2DataReader)sender);

            // Update autosplits only if enabled and the character was a freshly started character.
            if (e.IsAutosplitCharacter && this.Settings.DoAutosplit)
            {
                UpdateAutoSplits((D2DataReader)sender, e.Character);
            }
        }
        private void d2Reader_NewCharacter(object sender, NewCharacterEventArgs e)
        {
            this.Reset();
            Logger.Instance.WriteLine("A new character was created - autosplits OK for {0}", e.Character);
        }


        void UpdateDebugWindow(D2DataReader dataReader)
        {
            var debugWindow = getDebugWindow();
            if (debugWindow == null) return;


            // Fill in quest data.
            for (int difficulty = 0; difficulty < 3; ++difficulty)
            {
                ushort[] questBuffer = dataReader.GetQuestBuffer(difficulty);
                if (questBuffer != null)
                {
                    debugWindow.UpdateQuestData(questBuffer, difficulty);
                }
            }

            debugWindow.UpdateItemStats(dataReader);
        }

        private void UpdateAutoSplits(D2DataReader dataReader, Character character)
        {
            var difficulty = dataReader.GetDifficulty();
            foreach (AutoSplit autosplit in Settings.Autosplits)
            {
                if (autosplit.IsReached)
                {
                    continue;
                }
                if (autosplit.Type != AutoSplit.SplitType.Special)
                {
                    continue;
                }
                if (autosplit.Value == (int)AutoSplit.Special.GameStart)
                {
                    CompleteAutoSplit(autosplit, character);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100Percent
                    && character.CompletedQuestCounts[difficulty] == D2QuestHelper.Quests.Count
                    && autosplit.MatchesDifficulty(difficulty))
                {
                    CompleteAutoSplit(autosplit, character);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100PercentAllDifficulties
                    && character.CompletedQuestCounts[0] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[1] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[2] == D2QuestHelper.Quests.Count)
                {
                    CompleteAutoSplit(autosplit, character);
                }
            }

            bool haveUnreachedCharLevelSplits = false;
            bool haveUnreachedAreaSplits = false;
            bool haveUnreachedItemSplits = false;
            bool haveUnreachedQuestSplits = false;

            foreach (AutoSplit autosplit in Settings.Autosplits)
            {
                if (autosplit.IsReached || !autosplit.MatchesDifficulty(difficulty))
                {
                    continue;
                }
                switch (autosplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        haveUnreachedCharLevelSplits = true;
                        break;
                    case AutoSplit.SplitType.Area:
                        haveUnreachedAreaSplits = true;
                        break;
                    case AutoSplit.SplitType.Item:
                        haveUnreachedItemSplits = true;
                        break;
                    case AutoSplit.SplitType.Quest:
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
                itemsIds = dataReader.GetItemIds();
            }

            if (haveUnreachedAreaSplits)
            {
                area = dataReader.GetAreaId();
            }

            ushort[] questBuffer = null;

            if (haveUnreachedQuestSplits)
            {
                questBuffer = dataReader.GetQuestBuffer(difficulty);
            }

            foreach (AutoSplit autosplit in Settings.Autosplits)
            {
                if (autosplit.IsReached || !autosplit.MatchesDifficulty(difficulty))
                {
                    continue;
                }

                switch (autosplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        if (autosplit.Value <= character.Level)
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Area:
                        if (autosplit.Value == area)
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Item:
                        if (itemsIds.Contains(autosplit.Value))
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                    case AutoSplit.SplitType.Quest:
                        if (D2QuestHelper.IsQuestComplete((D2QuestHelper.Quest)autosplit.Value, questBuffer))
                        {
                            CompleteAutoSplit(autosplit, character);
                        }
                        break;
                }
            }
        }

        void CompleteAutoSplit(AutoSplit autosplit, Character character)
        {
            // Autosplit already reached.
            if (autosplit.IsReached)
            {
                return;
            }

            autosplit.IsReached = true;
            this.triggerAutosplit(character);

            int autoSplitIndex = this.Settings.Autosplits.IndexOf(autosplit);
            Logger.Instance.WriteLine("AutoSplit: #{0} ({1}, {2}) Reached.",
                autoSplitIndex, autosplit.Name, autosplit.Difficulty);
        }

        public void UpdateLabels(Character player, Dictionary<int, int> itemClassMap)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateLabels(player, itemClassMap)));
                return;
            }
            if ( Settings.DisplayLayoutHorizontal )
            {
                horizontalLayout1.UpdateLabels(player, itemClassMap);
            } else
            {
                verticalLayout2.UpdateLabels(player, itemClassMap);
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

        public void ApplySettings(ApplicationSettings settings)
        {
            Settings = settings;

            dataReader.SetD2Version(Settings.D2Version);

            if (Settings.DisplayLayoutHorizontal)
            {
                horizontalLayout1.ApplyLabelSettings(Settings);
                horizontalLayout1.ApplyRuneSettings(Settings);
            } else
            {
                verticalLayout2.ApplyLabelSettings(Settings);
                verticalLayout2.ApplyRuneSettings(Settings);
            }


            UpdateLayout();

            // Update debug window.
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }

            LoadConfigFileList();
            LogAutoSplits();
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
            int layout = Settings.DisplayLayoutHorizontal ? 0 : 1;
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateLayout()));
                return;
            }
            
            if (Settings.DisplayLayoutHorizontal)
            {
                verticalLayout2.Hide();
                horizontalLayout1.Show();
                horizontalLayout1.UpdateLayout(Settings);
            } else
            {
                horizontalLayout1.Hide();
                verticalLayout2.Show();
                verticalLayout2.UpdateLayout(Settings);
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

        private void LoadConfigFileList()
        {
            loadConfigMenuItem.DropDownItems.Clear();

            List<ToolStripItem> items = new List<ToolStripItem>();
            string settingsFolder = @".\Settings";

            if (!Directory.Exists(settingsFolder))
            {
                Directory.CreateDirectory(settingsFolder);
            }
            DirectoryInfo di = new DirectoryInfo(settingsFolder);
            foreach (FileInfo fi in di.GetFiles("*.conf", SearchOption.AllDirectories))
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = fi.Name.Substring(0,fi.Name.LastIndexOf('.'));
                tsmi.Tag = fi.FullName;
                tsmi.Click += LoadConfigFile;
                items.Add(tsmi);
            }
           
            loadConfigMenuItem.DropDownItems.AddRange(items.ToArray());
        }

        private void LoadConfigFile(object sender, EventArgs e)
        {
            var fileName = ((ToolStripMenuItem)sender).Tag.ToString();
            var settings = LoadSettings(fileName);
            Properties.Settings.Default.SettingsFile = fileName;
            ApplySettings(settings);
        }
    }
}
