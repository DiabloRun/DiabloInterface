using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader.Models
{
    public class ResistancePenalty
    {
        public static int GetPenaltyByGame(D2Game game)
        {
            // @see https://diablo.gamepedia.com/Resistances_(Diablo_II)
            // todo: read current resistance penalty directly from game
            switch ((GameDifficulty) game.Difficulty)
            {
                case GameDifficulty.Nightmare:
                    return game.LODFlag == 1 ? -40 : -20;
                case GameDifficulty.Hell:
                    return game.LODFlag == 1 ? -100 : -50;
                default:
                    return 0;
            }
        }
    }
}
