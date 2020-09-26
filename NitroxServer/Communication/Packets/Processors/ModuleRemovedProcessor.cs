using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class ModuleRemovedProcessor : AuthenticatedPacketProcessor<ModuleRemoved>
    {
        private readonly PlayerManager playerManager;

        public ModuleRemovedProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(ModuleRemoved packet, Player sendingPlayer)
        {
            sendingPlayer.RemoveModule(packet.ItemId);
            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
