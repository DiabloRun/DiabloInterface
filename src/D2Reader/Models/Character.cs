using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Skill;
using Zutatensuppe.D2Reader.Struct.Stat;
using Zutatensuppe.DiabloInterface.Core;
using Zutatensuppe.DiabloInterface.Core.Logging;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.D2Reader.Models
{
    public class Character
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

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

        public string Guid { get; set; }

        public CharacterClass CharClass { get; private set; }

        public Mode Mode { get; private set; }

        public bool IsDead { get; private set; }

        public bool IsHardcore { get; internal set; }
        public bool IsExpansion { get; internal set; }

        virtual public bool IsNewChar { get; internal set; }

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

        public int Life { get; private set; }
        public int LifeMax { get; private set; }

        public int Mana { get; private set; }
        public int ManaMax { get; private set; }

        public short Deaths = 0;

        public int Defense { get; private set; }

        public DateTime Created { get; set; }

        // TODO: use Item model for these:
        virtual public List<int> InventoryItemIds { get; internal set; }
        public List<ItemInfo> Items { get; internal set; }

        public int RealFRW()
        {
            return FasterRunWalk + ((VelocityPercent - (Mode == Mode.RUN ? 50 : 0))-100);
        }

        public int RealIAS()
        {
            return IncreasedAttackSpeed + (AttackRate - 100);
        }

        internal void Parse(
            UnitReader unitReader,
            GameInfo gameInfo
        ) {
            var data = unitReader.GetStatsMap(gameInfo.Player);

            CharClass = (CharacterClass)gameInfo.Player.eClass;

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
            Vitality = getStat(StatIdentifier.Vitality);
            Energy = getStat(StatIdentifier.Energy);

            Gold = getStat(StatIdentifier.Gold);
            GoldStash = getStat(StatIdentifier.GoldStash);

            VelocityPercent = getStat(StatIdentifier.VelocityPercent);
            AttackRate = getStat(StatIdentifier.AttackRate);
            Defense = getStat(StatIdentifier.Defense);

            int maxFire = BASE_MAX_RESIST + getStat(StatIdentifier.ResistFireMax);
            int maxCold = BASE_MAX_RESIST + getStat(StatIdentifier.ResistColdMax);
            int maxLightning = BASE_MAX_RESIST + getStat(StatIdentifier.ResistLightningMax);
            int maxPoison = BASE_MAX_RESIST + getStat(StatIdentifier.ResistPoisonMax);

            FireResist = Utility.Clamp(getStat(StatIdentifier.ResistFire) + penalty, MIN_RESIST, maxFire);
            ColdResist = Utility.Clamp(getStat(StatIdentifier.ResistCold) + penalty, MIN_RESIST, maxCold);
            LightningResist = Utility.Clamp(getStat(StatIdentifier.ResistLightning) + penalty, MIN_RESIST, maxLightning);
            PoisonResist = Utility.Clamp(getStat(StatIdentifier.ResistPoison) + penalty, MIN_RESIST, maxPoison);

            FasterHitRecovery = getStat(StatIdentifier.FasterHitRecovery);
            FasterRunWalk = getStat(StatIdentifier.FasterRunWalk);
            FasterCastRate = getStat(StatIdentifier.FasterCastRate);
            IncreasedAttackSpeed = getStat(StatIdentifier.IncreasedAttackSpeed);

            AttackerSelfDamage = getStat(StatIdentifier.AttackerSelfDamage);

            MagicFind = getStat(StatIdentifier.MagicFind);
            MonsterGold = getStat(StatIdentifier.MonsterGold);

            Mana = getStat(StatIdentifier.Mana) >> 8;
            ManaMax = getStat(StatIdentifier.ManaMax) >> 8;

            Life = getStat(StatIdentifier.Hitpoints) >> 8;
            LifeMax = getStat(StatIdentifier.HitpointsMax) >> 8;
        }

        public void UpdateMode(Mode mode)
        {
            Mode = mode;

            bool wasDead = IsDead;

            IsDead = (Mode == Mode.DEAD || Mode == Mode.DEATH) && Level > 0;

            if (IsDead && !wasDead)
            {
                Deaths++;
            }
        }

        public static bool DetermineIfNewChar(
            D2Unit unit,
            UnitReader unitReader,
            IInventoryReader inventoryReader,
            ISkillReader skillReader
        ) {
            if (!MatchesStartingProps(unit, unitReader))
            {
                Logger.Info("Starting Props don't match");
                return false;
            }
            if (!MatchesStartingItems(unit, inventoryReader))
            {
                Logger.Info("Starting Items don't match");
                return false;
            }
            if (!MatchesStartingSkills(unit, skillReader))
            {
                Logger.Info("Starting Skills don't match");
                return false;
            }
            return true;
        }

        private static bool MatchesStartingProps(D2Unit p, UnitReader unitReader)
        {
            // check -act2/3/4/5 level|xp
            int level = unitReader.GetStatValue(p, StatIdentifier.Level) ?? 0;
            if (level == 0)
                throw new Exception("Invalid level");
            int experience = unitReader.GetStatValue(p, StatIdentifier.Experience) ?? 0;

            // first we will check the level and XP
            // act should be set to the act we are currently in
            return
                (level == 1 && experience == 0 && p.actNo == 0)
                || (level == 16 && experience == 220165 && p.actNo == 1)
                || (level == 21 && experience == 839864 && p.actNo == 2)
                || (level == 27 && experience == 2563061 && p.actNo == 3)
                || (level == 33 && experience == 7383752 && p.actNo == 4);
        }

        private static bool MatchesStartingItems(D2Unit p, IInventoryReader inventoryReader)
        {
            int[] list = (
                from item
                in inventoryReader.EnumerateInventoryForward(p)
                select item.Unit.eClass
            ).ToArray();

            return list.SequenceEqual(StartingItems[(CharacterClass)p.eClass]);
        }

        private static bool MatchesStartingSkills(D2Unit p, ISkillReader skillReader)
        {
            int skillCount = 0;
            foreach (D2Skill skill in skillReader.EnumerateSkills(p))
            {
                var skillData = skillReader.ReadSkillData(skill);
                Skill skillId = (Skill)skillData.SkillId;
                if (!StartingSkills[(CharacterClass)p.eClass].ContainsKey(skillId))
                {
                    return false;
                }

                if (StartingSkills[(CharacterClass)p.eClass][skillId] != skillReader.GetTotalNumberOfSkillPoints(skill))
                {
                    return false;
                }
                skillCount++;
            }

            return skillCount == StartingSkills[(CharacterClass)p.eClass].Count;
        }
    }
}
