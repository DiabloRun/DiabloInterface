
namespace DiabloInterface
{
    public class D2Data
    {

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

        // eClass values of items relevant for quests
        public enum QuestItemId
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
