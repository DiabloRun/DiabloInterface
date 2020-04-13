namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public class AutoSplitService : IAutoSplitService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISettingsService settingsService;
        private readonly IGameService gameService;
        private readonly KeyService keyService;

        public AutoSplitService(
            ISettingsService settingsService,
            IGameService gameService,
            KeyService keyService
        ) {
            Logger.Info("Creating auto split service.");

            this.settingsService = settingsService;
            this.gameService = gameService;
            this.keyService = keyService;

            this.settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;
            this.gameService.DataRead += GameServiceOnDataRead;
            this.gameService.CharacterCreated += GameServiceOnCharacterCreated;
        }

        private void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            LogAutoSplits(e.Settings);
        }

        private void LogAutoSplits(ApplicationSettings settings)
        {
            if (settings.Autosplits.Count == 0)
            {
                Logger.Info("No auto splits configured.");
                return;
            }

            var logMessage = new StringBuilder();
            logMessage.Append("Configured auto splits:");

            int i = 0;
            foreach (var split in settings.Autosplits)
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
            var settings = settingsService.CurrentSettings;
            if (!settings.DoAutosplit || !e.Character.IsAutosplitChar)
                return;

            int i = 0;
            foreach (var split in settings.Autosplits)
            {
                if (!IsCompleteableAutoSplit(split, e))
                    continue;

                split.IsReached = true;
                keyService.TriggerHotkey(settings.AutosplitHotkey);
                Logger.Info($"AutoSplit reached: {AutoSplitString(i++, split)}");
            }
        }

        private bool IsCompleteableAutoSplit(AutoSplit split, DataReadEventArgs args)
        {
            if (split.IsReached || !split.MatchesDifficulty(args.CurrentDifficulty))
                return false;

            switch (split.Type)
            {
                case AutoSplit.SplitType.Special:
                    switch (split.Value)
                    {
                        case (int)AutoSplit.Special.GameStart:
                            return true;
                        case (int)AutoSplit.Special.Clear100Percent:
                            return args.Quests.DifficultyCompleted(args.CurrentDifficulty);
                        case (int)AutoSplit.Special.Clear100PercentAllDifficulties:
                            return args.Quests.FullyCompleted();
                        default:
                            return false;
                    }
                case AutoSplit.SplitType.Quest:
                    return args.Quests.QuestCompleted(args.CurrentDifficulty, (QuestId)split.Value);
                case AutoSplit.SplitType.CharLevel:
                    return split.Value <= args.Character.Level;
                case AutoSplit.SplitType.Area:
                    return split.Value == args.CurrentArea;
                case AutoSplit.SplitType.Item:
                case AutoSplit.SplitType.Gems:
                    return args.ItemIds.Contains(split.Value);
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
            var settings = settingsService.CurrentSettings;
            foreach (var autoSplit in settings.Autosplits)
            {
                autoSplit.IsReached = false;
            }
        }
    }
}
