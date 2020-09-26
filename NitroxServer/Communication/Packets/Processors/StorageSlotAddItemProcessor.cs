using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Items;

namespace NitroxServer.Communication.Packets.Processors
{
    public class StorageSlotAddItemProcessor : AuthenticatedPacketProcessor<StorageSlotItemAdd>
    {
        private readonly PlayerManager playerManager;
        private readonly InventoryManager inventoryManager;

        public StorageSlotAddItemProcessor(PlayerManager playerManager, InventoryManager inventoryManager)
        {
            this.playerManager = playerManager;
            this.inventoryManager = inventoryManager;
        }

        public override void Process(StorageSlotItemAdd packet, Player sendingPlayer)
        {
            inventoryManager.StorageItemAdded(packet.ItemData);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
