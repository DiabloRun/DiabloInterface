namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
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
            var payload = BuildPayload(dataReader.Game);

            return new Response()
            {
                Payload = payload,
                Status = payload != null ? ResponseStatus.Success : ResponseStatus.NotFound,
            };
        }

        object BuildPayload(Game game)
        {
            if (game == null)
                return null;

            var data = dataReader.Game.Quests.CompletedQuestIds;
            return data[GameDifficulty.Normal] == null ? null : data;
        }
    }
}
