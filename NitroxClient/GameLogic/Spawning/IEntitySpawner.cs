using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning
{
    /**
     * Allows us to create custom entity spawners for different entity types.
     */
    public interface IEntitySpawner
    {
        Optional<GameObject> Spawn(Entity entity, Optional<GameObject> parent, EntityCell cellRoot);
        bool SpawnsOwnChildren();
    }
}
