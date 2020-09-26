using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleMovementPacketProcessor : AuthenticatedPacketProcessor<VehicleMovement>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleMovementPacketProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleMovement packet, Player sendingPlayer)
        {
            vehicleManager.UpdateVehicle(packet.VehicleMovementData);

            if (sendingPlayer.Id == packet.PlayerId)
            {
                sendingPlayer.Position = packet.Position;
            }

            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
