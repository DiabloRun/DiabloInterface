using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
