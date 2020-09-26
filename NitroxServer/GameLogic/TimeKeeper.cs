using System;
using NitroxModel.Core;
using NitroxModel.Packets;

namespace NitroxServer.GameLogic
{
    public class TimeKeeper
    {
        // Values taken directly from hardcoded subnautica values
        private static readonly DateTime subnauticaFutureStartDate = new DateTime(2287, 5, 7, 9, 36, 0);
        private static readonly float subnauticaBeginTimeOffset = 1200f / 86400f *
                                                                  (3600f * subnauticaFutureStartDate.Hour +
                                                                   60f * subnauticaFutureStartDate.Minute +
                                                                   subnauticaFutureStartDate.Second);

        // Discrepancy value for player based time modifications
        private float correctionValue;

        public DateTime ServerStartTime { get; set; } = DateTime.UtcNow;

        public void SetDay()
        {
            correctionValue += 1200.0f - CurrentTime % 1200.0f + 600.0f;
            SendCurrentTimePacket();
        }

        public void SetNight()
        {
            correctionValue += 1200.0f - CurrentTime % 1200.0f;
            SendCurrentTimePacket();
        }

        public void SkipTime()
        {
            correctionValue += 600.0f - CurrentTime % 600.0f;
            SendCurrentTimePacket();
        }

        // Convenience for sending the TimeChange packet to a player or to all online players
        public void SendCurrentTimePacket(Player player = null)
        {
            if (player != null)
            {
                player.SendPacket(new TimeChange(CurrentTime));
            }
            else
            {
                PlayerManager playerManager = NitroxServiceLocator.LocateService<PlayerManager>();
                playerManager.SendPacketToAllPlayers(new TimeChange(CurrentTime));
            }
        }

        private float CurrentTime => subnauticaBeginTimeOffset + Convert.ToSingle((DateTime.UtcNow - ServerStartTime).TotalSeconds) + correctionValue;
    }
}
