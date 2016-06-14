using System.Collections.Generic;

namespace DiabloInterface
{
    public class AutoSplitFactory
    {
        static Dictionary<D2Data.Quest, string> QuestNames = new Dictionary<D2Data.Quest, string>()
        {
            { D2Data.Quest.A1Q1, "Den of Evil" },
            { D2Data.Quest.A1Q6, "Andariel" },
            { D2Data.Quest.A2Q6, "Duriel" },
            { D2Data.Quest.A3Q6, "Mephisto" },
            { D2Data.Quest.A4Q2, "Diablo" },
            { D2Data.Quest.A5Q5, "Ancients" },
            { D2Data.Quest.A5Q6, "Baal" },
        };

        /// <summary>
        /// Create a default auto split.
        /// </summary>
        /// <returns></returns>
        public AutoSplit Create()
        {
            return new AutoSplit();
        }

        /// <summary>
        /// Automatically fill in sequential quests such as Andariel to Duriel.
        /// Also handles difficulty levels.
        /// </summary>
        /// <param name="previous">The previous autosplit in the list. May be null.</param>
        /// <returns></returns>
        public AutoSplit CreateSequential(AutoSplit previous)
        {
            if (previous == null)
            {
                return new AutoSplit("Game Start", AutoSplit.SplitType.Special, (short)AutoSplit.Special.GameStart, 0);
            }

            // Follow up with Andariel after game start.
            if (previous.Type == AutoSplit.SplitType.Special && previous.Value == (int)AutoSplit.Special.GameStart)
            {
                return CreateForQuest(D2Data.Quest.A1Q6, 0);
            }

            // Sequence bosses.
            else if (previous.Type == AutoSplit.SplitType.Quest)
            {
                short difficulty = previous.Difficulty;
                D2Data.Quest quest = D2Data.Quest.A1Q1;

                switch ((D2Data.Quest)previous.Value)
                {
                    case D2Data.Quest.A1Q1: // Den
                        quest = D2Data.Quest.A1Q6; break;
                    case D2Data.Quest.A1Q6: // Andariel
                        quest = D2Data.Quest.A2Q6; break;
                    case D2Data.Quest.A2Q6: // Duriel
                        quest = D2Data.Quest.A3Q6; break;
                    case D2Data.Quest.A3Q6: // Mephisto
                        quest = D2Data.Quest.A4Q2; break;
                    case D2Data.Quest.A4Q2: // Diablo
                        quest = D2Data.Quest.A5Q6; break;
                    case D2Data.Quest.A5Q6: // Baal
                        // If we reached the last quest in hell, return an auto split with
                        // the default values.
                        if (++difficulty > 2) return Create();
                        quest = D2Data.Quest.A1Q6;
                        break;

                    // Unsequenced quest.
                    default: return Create();
                }

                return CreateForQuest(quest, difficulty);
            }

            return Create();
        }

        /// <summary>
        /// Create a named autosplit for the specified quest and difficulty.
        /// </summary>
        /// <param name="quest">The quest.</param>
        /// <param name="difficulty">Difficulty for quest.</param>
        /// <returns></returns>
        public AutoSplit CreateForQuest(D2Data.Quest quest, short difficulty)
        {
            string name;
            // Get quest name if exists or default to "Quest".
            if (!QuestNames.TryGetValue(quest, out name))
            {
                name = "Quest";
            }

            // Add difficulty to name if above normal.
            if (difficulty == 1)
                name += " (NM)";
            if (difficulty == 2)
                name += " (H)";

            return new AutoSplit(name, AutoSplit.SplitType.Quest, (short)quest, difficulty);
        }
    }
}
