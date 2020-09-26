using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PlayerStatsProcessor : AuthenticatedPacketProcessor<PlayerStats>
    {
        private readonly PlayerManager playerManager;

        public PlayerStatsProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(PlayerStats packet, Player sendingPlayer)
        {
            sendingPlayer.Stats = new NitroxModel.DataStructures.GameLogic.PlayerStatsData(packet.Oxygen, packet.MaxOxygen, packet.Health, packet.Food, packet.Water, packet.InfectionAmount);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
