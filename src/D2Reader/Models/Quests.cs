using System.Collections.Generic;
using System.Linq;

namespace Zutatensuppe.D2Reader.Models
{
    public class Quests
    {
        List<List<Quest>> list;

        public Quests(List<List<Quest>> list)
        {
            this.list = list;
        }

        public Quests() : this(new List<List<Quest>>()) { }

        public List<Quest> ByDifficulty(GameDifficulty difficulty)
        {
            return list.Count > (int)difficulty ? list[(int)difficulty] : null;
        }

        public float ProgressByDifficulty(GameDifficulty difficulty)
        {
            var c = ByDifficulty(difficulty);
            if (c == null) return 0;

            var fullyCompleted = c.Sum(quest => quest.IsFullyCompleted ? 1 : 0);
            return c.Count == 0 ? 1 : (fullyCompleted / (float)c.Count);
        }

        public bool FullyCompleted()
        {
            return DifficultyFullyCompleted(GameDifficulty.Normal)
                && DifficultyFullyCompleted(GameDifficulty.Nightmare)
                && DifficultyFullyCompleted(GameDifficulty.Hell);
        }

        public bool DifficultyFullyCompleted(GameDifficulty difficulty)
        {
            var c = ByDifficulty(difficulty);
            return c != null && c.All(quest => quest.IsFullyCompleted);
        }

        public bool QuestCompleted(GameDifficulty difficulty, QuestId id)
        {
            var c = ByDifficulty(difficulty);
            return c != null && c.First(quest => quest.Id == id).IsCompleted;
        }

        public Dictionary<GameDifficulty, List<QuestId>> CompletedQuestIds => new Dictionary<GameDifficulty, List<QuestId>>
        {
            {GameDifficulty.Normal, CompletedQuestIdsByDifficulty(GameDifficulty.Normal) },
            {GameDifficulty.Nightmare, CompletedQuestIdsByDifficulty(GameDifficulty.Nightmare) },
            {GameDifficulty.Hell, CompletedQuestIdsByDifficulty(GameDifficulty.Hell) },
        };

        public Dictionary<GameDifficulty, List<QuestId>> FullyCompletedQuestIds => new Dictionary<GameDifficulty, List<QuestId>>
        {
            {GameDifficulty.Normal, FullyCompletedQuestIdsByDifficulty(GameDifficulty.Normal) },
            {GameDifficulty.Nightmare, FullyCompletedQuestIdsByDifficulty(GameDifficulty.Nightmare) },
            {GameDifficulty.Hell, FullyCompletedQuestIdsByDifficulty(GameDifficulty.Hell) },
        };

        public static Dictionary<GameDifficulty, List<QuestId>> DefaultCompleteQuestIds => new Dictionary<GameDifficulty, List<QuestId>>
        {
            { GameDifficulty.Normal, new List<QuestId>() },
            { GameDifficulty.Nightmare, new List<QuestId>() },
            { GameDifficulty.Hell, new List<QuestId>() }
        };

        private List<QuestId> CompletedQuestIdsByDifficulty(GameDifficulty difficulty)
        {
            List<Quest> quests = ByDifficulty(difficulty);
            if (quests == null)
                return null;

            return (
                from quest in quests
                where quest.IsCompleted
                select quest.Id
            ).ToList();
        }

        private List<QuestId> FullyCompletedQuestIdsByDifficulty(GameDifficulty difficulty)
        {
            List<Quest> quests = ByDifficulty(difficulty);
            if (quests == null)
                return null;

            return (
                from quest in quests
                where quest.IsFullyCompleted
                select quest.Id
            ).ToList();
        }
    }
}
