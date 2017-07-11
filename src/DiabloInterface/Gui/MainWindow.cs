namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Struct.Item;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Data;
    using Zutatensuppe.DiabloInterface.Gui.Controls;
    using Zutatensuppe.DiabloInterface.Gui.Forms;
    using Zutatensuppe.DiabloInterface.IO;

    public partial class MainWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISettingsService settingsService;
        readonly IGameService gameService;
        readonly IAutoSplitService autoSplitService;

        DebugWindow debugWindow;
        AbstractLayout currentLayout;

        public MainWindow(ISettingsService settingsService, IGameService gameService, IAutoSplitService autoSplitService)
        {
            Logger.Info("Creating main window.");

            if (settingsService == null) throw new ArgumentNullException(nameof(settingsService));
            if (gameService == null) throw new ArgumentNullException(nameof(gameService));
            if (autoSplitService == null) throw new ArgumentNullException(nameof(autoSplitService));

            this.settingsService = settingsService;
            this.gameService = gameService;
            this.autoSplitService = autoSplitService;

            RegisterServiceEventHandlers();
            InitializeComponent();
            UpdateLayoutView(settingsService.CurrentSettings);
            PopulateSetingsFileListContextMenu(settingsService.SettingsFileCollection);
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;
            settingsService.SettingsCollectionChanged += SettingsServiceOnSettingsCollectionChanged;

            gameService.DataRead += GameServiceOnDataRead;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            ApplySettings(e.Settings);
        }

        void SettingsServiceOnSettingsCollectionChanged(object sender, SettingsCollectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsCollectionChanged(sender, e)));
                return;
            }

            PopulateSetingsFileListContextMenu(e.Collection);
        }

        void UpdateLayoutIfChanged(ApplicationSettings settings)
        {
            var isHorizontal = currentLayout is HorizontalLayout;
            if (isHorizontal != settings.DisplayLayoutHorizontal)
            {
                UpdateLayoutView(settings);
            }
        }

        void UpdateLayoutView(ApplicationSettings settings)
        {
            var nextLayout = CreateLayout(settings.DisplayLayoutHorizontal);
            if (currentLayout != null)
            {
                Controls.Remove(currentLayout);
                currentLayout.Dispose();
                currentLayout = null;
            }

            Controls.Add(nextLayout);
            currentLayout = nextLayout;
        }

        AbstractLayout CreateLayout(bool horizontal)
        {
            return horizontal
                ? new HorizontalLayout(settingsService, gameService) as AbstractLayout
                : new VerticalLayout(settingsService, gameService);
        }

        void MainWindowLoad(object sender, EventArgs e)
        {
            SetTitleWithApplicationVersion();
            CheckForUpdates();
            ApplySettings(settingsService.CurrentSettings);
        }

        void SetTitleWithApplicationVersion()
        {
            Text = $@"Diablo Interface v{Application.ProductVersion}";
            Update();
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

            Logger.Info("Applying settings to main window.");

            UpdateLayoutIfChanged(settings);
            UpdateAutoSplitsSettingsForDebugView(settings);
        }

        void UpdateAutoSplitsSettingsForDebugView(ApplicationSettings settings)
        {
            if (debugWindow != null && debugWindow.Visible)
            {
                debugWindow.UpdateAutosplits(settings.Autosplits);
            }
        }

        void PopulateSetingsFileListContextMenu(IEnumerable<FileInfo> settingsFileCollection)
        {
            loadConfigMenuItem.DropDownItems.Clear();
            IEnumerable<ToolStripItem> items = settingsFileCollection.Select(CreateSettingsToolStripMenuItem);
            loadConfigMenuItem.DropDownItems.AddRange(items.ToArray());
        }

        ToolStripMenuItem CreateSettingsToolStripMenuItem(FileInfo fileInfo)
        {
            var item = new ToolStripMenuItem
            {
                Text = Path.GetFileNameWithoutExtension(fileInfo.Name),
                Tag = fileInfo.FullName
            };

            item.Click += LoadConfigFile;

            return item;
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

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnDataRead(sender, e)));
                return;
            }

            WriteCharacterStatFiles(e.Character);

            UpdateDebugWindow(e.QuestBuffers, e.ItemStrings);
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
            autoSplitService.ResetAutoSplits();
            currentLayout?.Reset();
        }

        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            using (var settingsWindow = new SettingsWindow(settingsService))
            {
                settingsWindow.ShowDialog();
            }
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
            base.OnFormClosing(e);
        }

        void UnregisterServiceEvents()
        {
            settingsService.SettingsChanged -= SettingsServiceOnSettingsChanged;
            settingsService.SettingsCollectionChanged -= SettingsServiceOnSettingsCollectionChanged;

            gameService.DataRead -= GameServiceOnDataRead;
        }
    }
}
