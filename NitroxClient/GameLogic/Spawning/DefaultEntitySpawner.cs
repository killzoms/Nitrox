﻿using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel_Subnautica.Helper;
using UnityEngine;
using UWE;

namespace NitroxClient.GameLogic.Spawning
{
    public class DefaultEntitySpawner : IEntitySpawner
    {
        public Optional<GameObject> Spawn(Entity entity, Optional<GameObject> parent, EntityCell cellRoot)
        {
            TechType techType = entity.TechType.Enum();
            GameObject prefab;
            IPrefabRequest prefabRequest = PrefabDatabase.GetPrefabAsync(entity.ClassId);
            if (!prefabRequest.TryGetPrefab(out prefab)) // I realize its more code but Sorry couldnt stand all the warnings
            {
                prefab = CraftData.GetPrefabForTechType(techType, false);
                if (prefab == null)
                {
                    return Optional<GameObject>.Of(Utils.CreateGenericLoot(techType));
                }
            }

            GameObject gameObject = Utils.SpawnFromPrefab(prefab, null);
            gameObject.transform.position = entity.Transform.Position;
            gameObject.transform.rotation = entity.Transform.Rotation;
            gameObject.transform.localScale = entity.Transform.LocalScale;

            if (parent.IsPresent())
            {
                gameObject.transform.SetParent(parent.Get().transform, true);
            }

            NitroxEntity.SetNewId(gameObject, entity.Id);
            CrafterLogic.NotifyCraftEnd(gameObject, techType);

            if (parent.IsPresent() && parent.Get().GetComponent<LargeWorldEntityCell>() != null)
            {
                LargeWorldEntity lwe = gameObject.GetComponent<LargeWorldEntity>();
                cellRoot.AddEntity(lwe);
            }

            gameObject.SetActive(true);

            return Optional<GameObject>.Of(gameObject);
        }

        public bool SpawnsOwnChildren()
        {
            return false;
        }
    }
}
