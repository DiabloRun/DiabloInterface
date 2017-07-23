namespace Zutatensuppe.D2Reader.Models
{

    public enum ResistancePenaltyEnum
    {
        Normal = 0,
        Nightmare = -40,
        Hell = -100,
    }

    class ResistancePenalty
    {

        public static ResistancePenaltyEnum GetPenaltyByGameDifficulty(GameDifficulty gameDifficulty)
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Nightmare:
                    return ResistancePenaltyEnum.Nightmare;
                case GameDifficulty.Hell:
                    return ResistancePenaltyEnum.Hell;
                default:
                    return ResistancePenaltyEnum.Normal;
            }
        }
    }
}
