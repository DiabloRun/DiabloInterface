using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader
{
    class D2GameInfo
    {
        public D2Game Game { get; private set; }
        public D2Unit Player { get; private set; }
        public D2PlayerData PlayerData { get; private set; }

        public D2GameInfo(D2Game game, D2Unit player, D2PlayerData playerData)
        {
            Game = game;
            Player = player;
            PlayerData = playerData;
        }
    }
}
