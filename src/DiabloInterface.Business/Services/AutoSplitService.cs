namespace Zutatensuppe.DiabloInterface.Business.Services
{
    using System;
    using System.Collections.Generic;
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

        readonly ISettingsService settingsService;
        readonly IGameService gameService;

        public AutoSplitService(ISettingsService settingsService, IGameService gameService)
        {
            Logger.Info("Creating auto split service.");

            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

            RegisterServiceEventHandlers();
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;

            gameService.DataRead += GameServiceOnDataRead;
            gameService.CharacterCreated += GameServiceOnCharacterCreated;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            LogAutoSplits(e.Settings);
        }

        void LogAutoSplits(ApplicationSettings settings)
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

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            var settings = settingsService.CurrentSettings;
            if (settings.DoAutosplit)
            {
                UpdateAutoSplits(settings, e);
            }
        }

        void UpdateAutoSplits(ApplicationSettings settings, DataReadEventArgs eventArgs)
        {
            // Update autosplits only if the character was a freshly started character.
            if (!eventArgs.IsAutosplitCharacter) return;

            
            var difficulty = eventArgs.CurrentDifficulty;
            IList<QuestCollection> gameQuests = eventArgs.Quests;
            var quests = gameQuests[(int)difficulty];

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
                    && quests.IsFullyCompleted
                    && autoSplit.MatchesDifficulty(difficulty))
                {
                    CompleteAutoSplit(autoSplit);
                }

                if (autoSplit.Value == (int)AutoSplit.Special.Clear100PercentAllDifficulties
                    && gameQuests[0].IsFullyCompleted
                    && gameQuests[1].IsFullyCompleted
                    && gameQuests[2].IsFullyCompleted)
                {
                    CompleteAutoSplit(autoSplit);
                }
            }

            bool haveUnreachedCharLevelSplits = false;
            bool haveUnreachedAreaSplits = false;
            bool haveUnreachedItemSplits = false;
            bool haveUnreachedQuestSplits = false;
            bool haveUnreachedGemSplits = false;

            foreach (var autoSplit in settings.Autosplits)
            {
                if (autoSplit.IsReached || !autoSplit.MatchesDifficulty(difficulty))
                {
                    continue;
                }

                switch (autoSplit.Type)
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
                    case AutoSplit.SplitType.Gems:
                        haveUnreachedGemSplits = true;
                        break;
                }
            }

            // if no unreached splits, return
            if (!(haveUnreachedCharLevelSplits
                || haveUnreachedAreaSplits
                || haveUnreachedItemSplits
                || haveUnreachedQuestSplits
                || haveUnreachedGemSplits))
            {
                return;
            }

            var character = eventArgs.Character;

            foreach (var autoSplit in settings.Autosplits)
            {
                if (autoSplit.IsReached || !autoSplit.MatchesDifficulty(difficulty))
                {
                    continue;
                }

                switch (autoSplit.Type)
                {
                    case AutoSplit.SplitType.CharLevel:
                        if (autoSplit.Value <= character.Level)
                        {
                            CompleteAutoSplit(autoSplit);
                        }

                        break;
                    case AutoSplit.SplitType.Area:
                        if (autoSplit.Value == eventArgs.CurrentArea)
                        {
                            CompleteAutoSplit(autoSplit);
                        }

                        break;
                    case AutoSplit.SplitType.Item:
                        if (eventArgs.ItemIds.Contains(autoSplit.Value))
                        {
                            CompleteAutoSplit(autoSplit);
                        }

                        break;
                    case AutoSplit.SplitType.Quest:
                        if (quests == null) continue;
                        if (quests.IsQuestCompleted((QuestId)autoSplit.Value))
                        {
                            CompleteAutoSplit(autoSplit);
                        }

                        break;
                    case AutoSplit.SplitType.Gems:
                        if (eventArgs.ItemIds.Contains(autoSplit.Value))
                        {
                            CompleteAutoSplit(autoSplit);
                        }

                        break;
                }
            }
        }

        void CompleteAutoSplit(AutoSplit autosplit)
        {
            // Autosplit already reached.
            if (autosplit.IsReached) return;

            autosplit.IsReached = true;
            TriggerAutosplit();

            var autoSplitIndex = settingsService.CurrentSettings.Autosplits.IndexOf(autosplit);
            Logger.Info($"AutoSplit: #{autoSplitIndex} ({autosplit.Name}, {autosplit.Difficulty}) Reached.");
        }

        void TriggerAutosplit()
        {
            var settings = settingsService.CurrentSettings;
            if (settings.DoAutosplit && settings.AutosplitHotkey != Keys.None)
            {
                KeyManager.TriggerHotkey(settings.AutosplitHotkey);
            }
        }

        void GameServiceOnCharacterCreated(object sender, CharacterCreatedEventArgs e)
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
