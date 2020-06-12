namespace Zutatensuppe.D2Reader.Models
{
    public class Game
    {
        public int Area;
        public byte InventoryTab;
        public int PlayersX;
        public GameDifficulty Difficulty;
        public uint Seed;
        public bool SeedIsArg;
        public uint GameCount;
        public uint CharCount;
        public Quests Quests;
        public Character Character;
        public Hireling Hireling;
    }
}
