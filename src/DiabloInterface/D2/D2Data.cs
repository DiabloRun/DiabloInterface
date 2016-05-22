
namespace DiabloInterface
{
    public class D2Data
    {

        public enum BodyLoc
        {
            None = 0,
            Head,
            Neck,
            Torso,
            RightArm,
            LeftArm,
            RightRing,
            LeftRing,
            Belt,
            Feet,
            Gloves,
        };
        
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


        public const int CHAR_STR_IDX = 0;
        public const int CHAR_ENE_IDX = 1;
        public const int CHAR_DEX_IDX = 2;
        public const int CHAR_VIT_IDX = 3;

        public const int CHAR_CURRENT_LIFE_IDX = 6;
        public const int CHAR_MAX_LIFE_IDX = 7;

        public const int CHAR_CURRENT_MANA_IDX = 8;
        public const int CHAR_MAX_MANA_IDX = 9;

        public const int CHAR_CURRENT_STAMINA_IDX = 10;
        public const int CHAR_MAX_STAMINA_IDX = 11;

        public const int CHAR_LVL_IDX = 12;
        public const int CHAR_XP_IDX = 13;
        public const int CHAR_GOLD_BODY_IDX = 14;
        public const int CHAR_GOLD_STASH_IDX = 15;

        public const int CHAR_UNKNOWN1_IDX = 19;
        public const int CHAR_UNKNOWN2_IDX = 20;
        public const int CHAR_UNKNOWN3_IDX = 21;
        public const int CHAR_UNKNOWN4_IDX = 22;
        public const int CHAR_UNKNOWN5_IDX = 23;

        public const int CHAR_UNKNOWN6_IDX = 27;
        public const int CHAR_UNKNOWN7_IDX = 29;
        public const int CHAR_UNKNOWN8_IDX = 30;
        public const int CHAR_DEF_IDX = 31; //def (val + 1/4*dex)

        public const int CHAR_FIRE_RES_IDX = 39;
        public const int CHAR_FIRE_RES_ADD_IDX = 40;
        public const int CHAR_LIGHTNING_RES_IDX = 41;
        public const int CHAR_LIGHTNING_RES_ADD_IDX = 42;
        public const int CHAR_COLD_RES_IDX = 43;
        public const int CHAR_COLD_RES_ADD_IDX = 44;
        public const int CHAR_POISON_RES_IDX = 45;
        public const int CHAR_POISON_RES_ADD_IDX = 46;

        public const int CHAR_UNKNOWN10_IDX = 48;
        public const int CHAR_UNKNOWN11_IDX = 49;
        public const int CHAR_UNKNOWN12_IDX = 50;
        public const int CHAR_UNKNOWN13_IDX = 51;

        public const int CHAR_UNKNOWN14_IDX = 62;
        public const int CHAR_UNKNOWN15_IDX = 67;
        public const int CHAR_UNKNOWN16_IDX = 68;
        public const int CHAR_UNKNOWN17_IDX = 69;

        public const int CHAR_UNKNOWN18_IDX = 71;
        public const int CHAR_UNKNOWN19_IDX = 72;
        public const int CHAR_UNKNOWN20_IDX = 73;
        public const int CHAR_UNKNOWN21_IDX = 74;

        public const int CHAR_UNKNOWN22_IDX = 79;
        public const int CHAR_UNKNOWN23_IDX = 80;
        public const int CHAR_UNKNOWN24_IDX = 83;
        public const int CHAR_UNKNOWN25_IDX = 89;
        public const int CHAR_UNKNOWN26_IDX = 95;
        public const int CHAR_UNKNOWN27_IDX = 99;

        public const int CHAR_UNKNOWN28_IDX = 107;
        public const int CHAR_UNKNOWN29_IDX = 110;
        public const int CHAR_UNKNOWN30_IDX = 128;
        public const int CHAR_UNKNOWN31_IDX = 159;
        public const int CHAR_UNKNOWN32_IDX = 172;
        public const int CHAR_UNKNOWN33_IDX = 194;
        public const int CHAR_UNKNOWN34_IDX = 201;
        public const int CHAR_UNKNOWN35_IDX = 204;


        // see ItemStatCost.txt
        public enum UnitStatIdx
        {
            STRENGTH = 0,
            VITALITY = 1,
            DEXTERITY = 2,
            ENERGY = 3,

            LIFE = 7, // 3 + 1 byte
            MANA = 9, // 3 + 1 byte

            STAMINA = 11, // 3 + 1 byte

            PERC_DEF = 16, // unknown.. ?

            ATTACK_RATING = 19,

            BLOCK_CHANCE = 20, // base block chance + X = chance to block ?

            ONE_HAND_DMG_MIN = 21,
            ONE_HAND_DMG_MAX = 22,
            TWO_HAND_DMG_MIN = 23,
            TWO_HAND_DMG_MAX = 24,

            PERC_MANA_REGEN = 27,
            PERC_STAMINA_REGEN = 28,

            DEF = 31,

            unknown32 = 32, // some pointer, maybe to set info?? set by arcannas sign amulet 

            DMG_REDUCTION = 34,
            MAGIC_DMG_REDUCTION = 35,
            
            FIRE_RES = 39,
            FIRE_RES_ADD = 40,
            LIGHTNING_RES = 41,
            LIGHTNING_RES_ADD = 42,
            COLD_RES = 43,
            COLD_RES_ADD = 44,
            POISON_RES = 45,
            POISON_RES_ADD = 46,

            FIRE_DMG_MIN = 48,
            FIRE_DMG_MAX = 49,
            LIGHTNING_DMG_MIN = 50,
            LIGHTNING_DMG_MAX = 51,

            COLD_DMG_MIN = 54,
            COLD_DMG_MAX = 55,
            unknown56 = 56, // maybe cold duration/strength??  seen values: 25, 50

            // @see http://diablo2.diablowiki.net/Guide:Calculating_Poison_Damage_v1.10,_by_onderduiker
            // 
            // these are set at stuff with poison dmg:
            // examples: 
            // 150 dmg in 5 sek: 308|308|125|1
            //  21 dmg in 4 sek:  54| 54|100|1
            //  75 dmg in 5 sek: 154|154|125|1
            // poison damage:  POISON_DMG_BITRATE_MIN*POISON_DURATION/256 Poison damage over POISON_DURATION/25 seconds
            // and if bitrate differs: POISON_DMG_BITRATE_MIN*POISON_DURATION/256 - POISON_DMG_BITRATE_MAX*POISON_DURATION/256 Poison damage over POISON_DURATION/25 seconds
            POISON_DMG_BITRATE_MIN = 57, // poison bitrate min
            POISON_DMG_BITRATE_MAX = 58, // poison bitrate max
            POISON_DURATION = 59, // duration
            unknown11 = 326, // unknown, but seems always set when there is poison dmg

            LIFE_LEECH = 60,
            MANA_LEECH = 62,

            unknown3 = 67, // set to -5 at big shield (block speed?)
            unknown4 = 68, // set to 10 at cleglaws tooth.. (attack speed class? 10 = normal.. 10 = fast, 20 = very fast, -10 = slow, -10 = normal)

            COUNT = 70, // for example scroll tomes, keys, javelins,..

            DURABILITY_CURRENT = 72,
            DURABILITY_MAX = 73,
            RESPLENISH_LIFE = 74,

            ATTACKER_SELF_DMG = 78,
            PERC_GOLD_BY_MONSTERS = 79,
            PERC_MAGIC_FIND = 80,

            KNOCKBACK = 81,

            SKILLS_CLASS = 83, // + X to CLASS abilities (current class?)

            LIGHT_RADIUS = 89,

            FASTER_ATTACK_SPEED = 93,

            FASTER_RUN_WALK = 96,

            FASTER_HIT_RECOVERY = 99,

            FASTER_BLOCK_RATE = 102,

            unknown104 = 104, // set by arcannas sign amulet 

            FASTER_CAST_RATE = 105,

            unknown7 = 107, // set when item has + to ability (can be set multiple times. seems not to matter what the ability is: examples: + 1 to energyshield, + 3 to fire wall, + 2 to jump)

            PERC_POISON_STRENGTH_REDUCTION = 110,

            unknown20 = 112, // maybe "cause monster to flee" 1 = 25%, 2 = 50%, 4 = 75%, 8 = 100% 

            PERC_DAMAGE_TO_MANA = 114,
            IGNORE_TARGET_DEFENSE = 115, // not sure but likely

            PREVENT_MONSTER_HEAL = 117,
            HALF_FREEZE_DURATION = 118,
            PERC_ATTACK_RATING = 119,

            PERC_DMG_TO_DEMONS = 121,
            ATTACK_RATING_TO_DEMONS = 123,

            SKILLS_FIRE = 126, // +X to all fire skills (not 100% sure)
            SKILLS_ALL = 127, // +X to all skills (not 100% sure)
            ATTACKER_SELF_LIGHTNING_DMG = 128, // for example isenharts shield

            MANA_AFTER_KILL = 138,
            LIFE_AFTER_DEMON_KILL = 139,

            unknown13 = 140, // set at dirtfoot (boots)

            DEATH_STRIKE_CHANCE = 141,

            FIRE_ABSORBTION = 143,

            CANNOT_BE_FROZEN = 153, // unknown
            PERC_STAMINA_DRAIN_REDUCTION = 154,

            THROW_DAMAGE_MIN = 159,
            THROW_DAMAGE_MAX = 160,

            SKILLS_CLASS_SUB = 188, // +1 to fight skills barb / +1 to curses necro

            SOCKET_COUNT = 194,
            
            unknown14 = 201, // X perc chance for ABILITY on hit (seen for charged bolt, meteor,...)

            unknown15 = 204, // charges (for ability on item )  0x0000YYZZ   yy = max charges | zz = current charges

            // per level calculation: (int)(X*0.125*LEVEL) = shown value

            DEF_PER_LEVEL = 214, // (int)(X*0.125*LEVEL) = shown value
            LIFE_PER_LEVEL = 216, // 3+1bit instead 4bits! (int)(X*0.125*LEVEL)
            DMG_PER_LEVEL = 218, //

            ABSORB_FIRE_DMG_PER_LEVel = 235, // (int)(X*0.125*LEVEL) = shown value. set at rising sun amulet.. (maybe absorb fire dmg per level related)

            MAX_STAMINA_PER_LEVEL = 242, // (int)(X*0.125*LEVEL) = shown value. set at sandstorm treck boots.. (maybe max stamina per level related)

            SELF_REPAIR = 252, // self repair ?   3= 1 in 33 sek, 5 = 1 in 20 sek >>> 100/X = seconds in which 1 is repaired
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
