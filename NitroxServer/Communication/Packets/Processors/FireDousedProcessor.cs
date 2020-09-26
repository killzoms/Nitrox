using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class FireDousedProcessor : AuthenticatedPacketProcessor<FireDoused>
    {
        private readonly PlayerManager playerManager;

        public FireDousedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(FireDoused packet, Player sendingPlayer)
        {
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
