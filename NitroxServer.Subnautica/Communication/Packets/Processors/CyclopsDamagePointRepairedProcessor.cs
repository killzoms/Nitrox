using NitroxModel.Subnautica.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Subnautica.Communication.Packets.Processors
{
    public class CyclopsDamagePointRepairedProcessor : AuthenticatedPacketProcessor<CyclopsDamagePointRepaired>
    {
        private readonly PlayerManager playerManager;

        public CyclopsDamagePointRepairedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(CyclopsDamagePointRepaired packet, NitroxServer.Player player)
        {
            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
