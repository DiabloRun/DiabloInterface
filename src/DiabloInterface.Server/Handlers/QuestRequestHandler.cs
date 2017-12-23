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

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            int questId = int.Parse(arguments[0]);
            var quests = dataReader.CurrentQuests;
            var completed = quests == null || quests.IsQuestCompleted((QuestId)questId);
            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = new
                {
                    IsCompleted = completed
                }
            };
        }
    }
}
