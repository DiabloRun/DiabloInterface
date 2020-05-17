using System.Collections.Generic;
using System.Linq;

using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.D2Reader
{
    public static class QuestFactory
    {
        static readonly IReadOnlyDictionary<QuestId, QuestDetails> Quests = new Dictionary<QuestId, QuestDetails>()
        {
            { QuestId.Andariel, new QuestDetails { Id=QuestId.Andariel, Act=1, ActOrder=6, Name="Sisters to the Slaughter", CommonName="Andariel", IsBossQuest=true, } },
            { QuestId.Duriel, new QuestDetails { Id=QuestId.Duriel, Act=2, ActOrder=6, Name="The Seven Tombs", CommonName="Duriel", IsBossQuest=true, CompletionBitMask=(1 << 5), } },
            { QuestId.Mephisto, new QuestDetails { Id=QuestId.Mephisto, Act=3, ActOrder=6, Name="The Guardian", CommonName="Mephisto", IsBossQuest=true, } },
            { QuestId.Diablo, new QuestDetails { Id=QuestId.Diablo, Act=4, ActOrder=3, Name="Terror's End", CommonName="Diablo", IsBossQuest=true, } },
            { QuestId.Baal, new QuestDetails { Id=QuestId.Baal, Act=5, ActOrder=6, Name="Eve of Destruction", CommonName="Baal", IsBossQuest=true, } },

            { QuestId.DenOfEvil, new QuestDetails { Id=QuestId.DenOfEvil, Act=1, ActOrder=1, Name="Den of Evil", CommonName="Den of Evil", } },
            { QuestId.SistersBurialGrounds, new QuestDetails { Id=QuestId.SistersBurialGrounds, Act=1, ActOrder=2, Name="Sisters' Burial Grounds", CommonName="Blood Raven", } },
            { QuestId.ToolsOfTheTrade, new QuestDetails { Id=QuestId.ToolsOfTheTrade, Act=1, ActOrder=5, Name="Tools of the Trade", CommonName="Charsie Imbue", } },
            { QuestId.TheSearchForCain, new QuestDetails { Id=QuestId.TheSearchForCain, Act=1, ActOrder=3, Name="The Search for Cain", CommonName="Cain", } },
            { QuestId.TheForgottenTower, new QuestDetails { Id=QuestId.TheForgottenTower, Act=1, ActOrder=4, Name="The Forgotten Tower", CommonName="Countess", } },

            { QuestId.RadamentsLair, new QuestDetails { Id=QuestId.RadamentsLair, Act=2, ActOrder=1, Name="Radament's Lair", CommonName="Radament", } },
            { QuestId.TheHoradricStaff, new QuestDetails { Id=QuestId.TheHoradricStaff, Act=2, ActOrder=2, Name="The Horadric Staff", CommonName="Horadric Staff", } },
            { QuestId.TaintedSun, new QuestDetails { Id=QuestId.TaintedSun, Act=2, ActOrder=3, Name="Tainted Sun", CommonName="Claw Viper Temple", } },
            { QuestId.ArcaneSanctuary, new QuestDetails { Id=QuestId.ArcaneSanctuary, Act=2, ActOrder=4, Name="Arcane Sanctuary", CommonName="Arcane Sancuary", } },
            { QuestId.TheSummoner, new QuestDetails { Id=QuestId.TheSummoner, Act=2, ActOrder=5, Name="The Summoner", CommonName="Horazon", } },

            { QuestId.LamEsensTome, new QuestDetails { Id=QuestId.LamEsensTome, Act=3, ActOrder=4, Name="Lam Esen's Tome", CommonName="Battlemaid Sarina", } },
            { QuestId.KhalimsWill, new QuestDetails { Id=QuestId.KhalimsWill, Act=3, ActOrder=3, Name="Khalim's Will", CommonName="Khalim's Will", } },
            { QuestId.BladeOfTheOldReligion, new QuestDetails { Id=QuestId.BladeOfTheOldReligion, Act=3, ActOrder=2, Name="Blade of the Old Religion", CommonName="Gibdinn", } },
            { QuestId.TheGoldenBird, new QuestDetails { Id=QuestId.TheGoldenBird, Act=3, ActOrder=1, Name="The Golden Bird", CommonName="Alkor Quest", } },
            { QuestId.TheBlackenedTemple, new QuestDetails { Id=QuestId.TheBlackenedTemple, Act=3, ActOrder=5, Name="The Blackened Temple", CommonName="Travincal Council", } },

            { QuestId.TheFallenAngel, new QuestDetails { Id=QuestId.TheFallenAngel, Act=4, ActOrder=1, Name="The Fallen Angel", CommonName="Izual", } },
            { QuestId.HellsForge, new QuestDetails { Id=QuestId.HellsForge, Act=4, ActOrder=2, Name="Hell's Forge", CommonName="Hell Forge", } },

            { QuestId.SiegeOnHarrogath, new QuestDetails { Id=QuestId.SiegeOnHarrogath, Act=5, ActOrder=1, Name="Siege on Harrogath", CommonName="Shenk", } },
            { QuestId.RescueOnMountArreat, new QuestDetails { Id=QuestId.RescueOnMountArreat, Act=5, ActOrder=2, Name="Rescue on Mount Arreat", CommonName="Rescue Barbs", } },
            { QuestId.PrisonOfIce, new QuestDetails { Id=QuestId.PrisonOfIce, Act=5, ActOrder=3, Name="Prison of Ice", CommonName="Rescue Anya", } },
            { QuestId.BetrayalOfHarrogath, new QuestDetails { Id=QuestId.BetrayalOfHarrogath, Act=5, ActOrder=4, Name="Betrayal of Harrogath", CommonName="Kill Nihlathak", } },
            { QuestId.RiteOfPassage, new QuestDetails { Id=QuestId.RiteOfPassage, Act=5, ActOrder=5, Name="Rite of Passage", CommonName="Ancients" } },

            { QuestId.CowKing, new QuestDetails { Id=QuestId.CowKing, Act=0, ActOrder=0, Name="Cow King", CommonName="Cow King", CompletionBitMask=(1 << 10), FullCompletionBitMask=(1 << 10), } },
        };

        static readonly IReadOnlyDictionary<int, QuestId> QuestBufferIndexLookup = new Dictionary<int, QuestId>()
        {
            { 1, QuestId.DenOfEvil },
            { 2, QuestId.SistersBurialGrounds },
            { 3, QuestId.ToolsOfTheTrade },
            { 4, QuestId.TheSearchForCain },
            { 5, QuestId.TheForgottenTower },
            { 6, QuestId.Andariel },

            { 9, QuestId.RadamentsLair },
            { 10, QuestId.TheHoradricStaff },
            { 11, QuestId.TaintedSun },
            { 12, QuestId.ArcaneSanctuary },
            { 13, QuestId.TheSummoner },
            { 14, QuestId.Duriel },

            { 17, QuestId.LamEsensTome },
            { 18, QuestId.KhalimsWill },
            { 19, QuestId.BladeOfTheOldReligion },
            { 20, QuestId.TheGoldenBird },
            { 21, QuestId.TheBlackenedTemple },
            { 22, QuestId.Mephisto },

            { 25, QuestId.TheFallenAngel },
            { 26, QuestId.Diablo },
            { 27, QuestId.HellsForge },

            { 35, QuestId.SiegeOnHarrogath },
            { 36, QuestId.RescueOnMountArreat },
            { 37, QuestId.PrisonOfIce },
            { 38, QuestId.BetrayalOfHarrogath },
            { 39, QuestId.RiteOfPassage },
            { 40, QuestId.Baal },
        };

        public static Quest Create(QuestId id, ushort completionBits) =>
            Quests.TryGetValue(id, out var details) ? new Quest(details, completionBits) : null;

        public static Quest CreateByActAndOrder(int act, int quest)
        {
            return (from item in Quests
                where item.Value.Act == act && item.Value.ActOrder == quest
                select new Quest(item.Value, 0)).FirstOrDefault();
        }

        public static Quest CreateFromBufferIndex(int bufferIndex, ushort completionBits) =>
            QuestBufferIndexLookup.TryGetValue(bufferIndex, out var questId)
                ? Create(questId, completionBits) : null;
    }
}
