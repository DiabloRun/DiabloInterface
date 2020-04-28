namespace Zutatensuppe.D2Reader.Models
{
    using System;

    public class Quest
    {
        readonly QuestDetails details;

        internal Quest(QuestDetails details, ushort completionBits)
        {
            CompletionBits = completionBits;

            this.details = details ?? throw new ArgumentNullException(nameof(details));
        }

        /// <summary>
        /// Gets the quest bit vector data.
        /// </summary>
        public ushort CompletionBits { get; }

        /// <summary>
        /// Gets the quest identifier, specifying which quest it is.
        /// </summary>
        public QuestId Id => details.Id;

        /// <summary>
        /// Gets the act the quest belongs to.
        /// </summary>
        public int Act => details.Act;

        /// <summary>
        /// Gets the order the quest is listed in the game quest log of an act.
        /// </summary>
        public int ActOrder => details.ActOrder;

        /// <summary>
        /// Gets whether this is a quest that corresponds to an act boss.
        /// </summary>
        public bool IsBossQuest => details.IsBossQuest;

        /// <summary>
        /// Gets the quest name used in game.
        /// </summary>
        public string Name => details.Name;

        /// <summary>
        /// Gets the name commonly used to reference the quest.
        /// </summary>
        public string CommonName => details.CommonName;

        /// <summary>
        /// Gets whether this quest should count as completed towards 100% completion.
        /// </summary>
        public bool IsFullyCompleted => (CompletionBits & details.FullCompletionBitMask) != 0;

        /// <summary>
        /// Gets whether this quest should count as completed for the auto splitter.
        /// </summary>
        public bool IsCompleted => (CompletionBits & details.CompletionBitMask) != 0;
    }
}
