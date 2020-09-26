using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleDockingProcessor : AuthenticatedPacketProcessor<VehicleDocking>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleDockingProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleDocking packet, Player sendingPlayer)
        {
            Optional<VehicleModel> vehicle = vehicleManager.GetVehicleModel(packet.VehicleId);

            if (!vehicle.HasValue)
            {
                Log.Error($"VehicleDocking received for vehicle id {packet.VehicleId} that does not exist!");
                return;
            }

            vehicle.Value.DockingBayId = Optional.Of(packet.DockId);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
