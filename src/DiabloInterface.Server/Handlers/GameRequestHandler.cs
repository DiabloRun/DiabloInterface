using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class GameRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public GameRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
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
