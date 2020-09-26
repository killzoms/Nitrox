using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class SimulationOwnershipRequestProcessor : AuthenticatedPacketProcessor<SimulationOwnershipRequest>
    {
        private readonly PlayerManager playerManager;
        private readonly SimulationOwnershipData simulationOwnershipData;

        public SimulationOwnershipRequestProcessor(PlayerManager playerManager, SimulationOwnershipData simulationOwnershipData)
        {
            this.playerManager = playerManager;
            this.simulationOwnershipData = simulationOwnershipData;
        }

        public override void Process(SimulationOwnershipRequest packet, Player sendingPlayer)
        {
            bool aquiredLock = simulationOwnershipData.TryToAcquire(packet.Id, sendingPlayer, packet.LockType);

            if (aquiredLock)
            {
                SimulationOwnershipChange simulationOwnershipChange = new SimulationOwnershipChange(packet.Id, sendingPlayer.Id, packet.LockType);
                playerManager.SendPacketToOtherPlayers(simulationOwnershipChange, sendingPlayer);
            }

            SimulationOwnershipResponse responseToPlayer = new SimulationOwnershipResponse(packet.Id, aquiredLock, packet.LockType);
            sendingPlayer.SendPacket(responseToPlayer);
        }
    }
}
