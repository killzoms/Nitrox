using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel_Subnautica.DataStructures.GameLogic;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;
using NitroxToggleLights = NitroxModel.Packets.ToggleLights;

namespace NitroxServer_Subnautica.Communication.Packets.Processors
{
    public class SeamothToggleLightsProcessor : AuthenticatedPacketProcessor<NitroxToggleLights>
    {
        private readonly VehicleManager vehicleManager;
        private readonly PlayerManager playerManager;

        public SeamothToggleLightsProcessor(VehicleManager vehicleManager, PlayerManager playerManager)
        {
            this.vehicleManager = vehicleManager;
            this.playerManager = playerManager;
        }

        public override void Process(NitroxToggleLights packet, NitroxServer.Player player)
        {
            Optional<SeamothModel> opSeamoth = vehicleManager.GetVehicleModel<SeamothModel>(packet.Id);

            if (opSeamoth.HasValue && opSeamoth.Value.GetType() == typeof(SeamothModel))
            {
                opSeamoth.Value.LightOn = packet.IsOn;
            }
            else
            {
                Log.Error($"{nameof(SeamothToggleLightsProcessor)}: Can't find server model for seamoth with id {packet.Id}");
            }

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
