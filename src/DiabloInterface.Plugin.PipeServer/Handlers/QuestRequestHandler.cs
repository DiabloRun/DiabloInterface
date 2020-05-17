using System;
using System.Collections.Generic;

using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
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

            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = new
                {
                    IsCompleted = dataReader.Quests.QuestCompleted(dataReader.Difficulty, (QuestId)questId)
                }
            };
        }
    }
}
