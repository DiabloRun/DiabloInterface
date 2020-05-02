using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

[assembly: InternalsVisibleTo("DiabloInterface.Plugin.Autosplits.Test")]
namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    public class Plugin : BasePlugin
    {
        protected readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "Autosplit";

        protected override Type SettingsRendererType => typeof(SettingsRenderer);

        protected override Type DebugRendererType => typeof(DebugRenderer);

        internal Config Config { get; private set; } = new Config();

        public readonly KeyService keyService = new KeyService();

        public override void SetConfig(IPluginConfig c)
        {
            Config = c as Config;
            ApplyConfig();
            LogAutoSplits();
        }

        public override void Initialize(DiabloInterface di)
        {
            Logger.Info("Creating auto split service.");

            SetConfig(di.settings.CurrentSettings.PluginConf(Name));
            di.game.CharacterCreated += Game_CharacterCreated;
            di.game.DataRead += Game_DataRead;
        }

        private void Game_CharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            if (!Config.Enabled) return;

            Logger.Info($"A new character was created. Auto splits enabled for {e.Character.Name}");
            ResetAutoSplits();
            keyService.TriggerHotkey(Config.ResetHotkey.ToKeys());
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Config.Enabled) return;

            DoAutoSplits(e);
        }

        private void LogAutoSplits()
        {
            if (Config.Splits.Count == 0)
            {
                Logger.Info("No auto splits configured.");
                return;
            }

            var logMessage = new StringBuilder();
            logMessage.Append("Configured auto splits:");

            int i = 0;
            foreach (var split in Config.Splits)
            {
                logMessage.AppendLine();
                logMessage.Append(AutoSplitString(i++, split));
            }

            Logger.Info(logMessage.ToString());
        }

        private string AutoSplitString(int i, AutoSplit s) => $"#{i} [{s.Type}, {s.Value}, {s.Difficulty}] \"{s.Name}\"";

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
            if (!e.Character.IsNewChar)
                return;

            int i = 0;
            foreach (var split in Config.Splits)
            {
                if (!IsCompleteableAutoSplit(split, e))
                    continue;

                split.IsReached = true;
                keyService.TriggerHotkey(Config.Hotkey.ToKeys());
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
            foreach (var autoSplit in Config.Splits)
                autoSplit.IsReached = false;

            ApplyChanges();
        }

        public override void Reset()
        {
            ResetAutoSplits();
        }
    }
}
