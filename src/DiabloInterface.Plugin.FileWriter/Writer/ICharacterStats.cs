namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer
{
    public interface ICharacterStats
    {
        string Name { get; }

        int Level { get; }
        
        int Strength { get; }
        int Dexterity { get; }
        int Vitality { get; }
        int Energy { get; }
        
        int FireResist { get; }
        int ColdResist { get; }
        int LightningResist { get; }
        int PoisonResist { get; }
        
        int FasterHitRecovery { get; }
        int FasterRunWalk { get; }
        int FasterCastRate { get; }
        int IncreasedAttackSpeed { get; }

        int Gold { get; }
        int GoldStash { get; }

        int Deaths { get; }
    }
}
