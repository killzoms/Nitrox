using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Packets;
using NitroxModel.Server;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PlayerDeathEventProcessor : AuthenticatedPacketProcessor<PlayerDeathEvent>
    {
        private readonly PlayerManager playerManager;
        private readonly ServerConfig serverConfig;

        public PlayerDeathEventProcessor(PlayerManager playerManager, ServerConfig serverConfig)
        {
            this.playerManager = playerManager;
            this.serverConfig = serverConfig;
        }

        public override void Process(PlayerDeathEvent packet, Player sendingPlayer)
        {
            if (serverConfig.IsHardcore)
            {
                sendingPlayer.IsPermaDeath = true;
                sendingPlayer.SendPacket(new PlayerKicked("Permanent death from hardcore mode"));
            }

            sendingPlayer.LastStoredPosition = packet.DeathPosition;

            if (sendingPlayer.Permissions > Perms.MODERATOR)
            {
                sendingPlayer.SendPacket(new ChatMessage(ChatMessage.SERVER_ID, "You can use /back to go to your death location"));
            }

            playerManager.SendPacketToOtherPlayers(packet, sendingPlayer);
        }
    }
}
