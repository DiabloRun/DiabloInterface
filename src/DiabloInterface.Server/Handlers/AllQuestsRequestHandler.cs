namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;

    public class AllQuestsRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public AllQuestsRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        }

        public Response HandleRequest(Request request, IList<string> arguments)
        {
            return new Response()
            {
                Status = ResponseStatus.Success,
                Payload = new
                {
                    Normal = dataReader.Quests.ByDifficulty(GameDifficulty.Normal),
                    Nightmare = dataReader.Quests.ByDifficulty(GameDifficulty.Nightmare),
                    Hell = dataReader.Quests.ByDifficulty(GameDifficulty.Hell),
                }
            };
        }
    }
}
