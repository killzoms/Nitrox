using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleAddPacketProcessor : AuthenticatedPacketProcessor<VehicleCreated>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleAddPacketProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleCreated packet, Player sendingPlayer)
        {
            Log.Debug(packet);
            vehicleManager.AddVehicle(packet.Vehicle);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
