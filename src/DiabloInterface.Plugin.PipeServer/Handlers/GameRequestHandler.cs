using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    public class GameRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public GameRequestHandler(D2DataReader dataReader)
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

            return new
            {
                game.Area,
                game.Difficulty,
                game.PlayersX,
                game.GameCount,
                game.CharCount
            };
        }
    }
}
