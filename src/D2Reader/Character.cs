using Zutatensuppe.D2Reader.Struct.Stat;
using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.D2Reader.Struct.Stat;

    public class Character
    {
        const int MIN_RESIST = -100;
        const int BASE_MAX_RESIST = 75;

        public string Name { get; set; }

        public CharacterClass CharClass { get; private set; }

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

        public int VelocityPercent { get; private set; }
        public int AttackRate { get; private set; }

        public int Gold { get; private set; }
        public int GoldStash { get; private set; }

        public short Deaths;

        public int Defense { get; private set; }

        public int RealFRW()
        {
            return FasterRunWalk + ((VelocityPercent - (Mode == D2Data.Mode.RUN ? 50 : 0))-100);
        }

        public int RealIAS()
        {
            return IncreasedAttackSpeed + (AttackRate - 100);
        }

        /// <summary>
        /// fill the player data by dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="resistancPenalty"></param>
        internal void ParseStats(Dictionary<StatIdentifier, D2Stat> data, Dictionary<StatIdentifier, D2Stat> itemData, D2GameInfo gameInfo)
        {
            // Don't update stats while dead.
            if (IsDead)
                return;

            CharClass = (CharacterClass)gameInfo.Player.eClass;

            int penalty = (int)ResistancePenalty.GetPenaltyByGameDifficulty((GameDifficulty)gameInfo.Game.Difficulty);
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

            VelocityPercent = getStat(StatIdentifier.VelocityPercent);
            AttackRate = getStat(StatIdentifier.AttackRate);

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

        public void UpdateMode(D2Data.Mode mode)
        {
            Mode = mode;

            bool wasDead = IsDead;

            IsDead = (Mode == D2Data.Mode.DEAD || Mode == D2Data.Mode.DEATH) && Level > 0;

            if (IsDead && !wasDead)
            {
                Deaths++;
            }
        }
    }
}
