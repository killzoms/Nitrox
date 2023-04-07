using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    class VehicleDockingProcessor : AuthenticatedPacketProcessor<VehicleDocking>
    {
        private readonly PlayerManager playerManager;

        public VehicleDockingProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(VehicleDocking packet, Player player)
        {
            Optional<Entity> vehicle = EntityRegistry.GetEntityById<Entity>(packet.VehicleId);

            if (!vehicle.HasValue)
            {
                Log.Error($"Unable to find vehicle to dock {packet.VehicleId}");
                return;
            }

            Optional<Entity> dock = EntityRegistry.GetEntityById<Entity>(packet.DockId);

            if (!dock.HasValue)
            {
                Log.Error($"Unable to find dock {packet.DockId} for docking vehicle {packet.VehicleId}");
                return;
            }

            vehicle.Value.ParentId = packet.DockId;
            dock.Value.ChildEntities.Add(vehicle.Value);

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
