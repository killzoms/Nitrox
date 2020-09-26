using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleUndockingProcessor : AuthenticatedPacketProcessor<VehicleUndocking>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleUndockingProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleUndocking packet, Player sendingPlayer)
        {
            Optional<VehicleModel> vehicle = vehicleManager.GetVehicleModel(packet.VehicleId);

            if (!vehicle.HasValue)
            {
                return;
            }

            vehicle.Value.DockingBayId = Optional.Empty;
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
