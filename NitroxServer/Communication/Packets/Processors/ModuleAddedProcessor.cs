using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class ModuleAddedProcessor : AuthenticatedPacketProcessor<ModuleAdded>
    {
        private readonly PlayerManager playerManager;

        public ModuleAddedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(ModuleAdded packet, Player sendingPlayer)
        {
            sendingPlayer.AddModule(packet.EquippedItemData);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
