using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var completed = c.Sum(quest => quest.IsCompleted ? 1 : 0);
            return c.Count == 0 ? 1 : (completed / (float)c.Count);
        }

        public bool FullyCompleted()
        {
            return DifficultyCompleted(GameDifficulty.Normal)
                && DifficultyCompleted(GameDifficulty.Nightmare)
                && DifficultyCompleted(GameDifficulty.Hell);
        }

        public bool DifficultyCompleted(GameDifficulty difficulty)
        {
            var c = ByDifficulty(difficulty);
            return c != null && c.All(quest => quest.IsCompleted);
        }

        public bool QuestCompleted(GameDifficulty difficulty, QuestId id)
        {
            var c = ByDifficulty(difficulty);
            return c != null && c.First(quest => quest.Id == id).IsAutoSplitReached;
        }

        public Dictionary<GameDifficulty, List<QuestId>> CompletedQuestIds => new Dictionary<GameDifficulty, List<QuestId>>
        {
            {GameDifficulty.Normal, CompletedQuestIdsByDifficulty(GameDifficulty.Normal) },
            {GameDifficulty.Nightmare, CompletedQuestIdsByDifficulty(GameDifficulty.Nightmare) },
            {GameDifficulty.Hell, CompletedQuestIdsByDifficulty(GameDifficulty.Hell) },
        };

        public static Dictionary<GameDifficulty, List<QuestId>> DefaultCompleteQuestIds => new Dictionary<GameDifficulty, List<QuestId>>
        {
            { GameDifficulty.Normal, new List<QuestId>() },
            { GameDifficulty.Nightmare, new List<QuestId>() },
            { GameDifficulty.Hell, new List<QuestId>() }
        };

        public List<QuestId> CompletedQuestIdsByDifficulty(GameDifficulty difficulty)
        {
            List<QuestId> completedIds = new List<QuestId>();
            List<Quest> quests = ByDifficulty(difficulty);

            if (quests == null)
                return null;

            foreach (Quest quest in quests)
            {
                if (quest.IsCompleted)
                {
                    completedIds.Add(quest.Id);
                }
            }

            return completedIds;
        }
    }
}
