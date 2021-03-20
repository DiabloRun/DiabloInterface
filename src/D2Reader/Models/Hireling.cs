using System.Collections.Generic;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Stat;

namespace Zutatensuppe.D2Reader.Models
{
    public class Hireling
    {
        const int MIN_RESIST = -100;
        const int BASE_MAX_RESIST = 75;

        public string Name { get; private set; }

        // Act 1: 271
        // Act 2: 338
        // Act 3: 359
        // Act 5: 561
        public int Class { get; private set; }

        public int Level { get; private set; }
        public int Experience { get; private set; }

        public int Strength { get; private set; }
        public int Dexterity { get; private set; }

        public int FireResist { get; private set; }
        public int ColdResist { get; private set; }
        public int LightningResist { get; private set; }
        public int PoisonResist { get; private set; }

        public List<ItemInfo> Items { get; internal set; }

        public List<SkillInfo> Skills { get; internal set; }

        internal void Parse(
            D2Unit unit,
            UnitReader unitReader,
            IProcessMemoryReader reader,
            GameInfo gameInfo
        ) {
            var monsterData = unitReader.GetMonsterData(unit);

            Name = reader.ReadNullTerminatedString(monsterData.szMonName, 300, System.Text.Encoding.Unicode);

            Class = unit.eClass;

            var data = unitReader.GetStatsMap(unit);

            int penalty = ResistancePenalty.GetPenaltyByGame(gameInfo.Game);

            int ValueByStatID(Dictionary<StatIdentifier, D2Stat> d, StatIdentifier statID)
            {
                // Get the value if if the key exists, else assume zero.
                return d.TryGetValue(statID, out D2Stat stat) ? stat.Value : 0;
            }

            int getStat(StatIdentifier statID) => ValueByStatID(data, statID);

            Level = getStat(StatIdentifier.Level);
            Experience = getStat(StatIdentifier.Experience);

            Strength = getStat(StatIdentifier.Strength);
            Dexterity = getStat(StatIdentifier.Dexterity);

            int maxFire = BASE_MAX_RESIST + getStat(StatIdentifier.ResistFireMax);
            int maxCold = BASE_MAX_RESIST + getStat(StatIdentifier.ResistColdMax);
            int maxLightning = BASE_MAX_RESIST + getStat(StatIdentifier.ResistLightningMax);
            int maxPoison = BASE_MAX_RESIST + getStat(StatIdentifier.ResistPoisonMax);

            FireResist = Utility.Clamp(getStat(StatIdentifier.ResistFire) + penalty, MIN_RESIST, maxFire);
            ColdResist = Utility.Clamp(getStat(StatIdentifier.ResistCold) + penalty, MIN_RESIST, maxCold);
            LightningResist = Utility.Clamp(getStat(StatIdentifier.ResistLightning) + penalty, MIN_RESIST, maxLightning);
            PoisonResist = Utility.Clamp(getStat(StatIdentifier.ResistPoison) + penalty, MIN_RESIST, maxPoison);
        }
    }
}
