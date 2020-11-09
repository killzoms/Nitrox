using NitroxModel.DataStructures;
using NitroxModel.Logger;
using NitroxModel.Subnautica.DataStructures.GameLogic;
using NitroxModel.Subnautica.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Subnautica.Communication.Packets.Processors
{
    public class CyclopsToggleEngineStateProcessor : AuthenticatedPacketProcessor<CyclopsToggleEngineState>
    {
        private readonly VehicleManager vehicleManager;
        private readonly PlayerManager playerManager;

        public CyclopsToggleEngineStateProcessor(VehicleManager vehicleManager, PlayerManager playerManager)
        {
            this.vehicleManager = vehicleManager;
            this.playerManager = playerManager;
        }

        public override void Process(CyclopsToggleEngineState packet, NitroxServer.Player player)
        {
            Optional<CyclopsModel> opCyclops = vehicleManager.GetVehicleModel<CyclopsModel>(packet.Id);

            if (opCyclops.HasValue)
            {
                // If someone starts the engine, IsOn will be false, so only isStarting contains the info we need
                opCyclops.Value.EngineState = packet.IsStarting;
            }
            else
            {
                Log.Error($"{nameof(CyclopsToggleEngineStateProcessor)}: Can't find server model for cyclops with id {packet.Id}");
            }

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
