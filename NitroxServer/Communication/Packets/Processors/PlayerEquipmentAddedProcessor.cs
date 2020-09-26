using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PlayerEquipmentAddedProcessor : AuthenticatedPacketProcessor<PlayerEquipmentAdded>
    {
        private readonly PlayerManager playerManager;

        public PlayerEquipmentAddedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(PlayerEquipmentAdded packet, Player sendingPlayer)
        {
            EquippedItemData equippedItem = packet.EquippedItem;
            RemotePlayerEquipmentAdded equipmentAdded = new RemotePlayerEquipmentAdded(sendingPlayer.Id, packet.TechType);

            sendingPlayer.AddEquipment(equippedItem);
            playerManager.SendPacketToOtherPlayers(equipmentAdded, sendingPlayer);
        }
    }
}
