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
            var payload = BuildPayload(dataReader.Game, (QuestId)int.Parse(arguments[0]));

            return new Response()
            {
                Payload = payload,
                Status = payload != null ? ResponseStatus.Success : ResponseStatus.NotFound,
            };
        }

        object BuildPayload(Game game, QuestId questId)
        {
            if (game == null)
                return null;

            return new
            {
                IsCompleted = game.Quests.QuestCompleted(game.Difficulty, questId)
            };
        }
    }
}
