namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer
{
    using Zutatensuppe.D2Reader.Models;

    internal class CharacterStats : ICharacterStats
    {
        readonly Character character;

        public CharacterStats(Character character)
        {
            this.character = character;
        }

        public string Name => character.Name;
        public int Level => character.Level;
        public int Strength => character.Strength;
        public int Dexterity => character.Dexterity;
        public int Vitality => character.Vitality;
        public int Energy => character.Energy;
        public int FireResist => character.FireResist;
        public int ColdResist => character.ColdResist;
        public int LightningResist => character.LightningResist;
        public int PoisonResist => character.PoisonResist;
        public int FasterHitRecovery => character.FasterHitRecovery;
        public int FasterRunWalk => character.FasterRunWalk;
        public int FasterCastRate => character.FasterCastRate;
        public int IncreasedAttackSpeed => character.IncreasedAttackSpeed;
        public int Gold => character.Gold;
        public int GoldStash => character.GoldStash;
        public int Deaths => character.Deaths;
    }
}
