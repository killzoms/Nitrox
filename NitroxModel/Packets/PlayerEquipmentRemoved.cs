﻿using System;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxModel.Packets
{
    [Serializable]
    public class PlayerEquipmentRemoved : Packet
    {
        public NitroxTechType TechType { get; }
        public NitroxId EquippedItemId { get; }

        public PlayerEquipmentRemoved(NitroxTechType techType, NitroxId equippedItemId)
        {
            TechType = techType;
            EquippedItemId = equippedItemId;
        }

        public override string ToString()
        {
            return $"[PlayerEquipmentRemoved - TechType: {TechType}, EquippedItemId: {EquippedItemId}]";
        }
    }
}
