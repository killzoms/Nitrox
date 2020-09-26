﻿using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    public class VehicleChildUpdateProcessor : AuthenticatedPacketProcessor<VehicleChildUpdate>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleChildUpdateProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleChildUpdate packet, Player sendingPlayer)
        {
            vehicleManager.UpdateVehicleChildObjects(packet.VehicleId, packet.InteractiveChildIdentifiers);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
