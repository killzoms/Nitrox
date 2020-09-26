using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class DroppedItemPacketProcessor : AuthenticatedPacketProcessor<DroppedItem>
    {
        private readonly EntityManager entityManager;
        private readonly PlayerManager playerManager;
        private readonly EntitySimulation entitySimulation;

        public DroppedItemPacketProcessor(EntityManager entityManager, PlayerManager playerManager, EntitySimulation entitySimulation)
        {
            this.entityManager = entityManager;
            this.playerManager = playerManager;
            this.entitySimulation = entitySimulation;
        }

        public override void Process(DroppedItem packet, Player sendingPlayer)
        {
            bool existsInGlobalRoot = Map.Main.GlobalRootTechTypes.Contains(packet.TechType);
            Entity entity = new Entity(packet.ItemPosition, packet.ItemRotation, NitroxVector3.One, packet.TechType, 0, null, true, packet.WaterParkId.OrElse(null), packet.Bytes, existsInGlobalRoot, packet.Id);
            entityManager.RegisterNewEntity(entity);

            SimulatedEntity simulatedEntity = entitySimulation.AssignNewEntityToPlayer(entity, sendingPlayer);

            SimulationOwnershipChange ownershipChangePacket = new SimulationOwnershipChange(simulatedEntity);
            playerManager.SendPacketToAllPlayers(ownershipChangePacket);

            foreach (Player connectedPlayer in playerManager.GetConnectedPlayers())
            {
                if (connectedPlayer != sendingPlayer && connectedPlayer.CanSee(entity))
                {
                    connectedPlayer.SendPacket(new CellEntities(entity));
                }
            }
        }
    }
}
