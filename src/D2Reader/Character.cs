namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.D2Reader.Readers;
    using Zutatensuppe.D2Reader.Struct.Stat;
    using Zutatensuppe.DiabloInterface.Core;

    public class Character
    {
        // every class starts with:
        // 4 potions (0x24b)
        // 1 scroll ident (0x212)
        // 1 scroll tp (0x211)
        // and most also start with a 1 buckler (0x148) except necro and sorc
        // actually the order of the items in the inventory changes after restarting the game with the same char!
        // so we can be pretty sure if we found a new char or not.

        // but... sometimes, the char is not ready for being read and 0 items are returned...
        // not sure what to do about that.. todo: maybe read the list again after some ms if it is empty
        public static readonly Dictionary<CharacterClass, int[]> StartingItems = new Dictionary<CharacterClass, int[]>
        {
            // jav, buckler
            { CharacterClass.Amazon, new int[] { 0x2f, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // katar, buckler
            { CharacterClass.Assassin, new int[] { 0xaf, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // wand (no buckler)
            { CharacterClass.Necromancer, new int[] { 0xa, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // hand axe, buckler
            { CharacterClass.Barbarian, new int[] { 0x0, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // short sword, buckler
            { CharacterClass.Paladin, new int[] { 0x19, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // short staff (no buckler)
            { CharacterClass.Sorceress, new int[] { 0x3f, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }},
            // club, buckler
            { CharacterClass.Druid, new int[] { 0xe, 0x148, 0x24b, 0x24b, 0x24b, 0x24b, 0x211, 0x212 }}
        };

        // this is the map of skills and the skill points that each character class starts with
        // the maps are ordered in the order the game yields them when fetching skills
        // some classes (assassin/barb) have extra skills
        // some classes (sorc/necro) have skills on their starting weapons, so the skills exist, but have 0 skill points
        public static readonly Dictionary<CharacterClass, Dictionary<Skill, int>> StartingSkills = new Dictionary<CharacterClass, Dictionary<Skill, int>>
        {
            { CharacterClass.Amazon, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.UNSUMMON, 1 }, // unsummon
            } },
            { CharacterClass.Assassin, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.LEFT_HAND_SWING, 1 }, // left hand swing
                { Skill.UNSUMMON, 1 }, // unsummon
            } },
            { CharacterClass.Necromancer, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.UNSUMMON, 1 }, // unsummon
                { Skill.RAISE_SKELETON, 0 }, // raise skeleton
            } },
            { CharacterClass.Barbarian, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.LEFT_HAND_THROW, 1 }, // left hand throw
                { Skill.LEFT_HAND_SWING, 1 }, // left hand swing
                { Skill.UNSUMMON, 1 }, // unsummon
            } },
            { CharacterClass.Paladin, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.UNSUMMON, 1 }, // unsummon
            } },
            { CharacterClass.Sorceress, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.UNSUMMON, 1 }, // unsummon
                { Skill.FIREBOLT, 0 }, // firebolt
            } },
            { CharacterClass.Druid, new Dictionary<Skill, int> {
                { Skill.UNKNOWN, 1 }, // unknown
                { Skill.THROW, 1 }, // throw
                { Skill.KICK, 1 }, // kick
                { Skill.SCROLL_IDENT, 1 }, // scroll ident
                { Skill.TOME_IDENT, 1 }, // tome ident
                { Skill.SCROLL_TP, 1 }, // scroll tp
                { Skill.TOME_TP, 1 }, // tome tp
                { Skill.UNSUMMON, 1 }, // unsummon
            } },
        };

        const int MIN_RESIST = -100;
        const int BASE_MAX_RESIST = 75;

        public string Name { get; set; }

        public CharacterClass CharClass { get; private set; }

        public D2Data.Mode Mode { get; private set; }

        public bool IsDead { get; private set; }

        virtual public int Level { get; private set; }
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

        public int AttackerSelfDamage { get; private set; }

        public int MagicFind { get; private set; }
        public int MonsterGold { get; private set; }

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

        internal void ParseStats(UnitReader unitReader, D2GameInfo gameInfo)
        {
            ParseStats(
                unitReader.GetStatsMap(gameInfo.Player),
                unitReader.GetItemStatsMap(gameInfo.Player),
                gameInfo
            );
        }

        /// <summary>
        /// fill the player data by dictionary
        /// </summary>
        private void ParseStats(
            Dictionary<StatIdentifier, D2Stat> data,
            Dictionary<StatIdentifier, D2Stat> itemData,
            D2GameInfo gameInfo
        ) {
            CharClass = (CharacterClass)gameInfo.Player.eClass;

            int penalty = (int)ResistancePenalty.GetPenaltyByGameDifficulty((GameDifficulty)gameInfo.Game.Difficulty);

            int ValueByStatID(Dictionary<StatIdentifier, D2Stat> d, StatIdentifier statID)
            {
                // Get the value if if the key exists, else assume zero.
                return d.TryGetValue(statID, out D2Stat stat) ? stat.Value : 0;
            }

            Func<StatIdentifier, int> getStat = statID => ValueByStatID(data, statID);
            Func<StatIdentifier, int> getItemStat = statID => ValueByStatID(itemData, statID);

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

            FireResist = Utility.Clamp(getStat(StatIdentifier.ResistFire) + penalty, MIN_RESIST, maxFire);
            ColdResist = Utility.Clamp(getStat(StatIdentifier.ResistCold) + penalty, MIN_RESIST, maxCold);
            LightningResist = Utility.Clamp(getStat(StatIdentifier.ResistLightning) + penalty, MIN_RESIST, maxLightning);
            PoisonResist = Utility.Clamp(getStat(StatIdentifier.ResistPoison) + penalty, MIN_RESIST, maxPoison);

            FasterHitRecovery = getItemStat(StatIdentifier.FasterHitRecovery);
            FasterRunWalk = getItemStat(StatIdentifier.FasterRunWalk);
            FasterCastRate = getItemStat(StatIdentifier.FasterCastRate);
            IncreasedAttackSpeed = getItemStat(StatIdentifier.IncreasedAttackSpeed);

            AttackerSelfDamage = getStat(StatIdentifier.AttackerSelfDamage);

            MagicFind = getStat(StatIdentifier.MagicFind);
            MonsterGold = getStat(StatIdentifier.MonsterGold);

            Gold = getStat(StatIdentifier.Gold);
            GoldStash = getStat(StatIdentifier.GoldStash);
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
