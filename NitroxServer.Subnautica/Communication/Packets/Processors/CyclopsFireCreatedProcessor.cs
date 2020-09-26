using NitroxModel.Subnautica.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Subnautica.Communication.Packets.Processors
{
    public class CyclopsFireCreatedProcessor : AuthenticatedPacketProcessor<CyclopsFireCreated>
    {
        private readonly PlayerManager playerManager;

        public CyclopsFireCreatedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(CyclopsFireCreated packet, NitroxServer.Player player)
        {
            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
