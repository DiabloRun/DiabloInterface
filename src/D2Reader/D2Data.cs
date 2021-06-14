namespace Zutatensuppe.D2Reader
{
    public class D2Data
    {
        // The different values mean different things for different unit types,
        // so there are PlayerMode, MonsterMode, and maybe more in the future
        public enum PlayerMode
        {
            DEATH = 0, // 0x00
            NEUTRAL, // 0x01
            WALK, // 0x02
            RUN, // 0x03
            GET_HIT, // 0x04
            TOWN_NEUTRAL, // 0x05
            TOWN_WALK, // 0x06
            ATTACK_1, // 0x07
            ATTACK_2, // 0x08
            BLOCK, // 0x09
            CAST, // 0x0A
            THROW, // 0x0B
            KICK, // 0x0C
            SKILL_1, // 0x0D
            SKILL_2, // 0x0E
            SKILL_3, // 0x0F
            SKILL_4, // 0x10
            DEAD, // 0x11
            SEQUENCE, // 0x12
            KNOCK_BACK, // 0x13
        };

        public enum MonsterMode
        {
            DEATH = 0x0, // 0
            // [...]
            DEAD  = 0xC, // 12
            // [...]
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

        public enum Skill
        {
            UNKNOWN, // 0x00
            KICK, // 0x01
            THROW, // 0x02
            UNSUMMON, // 0x03
            LEFT_HAND_THROW, // 0x04
            LEFT_HAND_SWING, // 0x05

            FIREBOLT = 0x24, // 0x24

            RAISE_SKELETON = 0x46, // 0x46

            SCROLL_IDENT = 0xd9, // 0xd9
            TOME_IDENT, // 0xda
            SCROLL_TP, // 0xdb
            TOME_TP, // 0xdc

            // hacky PD2 only. TODO: separate this better
            PD2_MOVE_ONLY = 0x18a,
        }

        public enum Area
        {
            ROGUE_ENCAMPMENT = 1,
            LUT_GHOLEIN = 40,
            KURAST_DOCKTOWN = 75,
            PANDEMONIUM_FORTRESS = 103,
            HARROGATH = 109,
        }

        public enum PetClass
        {
            SUMMONED = 4, // seen for necro skeleton
            HIRELING = 7, 
        }

        public enum UnitClassFlag: byte
        {
            IS_SELECTABLE = 8, // if unit is selectable (for attack).
                               // flag is not set for example for chickens, hydras or npcs in town
        }
    }
}
