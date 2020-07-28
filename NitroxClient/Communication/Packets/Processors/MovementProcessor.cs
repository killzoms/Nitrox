using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxModel_Subnautica.DataStructures;

namespace NitroxClient.Communication.Packets.Processors
{
    public class MovementProcessor : ClientPacketProcessor<Movement>
    {
        private readonly PlayerManager remotePlayerManager;

        public MovementProcessor(PlayerManager remotePlayerManager)
        {
            this.remotePlayerManager = remotePlayerManager;
        }

        public override void Process(Movement packet)
        {
            Optional<RemotePlayer> remotePlayer = remotePlayerManager.Find(packet.PlayerId);

            if (remotePlayer.HasValue)
            {
                remotePlayer
                    .Value
                    .UpdatePosition(packet.Position.ToUnity(),
                        packet.Velocity.ToUnity(),
                        packet.BodyRotation.ToUnity(),
                        packet.AimingRotation.ToUnity());
            }
        }
    }
}
