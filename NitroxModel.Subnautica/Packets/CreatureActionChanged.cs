using System;
using NitroxModel.DataStructures;
using NitroxModel.Packets;
using NitroxModel.Subnautica.DataStructures.GameLogic.Creatures.Actions;

namespace NitroxModel.Subnautica.Packets
{
    [Serializable]
    public class CreatureActionChanged : Packet
    {
        public NitroxId Id { get; }
        public ISerializableCreatureAction NewAction { get; }

        public CreatureActionChanged(NitroxId id, ISerializableCreatureAction newAction)
        {
            Id = id;
            NewAction = newAction;
        }

        public override string ToString()
        {
            return $"[CreatureActionChanged - Id: {Id}, NewAction: {NewAction}]";
        }
    }
}
