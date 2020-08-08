using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ProtoBufNet;

namespace NitroxModel.DataStructures.GameLogic
{
    [Serializable]
    [ProtoContract]
    public class EscapePodModel : NitroxBehavior
    {
        public const int PLAYERS_PER_ESCAPEPOD = 50;

        [ProtoMember(1)]
        public NitroxId FabricatorId { get; set; }

        [ProtoMember(2)]
        public NitroxId MedicalFabricatorId { get; set; }

        [ProtoMember(3)]
        public NitroxId StorageContainerId { get; set; }

        [ProtoMember(4)]
        public NitroxId RadioId { get; set; }

        [ProtoMember(5)]
        public List<ushort> AssignedPlayers { get; set; } = new List<ushort>();

        [ProtoMember(6)]
        public bool Damaged { get; set; }

        [ProtoMember(7)]
        public bool RadioDamaged { get; set; }

        public EscapePodModel() // Protobuf
        { }

        protected EscapePodModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FabricatorId = (NitroxId)info.GetValue("fabricatorId", typeof(NitroxId));
            MedicalFabricatorId = (NitroxId)info.GetValue("medicalId", typeof(NitroxId));
            StorageContainerId = (NitroxId)info.GetValue("storageId", typeof(NitroxId));
            RadioId = (NitroxId)info.GetValue("radioId", typeof(NitroxId));
            AssignedPlayers = (List<ushort>)info.GetValue("assignedPlayers", typeof(List<ushort>));
            Damaged = info.GetBoolean("damaged");
            RadioDamaged = info.GetBoolean("radioDamaged");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("fabricatorId", FabricatorId);
            info.AddValue("medicalId", MedicalFabricatorId);
            info.AddValue("storageId", StorageContainerId);
            info.AddValue("radioId", RadioId);
            info.AddValue("assignedPlayers", AssignedPlayers);
            info.AddValue("damaged", Damaged);
            info.AddValue("radioDamaged", RadioDamaged);

        }

        public void InitEscapePodModel(NitroxId fabricatorId, NitroxId medicalFabricatorId, NitroxId storageContainerId, NitroxId radioId, bool damaged, bool radioDamaged)
        {
            FabricatorId = fabricatorId;
            MedicalFabricatorId = medicalFabricatorId;
            StorageContainerId = storageContainerId;
            RadioId = radioId;
            Damaged = damaged;
            RadioDamaged = radioDamaged;
        }

        public bool IsFull()
        {
            return AssignedPlayers.Count >= PLAYERS_PER_ESCAPEPOD;
        }

        public override string ToString()
        {
            return $"[EscapePodModel - Id: {Id} FabricatorId: {FabricatorId} MedicalFabricatorGuid: {MedicalFabricatorId} StorageContainerGuid: {StorageContainerId} RadioGuid: {RadioId} AssignedPlayers: {string.Join(", ", AssignedPlayers)} Damaged: {Damaged} RadioDamaged: {RadioDamaged}]";
        }
    }
}
