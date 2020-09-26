using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class CellVisibilityChangedProcessor : AuthenticatedPacketProcessor<CellVisibilityChanged>
    {
        private readonly EntityManager entityManager;
        private readonly EntitySimulation entitySimulation;
        private readonly PlayerManager playerManager;

        public CellVisibilityChangedProcessor(EntityManager entityManager, EntitySimulation entitySimulation, PlayerManager playerManager)
        {
            this.entityManager = entityManager;
            this.entitySimulation = entitySimulation;
            this.playerManager = playerManager;
        }

        public override void Process(CellVisibilityChanged packet, Player sendingPlayer)
        {
            sendingPlayer.AddCells(packet.Added);
            sendingPlayer.RemoveCells(packet.Removed);

            SendNewlyVisibleEntities(sendingPlayer, packet.Added);

            List<SimulatedEntity> ownershipChanges = entitySimulation.CalculateSimulationChangesFromCellSwitch(sendingPlayer, packet.Added, packet.Removed);
            BroadcastSimulationChanges(ownershipChanges);
        }

        private void SendNewlyVisibleEntities(Player player, AbsoluteEntityCell[] visibleCells)
        {
            List<Entity> newlyVisibleEntities = entityManager.GetVisibleEntities(visibleCells);

            if (newlyVisibleEntities.Count > 0)
            {
                player.SendPacket(new CellEntities(newlyVisibleEntities));
            }
        }

        private void BroadcastSimulationChanges(List<SimulatedEntity> ownershipChanges)
        {
            if (ownershipChanges.Count > 0)
            {
                // TODO: This should be moved to `SimulationOwnership`
                playerManager.SendPacketToAllPlayers(new SimulationOwnershipChange(ownershipChanges));
            }
        }
    }
}
