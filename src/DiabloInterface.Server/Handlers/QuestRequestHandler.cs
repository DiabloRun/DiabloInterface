namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;

    public class QuestRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public QuestRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        }

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            int questId = int.Parse(arguments[0]);
            var quests = dataReader.CurrentQuests;
            if (quests == null) return BuildQuestResponse(completed: true);

            var completed = quests.IsQuestCompleted((QuestId)questId);
            return BuildQuestResponse(completed);
        }

        QueryResponse BuildQuestResponse(bool completed)
        {
            return new QueryResponse()
            {
                Status = QueryStatus.Success,
                Payload = new
                {
                    IsCompleted = completed
                }
            };
        }
    }
}
