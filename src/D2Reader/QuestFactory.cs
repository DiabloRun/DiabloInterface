using System.Collections.Generic;
using System.Linq;

using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.D2Reader
{
    public static class QuestFactory
    {
        static readonly IReadOnlyDictionary<QuestId, QuestDetails> Quests = new Dictionary<QuestId, QuestDetails>()
        {
            { QuestId.Andariel, new QuestDetails {
                Id                    = QuestId.Andariel,
                Act                   = 1,
                ActOrder              = 6,
                Name                  = "Sisters to the Slaughter",
                CommonName            = "Andariel",
                IsBossQuest           = true,
                BufferIndex           = 6,
            } },
            { QuestId.Duriel, new QuestDetails {
                Id                    = QuestId.Duriel,
                Act                   = 2,
                ActOrder              = 6,
                Name                  = "The Seven Tombs",
                CommonName            = "Duriel",
                IsBossQuest           = true,
                BufferIndex           = 14,
                CompletionBitMask     = (1 << 5),
            } },
            { QuestId.Mephisto, new QuestDetails {
                Id                    = QuestId.Mephisto,
                Act                   = 3,
                ActOrder              = 6,
                Name                  = "The Guardian",
                CommonName            = "Mephisto",
                IsBossQuest           = true,
                BufferIndex           = 22,
            } },
            { QuestId.Diablo, new QuestDetails {
                Id                    = QuestId.Diablo,
                Act                   = 4,
                ActOrder              = 3,
                Name                  = "Terror's End",
                CommonName            = "Diablo",
                IsBossQuest           = true,
                BufferIndex           = 26,
            } },
            { QuestId.Baal, new QuestDetails {
                Id                    = QuestId.Baal,
                Act                   = 5,
                ActOrder              = 6,
                Name                  = "Eve of Destruction",
                CommonName            = "Baal",
                IsBossQuest           = true,
                BufferIndex           = 40,
            } },

            { QuestId.DenOfEvil, new QuestDetails {
                Id                    = QuestId.DenOfEvil,
                Act                   = 1,
                ActOrder              = 1,
                Name                  = "Den of Evil",
                CommonName            = "Den of Evil",
                BufferIndex           = 1,
            } },
            { QuestId.SistersBurialGrounds, new QuestDetails {
                Id                    = QuestId.SistersBurialGrounds,
                Act                   = 1,
                ActOrder              = 2,
                Name                  = "Sisters' Burial Grounds",
                CommonName            = "Blood Raven",
                BufferIndex           = 2,
            } },
            { QuestId.ToolsOfTheTrade, new QuestDetails {
                Id                    = QuestId.ToolsOfTheTrade,
                Act                   = 1,
                ActOrder              = 5,
                Name                  = "Tools of the Trade",
                CommonName            = "Charsie Imbue",
                BufferIndex           = 3,
            } },
            { QuestId.TheSearchForCain, new QuestDetails {
                Id                    = QuestId.TheSearchForCain,
                Act                   = 1,
                ActOrder              = 3,
                Name                  = "The Search for Cain",
                CommonName            = "Cain",
                BufferIndex           = 4,
            } },
            { QuestId.TheForgottenTower, new QuestDetails {
                Id                    = QuestId.TheForgottenTower,
                Act                   = 1,
                ActOrder              = 4,
                Name                  = "The Forgotten Tower",
                CommonName            = "Countess",
                BufferIndex           = 5,
            } },

            { QuestId.RadamentsLair, new QuestDetails {
                Id                    = QuestId.RadamentsLair,
                Act                   = 2,
                ActOrder              = 1,
                Name                  = "Radament's Lair",
                CommonName            = "Radament",
                BufferIndex           = 9,
            } },
            { QuestId.TheHoradricStaff, new QuestDetails {
                Id                    = QuestId.TheHoradricStaff,
                Act                   = 2,
                ActOrder              = 2,
                Name                  = "The Horadric Staff",
                CommonName            = "Horadric Staff",
                BufferIndex           = 10,
            } },
            { QuestId.TaintedSun, new QuestDetails {
                Id                    = QuestId.TaintedSun,
                Act                   = 2,
                ActOrder              = 3,
                Name                  = "Tainted Sun",
                CommonName            = "Claw Viper Temple",
                BufferIndex           = 11,
            } },
            { QuestId.ArcaneSanctuary, new QuestDetails {
                Id                    = QuestId.ArcaneSanctuary,
                Act                   = 2,
                ActOrder              = 4,
                Name                  = "Arcane Sanctuary",
                CommonName            = "Arcane Sancuary",
                BufferIndex           = 12,
            } },
            { QuestId.TheSummoner, new QuestDetails {
                Id                    = QuestId.TheSummoner,
                Act                   = 2,
                ActOrder              = 5,
                Name                  = "The Summoner",
                CommonName            = "Horazon",
                BufferIndex           = 13,
            } },

            { QuestId.LamEsensTome, new QuestDetails {
                Id                    = QuestId.LamEsensTome,
                Act                   = 3,
                ActOrder              = 4,
                Name                  = "Lam Esen's Tome",
                CommonName            = "Battlemaid Sarina",
                BufferIndex           = 17,
            } },
            { QuestId.KhalimsWill, new QuestDetails {
                Id                    = QuestId.KhalimsWill,
                Act                   = 3,
                ActOrder              = 3,
                Name                  = "Khalim's Will",
                CommonName            = "Khalim's Will",
                BufferIndex           = 18,
            } },
            { QuestId.BladeOfTheOldReligion, new QuestDetails {
                Id                    = QuestId.BladeOfTheOldReligion,
                Act                   = 3,
                ActOrder              = 2,
                Name                  = "Blade of the Old Religion",
                CommonName            = "Gibdinn",
                BufferIndex           = 19,
            } },
            { QuestId.TheGoldenBird, new QuestDetails {
                Id                    = QuestId.TheGoldenBird,
                Act                   = 3,
                ActOrder              = 1,
                Name                  = "The Golden Bird",
                CommonName            = "Alkor Quest",
                BufferIndex           = 20,
            } },
            { QuestId.TheBlackenedTemple, new QuestDetails {
                Id                    = QuestId.TheBlackenedTemple,
                Act                   = 3,
                ActOrder              = 5,
                Name                  = "The Blackened Temple",
                CommonName            = "Travincal Council",
                BufferIndex           = 21,
            } },

            { QuestId.TheFallenAngel, new QuestDetails {
                Id                    = QuestId.TheFallenAngel,
                Act                   = 4,
                ActOrder              = 1,
                Name                  = "The Fallen Angel",
                CommonName            = "Izual",
                BufferIndex           = 25,
            } },
            { QuestId.HellsForge, new QuestDetails {
                Id                    = QuestId.HellsForge,
                Act                   = 4,
                ActOrder              = 2,
                Name                  = "Hell's Forge",
                CommonName            = "Hell Forge",
                BufferIndex           = 27,
            } },

            { QuestId.SiegeOnHarrogath, new QuestDetails {
                Id                    = QuestId.SiegeOnHarrogath,
                Act                   = 5,
                ActOrder              = 1,
                Name                  = "Siege on Harrogath",
                CommonName            = "Shenk",
                BufferIndex           = 35,
            } },
            { QuestId.RescueOnMountArreat, new QuestDetails {
                Id                    = QuestId.RescueOnMountArreat,
                Act                   = 5,
                ActOrder              = 2,
                Name                  = "Rescue on Mount Arreat",
                CommonName            = "Rescue Barbs",
                BufferIndex           = 36,
            } },
            { QuestId.PrisonOfIce, new QuestDetails {
                Id                    = QuestId.PrisonOfIce,
                Act                   = 5,
                ActOrder              = 3,
                Name                  = "Prison of Ice",
                CommonName            = "Rescue Anya",
                BufferIndex           = 37,
            } },
            { QuestId.BetrayalOfHarrogath, new QuestDetails {
                Id                    = QuestId.BetrayalOfHarrogath,
                Act                   = 5,
                ActOrder              = 4,
                Name                  = "Betrayal of Harrogath",
                CommonName            = "Kill Nihlathak",
                BufferIndex           = 38,
            } },
            { QuestId.RiteOfPassage, new QuestDetails {
                Id                    = QuestId.RiteOfPassage,
                Act                   = 5,
                ActOrder              = 5,
                Name                  = "Rite of Passage",
                CommonName            = "Ancients",
                BufferIndex           = 39,
            } },

            { QuestId.CowKing, new QuestDetails {
                Id                    = QuestId.CowKing,
                Act                   = 0,
                ActOrder              = 0,
                Name                  = "Cow King",
                CommonName            = "Cow King",
                CompletionBitMask     = (1 << 10),
                FullCompletionBitMask = (1 << 10),
                BufferIndex           = 4,
            } },
        };

        public static Quest Create(QuestId id, ushort completionBits) =>
            Quests.TryGetValue(id, out var details) ? new Quest(details, completionBits) : null;

        public static Quest CreateByActAndOrder(int act, int quest)
        {
            return (from item in Quests
                where item.Value.Act == act && item.Value.ActOrder == quest
                select new Quest(item.Value, 0)).FirstOrDefault();
        }

        public static List<Quest> CreateListFromBuffer(IEnumerable<ushort> questBuffer)
        {
            List<Quest> quests = new List<Quest>();
            foreach (var pair in Quests)
            {
                var bits = questBuffer.ElementAtOrDefault(pair.Value.BufferIndex);
                quests.Add(Create(pair.Key, bits));
            }
            return quests;
        }
    }
}
