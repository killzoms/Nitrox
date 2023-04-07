using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    class SubRootChangedPacketProcessor : AuthenticatedPacketProcessor<SubRootChanged>
    {
        private readonly PlayerManager playerManager;

        public SubRootChangedPacketProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(SubRootChanged packet, Player player)
        {
            EntityRegistry.SetParent(player.GameObjectId, packet.SubRootId.OrNull());
            player.SubRootId = packet.SubRootId;
            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
