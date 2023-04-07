using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class EntityMetadataUpdateProcessor : AuthenticatedPacketProcessor<EntityMetadataUpdate>
    {
        private readonly PlayerManager playerManager;

        public EntityMetadataUpdateProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(EntityMetadataUpdate packet, Player sendingPlayer)
        {
            Optional<Entity> entity = EntityRegistry.GetEntityById<Entity>(packet.Id);

            if (entity.HasValue)
            {
                entity.Value.Metadata = packet.NewValue;
                SendUpdateToVisiblePlayers(packet, sendingPlayer, entity.Value);
            }
            else
            {
                Log.Error($"Entity metadata updated on an entity unknown to the server {packet.Id} {packet.NewValue.GetType()} ");
            }
        }

        private void SendUpdateToVisiblePlayers(EntityMetadataUpdate packet, Player sendingPlayer, Entity entity)
        {
            foreach (Player player in playerManager.GetConnectedPlayers())
            {
                bool updateVisibleToPlayer = player.CanSee(entity);

                if (player != sendingPlayer && updateVisibleToPlayer)
                {
                    player.SendPacket(packet);
                }
            }
        }
    }
}
