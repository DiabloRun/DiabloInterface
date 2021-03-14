using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    public class HirelingRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public HirelingRequestHandler(D2DataReader dataReader)
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
            if (game == null || game.Hireling == null)
                return null;

            var hireling = game.Hireling;

            return new
            {
                hireling.Name,
                hireling.Class,
                hireling.Level,
                hireling.Experience,
                Stats = new
                {
                    hireling.Strength,
                    hireling.Dexterity,
                },
                Resistances = new
                {
                    Fire = hireling.FireResist,
                    Cold = hireling.ColdResist,
                    Lightning = hireling.LightningResist,
                    Poison = hireling.PoisonResist,
                },
                hireling.Items,
                hireling.Skills,
            };
        }
    }
}
