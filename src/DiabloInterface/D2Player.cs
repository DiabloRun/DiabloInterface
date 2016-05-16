using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface
{
    public class D2Player
    {
        public string name;

        public D2Data.Mode mode;

        public bool isDead = false;

        public bool newlyStarted = false; // if true, the char has been created since tool is running. (eligable for autosplits)

        public short lvl;
        public short strength;
        public short dexterity;
        public short vitality;
        public short energy;

        public short fireRes;
        public short coldRes;
        public short lightningRes;
        public short poisonRes;

        // x to fire/cold/light/poison res items give these stats:
        public short fireResAdditional;
        public short coldResAdditional;
        public short lightningResAdditional;
        public short poisonResAdditional;

        // ready to use resistances
        public short calculatedFireRes;
        public short calculatedColdRes;
        public short calculatedLightningRes;
        public short calculatedPoisonRes;

        public int goldBody;
        public int goldStash;

        public short deaths;

        public int xp;

        public short defense;

        // note: is float/int mix in d2
        //public int currentLife;
        //public int currentMana;
        //public int currentStamina;

        //public int maxLife;
        //public int maxMana;
        //public int maxStamina;

        /// <summary>
        /// fill the player data by dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="resistancPenalty"></param>
        public void fill(Dictionary<int, int> dict, D2Data.Penalty resistancPenalty)
        {
            lvl = (short)(dict.ContainsKey(D2Data.CHAR_LVL_IDX) ? dict[D2Data.CHAR_LVL_IDX] : 0);
            
            xp = (dict.ContainsKey(D2Data.CHAR_XP_IDX) ? dict[D2Data.CHAR_XP_IDX] : 0);

            defense = (short)(dict.ContainsKey(D2Data.CHAR_DEF_IDX) ? dict[D2Data.CHAR_DEF_IDX] : 0);

            fireRes = (short)(dict.ContainsKey(D2Data.CHAR_FIRE_RES_IDX) ? dict[D2Data.CHAR_FIRE_RES_IDX] : 0);
            coldRes = (short)(dict.ContainsKey(D2Data.CHAR_COLD_RES_IDX) ? dict[D2Data.CHAR_COLD_RES_IDX] : 0);
            lightningRes = (short)(dict.ContainsKey(D2Data.CHAR_LIGHTNING_RES_IDX) ? dict[D2Data.CHAR_LIGHTNING_RES_IDX] : 0);
            poisonRes = (short)(dict.ContainsKey(D2Data.CHAR_POISON_RES_IDX) ? dict[D2Data.CHAR_POISON_RES_IDX] : 0);

            strength = (short)(dict.ContainsKey(D2Data.CHAR_STR_IDX) ? dict[D2Data.CHAR_STR_IDX] : 0);
            dexterity = (short)(dict.ContainsKey(D2Data.CHAR_DEX_IDX) ? dict[D2Data.CHAR_DEX_IDX] : 0);
            vitality = (short)(dict.ContainsKey(D2Data.CHAR_VIT_IDX) ? dict[D2Data.CHAR_VIT_IDX] : 0);
            energy = (short)(dict.ContainsKey(D2Data.CHAR_ENE_IDX) ? dict[D2Data.CHAR_ENE_IDX] : 0);

            fireResAdditional = (short)(dict.ContainsKey(D2Data.CHAR_FIRE_RES_ADD_IDX) ? dict[D2Data.CHAR_FIRE_RES_ADD_IDX] : 0);
            coldResAdditional = (short)(dict.ContainsKey(D2Data.CHAR_COLD_RES_ADD_IDX) ? dict[D2Data.CHAR_COLD_RES_ADD_IDX] : 0);
            lightningResAdditional = (short)(dict.ContainsKey(D2Data.CHAR_LIGHTNING_RES_ADD_IDX) ? dict[D2Data.CHAR_LIGHTNING_RES_ADD_IDX] : 0);
            poisonResAdditional = (short)(dict.ContainsKey(D2Data.CHAR_POISON_RES_ADD_IDX) ? dict[D2Data.CHAR_POISON_RES_ADD_IDX] : 0);

            calculatedFireRes = (short)Math.Min(fireRes + (short)resistancPenalty, 75 + fireResAdditional);
            calculatedColdRes = (short)Math.Min(coldRes + (short)resistancPenalty, 75 + coldResAdditional);
            calculatedLightningRes = (short)Math.Min(lightningRes + (short)resistancPenalty, 75 + lightningResAdditional);
            calculatedPoisonRes = (short)Math.Min(poisonRes + (short)resistancPenalty, 75 + poisonResAdditional);

            goldBody = (dict.ContainsKey(D2Data.CHAR_GOLD_BODY_IDX) ? dict[D2Data.CHAR_GOLD_BODY_IDX] : 0);
            goldStash = (dict.ContainsKey(D2Data.CHAR_GOLD_STASH_IDX) ? dict[D2Data.CHAR_GOLD_STASH_IDX] : 0);
        }

        /// <summary>
        /// Handle death of player. Deathcount up, if new death.
        /// </summary>
        public void handleDeath()
        {
            if (lvl > 0 && (mode == D2Data.Mode.DEAD || mode == D2Data.Mode.DEATH))
            {
                if (!isDead)
                {
                    isDead = true;
                    deaths++;
                }
            }
            else
            {
                isDead = false;
            }
        }

    }
}
