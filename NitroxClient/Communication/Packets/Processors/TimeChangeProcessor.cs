using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxModel.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    public class TimeChangeProcessor : ClientPacketProcessor<TimeChange>
    {
        public override void Process(TimeChange packet)
        {
            DayNightCycle.main.timePassedAsDouble = packet.CurrentTime; //TODO: account for player latency
        }
    }
}
