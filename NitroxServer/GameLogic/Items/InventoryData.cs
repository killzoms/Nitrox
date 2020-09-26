using System.Collections.Generic;
using System.Linq;
using NitroxModel.DataStructures.GameLogic;
using ProtoBufNet;

namespace NitroxServer.GameLogic.Items
{
    [ProtoContract]
    public class InventoryData
    {
        [ProtoMember(1)]
        public List<ItemData> InventoryItems { get; set; } = new List<ItemData>();

        [ProtoMember(2)]
        public List<ItemData> StorageSlotItems { get; set; } = new List<ItemData>();

        private InventoryData()
        {
        }

        public static InventoryData From(IEnumerable<ItemData> inventoryItems, IEnumerable<ItemData> storageSlotItems)
        {
            return new InventoryData
            {
                InventoryItems = inventoryItems.ToList(),
                StorageSlotItems = storageSlotItems.ToList()
            };
        }

        public override string ToString()
        {
            return $"[{nameof(InventoryData)} - {nameof(InventoryItems)}: {InventoryItems.Count}, {nameof(StorageSlotItems)}: {StorageSlotItems.Count}]";
        }
    }
}
