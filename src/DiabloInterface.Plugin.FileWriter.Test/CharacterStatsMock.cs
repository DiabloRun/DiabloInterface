using Zutatensuppe.DiabloInterface.Plugin.FileWriter;

namespace DiabloInterface.Plugin.FileWriter.Test
{
    internal class CharacterStatsMock : ICharacterStats
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int Energy { get; set; }
        public int FireResist { get; set; }
        public int ColdResist { get; set; }
        public int LightningResist { get; set; }
        public int PoisonResist { get; set; }
        public int FasterHitRecovery { get; set; }
        public int FasterRunWalk { get; set; }
        public int FasterCastRate { get; set; }
        public int IncreasedAttackSpeed { get; set; }
        public int Gold { get; set; }
        public int GoldStash { get; set; }
        public int Deaths { get; set; }
    }
}
