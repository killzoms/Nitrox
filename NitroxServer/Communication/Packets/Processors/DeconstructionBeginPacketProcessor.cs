using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Bases;

namespace NitroxServer.Communication.Packets.Processors
{
    public class DeconstructionBeginPacketProcessor : AuthenticatedPacketProcessor<DeconstructionBegin>
    {
        private readonly BaseManager baseManager;
        private readonly PlayerManager playerManager;

        public DeconstructionBeginPacketProcessor(BaseManager baseManager, PlayerManager playerManager)
        {
            this.baseManager = baseManager;
            this.playerManager = playerManager;
        }

        public override void Process(DeconstructionBegin packet, Player sendingPlayer)
        {
            baseManager.BasePieceDeconstructionBegin(packet.Id);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
