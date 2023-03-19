using System.Collections.Generic;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using UnityEngine;
using UWE;

namespace NitroxClient.Communication.Packets.Processors
{
    class BatchEntitiesProcessor : ClientPacketProcessor<BatchEntities>
    {
        private readonly Entities entities;

        public BatchEntitiesProcessor(Entities entities)
        {
            this.entities = entities;
        }

        public override void Process(BatchEntities packet)
        {
            if (packet.ForceRespawn)
            {
                CleanupExistingEntities(packet.Entities);
            }

            CoroutineHost.StartCoroutine(entities.SpawnAsync(packet.Entities));
        }

        private void CleanupExistingEntities(List<Entity> dirtyEntities)
        {
            foreach (Entity entity in dirtyEntities)
            {
                entities.RemoveEntityHierarchy(entity);

                Optional<GameObject> gameObject = NitroxEntity.GetObjectFrom(entity.Id);

                if (gameObject.HasValue)
                {
                    UnityEngine.Object.Destroy(gameObject.Value);
                }
            }
        }
    }
}