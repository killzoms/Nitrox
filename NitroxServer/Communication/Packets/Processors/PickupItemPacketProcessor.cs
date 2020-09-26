using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PickupItemPacketProcessor : AuthenticatedPacketProcessor<PickupItem>
    {
        private readonly EntityManager entityManager;
        private readonly PlayerManager playerManager;
        private readonly SimulationOwnershipData simulationOwnershipData;

        public PickupItemPacketProcessor(EntityManager entityManager, PlayerManager playerManager, SimulationOwnershipData simulationOwnershipData)
        {
            this.entityManager = entityManager;
            this.playerManager = playerManager;
            this.simulationOwnershipData = simulationOwnershipData;
        }

        public override void Process(PickupItem packet, Player sendingPlayer)
        {
            if (simulationOwnershipData.RevokeOwnerOfId(packet.Id))
            {
                SimulationOwnershipChange simulationOwnershipChange = new SimulationOwnershipChange(packet.Id, ushort.MaxValue, NitroxModel.DataStructures.SimulationLockType.TRANSIENT);
                playerManager.SendPacketToAllPlayers(simulationOwnershipChange);
            }

            entityManager.PickUpEntity(packet.Id);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
