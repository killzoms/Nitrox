using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleColorChangeProcessor : AuthenticatedPacketProcessor<VehicleColorChange>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleColorChangeProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleColorChange packet, Player sendingPlayer)
        {
            Log.Debug(packet);
            vehicleManager.UpdateVehicleColors(packet.Index, packet.Id, packet.HSB);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
