namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;

    public class CompletedQuestsRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public CompletedQuestsRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            var payload = new
            {
                Normal = CompletedQuestIdsByDifficulty(GameDifficulty.Normal),
                Nightmare = CompletedQuestIdsByDifficulty(GameDifficulty.Nightmare),
                Hell = CompletedQuestIdsByDifficulty(GameDifficulty.Hell)
            };

            return new Response()
            {
                Status = payload.Normal == null ? ResponseStatus.NotFound : ResponseStatus.Success,
                Payload = payload
            };
        }

        private List<QuestId> CompletedQuestIdsByDifficulty(GameDifficulty difficulty)
        {
            List<QuestId> completedIds = new List<QuestId>();
            List<Quest> quests = dataReader.Quests.ByDifficulty(difficulty);

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
