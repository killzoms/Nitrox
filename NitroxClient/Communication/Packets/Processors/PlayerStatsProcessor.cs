using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic.HUD;
using NitroxClient.MonoBehaviours.Gui.HUD;
using NitroxModel.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    class PlayerStatsProcessor : ClientPacketProcessor<PlayerStats>
    {
        private readonly PlayerVitalsManager vitalsManager;

        public PlayerStatsProcessor(PlayerVitalsManager vitalsManager)
        {
            this.vitalsManager = vitalsManager;
        }

        public override void Process(PlayerStats packet)
        {
            RemotePlayerVitals vitals = vitalsManager.CreateForPlayer(packet.PlayerId);

            vitals.SetOxygen(packet.Oxygen, packet.MaxOxygen);
            vitals.SetHealth(packet.Health);
            vitals.SetFood(packet.Food);
            vitals.SetWater(packet.Water);
        }
    }
}
