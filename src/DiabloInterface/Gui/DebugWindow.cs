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
    using Zutatensuppe.DiabloInterface.Business.Plugin;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Gui.Controls;
    using Zutatensuppe.DiabloInterface.Gui.Forms;

    public partial class DebugWindow : WsExCompositedForm
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISettingsService settingsService;
        private readonly IGameService gameService;
        private readonly List<IPlugin> plugins;
        readonly Dictionary<GameDifficulty, QuestDebugRow[,]> questRows =
            new Dictionary<GameDifficulty, QuestDebugRow[,]>();

        IReadOnlyDictionary<BodyLocation, string> itemStrings;

        Dictionary<Label, BodyLocation> locs;
        Label clickedLabel;
        Label hoveredLabel;

        public DebugWindow(
            ISettingsService settingsService,
            IGameService gameService,
            List<IPlugin> plugins
        ) {
            Logger.Info("Creating debug window.");

            this.plugins = plugins;
            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

            RegisterServiceEventHandlers();

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing debug window.");
                UnregisterServiceEventHandlers();
            };

            InitializeComponent();

            foreach (IPlugin p in plugins)
            {
                var r = p.DebugRenderer();
                if (r != null) r.ApplyChanges();
            }
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;
            gameService.DataRead += GameServiceOnDataRead;
            foreach (IPlugin p in plugins)
                p.Changed += PluginDataChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            settingsService.SettingsChanged -= SettingsServiceOnSettingsChanged;
            gameService.DataRead -= GameServiceOnDataRead;
            foreach (IPlugin p in plugins)
                p.Changed -= PluginDataChanged;
        }

        private void PluginDataChanged(object sender, IPlugin e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => PluginDataChanged(sender, e)));
                return;
            }

            var r = e.DebugRenderer();
            if (r != null) r.ApplyChanges();
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsChanged(sender, e)));
                return;
            }

            foreach (IPlugin p in plugins)
            {
                var r = p.DebugRenderer();
                if (r != null) r.ApplyChanges();
            }
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

            UpdateItemStats(e.Character.EquippedItemStrings);
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
    }
}
