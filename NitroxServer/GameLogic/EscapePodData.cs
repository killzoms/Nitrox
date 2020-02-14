using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using ProtoBufNet;

namespace NitroxServer.GameLogic
{
    [ProtoContract]
    public class EscapePodData
    {
        [ProtoMember(3)]
        public EscapePodModel PodNotFullYet;

        [ProtoMember(1)]
        public List<EscapePodModel> EscapePods = new List<EscapePodModel>();

        [ProtoMember(2)]
        public Dictionary<ushort, EscapePodModel> EscapePodsByPlayerId = new Dictionary<ushort, EscapePodModel>();
    }
}
