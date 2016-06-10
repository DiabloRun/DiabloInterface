using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;

namespace DiabloInterface
{
    public class Character
    {
        const int BASE_MAX_RESIST = 75;

        public string name;

        public D2Data.Mode Mode { get; private set; }

        public bool IsDead { get; private set; }

        public int Level { get; private set; }
        public int Experience { get; private set; }

        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Vitality { get; private set; }
        public int Energy { get; private set; }

        public int FireResist { get; private set; }
        public int ColdResist { get; private set; }
        public int LightningResist { get; private set; }
        public int PoisonResist { get; private set; }

        public int Gold { get; private set; }
        public int GoldStash { get; private set; }

        public short Deaths;

        public int Defense { get; private set; }

        /// <summary>
        /// fill the player data by dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="resistancPenalty"></param>
        public void ParseStats(Dictionary<StatIdentifier, D2Stat> data, int gameDifficulty)
        {
            // Don't update stats while dead.
            if (IsDead) return;

            int penalty = GetResistancePenalty(gameDifficulty);
            Func<StatIdentifier, int> getStat = statID =>
            {
                D2Stat stat;
                // Get the value if if the key exists, else assume zero.
                return data.TryGetValue(statID, out stat) ? stat.Value : 0;
            };

            Level = getStat(StatIdentifier.Level);
            Experience = getStat(StatIdentifier.Experience);

            Strength = getStat(StatIdentifier.Strength);
            Dexterity = getStat(StatIdentifier.Dexterity);
            Vitality = getStat(StatIdentifier.Vitality);
            Energy = getStat(StatIdentifier.Energy);

            Defense = getStat(StatIdentifier.Defense);

            int maxFire = BASE_MAX_RESIST + getStat(StatIdentifier.ResistFireMax);
            int maxCold = BASE_MAX_RESIST + getStat(StatIdentifier.ResistColdMax);
            int maxLightning = BASE_MAX_RESIST + getStat(StatIdentifier.ResistLightningMax);
            int maxPoison = BASE_MAX_RESIST + getStat(StatIdentifier.ResistPoisonMax);

            FireResist = Math.Min(getStat(StatIdentifier.ResistFire) + penalty, maxFire);
            ColdResist = Math.Min(getStat(StatIdentifier.ResistCold) + penalty, maxCold);
            LightningResist = Math.Min(getStat(StatIdentifier.ResistLightning) + penalty, maxLightning);
            PoisonResist = Math.Min(getStat(StatIdentifier.ResistPoison) + penalty, maxPoison);

            Gold = getStat(StatIdentifier.Gold);
            GoldStash = getStat(StatIdentifier.GoldStash);
        }

        private int GetResistancePenalty(int gameDifficulty)
        {
            switch (gameDifficulty)
            {
                case 1: return -40;
                case 2: return -100;
                default: return 0;
            }
        }

        public void UpdateMode(D2Data.Mode mode)
        {
            Mode = mode;

            // Handle player death.
            if (Level > 0 && (Mode == D2Data.Mode.DEAD || Mode == D2Data.Mode.DEATH))
            {
                if (!IsDead)
                {
                    IsDead = true;
                    Deaths++;
                }
            }
            else
            {
                IsDead = false;
            }
        }

    }
}
