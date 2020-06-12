using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Handlers
{
    public class CharacterRequestHandler : IRequestHandler
    {
        readonly D2DataReader dataReader;

        public CharacterRequestHandler(D2DataReader dataReader)
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
            if (game == null || game.Character == null)
                return null;

            var character = game.Character;

            return new
            {
                character.Name,
                character.Guid,
                character.Created,
                character.CharClass,
                character.IsHardcore,
                character.IsExpansion,
                character.IsDead,
                character.Deaths,
                character.Level,
                character.Experience,
                Stats = new
                {
                    character.Strength,
                    character.Dexterity,
                    character.Vitality,
                    character.Energy
                },
                Resistances = new
                {
                    Fire = character.FireResist,
                    Cold = character.ColdResist,
                    Lightning = character.LightningResist,
                    Poison = character.PoisonResist
                },
                character.Gold,
                character.GoldStash,
                character.FasterCastRate,
                character.FasterHitRecovery,
                character.FasterRunWalk,
                character.IncreasedAttackSpeed,
                character.MagicFind,
                character.MonsterGold,
                character.AttackerSelfDamage,
            };
        }
    }
}
