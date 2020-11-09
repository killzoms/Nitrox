using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Subnautica.DataStructures.GameLogic;
using NitroxModel.Subnautica.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Subnautica.Communication.Packets.Processors
{
    public class CyclopsToggleFloodlightsProcessor : AuthenticatedPacketProcessor<CyclopsToggleFloodLights>
    {
        private readonly VehicleManager vehicleManager;
        private readonly PlayerManager playerManager;

        public CyclopsToggleFloodlightsProcessor(VehicleManager vehicleManager, PlayerManager playerManager)
        {
            this.vehicleManager = vehicleManager;
            this.playerManager = playerManager;
        }

        public override void Process(CyclopsToggleFloodLights packet, NitroxServer.Player player)
        {
            Optional<CyclopsModel> opCyclops = vehicleManager.GetVehicleModel<CyclopsModel>(packet.Id);

            if (opCyclops.HasValue)
            {
                opCyclops.Value.FloodLightsOn = packet.IsOn;
            }
            else
            {
                Log.Error($"{nameof(CyclopsToggleFloodlightsProcessor)}: Can't find server model for cyclops with id {packet.Id}");
            }

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
