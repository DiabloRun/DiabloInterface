namespace Zutatensuppe.D2Reader
{
    class D2GameInfo
    {
        public Struct.D2Game Game { get; private set; }
        public Struct.D2Unit Player { get; private set; }
        public Struct.D2PlayerData PlayerData { get; private set; }

        public D2GameInfo(Struct.D2Game game, Struct.D2Unit player, Struct.D2PlayerData playerData)
        {
            Game = game;
            Player = player;
            PlayerData = playerData;
        }
    }
}
