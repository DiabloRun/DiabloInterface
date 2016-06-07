using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;

namespace DiabloInterface.D2.Readers
{
    class RangeStatData
    {
        public int DamageMin { get; private set; }
        public bool IsDamageMinSecondary { get; private set; }
        public int DamageMax { get; private set; }
        public bool IsDamageMaxSecondary { get; private set; }
        public bool HasDamageRange { get; private set; }
        public bool HasHandledDamageRange { get; private set; }
        public int DamagePercentMin { get; private set; }
        public int DamagePercentMax { get; private set; }
        public bool HasDamagePercentRange { get; private set; }
        public int FireMinDamage { get; private set; }
        public int FireMaxDamage { get; private set; }
        public bool HasFireRange { get; private set; }
        public int LightningMinDamage { get; private set; }
        public int LightningMaxDamage { get; private set; }
        public bool HasLightningRange { get; private set; }
        public int ColdMinDamage { get; private set; }
        public int ColdMaxDamage { get; private set; }
        public bool HasColdRange { get; private set; }
        public int PoisonMinDamage { get; private set; }
        public int PoisonMaxDamage { get; private set; }
        public int PoisonDuration { get; private set; }
        public int PoisonDivisor { get; private set; }
        public bool HasPoisonRange { get; private set; }
        public int MagicMinDamage { get; private set; }
        public int MagicMaxDamage { get; private set; }
        public bool HasMagicRange { get; private set; }

        private StringLookupTable stringReader;

        public RangeStatData(StringLookupTable reader, List<D2Stat> stats)
        {
            stringReader = reader;

            Func<StatIdentifier, D2Stat> findStat = id => {
                int index = stats.FindIndex(x => x.LoStatID == (ushort)id);
                return index >= 0 ? stats[index] : null;
            };

            foreach (var stat in stats)
            {
                if (!Enum.IsDefined(typeof(StatIdentifier), stat.LoStatID))
                    continue;

                switch ((StatIdentifier)stat.LoStatID)
                {
                    case StatIdentifier.DamageMin:
                        DamageMin = stat.Value;
                        if (DamageMin == 0)
                        {
                            var twoHandDamage = findStat(StatIdentifier.SecondaryDamageMin);
                            if (twoHandDamage != null) DamageMin = twoHandDamage.Value;

                            IsDamageMinSecondary = DamageMin != 0;
                        }
                        break;
                    case StatIdentifier.DamageMax:
                        DamageMax = stat.Value;
                        if (DamageMax == 0)
                        {
                            var twoHandDamage = findStat(StatIdentifier.SecondaryDamageMax);
                            if (twoHandDamage != null) DamageMax = twoHandDamage.Value;

                            IsDamageMaxSecondary = DamageMax != 0;
                        }
                        break;
                    case StatIdentifier.SecondaryDamageMin:
                        {
                            var oneHandDamage = findStat(StatIdentifier.DamageMin);
                            if (oneHandDamage != null) DamageMin = oneHandDamage.Value;

                            if (DamageMin == 0)
                            {
                                DamageMin = stat.Value;
                                IsDamageMinSecondary = stat.Value != 0;
                            }
                            break;
                        }
                    case StatIdentifier.SecondaryDamageMax:
                        {
                            var oneHandDamage = findStat(StatIdentifier.DamageMax);
                            if (oneHandDamage != null) DamageMax = oneHandDamage.Value;

                            if (DamageMax == 0)
                            {
                                DamageMax = stat.Value;
                                IsDamageMaxSecondary = stat.Value != 0;
                            }
                            break;
                        }
                    case StatIdentifier.ItemDamageMinPercent:   DamagePercentMin = stat.Value; break;
                    case StatIdentifier.ItemDamageMaxPercent:   DamagePercentMax = stat.Value; break;
                    case StatIdentifier.FireDamageMin:          FireMinDamage = stat.Value; break;
                    case StatIdentifier.FireDamageMax:          FireMaxDamage = stat.Value; break;
                    case StatIdentifier.LightningDamageMin:     LightningMinDamage = stat.Value; break;
                    case StatIdentifier.LightningDamageMax:     LightningMaxDamage = stat.Value; break;
                    case StatIdentifier.MagicDamageMin:         MagicMinDamage = stat.Value; break;
                    case StatIdentifier.MagicDamageMax:         MagicMaxDamage = stat.Value; break;
                    case StatIdentifier.ColdDamageMin:          ColdMinDamage = stat.Value; break;
                    case StatIdentifier.ColdDamageMax:          ColdMaxDamage = stat.Value; break;
                    case StatIdentifier.PoisonDamageMin:        PoisonMinDamage = stat.Value; break;
                    case StatIdentifier.PoisonDamageMax:        PoisonMaxDamage = stat.Value; break;
                    case StatIdentifier.PoisonDamageDuration:   PoisonDuration = stat.Value; break;
                    case StatIdentifier.PoisonDivisor:          PoisonDivisor = stat.Value; break;
                    default: break;
                }
            }

            HasDamageRange = HasRangeFor(DamageMin, DamageMax);
            HasDamagePercentRange = HasRangeFor(DamagePercentMin, DamagePercentMax);
            HasFireRange = HasRangeFor(FireMinDamage, FireMaxDamage);
            HasLightningRange = HasRangeFor(LightningMinDamage, LightningMaxDamage);
            HasMagicRange = HasRangeFor(MagicMinDamage, MagicMaxDamage);
            HasColdRange = HasRangeFor(ColdMinDamage, ColdMaxDamage);
            HasPoisonRange = HasRangeFor(PoisonMinDamage, PoisonMaxDamage);
        }

        bool HasRangeFor(int min, int max)
        {
            return min != 0 && max != 0;
        }

        string FormatSimpleDamage(int min, int max, ushort equalStringId, ushort differStringId)
        {
            int arguments;
            if (min == max)
            {
                // Example: "Adds 1 to minimum damage"
                string format = stringReader.GetString(equalStringId);
                format = stringReader.ConvertCFormatString(format, out arguments).TrimEnd();
                if (arguments != 1) return null;
                return string.Format(format, min);
            }
            else
            {
                // Example: "Adds 1-2 damage"
                string format = stringReader.GetString(differStringId);
                format = stringReader.ConvertCFormatString(format, out arguments).TrimEnd();
                if (arguments != 2) return null;
                return string.Format(format, min, max);
            }
        }

        public bool TryHandleStat(D2Stat stat, out string description)
        {
            description = null;
            StatIdentifier statId = 0;
            if (Enum.IsDefined(typeof(StatIdentifier), stat.LoStatID))
                statId = (StatIdentifier)stat.LoStatID;
            else return false;

            switch (statId)
            {
                //Handle one and two handed damage.
                case StatIdentifier.DamageMin:
                case StatIdentifier.DamageMax:
                case StatIdentifier.SecondaryDamageMin:
                case StatIdentifier.SecondaryDamageMax:
                    // Only print once if it's a range.
                    if (HasHandledDamageRange)
                        return true;

                    // Skip two-handed damage if there is a one-handed damage source or if no damage is defined.
                    if (stat.IsOfType(StatIdentifier.SecondaryDamageMin) && !IsDamageMinSecondary)
                        return true;
                    if (stat.IsOfType(StatIdentifier.SecondaryDamageMax) && !IsDamageMaxSecondary)
                        return true;

                    // If not a range, print normally.
                    if (!HasDamageRange) return false;
                    // We also print twice if they are the same.
                    if (DamageMin == DamageMax)
                        return false;

                    HasHandledDamageRange = true;
                    description = FormatSimpleDamage(DamageMin, DamageMax,
                        StringConstants.DamageRange,
                        StringConstants.DamageRange);
                    return true;

                // Handle enhanced damage.
                case StatIdentifier.ItemDamageMinPercent:
                    {
                        if (!HasDamagePercentRange)
                            return false;
                        string enhanced = stringReader.GetString(StringConstants.EnhancedDamage);
                        description = string.Format("+{0}% {1}", stat.Value, enhanced.TrimEnd());
                        return true;
                    }
                case StatIdentifier.ItemDamageMaxPercent:
                    return HasDamagePercentRange;

                // Handle fire damage ranges.
                case StatIdentifier.FireDamageMin:
                    if (!HasFireRange) return false;
                    description = FormatSimpleDamage(
                        FireMinDamage,
                        FireMaxDamage,
                        StringConstants.FireDamageRange,
                        StringConstants.FireDamage);
                    return true;
                case StatIdentifier.FireDamageMax:
                    return HasFireRange;

                // Handle lightning damage ranges.
                case StatIdentifier.LightningDamageMin:
                    if (!HasLightningRange) return false;
                    description = FormatSimpleDamage(
                        LightningMinDamage,
                        LightningMaxDamage,
                        StringConstants.LightningDamageRange,
                        StringConstants.LightningDamage);
                    return true;
                case StatIdentifier.LightningDamageMax:
                    return HasLightningRange;

                // Handle magic damage ranges.
                case StatIdentifier.MagicDamageMin:
                    if (!HasMagicRange) return false;
                    description = FormatSimpleDamage(
                        MagicMinDamage,
                        MagicMaxDamage,
                        StringConstants.MagicDamageRange,
                        StringConstants.MagicDamage);
                    return true;
                case StatIdentifier.MagicDamageMax:
                    return HasMagicRange;

                // Handle cold damage ranges.
                case StatIdentifier.ColdDamageMin:
                    if (!HasColdRange) return false;
                    description = FormatSimpleDamage(
                        ColdMinDamage,
                        ColdMaxDamage,
                        StringConstants.ColdDamageRange,
                        StringConstants.ColdDamage);
                    return true;
                case StatIdentifier.ColdDamageMax:
                    return HasColdRange;

                // Handle poison damage ranges.
                case StatIdentifier.PoisonDamageMax:
                case StatIdentifier.PoisonDamageDuration:
                    return HasPoisonRange;
                case StatIdentifier.PoisonDamageMin:
                    {
                        if (!HasPoisonRange) return false;
                        int divisor = PoisonDivisor == 0 ? 1 : PoisonDivisor;
                        int duration = PoisonDuration / divisor;
                        int min = (PoisonMinDamage * duration + 128) / 256;
                        int max = (PoisonMaxDamage * duration + 128) / 256;
                        duration /= 25;

                        if (min == max)
                        {
                            // Example: "6 Poison damage over 2 seconds"
                            int arguments;
                            string format = stringReader.GetString(StringConstants.PoisonOverTimeSame);
                            format = stringReader.ConvertCFormatString(format, out arguments);
                            if (arguments != 2) return true;
                            description = string.Format(format, min, duration).TrimEnd();
                        }
                        else
                        {
                            // Example: "2-10 Poison damage over 2 seconds"
                            int arguments;
                            string format = stringReader.GetString(StringConstants.PoisonOverTime);
                            format = stringReader.ConvertCFormatString(format, out arguments);
                            if (arguments != 3) return true;
                            description = string.Format(format, min, max, duration).TrimEnd();
                        }

                        return true;
                    }

                    // By default, the stat is not handled.
                default: return false;
            }
        }
    }
}
