using NitroxModel.DataStructures;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PlayerEquipmentRemovedProcessor : AuthenticatedPacketProcessor<PlayerEquipmentRemoved>
    {
        private readonly PlayerManager playerManager;

        public PlayerEquipmentRemovedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(PlayerEquipmentRemoved packet, Player sendingPlayer)
        {
            NitroxId itemId = packet.EquippedItemId;

            sendingPlayer.RemoveEquipment(itemId);
            RemotePlayerEquipmentRemoved equipmentRemoved = new RemotePlayerEquipmentRemoved(sendingPlayer.Id, packet.TechType);

            playerManager.SendPacketToOtherPlayers(equipmentRemoved, sendingPlayer);
        }
    }
}
