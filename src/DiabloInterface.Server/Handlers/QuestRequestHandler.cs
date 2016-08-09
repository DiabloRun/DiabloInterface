using System.Collections.Generic;
using Zutatensuppe.D2Reader;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class QuestRequestHandler : IRequestHandler
    {
        D2DataReader dataReader;

        public QuestRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            int questId = int.Parse(arguments[0]);
            var questBuffer = dataReader.CurrentQuestBuffer;
            if (questBuffer == null)
                return BuildQuestResponse(completed: true);

            bool completed = D2QuestHelper.IsQuestComplete((D2QuestHelper.Quest)questId, questBuffer);
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
