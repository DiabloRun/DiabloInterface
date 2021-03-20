using Zutatensuppe.D2Reader.Struct.Stat;
using System;
using System.Collections.Generic;

namespace Zutatensuppe.D2Reader.Readers
{
    class RangeStatDamage
    {
        public int min;
        public int max;

        public ushort equalStringId;
        public ushort differStringId;

        public RangeStatDamage(ushort equalStringId, ushort differStringId)
        {
            this.equalStringId = equalStringId;
            this.differStringId = differStringId;
        }

        public bool HasRange()
        {
            return min != 0 && max != 0;
        }

        public string ToString(IStringReader stringLookupTable)
        {
            if (min == max)
            {
                // Example: "Adds 1 to minimum damage"
                return _toString(stringLookupTable, new object[] { min }, equalStringId);
            }

            // Example: "Adds 1-2 damage"
            return _toString(stringLookupTable, new object[] { min, max }, differStringId);
        }

        protected string _toString(IStringReader stringLookupTable, object[] args, ushort stringId)
        {
            string format = stringLookupTable.ConvertCFormatString(
                stringLookupTable.GetString(stringId),
                out int arguments
            );
            if (arguments != args.Length)
            {
                return null;
            }
            return string.Format(format, args).TrimEnd();
        }

    }

    class RangeStatPoisonDamage : RangeStatDamage
    {
        public int duration;
        public int divisor;

        public RangeStatPoisonDamage() : base(StringConstants.PoisonOverTimeSame, StringConstants.PoisonOverTime)
        {
        }

        public new string ToString(IStringReader stringLookupTable)
        {
            int min = CalculatedDamage(this.min);
            int max = CalculatedDamage(this.max);
            if (min == max)
            {
                // Example: "6 Poison damage over 2 seconds"
                return _toString(stringLookupTable, new object[] { min, CalculatedDuration() }, equalStringId);
            }

            // Example: "2-10 Poison damage over 2 seconds"
            return _toString(stringLookupTable, new object[] { min, max, CalculatedDuration() }, differStringId);
        }

        private int CalculatedDuration()
        {
            return EffectiveDuration() / 25;
        }

        private int CalculatedDamage(int dmg)
        {
            return (dmg * EffectiveDuration() + 128) / 256;
        }

        private int EffectiveDuration()
        {
            return duration / (divisor == 0 ? 1 : divisor);
        }
    }

    class RangeStatData
    {
        private RangeStatDamage damage;
        private RangeStatDamage damagePercent;
        private RangeStatDamage fireDamage;
        private RangeStatDamage lightningDamage;
        private RangeStatDamage coldDamage;
        private RangeStatDamage magicDamage;
        private RangeStatPoisonDamage poisonDamage;

        private bool IsDamageMinSecondary;
        private bool IsDamageMaxSecondary;
        private bool HasHandledDamageRange;

        private IStringReader stringReader;

        public RangeStatData(IStringReader reader, List<D2Stat> stats)
        {
            stringReader = reader;

            damage = new RangeStatDamage(StringConstants.DamageRange, StringConstants.DamageRange);
            damagePercent = new RangeStatDamage(0, 0);
            fireDamage = new RangeStatDamage(StringConstants.FireDamage, StringConstants.FireDamageRange);
            lightningDamage = new RangeStatDamage(StringConstants.LightningDamage, StringConstants.LightningDamageRange);
            coldDamage = new RangeStatDamage(StringConstants.ColdDamage, StringConstants.ColdDamageRange);
            magicDamage = new RangeStatDamage(StringConstants.MagicDamage, StringConstants.MagicDamageRange);
            poisonDamage = new RangeStatPoisonDamage();

            ReadStatData(stats);
        }

        private void ReadStatData(List<D2Stat> stats)
        {
            D2Stat findStat(StatIdentifier id)
            {
                int index = stats.FindIndex(x => x.LoStatID == (ushort)id);
                return index >= 0 ? stats[index] : null;
            }

            foreach (var stat in stats)
            {
                if (!Enum.IsDefined(typeof(StatIdentifier), stat.LoStatID))
                    continue;

                switch ((StatIdentifier)stat.LoStatID)
                {
                    case StatIdentifier.DamageMin:
                        damage.min = stat.Value;
                        if (damage.min == 0)
                        {
                            var twoHandDamage = findStat(StatIdentifier.SecondaryDamageMin);
                            if (twoHandDamage != null) damage.min = twoHandDamage.Value;

                            IsDamageMinSecondary = damage.min != 0;
                        }
                        break;
                    case StatIdentifier.DamageMax:
                        damage.max = stat.Value;
                        if (damage.max == 0)
                        {
                            var twoHandDamage = findStat(StatIdentifier.SecondaryDamageMax);
                            if (twoHandDamage != null) damage.max = twoHandDamage.Value;

                            IsDamageMaxSecondary = damage.max != 0;
                        }
                        break;
                    case StatIdentifier.SecondaryDamageMin:
                        var oneHandDamageMin = findStat(StatIdentifier.DamageMin);
                        if (oneHandDamageMin != null) damage.min = oneHandDamageMin.Value;

                        if (damage.min == 0)
                        {
                            damage.min = stat.Value;
                            IsDamageMinSecondary = stat.Value != 0;
                        }
                        break;
                    case StatIdentifier.SecondaryDamageMax:
                        var oneHandDamageMax = findStat(StatIdentifier.DamageMax);
                        if (oneHandDamageMax != null) damage.max = oneHandDamageMax.Value;

                        if (damage.max == 0)
                        {
                            damage.max = stat.Value;
                            IsDamageMaxSecondary = stat.Value != 0;
                        }
                        break;
                    case StatIdentifier.ItemDamageMinPercent: damagePercent.min = stat.Value; break;
                    case StatIdentifier.ItemDamageMaxPercent: damagePercent.max = stat.Value; break;
                    case StatIdentifier.FireDamageMin: fireDamage.min = stat.Value; break;
                    case StatIdentifier.FireDamageMax: fireDamage.max = stat.Value; break;
                    case StatIdentifier.LightningDamageMin: lightningDamage.min = stat.Value; break;
                    case StatIdentifier.LightningDamageMax: lightningDamage.max = stat.Value; break;
                    case StatIdentifier.MagicDamageMin: magicDamage.min = stat.Value; break;
                    case StatIdentifier.MagicDamageMax: magicDamage.max = stat.Value; break;
                    case StatIdentifier.ColdDamageMin: coldDamage.min = stat.Value; break;
                    case StatIdentifier.ColdDamageMax: coldDamage.max = stat.Value; break;
                    case StatIdentifier.PoisonDamageMin: poisonDamage.min = stat.Value; break;
                    case StatIdentifier.PoisonDamageMax: poisonDamage.max = stat.Value; break;
                    case StatIdentifier.PoisonDamageDuration: poisonDamage.duration = stat.Value; break;
                    case StatIdentifier.PoisonDivisor: poisonDamage.divisor = stat.Value; break;
                    default: break;
                }
            }
        }

        public bool TryHandleStat(D2Stat stat, out string description)
        {
            description = null;

            if (!stat.HasValidLoStatIdentifier())
                return false;

            switch ((StatIdentifier)stat.LoStatID)
            {
                // Handle one and two handed damage.
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
                    if (!damage.HasRange())
                        return false;

                    // We also print twice if they are the same.
                    if (damage.min == damage.max)
                        return false;

                    HasHandledDamageRange = true;
                    description = damage.ToString(stringReader);
                    return true;

                // Handle enhanced damage.
                case StatIdentifier.ItemDamageMinPercent:
                    if (!damagePercent.HasRange())
                        return false;
                    description = string.Format(
                        "+{0}% {1}",
                        stat.Value,
                        stringReader.GetString(StringConstants.EnhancedDamage).TrimEnd()
                    );
                    return true;
                case StatIdentifier.ItemDamageMaxPercent:
                    return damagePercent.HasRange();

                // Handle fire damage ranges.
                case StatIdentifier.FireDamageMin:
                    if (!fireDamage.HasRange())
                        return false;
                    description = fireDamage.ToString(stringReader);
                    return true;
                case StatIdentifier.FireDamageMax:
                    return fireDamage.HasRange();

                // Handle lightning damage ranges.
                case StatIdentifier.LightningDamageMin:
                    if (!lightningDamage.HasRange())
                        return false;
                    description = lightningDamage.ToString(stringReader);
                    return true;
                case StatIdentifier.LightningDamageMax:
                    return lightningDamage.HasRange();

                // Handle magic damage ranges.
                case StatIdentifier.MagicDamageMin:
                    if (!magicDamage.HasRange())
                        return false;
                    description = magicDamage.ToString(stringReader);
                    return true;
                case StatIdentifier.MagicDamageMax:
                    return magicDamage.HasRange();

                // Handle cold damage ranges.
                case StatIdentifier.ColdDamageMin:
                    if (!coldDamage.HasRange())
                        return false;
                    description = coldDamage.ToString(stringReader);
                    return true;
                case StatIdentifier.ColdDamageMax:
                    return coldDamage.HasRange();

                // Handle poison damage ranges.
                case StatIdentifier.PoisonDamageMax:
                case StatIdentifier.PoisonDamageDuration:
                    return poisonDamage.HasRange();
                case StatIdentifier.PoisonDamageMin:
                    if (!poisonDamage.HasRange())
                        return false;
                    description = poisonDamage.ToString(stringReader);
                    return true;

                    // By default, the stat is not handled.
                default:
                    return false;
            }
        }
    }
}
