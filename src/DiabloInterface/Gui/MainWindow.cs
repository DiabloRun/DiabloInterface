namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Struct.Item;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Data;
    using Zutatensuppe.DiabloInterface.Gui.Forms;
    using Zutatensuppe.DiabloInterface.IO;
    using Zutatensuppe.DiabloInterface.Server;
    using Zutatensuppe.DiabloInterface.Server.Handlers;

    public partial class MainWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISettingsService settingsService;
        readonly IGameService gameService;

        SettingsWindow settingsWindow;
        DebugWindow debugWindow;
        DiabloInterfaceServer pipeServer;

        public MainWindow(ISettingsService settingsService, IGameService gameService)
        {
            Logger.Info("Creating main window.");

            this.settingsService = settingsService;
            this.gameService = gameService;

            RegisterServiceEventHandlers();
            InitializeComponent();
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;

            gameService.CharacterCreated += GameServiceOnCharacterCreated;
            gameService.DataRead += GameServiceOnDataRead;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            ApplySettings(e.Settings);
        }

        void MainWindowLoad(object sender, EventArgs e)
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
            InitializePipeServer();
            CheckForUpdates();

            ApplySettings(settingsService.CurrentSettings);
        }

        void InitializePipeServer()
        {
            if (pipeServer != null) return;

            const string PipeName = "DiabloInterfacePipe";

            var dataReader = gameService.DataReader;
            pipeServer = new DiabloInterfaceServer(PipeName);
            pipeServer.AddRequestHandler(@"version", () => new VersionRequestHandler(Assembly.GetEntryAssembly()));
            pipeServer.AddRequestHandler(@"items", () => new AllItemsRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"items/(\w+)", () => new ItemRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"characters/(current|active)", () => new CharacterRequestHandler(dataReader));
            pipeServer.AddRequestHandler(@"quests/(\d+)", () => new QuestRequestHandler(dataReader));
        }

        void CheckForUpdates()
        {
            if (settingsService.CurrentSettings.CheckUpdates)
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

            UpdateLayoutSettings(settings);
            UpdateAutoSplitsSettingsForDebugView(settings);
            LoadSettingsFileList();
            LogAutoSplits();
        }

        void UpdateLayoutSettings(ApplicationSettings settings)
        {
            if (settings.DisplayLayoutHorizontal)
            {
                verticalLayout2.MakeInactive();
                horizontalLayout1.MakeActive(settings);
            }
            else
            {
                horizontalLayout1.MakeInactive();
                verticalLayout2.MakeActive(settings);
            }
        }

        void UpdateAutoSplitsSettingsForDebugView(ApplicationSettings settings)
        {
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(settings.Autosplits);
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

            // TODO: LoadSettings should throw a custom Exception with information about why this happened.
            if (!settingsService.LoadSettings(fileName))
            {
                Logger.Error($"Failed to load settings from file: {fileName}.");
                MessageBox.Show(
                    $@"Failed to load settings.{Environment.NewLine}See the error log for more details.",
                    @"Settings Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        void LogAutoSplits()
        {
            var settings = settingsService.CurrentSettings;
            var logMessage = new StringBuilder();
            logMessage.Append("Configured autosplits:");

            for (var i = 0; i < settings.Autosplits.Count; ++i)
            {
                var split = settings.Autosplits[i];

                logMessage.AppendLine();
                logMessage.Append("  ");
                logMessage.Append($"#{i} [{split.Type}, {split.Value}, {split.Difficulty}] \"{split.Name}\"");
            }

            Logger.Info(logMessage.ToString());
        }

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            UpdateLabels(e.Character, e.ItemClassMap);
            WriteCharacterStatFiles(e.Character);

            UpdateDebugWindow(e.QuestBuffers, e.ItemStrings);

            // Update autosplits only if enabled and the character was a freshly started character.
            if (e.IsAutosplitCharacter && settingsService.CurrentSettings.DoAutosplit)
            {
                UpdateAutoSplits(e.QuestBuffers, e.CurrentArea, e.CurrentDifficulty, e.ItemIds, e.Character);
            }
        }

        void GameServiceOnCharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            Reset();
            Logger.Info($"A new character was created - autosplits OK for {e.Character.Name}");
        }

        void Reset()
        {
            foreach (AutoSplit autosplit in settingsService.CurrentSettings.Autosplits)
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
            foreach (AutoSplit autosplit in settingsService.CurrentSettings.Autosplits)
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

            foreach (AutoSplit autosplit in settingsService.CurrentSettings.Autosplits)
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

            foreach (AutoSplit autosplit in settingsService.CurrentSettings.Autosplits)
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

            var autoSplitIndex = settingsService.CurrentSettings.Autosplits.IndexOf(autosplit);
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

            if (settingsService.CurrentSettings.DisplayLayoutHorizontal)
            {
                horizontalLayout1.UpdateLabels(player, itemClassMap);
            }
            else
            {
                verticalLayout2.UpdateLabels(player, itemClassMap);
            }
        }

        void TriggerAutosplit()
        {
            var settings = settingsService.CurrentSettings;
            if (settings.DoAutosplit && settings.AutosplitHotkey != Keys.None)
            {
                KeyManager.TriggerHotkey(settings.AutosplitHotkey);
            }
        }

        void WriteCharacterStatFiles(Character player)
        {
            if (!settingsService.CurrentSettings.CreateFiles) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, settingsService.CurrentSettings.FileFolder);
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
                settingsWindow = new SettingsWindow(settingsService);
            }

            settingsWindow.ShowDialog();
        }

        void debugMenuItem_Click(object sender, EventArgs e)
        {
            if (debugWindow == null || debugWindow.IsDisposed)
            {
                debugWindow = new DebugWindow();
                debugWindow.UpdateAutosplits(settingsService.CurrentSettings.Autosplits);

                var dataReader = gameService.DataReader;
                var flags = dataReader.ReadFlags.SetFlag(DataReaderEnableFlags.EquippedItemStrings);
                dataReader.ReadFlags = flags;

                debugWindow.FormClosed += DebugWindow_FormClosed;
            }

            debugWindow.Show();
        }

        void DebugWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            var dataReader = gameService.DataReader;
            var flags = dataReader.ReadFlags.ClearFlag(DataReaderEnableFlags.EquippedItemStrings);
            dataReader.ReadFlags = flags;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterServiceEvents();

            pipeServer.Stop();
            base.OnFormClosing(e);
        }

        void UnregisterServiceEvents()
        {
            settingsService.SettingsChanged -= SettingsServiceOnSettingsChanged;

            gameService.CharacterCreated -= GameServiceOnCharacterCreated;
            gameService.DataRead -= GameServiceOnDataRead;
        }
    }
}
