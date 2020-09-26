using System;
using ProtoBufNet;

namespace NitroxServer.GameLogic
{
    [Serializable]
    [ProtoContract]
    public class StoryTimingData
    {
        [ProtoMember(1)]
        public double ElapsedTime { get; set; }

        [ProtoMember(2)]
        public double? AuroraExplosionTime { get; set; }

        public static StoryTimingData From(EventTriggerer eventTriggerer)
        {
            StoryTimingData inst = new StoryTimingData
            {
                ElapsedTime = eventTriggerer.GetRealElapsedTime(),
                AuroraExplosionTime = eventTriggerer.AuroraExplosionTime
            };
            return inst;
        }
    }
}
