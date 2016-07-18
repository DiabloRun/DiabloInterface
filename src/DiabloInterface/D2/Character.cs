using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;

namespace DiabloInterface
{
    public class Character
    {
        const int MIN_RESIST = -100;
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

        public int FasterHitRecovery { get; private set; }
        public int FasterRunWalk { get; private set; }
        public int FasterCastRate { get; private set; }
        public int IncreasedAttackSpeed { get; private set; }

        public int Gold { get; private set; }
        public int GoldStash { get; private set; }

        public int[] CompletedQuestCounts { get; set; } = new int[3] { 0, 0, 0 };

        public short Deaths;

        public int Defense { get; private set; }

        /// <summary>
        /// fill the player data by dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="resistancPenalty"></param>
        public void ParseStats(Dictionary<StatIdentifier, D2Stat> data, Dictionary<StatIdentifier, D2Stat> itemData, int gameDifficulty)
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
            Func<StatIdentifier, int> getItemStat = statID =>
            {
                D2Stat stat;
                // Get the value if if the key exists, else assume zero.
                return itemData.TryGetValue(statID, out stat) ? stat.Value : 0;
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

            FireResist = Clamp(getStat(StatIdentifier.ResistFire) + penalty, MIN_RESIST, maxFire);
            ColdResist = Clamp(getStat(StatIdentifier.ResistCold) + penalty, MIN_RESIST, maxCold);
            LightningResist = Clamp(getStat(StatIdentifier.ResistLightning) + penalty, MIN_RESIST, maxLightning);
            PoisonResist = Clamp(getStat(StatIdentifier.ResistPoison) + penalty, MIN_RESIST, maxPoison);

            FasterHitRecovery = getItemStat(StatIdentifier.FasterHitRecovery);
            FasterRunWalk = getItemStat(StatIdentifier.FasterRunWalk);
            FasterCastRate = getItemStat(StatIdentifier.FasterCastRate);
            IncreasedAttackSpeed = getItemStat(StatIdentifier.IncreasedAttackSpeed);

            Gold = getStat(StatIdentifier.Gold);
            GoldStash = getStat(StatIdentifier.GoldStash);
        }

        static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            else if (value.CompareTo(max) > 0)
                return max;
            else return value;
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
