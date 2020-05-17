namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits
{
    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;

    public class AutoSplitFactory
    {
        /// <summary>
        /// Create a default auto split.
        /// </summary>
        /// <returns>A default auto split.</returns>
        public AutoSplit CreateDefault()
        {
            return new AutoSplit();
        }

        /// <summary>
        /// Automatically fill in sequential quests such as Andariel to Duriel.
        /// Also handles difficulty levels.
        /// </summary>
        /// <param name="previous">The previous autosplit in the list. May be null.</param>
        /// <returns>A sequenced auto split.</returns>
        public AutoSplit CreateSequential(AutoSplit previous)
        {
            if (previous == null)
            {
                return new AutoSplit("Game Start", AutoSplit.SplitType.Special, (short)AutoSplit.Special.GameStart, 0);
            }

            // Follow up with Andariel after game start.
            if (previous.Type == AutoSplit.SplitType.Special && previous.Value == (int)AutoSplit.Special.GameStart)
            {
                return CreateForQuest(QuestId.Andariel, 0);
            }

            // Sequence bosses.
            if (previous.Type == AutoSplit.SplitType.Quest)
            {
                short difficulty = previous.Difficulty;
                QuestId quest;

                switch ((QuestId)previous.Value)
                {
                    case QuestId.DenOfEvil:
                        quest = QuestId.Andariel; break;
                    case QuestId.Andariel:
                        quest = QuestId.Duriel; break;
                    case QuestId.Duriel:
                        quest = QuestId.Mephisto; break;
                    case QuestId.Mephisto:
                        quest = QuestId.Diablo; break;
                    case QuestId.Diablo:
                        quest = QuestId.Baal; break;
                    case QuestId.Baal:
                        // If we reached the last quest in hell, return an auto split with
                        // the default values.
                        if (++difficulty > 2) return CreateDefault();
                        quest = QuestId.Andariel;
                        break;

                    // Unsequenced quest.
                    default: return CreateDefault();
                }

                return CreateForQuest(quest, (GameDifficulty)difficulty);
            }

            return CreateDefault();
        }

        /// <summary>
        /// Create a named autosplit for the specified quest and difficulty.
        /// </summary>
        /// <param name="questId">The quest.</param>
        /// <param name="difficulty">Difficulty for quest.</param>
        /// <returns>An autosplit for a specific quest.</returns>
        AutoSplit CreateForQuest(QuestId questId, GameDifficulty difficulty)
        {
            var quest = QuestFactory.Create(questId, 0);
            var name = quest != null ? quest.CommonName : "Quest";

            // Add difficulty to name if above normal.
            if (difficulty == GameDifficulty.Nightmare)
                name += " (NM)";
            if (difficulty == GameDifficulty.Hell)
                name += " (H)";

            return new AutoSplit(name, AutoSplit.SplitType.Quest, (short)questId, (short)difficulty);
        }
    }
}
