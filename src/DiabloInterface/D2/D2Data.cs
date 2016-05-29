
namespace DiabloInterface
{
    public class D2Data
    {
        public enum Quest
        {
            // WARRIF = 0,
            A1Q1 = 2, // Den of Evil
            A1Q2 = 4, // Sisters' Burial Grounds
            A1Q3 = 6, // Tools of the Trade
            A1Q4 = 8, // The Search for Cain
            A1Q5 = 10, // The Forgotten Tower
            A1Q6 = 12, // Sisters to the Slaughter

            A2Q1 = 18, // Radament's Lair
            A2Q2 = 20, // The Horadric Staff
            A2Q3 = 22, // Tainted Sun
            A2Q4 = 24, // Arcane Sanctuary
            A2Q5 = 26, // The Summoner
            A2Q6 = 28, // The Seven Tombs

            A3Q1 = 34, // Lam Esen's Tome
            A3Q2 = 36, // Khalim's Will
            A3Q3 = 38, // Blade of the Old Religion
            A3Q4 = 40, // The Golden Bird
            A3Q5 = 42, // The Blackened Temple
            A3Q6 = 44, // The Guardian

            A4Q1 = 50, // The Fallen Angel
            A4Q2 = 52, // Terror's End
            A4Q3 = 54, // Hell's Forge

            A5Q1 = 70, // Siege on Harrogath
            A5Q2 = 72, // Rescue on Mount Arreat
            A5Q3 = 74, // Prison of Ice
            A5Q4 = 76, // Betrayal of Harrogath
            A5Q5 = 78, // Rite of Passage
            A5Q6 = 80, // Eve of Destruction
        }


        public enum Mode
        {
            DEATH = 0,
            NEUTRAL,
            WALK,
            RUN,
            GET_HIT,
            TOWN_NEUTRAL,
            TOWN_WALK,
            ATTACK_1,
            ATTACK_2,
            BLOCK,
            CAST,
            THROW,
            KICK,
            SKILL_1,
            SKILL_2,
            SKILL_3,
            SKILL_4,
            DEAD,
            SEQUENCE,
            KNOCK_BACK,
        };

        public enum Penalty
        {
            NORMAL = 0,
            NIGHTMARE = -40,
            HELL = -100,
        }

        public enum ItemId
        {
            GIBDINN = 87,
            WIRTS_LEG = 88,
            HORADRIC_MALUS = 89,
            HELLFORGE_HAMMER = 90,
            HORADRIC_CUBE = 549,
            HORADRIC_SHAFT = 92,
            HORADRIC_STAFF = 91,
            HORADRIC_AMULET = 521,
            KHALIM_EYE = 553,
            KHALIM_HEART = 554,
            KHALIM_BRAIN = 555,
            KHALIM_FLAIL = 173,
            KHALIM_WILL = 174,
            INIFUSS_SCROLL = 524,
            POTION_OF_LIFE = 545,
            JADE_FIGURINE = 546,
            GOLDEN_BIRD = 547,
            LAM_ESEN_TOME = 548,
            MEPHISTOS_SOULSTONE = 551,
            BOOK_OF_SKILL = 552,
        }
    }
}
