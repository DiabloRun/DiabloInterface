using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader.Models
{
    class GameInfo
    {
        public D2Game Game { get; private set; }
        public uint GameId { get; private set; }
        public D2Client Client { get; private set; }
        public D2Unit Player { get; private set; }
        public D2PlayerData PlayerData { get; private set; }

        public GameInfo(D2Game game, uint gameId, D2Client client, D2Unit player, D2PlayerData playerData)
        {
            Game = game;
            GameId = gameId;
            Client = client;
            Player = player;
            PlayerData = playerData;
        }
    }
}
