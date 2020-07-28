using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic.Spawning.Metadata;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures.Util;
using NitroxModel.Helper;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors
{
    public class EntityMetadataUpdateProcessor : ClientPacketProcessor<EntityMetadataUpdate>
    {
        public override void Process(EntityMetadataUpdate packet)
        {
            GameObject gameObject = NitroxEntity.RequireObjectFrom(packet.Id);

            Optional<EntityMetadataProcessor> metadataProcessor = EntityMetadataProcessor.FromMetaData(packet.NewValue);
            Validate.IsTrue(metadataProcessor.HasValue, $"No processor found for EntityMetadata of type {packet.NewValue.GetType()}");

            metadataProcessor.Value.ProcessMetadata(gameObject, packet.NewValue);
        }
    }
}
