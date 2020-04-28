using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Settings;
using Zutatensuppe.DiabloInterface.Services;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    public class AutosplitPluginConfig : PluginConfig
    {
        public bool Enabled { get { return Is("Enabled"); } set { Set("Enabled", value); } }
        public Keys Hotkey { get { return GetKeys("Hotkey"); } set { Set("Hotkey", value); } }
        public List<AutoSplit> Splits { get { return (List<AutoSplit>)Get("Splits"); } set { Set("Splits", value); } }

        public AutosplitPluginConfig()
        {
            Enabled = false;
            Hotkey = Keys.None;
            Splits = new List<AutoSplit>();
        }

        public AutosplitPluginConfig(PluginConfig s) :this()
        {
            if (s != null)
            {
                Enabled = s.Is("Enabled");
                Hotkey = s.GetKeys("Hotkey");
                Splits = s.Get("Splits") as List<AutoSplit>;
                if (Splits == null)
                    Splits = new List<AutoSplit>();
            }
        }
    }

    public class AutoSplitService : IPlugin
    {
        public string Name => "Autosplit";

        internal AutosplitPluginConfig Cfg { get; private set; } = new AutosplitPluginConfig();
        
        private ILogger Logger;

        public readonly KeyService keyService = new KeyService();

        private DiabloInterface di;

        public AutoSplitService(DiabloInterface di)
        {
            Logger = di.Logger(this);
            Logger.Info("Creating auto split service.");
            this.di = di;
        }

        public void Initialize()
        {
            di.game.CharacterCreated += Game_CharacterCreated;
            di.game.DataRead += Game_DataRead;
            di.settings.Changed += SettingsService_Changed;
            Init(di.settings.CurrentSettings);
        }

        private void Game_CharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            Logger.Info($"A new character was created. Auto splits enabled for {e.Character.Name}");
            ResetAutoSplits();
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            DoAutoSplits(e);
        }

        private void SettingsService_Changed(object sender, ApplicationSettingsEventArgs e)
        {
            Init(e.Settings);
        }

        private void Init(ApplicationSettings s)
        {
            Cfg = new AutosplitPluginConfig(s.PluginConf(Name));
            ReloadWithCurrentSettings(Cfg);
            LogAutoSplits();
        }

        private void LogAutoSplits()
        {
            if (Cfg.Splits.Count == 0)
            {
                Logger.Info("No auto splits configured.");
                return;
            }

            var logMessage = new StringBuilder();
            logMessage.Append("Configured auto splits:");

            int i = 0;
            foreach (var split in Cfg.Splits)
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
            if (!Cfg.Enabled || !e.Character.IsNewChar)
                return;

            int i = 0;
            foreach (var split in Cfg.Splits)
            {
                if (!IsCompleteableAutoSplit(split, e))
                    continue;

                split.IsReached = true;
                keyService.TriggerHotkey(Cfg.Hotkey);
                Logger.Info($"AutoSplit reached: {AutoSplitString(i++, split)}");
            }
            ApplyChanges();
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
                            return args.Quests.DifficultyFullyCompleted(args.Game.Difficulty);
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

        public void ResetAutoSplits()
        {
            foreach (var autoSplit in Cfg.Splits)
            {
                autoSplit.IsReached = false;
            }
            ApplyChanges();
        }

        public void Reset()
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

        private void ApplyChanges()
        {
            if (sr != null)
                sr.ApplyChanges();
            if (dr != null)
                dr.ApplyChanges();
        }

        private void ReloadWithCurrentSettings(AutosplitPluginConfig cfg)
        {
            if (sr != null)
                sr.Set(cfg);
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
        List<AutosplitBinding> autoSplitBindings;
        public Control Render()
        {
            if (autosplitPanel == null || autosplitPanel.IsDisposed)
            {
                Init();
            }
            return autosplitPanel;
        }

        private void Init()
        {
            autosplitPanel = new Panel();
            autosplitPanel.AutoScroll = true;
            autosplitPanel.Dock = DockStyle.Fill;
            autosplitPanel.Location = new Point(3, 16);
            autosplitPanel.Size = new Size(281, 105);

            ApplyChanges();
        }

        public void ApplyChanges()
        {
            if (autosplitPanel.InvokeRequired)
            {
                autosplitPanel.Invoke((Action)(() => ApplyChanges()));
                return;
            }

            // Unbinds and clears the binding list.
            ClearAutoSplitBindings();

            int y = 0;
            autosplitPanel.Controls.Clear();
            foreach (AutoSplit autoSplit in p.Cfg.Splits)
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
    }

    class AutosplitSettingsRenderer : IPluginSettingsRenderer
    {
        private AutoSplitService p;

        private TableLayoutPanel AutoSplitLayout;
        AutoSplitTable autoSplitTable;
        HotkeyControl autoSplitHotkeyControl;
        private Button AddAutoSplitButton;
        private CheckBox EnableAutosplitCheckBox;
        private Label AutoSplitHotkeyLabel;
        private Button AutoSplitTestHotkeyButton;
        private FlowLayoutPanel AutoSplitToolbar;

        public AutosplitSettingsRenderer(AutoSplitService p)
        {
            this.p = p;
        }

        public Control Render()
        {
            if (AutoSplitLayout == null || AutoSplitLayout.IsDisposed)
            {
                Init();
            }
            return AutoSplitLayout;
        }

        private void Init()
        {
            autoSplitHotkeyControl = new HotkeyControl();
            autoSplitHotkeyControl.Hotkey = Keys.None;
            autoSplitHotkeyControl.AutoSize = true;
            autoSplitHotkeyControl.Text = "None";
            autoSplitHotkeyControl.UseKeyWhitelist = true;
            autoSplitHotkeyControl.HotkeyChanged += new EventHandler<Keys>(AutoSplitHotkeyControlOnHotkeyChanged);

            AutoSplitHotkeyLabel = new Label();
            AutoSplitHotkeyLabel.AutoSize = true;
            AutoSplitHotkeyLabel.Text = "Split-Hotkey:";

            AutoSplitTestHotkeyButton = new Button();
            AutoSplitTestHotkeyButton.AutoSize = true;
            AutoSplitTestHotkeyButton.Text = "Test Hotkey";
            AutoSplitTestHotkeyButton.Click += new EventHandler(AutoSplitTestHotkey_Click);

            EnableAutosplitCheckBox = new CheckBox();
            EnableAutosplitCheckBox.AutoSize = true;
            EnableAutosplitCheckBox.Text = "Enable";

            AddAutoSplitButton = new Button();
            AddAutoSplitButton.AutoSize = true;
            AddAutoSplitButton.Text = "Add Split";
            AddAutoSplitButton.Click += new EventHandler(AddAutoSplitButton_Clicked);

            AutoSplitToolbar = new FlowLayoutPanel();
            AutoSplitToolbar.FlowDirection = FlowDirection.LeftToRight;
            AutoSplitToolbar.AutoSize = true;
            AutoSplitToolbar.Controls.Add(AutoSplitHotkeyLabel);
            AutoSplitToolbar.Controls.Add(autoSplitHotkeyControl);
            AutoSplitToolbar.Controls.Add(EnableAutosplitCheckBox);
            AutoSplitToolbar.Controls.Add(AutoSplitTestHotkeyButton);
            AutoSplitToolbar.Controls.Add(AddAutoSplitButton);
            AutoSplitToolbar.Dock = DockStyle.Fill;
            AutoSplitToolbar.Margin = new Padding(0);

            autoSplitTable = new AutoSplitTable(p.Cfg) { Dock = DockStyle.Fill };

            AutoSplitLayout = new TableLayoutPanel();
            AutoSplitLayout.ColumnCount = 1;
            AutoSplitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AutoSplitLayout.Dock = DockStyle.Fill;
            AutoSplitLayout.Location = new Point(0, 0);
            AutoSplitLayout.Margin = new Padding(0);
            AutoSplitLayout.RowCount = 2;
            AutoSplitLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AutoSplitLayout.RowStyles.Add(new RowStyle());
            AutoSplitLayout.Size = new Size(800, 476);
            AutoSplitLayout.Controls.Add(AutoSplitToolbar, 0, 1);
            AutoSplitLayout.Controls.Add(autoSplitTable);

            Set(p.Cfg);
            ApplyChanges();
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
                || p.Cfg.Enabled != EnableAutosplitCheckBox.Checked
                || p.Cfg.Hotkey != autoSplitHotkeyControl.Hotkey;
        }

        public PluginConfig Get()
        {
            var conf = new AutosplitPluginConfig();
            conf.Enabled = EnableAutosplitCheckBox.Checked;
            conf.Hotkey = autoSplitHotkeyControl.Hotkey;
            conf.Splits = autoSplitTable.AutoSplits.ToList();
            return conf;
        }

        internal void Set(AutosplitPluginConfig conf)
        {
            if (AutoSplitLayout.InvokeRequired)
            {
                AutoSplitLayout.Invoke((Action)(() => Set(conf)));
                return;
            }

            EnableAutosplitCheckBox.Checked = conf.Enabled;
            autoSplitHotkeyControl.ForeColor = conf.Hotkey == Keys.None ? Color.Red : Color.Black;
            autoSplitHotkeyControl.Hotkey = conf.Hotkey;

            autoSplitTable.Set(conf);
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
