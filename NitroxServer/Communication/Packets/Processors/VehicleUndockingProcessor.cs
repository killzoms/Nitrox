using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    class VehicleUndockingProcessor : AuthenticatedPacketProcessor<VehicleUndocking>
    {
        private readonly PlayerManager playerManager;

        public VehicleUndockingProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(VehicleUndocking packet, Player player)
        {
            Optional<Entity> vehicle = EntityRegistry.GetEntityById<Entity>(packet.VehicleId);

            if (!vehicle.HasValue)
            {
                Log.Error($"Unable to find vehicle to undock {packet.VehicleId}");
                return;
            }

            Optional<Entity> parent = EntityRegistry.GetEntityById<Entity>(vehicle.Value.ParentId);

            if (!parent.HasValue)
            {
                Log.Error($"Unable to find docked vehicles parent {vehicle.Value.ParentId} to undock from");
                return;
            }

            parent.Value.ChildEntities.Remove(vehicle.Value);

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
