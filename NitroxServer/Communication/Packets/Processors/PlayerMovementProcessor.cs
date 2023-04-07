using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    class PlayerMovementProcessor : AuthenticatedPacketProcessor<PlayerMovement>
    {
        private readonly PlayerManager playerManager;

        public PlayerMovementProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(PlayerMovement packet, Player player)
        {
            Optional<PlayerWorldEntity> playerEntity = EntityRegistry.GetEntityById<PlayerWorldEntity>(player.PlayerContext.PlayerNitroxId);

            if (playerEntity.HasValue)
            {
                playerEntity.Value.Transform.Position = packet.Position;
                playerEntity.Value.Transform.Rotation = packet.BodyRotation;
            }

            player.Position = packet.Position;
            player.Rotation = packet.BodyRotation;
            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
