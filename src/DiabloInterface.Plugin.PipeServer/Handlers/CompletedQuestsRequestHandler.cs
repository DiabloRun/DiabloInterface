namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

    public class CompletedQuestsRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public CompletedQuestsRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            var payload = dataReader.Quests.CompletedQuestIds;
            return new Response()
            {
                Status = payload[D2Reader.Models.GameDifficulty.Normal] == null
                    ? ResponseStatus.NotFound
                    : ResponseStatus.Success,
                Payload = payload
            };
        }
    }
}
