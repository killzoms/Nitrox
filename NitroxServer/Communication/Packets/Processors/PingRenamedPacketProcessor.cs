using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PingRenamedPacketProcessor : AuthenticatedPacketProcessor<PingRenamed>
    {
        private readonly EntityManager entities;
        private readonly PlayerManager playerManager;

        public PingRenamedPacketProcessor(PlayerManager playerManager, EntityManager entities)
        {
            this.playerManager = playerManager;
            this.entities = entities;
        }

        public override void Process(PingRenamed packet, Player sendingPlayer)
        {
            Optional<Entity> beaconEntity = entities.GetEntityById(packet.Id);
            if (!beaconEntity.HasValue)
            {
                Log.Error($"Beacon entity could not be found on server with nitrox id '{packet.Id}'");
                return;
            }

            Log.Info($"Received ping rename: {packet} by player: {sendingPlayer}");
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);

            // Persist label change on server for future players
            beaconEntity.Value.SerializedGameObject = packet.BeaconGameObjectSerialized;
        }
    }
}
