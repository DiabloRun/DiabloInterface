using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Autosplit;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Data;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using Zutatensuppe.DiabloInterface.IO;
using Zutatensuppe.DiabloInterface.Server;
using Zutatensuppe.DiabloInterface.Server.Handlers;
using Zutatensuppe.DiabloInterface.Settings;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public partial class MainWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

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
            Settings = LoadPreviousSettings();

            InitializeDataReader();
            InitializePipeServer();
            InitializeDataReaderThread();
            CheckForUpdates();

            ApplySettings(Settings);
        }

        static ApplicationSettings LoadPreviousSettings()
        {
            try
            {
                return LoadPersistedSettings() ?? LoadDefaultSettings();
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load previous settings.", e);
                MessageBox.Show($@"Failed to load settings.{Environment.NewLine}See the error log for more details.",
                    @"Settings Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw;
            }
        }

        static ApplicationSettings LoadPersistedSettings(string fileName = null)
        {
            var persistence = new SettingsPersistence();
            return string.IsNullOrWhiteSpace(fileName)
                ? persistence.Load()
                : persistence.Load(fileName);
        }

        static ApplicationSettings LoadDefaultSettings()
        {
            Logger.Info("No previous settings detected. Loading default.");
            return ApplicationSettings.Default;
        }

        void InitializeDataReader()
        {
            if (dataReader != null) return;

            dataReader = new D2DataReader(Settings.D2Version);
            dataReader.NewCharacter += dataReader_NewCharacter;
            dataReader.DataRead += dataReader_DataRead;
            dataReader.DataReader += dataReader_DataReader;
        }

        void InitializeDataReaderThread()
        {
            if (dataReaderThread != null) return;

            dataReaderThread = new Thread(dataReader.readDataThreadFunc) { IsBackground = true };
            dataReaderThread.Start();
        }

        void InitializePipeServer()
        {
            if (pipeServer != null)
                return;

            const string pipeName = "DiabloInterfacePipe";
            pipeServer = new DiabloInterfaceServer(pipeName);
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

        void CheckForUpdates()
        {
            if (Settings.CheckUpdates)
            {
                VersionChecker.CheckForUpdate(false);
            }
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

            UpdateInterfaceLayout();
            UpdateAutoSplitsForDebugView();
            LoadSettingsFileList();
            LogAutoSplits();
        }

        void UpdateInterfaceLayout()
        {
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
        }

        void UpdateAutoSplitsForDebugView()
        {
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(Settings.Autosplits);
            }
        }

        void LoadSettingsFileList()
        {
            loadConfigMenuItem.DropDownItems.Clear();

            DirectoryInfo settingsDirectory = GetSettingsDirectory();
            FileInfo[] files = settingsDirectory.GetFiles("*.conf", SearchOption.AllDirectories);

            ToolStripItem[] items = (from fileInfo in files
                select CreateSettingsToolStripMenuItem(fileInfo) as ToolStripItem).ToArray();

            loadConfigMenuItem.DropDownItems.AddRange(items);
        }

        ToolStripMenuItem CreateSettingsToolStripMenuItem(FileInfo fileInfo)
        {
            var item = new ToolStripMenuItem
            {
                Text = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.')),
                Tag = fileInfo.FullName
            };
            item.Click += LoadConfigFile;

            return item;
        }

        static DirectoryInfo GetSettingsDirectory()
        {
            var directory = new DirectoryInfo(@".\Settings");
            if (directory.Exists) return directory;

            Logger.Info($"Creating settings directory at: {directory.FullName}");
            directory.Create();

            return directory;
        }

        void LoadConfigFile(object sender, EventArgs e)
        {
            var fileName = ((ToolStripMenuItem)sender).Tag.ToString();

            ApplicationSettings settings;
            if (TryLoadSettingsFromFile(fileName, out settings))
            {
                Properties.Settings.Default.SettingsFile = fileName;
                ApplySettings(settings);
            }
        }

        static bool TryLoadSettingsFromFile(string fileName, out ApplicationSettings settings)
        {
            try
            {
                settings = LoadPersistedSettings(fileName);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to load settings from file: {fileName}.", e);
                MessageBox.Show($@"Failed to load settings.{Environment.NewLine}See the error log for more details.",
                    @"Settings Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            settings = null;
            return false;
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
            WriteCharacterStatFiles(e.Character);

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

        void WriteCharacterStatFiles(Character player)
        {
            if (!Settings.CreateFiles) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, Settings.FileFolder);
            var stats = new CharacterStats(player);

            statWriter.WriteFiles(stats);
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
