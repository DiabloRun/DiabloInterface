using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;

namespace Zutatensuppe.DiabloInterface.Server.Handlers
{
    public class CharacterRequestHandler : IRequestHandler
    {
        const string InvalidMessage = "Character request handler must accept one argument set to either 'current' or 'active'";

        D2DataReader dataReader;

        public CharacterRequestHandler(D2DataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
        {
            if (arguments.Count == 0)
                throw new RequestHandlerInvalidException(InvalidMessage);

            object payload = null;
            switch (arguments[0].ToLowerInvariant())
            {
                case "active":
                    payload = BuildPayload(dataReader.ActiveCharacter, dataReader.ActiveCharacterTimestamp);
                    break;

                case "current":
                    payload = BuildPayload(dataReader.CurrentCharacter, DateTime.Now);
                    break;

                default:
                    throw new RequestHandlerInvalidException(InvalidMessage);
            }

            return new QueryResponse()
            {
                Payload = payload,
                Status = QueryStatus.Success,
            };
        }

        object BuildPayload(Character character, DateTime timestamp)
        {
            if (character == null)
                return null;

            Console.WriteLine("Character name: ", character.Name);

            return new
            {
                IsCurrentCharacter = character == dataReader.CurrentCharacter,
                Name = character.Name,
                Created = timestamp,
                Level = character.Level,
                Experience = character.Experience,
                Stats = new
                {
                    Strength = character.Strength,
                    Dexterity = character.Dexterity,
                    Vitality = character.Vitality,
                    Energy = character.Energy
                },
                Resistances = new
                {
                    Fire = character.FireResist,
                    Cold = character.ColdResist,
                    Lightning = character.LightningResist,
                    Poison = character.PoisonResist
                },
                Gold = character.Gold,
                GoldStash = character.GoldStash
            };
        }
    }
}
