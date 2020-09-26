using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using ProtoBufNet;

namespace NitroxServer.GameLogic.Players
{
    [ProtoContract]
    public class PersistedPlayerData
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public List<EquippedItemData> EquippedItems { get; set; } = new List<EquippedItemData>();

        [ProtoMember(3)]
        public List<EquippedItemData> Modules { get; set; } = new List<EquippedItemData>();

        [ProtoMember(4)]
        public ushort Id { get; set; }

        [ProtoMember(5)]
        public NitroxVector3 SpawnPosition { get; set; }

        [ProtoMember(6)]
        public PlayerStatsData CurrentStats { get; set; }

        [ProtoMember(7)]
        public NitroxId SubRootId { get; set; }

        [ProtoMember(8)]
        public Perms Permissions { get; set; } = Perms.PLAYER;

        [ProtoMember(9)]
        public NitroxId NitroxId { get; set; }

        [ProtoMember(10)]
        public bool IsPermaDeath { get; set; }
    }
}
