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

            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));

            RegisterServiceEventHandlers();
        }

        private void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;

            gameService.DataRead += GameServiceOnDataRead;
            gameService.CharacterCreated += GameServiceOnCharacterCreated;
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

            for (var i = 0; i < settings.Autosplits.Count; ++i)
            {
                var split = settings.Autosplits[i];

                logMessage.AppendLine();
                logMessage.Append("  ");
                logMessage.Append($"#{i} [{split.Type}, {split.Value}, {split.Difficulty}] \"{split.Name}\"");
            }

            Logger.Info(logMessage.ToString());
        }

        private void GameServiceOnDataRead(object sender, DataReadEventArgs e)
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
            if (settings.DoAutosplit)
            {
                UpdateAutoSplits(settings, e);
            }
        }

        private void UpdateAutoSplits(ApplicationSettings settings, DataReadEventArgs eventArgs)
        {
            // Update autosplits only if the character was a freshly started character.
            if (!eventArgs.Character.IsAutosplitChar) return;

            var difficulty = eventArgs.CurrentDifficulty;
            IList<QuestCollection> gameQuests = eventArgs.Quests;
            var currentQuests = gameQuests[(int)difficulty];

            foreach (var autoSplit in settings.Autosplits)
            {
                if (autoSplit.IsReached)
                {
                    continue;
                }

                if (autoSplit.Type != AutoSplit.SplitType.Special)
                {
                    continue;
                }

                if (autoSplit.Value == (int)AutoSplit.Special.GameStart)
                {
                    CompleteAutoSplit(autoSplit);
                }

                if (autoSplit.Value == (int)AutoSplit.Special.Clear100Percent
                    && currentQuests.IsFullyCompleted
                    && autoSplit.MatchesDifficulty(difficulty))
                {
                    CompleteAutoSplit(autoSplit);
                }

                if (autoSplit.Value == (int)AutoSplit.Special.Clear100PercentAllDifficulties
                    && gameQuests.All(quests => quests.IsFullyCompleted))
                {
                    CompleteAutoSplit(autoSplit);
                }
            }

            // if no unreached splits, return
            if (!HaveUnreachedSplits(settings.Autosplits, difficulty))
            {
                return;
            }

            foreach (var autoSplit in settings.Autosplits)
            {
                if(IsCompleteableAutosplit(autoSplit, currentQuests, eventArgs, difficulty))
                {
                    CompleteAutoSplit(autoSplit);
                }
            }
        }

        private static bool IsCompleteableAutosplit(
            AutoSplit autoSplit,
            QuestCollection currentQuests,
            DataReadEventArgs eventArgs,
            GameDifficulty difficulty
        ) {
            if (autoSplit.IsReached || !autoSplit.MatchesDifficulty(difficulty))
            {
                return false;
            }

            switch (autoSplit.Type)
            {
                case AutoSplit.SplitType.CharLevel:
                    return autoSplit.Value <= eventArgs.Character.Level;
                case AutoSplit.SplitType.Area:
                    return autoSplit.Value == eventArgs.CurrentArea;
                case AutoSplit.SplitType.Item:
                    return eventArgs.ItemIds.Contains(autoSplit.Value);
                case AutoSplit.SplitType.Quest:
                    return currentQuests != null && currentQuests.IsQuestCompleted((QuestId)autoSplit.Value);
                case AutoSplit.SplitType.Gems:
                    return eventArgs.ItemIds.Contains(autoSplit.Value);
            }
            return false;
        }

        private static bool HaveUnreachedSplits(List<AutoSplit> autoSplits, GameDifficulty difficulty)
        {
            foreach (var autoSplit in autoSplits)
            {
                if (autoSplit.IsReached || !autoSplit.MatchesDifficulty(difficulty))
                {
                    continue;
                }

                switch (autoSplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        return true;
                    case AutoSplit.SplitType.Area:
                        return true;
                    case AutoSplit.SplitType.Item:
                        return true;
                    case AutoSplit.SplitType.Quest:
                        return true;
                    case AutoSplit.SplitType.Gems:
                        return true;
                }
            }

            return false;
        }

        private void CompleteAutoSplit(AutoSplit split)
        {
            // Autosplit already reached.
            if (split.IsReached) return;

            split.IsReached = true;
            TriggerAutosplit();

            var i = settingsService.CurrentSettings.Autosplits.IndexOf(split);
            Logger.Info($"AutoSplit: #{i} ({split.Name}, {split.Difficulty}) Reached.");
        }

        private void TriggerAutosplit()
        {
            var settings = settingsService.CurrentSettings;
            if (settings.DoAutosplit && settings.AutosplitHotkey != Keys.None)
            {
                keyService.TriggerHotkey(settings.AutosplitHotkey);
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
