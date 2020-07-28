using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic.Bases;
using NitroxModel.Logger;
using NitroxModel.Packets;

namespace NitroxClient.Communication.Packets.Processors
{
    public class ConstructionCompletedProcessor : ClientPacketProcessor<ConstructionCompleted>
    {
        private readonly BuildThrottlingQueue buildEventQueue;

        public ConstructionCompletedProcessor(BuildThrottlingQueue buildEventQueue)
        {
            this.buildEventQueue = buildEventQueue;
        }

        public override void Process(ConstructionCompleted packet)
        {
            Log.Debug("Processing ConstructionCompleted " + packet.PieceId + " " + packet.BaseId);
            buildEventQueue.EnqueueConstructionCompleted(packet.PieceId, packet.BaseId);
        }
    }
}
