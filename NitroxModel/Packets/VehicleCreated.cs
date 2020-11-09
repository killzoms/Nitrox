using System;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxModel.Packets
{
    [Serializable]
    public class VehicleCreated : Packet
    {
        public string PlayerName { get; }
        public VehicleModel Vehicle { get; }

        public VehicleCreated(VehicleModel createdVehicle, string playerName)
        {
            Vehicle = createdVehicle;
            PlayerName = playerName;
        }

        public override string ToString()
        {
            return $"[VehicleCreated - PlayerName: {PlayerName}, Vehicle: {Vehicle}]";
        }
    }
}
