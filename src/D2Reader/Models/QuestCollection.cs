namespace Zutatensuppe.D2Reader.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class QuestCollection : IEnumerable<Quest>
    {
        readonly IReadOnlyList<Quest> quests;

        public QuestCollection(IReadOnlyList<Quest> quests)
        {
            this.quests = quests ?? throw new ArgumentNullException(nameof(quests));
        }

        public float CompletionProgress
        {
            get
            {
                var completed = quests.Sum(quest => quest.IsCompleted ? 1 : 0);
                return quests.Count == 0 ? 1 : (completed / (float)quests.Count);
            }
        }

        public bool IsFullyCompleted => quests.All(quest => quest.IsCompleted);

        public bool IsQuestCompleted(QuestId questId) =>
            quests.First(quest => quest.Id == questId).IsAutoSplitReached;

        public IEnumerator<Quest> GetEnumerator()
        {
            return quests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return quests.GetEnumerator();
        }
    }
}
