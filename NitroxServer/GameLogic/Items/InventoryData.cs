﻿using NitroxModel.DataStructures.GameLogic;
using ProtoBufNet;
using System.Collections.Generic;
using NitroxModel.DataStructures;

namespace NitroxServer.GameLogic.Items
{
    [ProtoContract]
    public class InventoryData
    {
        [ProtoMember(1)]
        private Dictionary<NitroxId, ItemData> insertedInventoryItemsById = new Dictionary<NitroxId, ItemData>();
        
        [ProtoMember(2)]
        private Dictionary<NitroxId, ItemData> storageSlotItemsById = new Dictionary<NitroxId, ItemData>();

        public void ItemAdded(ItemData itemData)
        {
            lock(insertedInventoryItemsById)
            {
                insertedInventoryItemsById[itemData.ItemId] = itemData;
            }
        }

        public void ItemRemoved(NitroxId itemId)
        {
            lock (insertedInventoryItemsById)
            {
                insertedInventoryItemsById.Remove(itemId);
            }
        }
        
        public List<ItemData> GetAllItemsForInitialSync()
        {
            lock (insertedInventoryItemsById)
            {
                return new List<ItemData>(insertedInventoryItemsById.Values);
            }
        }

        
        public void StorageItemAdded(ItemData itemData)
        {
            lock (storageSlotItemsById)
            {
                storageSlotItemsById[itemData.ContainerId] = itemData;
            }
        }

        public bool StorageItemRemoved(NitroxId ownerId)
        {
            bool success = false;
            lock (storageSlotItemsById)
            {
                success = storageSlotItemsById.Remove(ownerId);
            }
            return success;
        }

        public List<ItemData> GetAllStorageItemsForInitialSync()
        {
            lock (storageSlotItemsById)
            {
                return new List<ItemData>(storageSlotItemsById.Values);
            }
        }
    }
}
