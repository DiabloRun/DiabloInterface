namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.D2Reader.Struct.Item;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Gui.Controls;
    using Zutatensuppe.DiabloInterface.Gui.Forms;
    using Zutatensuppe.DiabloInterface.Server;

    public partial class DebugWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISettingsService settingsService;
        private readonly IGameService gameService;
        private readonly ServerService serverService;
        readonly Dictionary<GameDifficulty, QuestDebugRow[,]> questRows =
            new Dictionary<GameDifficulty, QuestDebugRow[,]>();

        List<AutosplitBinding> autoSplitBindings;
        IReadOnlyDictionary<BodyLocation, string> itemStrings;

        Dictionary<Label, BodyLocation> locs;
        Label clickedLabel;
        Label hoveredLabel;

        public DebugWindow(
            ISettingsService settingsService,
            IGameService gameService,
            ServerService serverService
        ) {
            Logger.Info("Creating debug window.");

            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.serverService = serverService ?? throw new ArgumentNullException(nameof(serverService));

            EnableReaderDebugData();
            RegisterServiceEventHandlers();

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing debug window.");
                UnregisterServiceEventHandlers();
                DisableReaderDebugData();
            };

            InitializeComponent();
            ApplyAutoSplitSettings(settingsService.CurrentSettings.Autosplits);
        }

        void EnableReaderDebugData()
        {
            var dataReader = gameService.DataReader;
            var flags = dataReader.ReadFlags.SetFlag(DataReaderEnableFlags.EquippedItemStrings);
            dataReader.ReadFlags = flags;
        }

        void DisableReaderDebugData()
        {
            var dataReader = gameService.DataReader;
            var flags = dataReader.ReadFlags.ClearFlag(DataReaderEnableFlags.EquippedItemStrings);
            dataReader.ReadFlags = flags;
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;
            gameService.DataRead += GameServiceOnDataRead;
            serverService.StatusChanged += ServerServiceStatusChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            settingsService.SettingsChanged -= SettingsServiceOnSettingsChanged;
            gameService.DataRead -= GameServiceOnDataRead;
        }

        void ServerServiceStatusChanged(object sender, ServerStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => ServerServiceStatusChanged(sender, e)));
                return;
            }

            txtPipeServer.Text = "";
            foreach (KeyValuePair<string, DiabloInterfaceServer> s in e.Servers)
            {
                txtPipeServer.Text += s.Key + ": " + (s.Value.Running ? "RUNNING" :"NOT RUNNING") + "\n";
            }
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsChanged(sender, e)));
                return;
            }

            ApplyAutoSplitSettings(e.Settings.Autosplits);
        }

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnDataRead(sender, e)));
                return;
            }

            foreach (GameDifficulty difficulty in Enum.GetValues(typeof(GameDifficulty)))
            {
                var quests = e.Quests.ByDifficulty(difficulty);
                if (quests == null) continue;

                UpdateQuestData(quests, difficulty);
            }

            UpdateItemStats(e.ItemStrings);
        }

        void UpdateQuestData(List<Quest> quests, GameDifficulty difficulty)
        {
            QuestDebugRow[,] rows = questRows[difficulty];

            foreach (var quest in quests)
            {
                try
                {
                    rows[quest.Act - 1, quest.ActOrder - 1].Update(quest);
                }
                catch (NullReferenceException)
                {
                    // System.NullReferenceException
                }
            }
        }

        void ClearAutoSplitBindings()
        {
            if (autoSplitBindings == null)
            {
                autoSplitBindings = new List<AutosplitBinding>();
            }

            foreach (var binding in autoSplitBindings)
            {
                binding.Unbind();
            }

            autoSplitBindings.Clear();
        }

        void ApplyAutoSplitSettings(List<AutoSplit> autoSplits)
        {
            if (DesignMode) return;
            // Unbinds and clears the binding list.
            ClearAutoSplitBindings();

            int y = 0;
            autosplitPanel.Controls.Clear();
            foreach (AutoSplit autoSplit in autoSplits)
            {
                Label splitLabel = new Label();
                splitLabel.SetBounds(0, y, autosplitPanel.Bounds.Width, 16);
                splitLabel.Text = autoSplit.Name;
                splitLabel.ForeColor = autoSplit.IsReached ? Color.Green : Color.Red;

                Action<AutoSplit> splitReached = s => splitLabel.ForeColor = Color.Green;
                Action<AutoSplit> splitReset = s => splitLabel.ForeColor = Color.Red;

                // Bind autosplit events.
                var binding = new AutosplitBinding(autoSplit, splitReached, splitReset);
                autoSplitBindings.Add(binding);

                autosplitPanel.Controls.Add(splitLabel);
                y += 16;
            }
        }

        void DebugWindow_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            questRows[GameDifficulty.Normal] = CreateQuestRow(tabPage1);
            questRows[GameDifficulty.Nightmare] = CreateQuestRow(tabPage2);
            questRows[GameDifficulty.Hell] = CreateQuestRow(tabPage3);

            locs = new Dictionary<Label, BodyLocation>
            {
                {label1, BodyLocation.Head},
                {label10, BodyLocation.Amulet},
                {label3, BodyLocation.PrimaryRight},
                {label2, BodyLocation.PrimaryLeft},
                {label4, BodyLocation.BodyArmor},
                {label7, BodyLocation.RingLeft},
                {label8, BodyLocation.RingRight},
                {label5, BodyLocation.Gloves},
                {label6, BodyLocation.Belt},
                {label9, BodyLocation.Boots}
            };
        }

        static QuestDebugRow[,] CreateQuestRow(TabPage tabPage)
        {
            var questRows = new QuestDebugRow[5, 6];

            for (int actIndex = 0; actIndex < 5; actIndex++)
            {
                int y = actIndex * 100 - (actIndex > 3 ? 3 * 16 : 0);

                tabPage.Controls.Add(new Label
                {
                    Text = "Act " + (actIndex + 1),
                    Width = 40,
                    Location = new Point(20, y)
                });

                for (int questIndex = 0; questIndex < (actIndex == 3 ? 3 : 6); questIndex++)
                {
                    var quest = QuestFactory.CreateByActAndOrder(actIndex + 1, questIndex + 1);
                    var row = new QuestDebugRow(quest);
                    row.Location = new Point(60, y + (questIndex * 16));
                    tabPage.Controls.Add(row);

                    questRows[actIndex, questIndex] = row;
                }
            }

            return questRows;
        }

        void UpdateItemStats(IReadOnlyDictionary<BodyLocation, string> itemStrings)
        {
            if (DesignMode) return;
            this.itemStrings = itemStrings;
            UpdateItemDebugInformation();
        }
        
        void UpdateItemDebugInformation()
        {
            // hover has precedence vs clicked labels
            Label l = hoveredLabel != null ? hoveredLabel : (clickedLabel != null ? clickedLabel : null);
            if (l == null || itemStrings == null || !locs.ContainsKey(l) || !itemStrings.ContainsKey(locs[l]))
            {
                textItemDesc.Text = "";
                return;
            }
            
            textItemDesc.Text = itemStrings[locs[l]];
        }

        private void LabelClick(object sender, EventArgs e)
        {
            if (clickedLabel == sender)
            {
                clickedLabel = null;
                ((Label)sender).ForeColor = Color.Aquamarine;
            }
            else
            {
                clickedLabel = (Label)sender;
                ((Label)sender).ForeColor = Color.Yellow;
            }
            UpdateItemDebugInformation();
        }

        private void LabelMouseEnter(object sender, EventArgs e)
        {
            hoveredLabel = (Label)sender;
            UpdateItemDebugInformation();
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            if (hoveredLabel != sender)
            {
                return;
            }
            hoveredLabel = null;
            UpdateItemDebugInformation();
        }

        /// <summary>
        /// Helper class for binding/unbinding AutoSplit event handlers.
        /// </summary>
        class AutosplitBinding
        {
            bool didUnbind;
            AutoSplit autoSplit;
            Action<AutoSplit> reachedHandler;
            Action<AutoSplit> resetHandler;

            public AutosplitBinding(AutoSplit autoSplit, Action<AutoSplit> reachedHandler, Action<AutoSplit> resetHandler)
            {
                this.autoSplit = autoSplit;
                this.reachedHandler = reachedHandler;
                this.resetHandler = resetHandler;

                this.autoSplit.Reached += reachedHandler;
                this.autoSplit.Reset += resetHandler;
            }

            ~AutosplitBinding()
            {
                Unbind();
            }

            /// <summary>
            /// Unbding the autosplit handlers.
            /// </summary>
            public void Unbind()
            {
                if (didUnbind) return;

                didUnbind = true;
                autoSplit.Reached -= reachedHandler;
                autoSplit.Reset -= resetHandler;
            }
        }
    }
}
