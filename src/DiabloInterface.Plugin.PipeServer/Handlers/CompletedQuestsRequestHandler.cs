namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

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
                Normal = dataReader.Quests.CompletedQuestIdsByDifficulty(GameDifficulty.Normal),
                Nightmare = dataReader.Quests.CompletedQuestIdsByDifficulty(GameDifficulty.Nightmare),
                Hell = dataReader.Quests.CompletedQuestIdsByDifficulty(GameDifficulty.Hell)
            };

            return new Response()
            {
                Status = payload.Normal == null ? ResponseStatus.NotFound : ResponseStatus.Success,
                Payload = payload
            };
        }
    }
}
