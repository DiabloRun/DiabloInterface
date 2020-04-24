using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Plugin;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class AutoSplitService : IPlugin
    {
        public string Name => "Autosplit";

        public event EventHandler<IPlugin> Changed;

        public PluginData Data { get; } = new PluginData();

        public PluginConfig Cfg { get; } = new PluginConfig(new Dictionary<string, object>()
        {
            { "Enabled", false },
            { "Hotkey", Keys.None },
            { "Splits", new List<AutoSplit>() },
        });

        private bool DoAutosplit => Cfg.GetBool("Enabled");
        private Keys AutosplitHotkey => (Keys)Cfg.Get("Hotkey");
        internal List<AutoSplit> Autosplits => (List<AutoSplit>)Cfg.Get("Splits");

        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public readonly KeyService keyService;
        public readonly ISettingsService settingsService;
        public AutoSplitService(
            GameService gameService,
            ISettingsService settingsService
        )
        {
            Logger.Info("Creating auto split service.");

            this.keyService = new KeyService();
            this.settingsService = settingsService;
            settingsService.SettingsChanged += (object sender, ApplicationSettingsEventArgs args) =>
            {
                UpdateDataFromSettings(args.Settings);
            };
            UpdateDataFromSettings(settingsService.CurrentSettings);
        }

        private void UpdateDataFromSettings(ApplicationSettings s)
        {
            Cfg.Apply(s.PluginConf("Autosplit"));

            LogAutoSplits();
        }

        private void LogAutoSplits()
        {
            if (Autosplits.Count == 0)
            {
                Logger.Info("No auto splits configured.");
                return;
            }

            var logMessage = new StringBuilder();
            logMessage.Append("Configured auto splits:");

            int i = 0;
            foreach (var split in Autosplits)
            {
                logMessage.AppendLine();
                logMessage.Append(AutoSplitString(i++, split));
            }

            Logger.Info(logMessage.ToString());
        }

        private string AutoSplitString(int i, AutoSplit s)
        {
            return $"#{i} [{s.Type}, {s.Value}, {s.Difficulty}] \"{s.Name}\"";
        }

        private void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            DoAutoSplits(e);
        }

        private void DoAutoSplits(DataReadEventArgs e)
        {
            // TODO: fix bug... when splits are add during the run, the last split seems to trigger again on save
            // setup autosplits:
            // - start game
            // - area (cold plains)
            // start game, go to cold plains (2 splits should have happened)
            // add another autosplit:
            // - area (stony fields)
            // should not trigger another split automatically, but does
            if (!DoAutosplit || !e.Character.IsAutosplitChar)
                return;

            int i = 0;
            foreach (var split in Autosplits)
            {
                if (!IsCompleteableAutoSplit(split, e))
                    continue;

                split.IsReached = true;
                keyService.TriggerHotkey(AutosplitHotkey);
                Logger.Info($"AutoSplit reached: {AutoSplitString(i++, split)}");
            }
        }

        private bool IsCompleteableAutoSplit(AutoSplit split, DataReadEventArgs args)
        {
            if (split.IsReached || !split.MatchesDifficulty(args.Game.Difficulty))
                return false;

            switch (split.Type)
            {
                case AutoSplit.SplitType.Special:
                    switch (split.Value)
                    {
                        case (int)AutoSplit.Special.GameStart:
                            return true;
                        case (int)AutoSplit.Special.Clear100Percent:
                            return args.Quests.DifficultyCompleted(args.Game.Difficulty);
                        case (int)AutoSplit.Special.Clear100PercentAllDifficulties:
                            return args.Quests.FullyCompleted();
                        default:
                            return false;
                    }
                case AutoSplit.SplitType.Quest:
                    return args.Quests.QuestCompleted(args.Game.Difficulty, (QuestId)split.Value);
                case AutoSplit.SplitType.CharLevel:
                    return split.Value <= args.Character.Level;
                case AutoSplit.SplitType.Area:
                    return split.Value == args.Game.Area;
                case AutoSplit.SplitType.Item:
                case AutoSplit.SplitType.Gems:
                    return args.Character.InventoryItemIds.Contains(split.Value);
                default:
                    return false;
            }
        }

        private void GameServiceOnCharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            Logger.Info($"A new character was created. Auto splits enabled for {e.Character.Name}");

            ResetAutoSplits();
        }

        public void ResetAutoSplits()
        {
            foreach (var autoSplit in Autosplits)
            {
                autoSplit.IsReached = false;
            }
        }

        public void OnSettingsChanged()
        {
        }

        public void OnCharacterCreated(CharacterCreatedEventArgs e)
        {
            Logger.Info($"A new character was created. Auto splits enabled for {e.Character.Name}");
            ResetAutoSplits();
        }

        public void OnDataRead(DataReadEventArgs e)
        {
            DoAutoSplits(e);
        }

        public void OnReset()
        {
            ResetAutoSplits();
        }

        public void Dispose()
        {
        }

        AutosplitSettingsRenderer sr;
        public IPluginSettingsRenderer SettingsRenderer()
        {
            if (sr == null)
                sr = new AutosplitSettingsRenderer(this);
            return sr;
        }

        AutosplitDebugRenderer dr;
        public IPluginDebugRenderer DebugRenderer()
        {
            if (dr == null)
                dr = new AutosplitDebugRenderer(this);
            return dr;
        }
    }

    class AutosplitDebugRenderer : IPluginDebugRenderer
    {
        private AutoSplitService p;
        public AutosplitDebugRenderer(AutoSplitService p)
        {
            this.p = p;
        }

        private Panel autosplitPanel;
        private GroupBox groupBox2;
        List<AutosplitBinding> autoSplitBindings;
        public Control Render()
        {
            if (groupBox2 == null || groupBox2.IsDisposed)
            {
                Init();
            }
            return groupBox2;
        }

        private void Init()
        {
            autosplitPanel = new Panel();
            autosplitPanel.AutoScroll = true;
            autosplitPanel.Dock = DockStyle.Fill;
            autosplitPanel.Location = new Point(3, 16);
            autosplitPanel.Size = new Size(281, 105);

            groupBox2 = new GroupBox();
            groupBox2.Controls.Add(autosplitPanel);
            groupBox2.Location = new Point(576, 4);
            groupBox2.Size = new Size(287, 124);
            groupBox2.TabStop = false;
            groupBox2.Text = "Splits";
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

        void ApplyAutoSplitSettings()
        {
            // Unbinds and clears the binding list.
            ClearAutoSplitBindings();

            int y = 0;
            autosplitPanel.Controls.Clear();
            foreach (AutoSplit autoSplit in p.Autosplits)
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

        public void ApplyChanges()
        {
        }
    }

    class AutosplitSettingsRenderer : IPluginSettingsRenderer
    {
        private AutoSplitService p;

        GroupBox pluginBox;
        AutoSplitTable autoSplitTable;
        HotkeyControl autoSplitHotkeyControl;
        private Button AddAutoSplitButton;
        private CheckBox EnableAutosplitCheckBox;
        private Label AutoSplitHotkeyLabel;
        private Button AutoSplitTestHotkeyButton;
        private Panel AutoSplitToolbar;
        private TableLayoutPanel AutoSplitLayout;

        public AutosplitSettingsRenderer(AutoSplitService p)
        {
            this.p = p;
        }

        public Control Render()
        {
            if (pluginBox == null || pluginBox.IsDisposed)
            {
                Init();
            }
            return pluginBox;
        }

        private void Init()
        {
            autoSplitHotkeyControl = new HotkeyControl();
            autoSplitHotkeyControl.Hotkey = Keys.None;
            autoSplitHotkeyControl.Location = new Point(80, 7);
            autoSplitHotkeyControl.Size = new Size(82, 20);
            autoSplitHotkeyControl.Text = "None";
            autoSplitHotkeyControl.UseKeyWhitelist = true;
            autoSplitHotkeyControl.HotkeyChanged += new EventHandler<Keys>(AutoSplitHotkeyControlOnHotkeyChanged);

            AutoSplitHotkeyLabel = new Label();
            AutoSplitHotkeyLabel.AutoSize = true;
            AutoSplitHotkeyLabel.Location = new Point(3, 10);
            AutoSplitHotkeyLabel.Size = new Size(67, 13);
            AutoSplitHotkeyLabel.Text = "Split-Hotkey:";

            AutoSplitTestHotkeyButton = new Button();
            AutoSplitTestHotkeyButton.Location = new Point(168, 5);
            AutoSplitTestHotkeyButton.Size = new Size(75, 23);
            AutoSplitTestHotkeyButton.Text = "Test Hotkey";
            AutoSplitTestHotkeyButton.Click += new EventHandler(AutoSplitTestHotkey_Click);

            EnableAutosplitCheckBox = new CheckBox();
            EnableAutosplitCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            EnableAutosplitCheckBox.AutoSize = true;
            EnableAutosplitCheckBox.Location = new Point(358, 9);
            EnableAutosplitCheckBox.Size = new Size(59, 17);
            EnableAutosplitCheckBox.Text = "Enable";

            AddAutoSplitButton = new Button();
            AddAutoSplitButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddAutoSplitButton.Location = new Point(422, 5);
            AddAutoSplitButton.Size = new Size(79, 23);
            AddAutoSplitButton.Text = "Add Split";
            AddAutoSplitButton.Click += new EventHandler(AddAutoSplitButton_Clicked);

            AutoSplitToolbar = new Panel();
            AutoSplitToolbar.AutoSize = true;
            AutoSplitToolbar.Controls.Add(autoSplitHotkeyControl);
            AutoSplitToolbar.Controls.Add(AutoSplitHotkeyLabel);
            AutoSplitToolbar.Controls.Add(AutoSplitTestHotkeyButton);
            AutoSplitToolbar.Controls.Add(EnableAutosplitCheckBox);
            AutoSplitToolbar.Controls.Add(AddAutoSplitButton);
            AutoSplitToolbar.Dock = DockStyle.Fill;
            AutoSplitToolbar.Location = new Point(0, 445);
            AutoSplitToolbar.Margin = new Padding(0);
            AutoSplitToolbar.Size = new Size(505, 31);

            autoSplitTable = new AutoSplitTable(p.settingsService) { Dock = DockStyle.Fill };

            AutoSplitLayout = new TableLayoutPanel();
            AutoSplitLayout.ColumnCount = 1;
            AutoSplitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AutoSplitLayout.Dock = DockStyle.Fill;
            AutoSplitLayout.Location = new Point(0, 0);
            AutoSplitLayout.Margin = new Padding(0);
            AutoSplitLayout.RowCount = 2;
            AutoSplitLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AutoSplitLayout.RowStyles.Add(new RowStyle());
            AutoSplitLayout.Size = new Size(505, 476);
            AutoSplitLayout.Controls.Add(AutoSplitToolbar, 0, 1);
            AutoSplitLayout.Controls.Add(autoSplitTable);

            pluginBox = new GroupBox();
            pluginBox.Controls.Add(AutoSplitLayout);
        }

        void AddAutoSplitButton_Clicked(object sender, EventArgs e)
        {
            var splits = autoSplitTable.AutoSplits;
            var factory = new AutoSplitFactory();

            var row = autoSplitTable.AddAutoSplit(factory.CreateSequential(splits.LastOrDefault()));
            if (row != null)
            {
                autoSplitTable.ScrollControlIntoView(row);
            }

            // Automatically enable auto splits when adding.
            EnableAutosplitCheckBox.Checked = true;
        }

        void AutoSplitTestHotkey_Click(object sender, EventArgs e)
        {
            p.keyService.TriggerHotkey(autoSplitHotkeyControl.Hotkey);
        }

        void AutoSplitHotkeyControlOnHotkeyChanged(object sender, Keys e)
        {
            autoSplitHotkeyControl.ForeColor = e == Keys.None ? Color.Red : SystemColors.WindowText;
        }

        public bool IsDirty()
        {
            return autoSplitTable.IsDirty
                || p.Cfg.GetBool("Enabled") != EnableAutosplitCheckBox.Checked
                || p.Cfg.GetKeys("Hotkey") != autoSplitHotkeyControl.Hotkey;
        }

        public PluginConfig Get()
        {
            return new PluginConfig(new Dictionary<string, object>()
            {
                {"Enabled", EnableAutosplitCheckBox.Checked},
                {"Hotkey", autoSplitHotkeyControl.Hotkey },
                {"Splits", autoSplitTable.AutoSplits.ToList()},
            });
        }

        public void Set(PluginConfig cfg)
        {
            EnableAutosplitCheckBox.Checked = cfg.GetBool("Enabled");
            autoSplitHotkeyControl.ForeColor = cfg.GetKeys("Hotkey") == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Hotkey = cfg.GetKeys("Hotkey");

            autoSplitTable?.MarkClean();
        }

        public void ApplyChanges()
        {
        }
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

        public AutosplitBinding(
            AutoSplit autoSplit,
            Action<AutoSplit> reachedHandler,
            Action<AutoSplit> resetHandler
        )
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
