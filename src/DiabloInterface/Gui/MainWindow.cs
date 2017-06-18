using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Autosplit;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using Zutatensuppe.DiabloInterface.Server;
using Zutatensuppe.DiabloInterface.Server.Handlers;
using Zutatensuppe.DiabloInterface.Settings;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public partial class MainWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        const string ItemServerPipeName = "DiabloInterfacePipe";

        ApplicationSettings Settings { get; set; }

        Thread dataReaderThread;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;

        D2DataReader dataReader;
        DiabloInterfaceServer pipeServer;

        public MainWindow()
        {
            RegisterWindowEventHandlers();
            InitializeComponent();
        }

        void RegisterWindowEventHandlers()
        {
            // These events are not exposed through the Designer.
            Disposed += MainWindow_Disposed;
        }

        void MainWindow_Load(object sender, EventArgs e)
        {
            SetTitleWithApplicationVersion();
            Initialize();
        }

        void SetTitleWithApplicationVersion()
        {
            Text = $@"Diablo Interface v{Application.ProductVersion}";
            Update();
        }

        void Initialize()
        {
            try
            {
                Settings = LoadSettings();
            }
            catch (Exception e)
            {
                // Log error and show error message.
                Logger.Error("Unhandled Settings Error", e);
                MessageBox.Show(
                    @"An unhandled exception was caught trying to load the settings." + Environment.NewLine +
                    @"Please report the error and include the log found in the log folder.",
                    @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw;
            }

            if (dataReader == null)
            {
                dataReader = new D2DataReader(Settings.D2Version);
                dataReader.NewCharacter += dataReader_NewCharacter;
                dataReader.DataRead += dataReader_DataRead;
                dataReader.DataReader += dataReader_DataReader;
            }

            InitializePipeServer();

            if (dataReaderThread == null)
            {
                dataReaderThread = new Thread(dataReader.readDataThreadFunc) {IsBackground = true};
                dataReaderThread.Start();
            }

            if (Settings.CheckUpdates)
            {
                VersionChecker.CheckForUpdate(false);
            }

            ApplySettings(Settings);
        }

        ApplicationSettings LoadSettings()
        {
            var persistence = new SettingsPersistence();
            var settings = persistence.Load();

            if (settings == null)
            {
                Logger.Info("Loaded default settings.");

                // Return default settings.
                return new ApplicationSettings();
            }

            return settings;
        }

        ApplicationSettings LoadSettings(string fileName)
        {
            var persistence = new SettingsPersistence();

            ApplicationSettings settings;

            if (fileName == String.Empty)
                settings = persistence.Load();
            else
                settings = persistence.Load(fileName);

            if (settings == null)
            {
                Logger.Info("Loaded default settings.");

                // Return default settings.
                return new ApplicationSettings();
            }

            return settings;
        }

        void InitializePipeServer()
        {
            if (pipeServer != null)
                return;

            pipeServer = new DiabloInterfaceServer(ItemServerPipeName);
            pipeServer.AddRequestHandler(@"version", () =>
                new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"items", () =>
                new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () =>
                new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () =>
                new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () =>
                new QuestRequestHandler(dataReader));
        }

        void ApplySettings(ApplicationSettings settings)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => ApplySettings(settings)));
                return;
            }

            Settings = settings;

            dataReader.SetD2Version(Settings.D2Version);

            if (Settings.DisplayLayoutHorizontal)
            {
                verticalLayout2.MakeInactive();
                horizontalLayout1.MakeActive(Settings);
            }
            else
            {
                horizontalLayout1.MakeInactive();
                verticalLayout2.MakeActive(Settings);
            }

            // Update debug window.
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }

            LoadConfigFileList();
            LogAutoSplits();
        }

        void LoadConfigFileList()
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
                tsmi.Text = fi.Name.Substring(0, fi.Name.LastIndexOf('.'));
                tsmi.Tag = fi.FullName;
                tsmi.Click += LoadConfigFile;
                items.Add(tsmi);
            }

            loadConfigMenuItem.DropDownItems.AddRange(items.ToArray());
        }

        void LoadConfigFile(object sender, EventArgs e)
        {
            var fileName = ((ToolStripMenuItem)sender).Tag.ToString();
            var settings = LoadSettings(fileName);
            Properties.Settings.Default.SettingsFile = fileName;
            ApplySettings(settings);
        }

        void LogAutoSplits()
        {
            var logMessage = new StringBuilder();
            logMessage.Append("Configured autosplits:");

            for (var i = 0; i < Settings.Autosplits.Count; ++i)
            {
                var split = Settings.Autosplits[i];

                logMessage.AppendLine();
                logMessage.Append("  ");
                logMessage.Append($"#{i} [{split.Type}, {split.Value}, {split.Difficulty}] \"{split.Name}\"");
            }

            Logger.Info(logMessage.ToString());
        }

        void dataReader_DataReader(object sender, DataReaderEventArgs e)
        {
            Logger.Info("Generic Data reader event: "+ e.type.ToString());
        }

        void dataReader_DataRead(object sender, DataReadEventArgs e)
        {
            UpdateLabels(e.Character, e.ItemClassMap);
            WriteFiles(e.Character);

            UpdateDebugWindow(e.QuestBuffers, e.ItemStrings);

            // Update autosplits only if enabled and the character was a freshly started character.
            if (e.IsAutosplitCharacter && Settings.DoAutosplit)
            {
                UpdateAutoSplits(e.QuestBuffers, e.CurrentArea, e.CurrentDifficulty, e.ItemIds, e.Character);
            }
        }

        void dataReader_NewCharacter(object sender, NewCharacterEventArgs e)
        {
            Reset();
            Logger.Info($"A new character was created - autosplits OK for {e.Character.Name}");
        }

        void Reset()
        {
            foreach (AutoSplit autosplit in Settings.Autosplits)
            {
                autosplit.IsReached = false;
            }
            horizontalLayout1.Reset();
            verticalLayout2.Reset();
        }

        void UpdateDebugWindow(Dictionary<int, ushort[]> questBuffers, Dictionary<BodyLocation, string> itemStrings)
        {
            if (debugWindow == null)
            {
                return;
            }

            // Fill in quest data.
            for (int difficulty = 0; difficulty < 3; ++difficulty)
            {
                if (questBuffers.ContainsKey(difficulty))
                {
                    debugWindow.UpdateQuestData(questBuffers[difficulty], difficulty);
                }
            }

            debugWindow.UpdateItemStats(itemStrings);
        }

        void UpdateAutoSplits(Dictionary<int, ushort[]> questBuffers, int areaId, byte difficulty, List<int> itemIds, Character character)
        {
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
                    CompleteAutoSplit(autosplit);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100Percent
                    && character.CompletedQuestCounts[difficulty] == D2QuestHelper.Quests.Count
                    && autosplit.MatchesDifficulty(difficulty))
                {
                    CompleteAutoSplit(autosplit);
                }
                if (autosplit.Value == (int)AutoSplit.Special.Clear100PercentAllDifficulties
                    && character.CompletedQuestCounts[0] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[1] == D2QuestHelper.Quests.Count
                    && character.CompletedQuestCounts[2] == D2QuestHelper.Quests.Count)
                {
                    CompleteAutoSplit(autosplit);
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

            ushort[] questBuffer = null;

            if (haveUnreachedQuestSplits && questBuffers.ContainsKey(difficulty))
            {
                questBuffer = questBuffers[difficulty];
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
                            CompleteAutoSplit(autosplit);
                        }
                        break;
                    case AutoSplit.SplitType.Area:
                        if (autosplit.Value == areaId)
                        {
                            CompleteAutoSplit(autosplit);
                        }
                        break;
                    case AutoSplit.SplitType.Item:
                        if (itemIds.Contains(autosplit.Value))
                        {
                            CompleteAutoSplit(autosplit);
                        }
                        break;
                    case AutoSplit.SplitType.Quest:
                        if (D2QuestHelper.IsQuestComplete((D2QuestHelper.Quest)autosplit.Value, questBuffer))
                        {
                            CompleteAutoSplit(autosplit);
                        }
                        break;
                }
            }
        }

        void CompleteAutoSplit(AutoSplit autosplit)
        {
            // Autosplit already reached.
            if (autosplit.IsReached)
            {
                return;
            }

            autosplit.IsReached = true;
            TriggerAutosplit();

            var autoSplitIndex = Settings.Autosplits.IndexOf(autosplit);
            Logger.Info($"AutoSplit: #{autoSplitIndex} ({autosplit.Name}, {autosplit.Difficulty}) Reached.");
        }

        void UpdateLabels(Character player, Dictionary<int, int> itemClassMap)
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

        void TriggerAutosplit()
        {
            if (Settings.DoAutosplit && Settings.AutosplitHotkey != Keys.None)
            {
                KeyManager.TriggerHotkey(Settings.AutosplitHotkey);
            }
        }

        void WriteFiles(Character player)
        {
            // TODO: Only write files if content changed.
            if (!Settings.CreateFiles)
            {
                return;
            }

            if (!Directory.Exists(Settings.FileFolder))
            {
                Directory.CreateDirectory(Settings.FileFolder);
            }

            File.WriteAllText(Settings.FileFolder + "/name.txt", player.Name);
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

        void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        void resetMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsWindow == null || settingsWindow.IsDisposed)
            {
                settingsWindow = new SettingsWindow(Settings);
                settingsWindow.SettingsUpdated += ApplySettings;
            }

            settingsWindow.ShowDialog();
        }

        void debugMenuItem_Click(object sender, EventArgs e)
        {
            if (debugWindow == null || debugWindow.IsDisposed)
            {
                debugWindow = new DebugWindow();
                debugWindow.UpdateAutosplits(Settings.Autosplits);

                dataReader.RequiredData |= D2DataReader.READ_EQUIPPED_ITEM_STRINGS;

                debugWindow.FormClosed += DebugWindow_FormClosed;
            }

            debugWindow.Show();
        }

        void DebugWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataReader.RequiredData &= ~D2DataReader.READ_EQUIPPED_ITEM_STRINGS;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            pipeServer.Stop();
            base.OnFormClosing(e);
        }

        void MainWindow_Disposed(object sender, EventArgs e)
        {
            DisposeDataReader();
        }

        void DisposeDataReader()
        {
            dataReader?.Dispose();
            dataReader = null;
        }
    }
}
