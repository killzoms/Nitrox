using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static NitroxModel.Serialization.ServerList;

namespace NitroxModel.DataStructures.GameLogic.Entities;

public static class EntityRegistry
{
    private static readonly Dictionary<NitroxId, IEntity> entitiesById = new Dictionary<NitroxId, IEntity>(); // All Entities
    private static readonly Dictionary<NitroxId, Dictionary<NitroxId, IEntity>> childEntitiesByParentId = new Dictionary<NitroxId, Dictionary<NitroxId, IEntity>>();

    private static readonly Dictionary<NitroxInt3, Dictionary<NitroxId, WorldEntity>> entitiesByBatchId = new Dictionary<NitroxInt3, Dictionary<NitroxId, WorldEntity>>();
    private static readonly Dictionary<AbsoluteEntityCell, Dictionary<NitroxId, WorldEntity>> entitiesByCellId = new Dictionary<AbsoluteEntityCell, Dictionary<NitroxId, WorldEntity>>();
    private static readonly Dictionary<NitroxId, WorldEntity> globalEntitiesById = new Dictionary<NitroxId, WorldEntity>();

    public static IEnumerable<Entity> GetAllEntities()
    {
        return GetEntitiesOfType<Entity>().Values;
    }

    public static T GetEntityById<T>(NitroxId id) where T : class, IEntity
    {
        entitiesById.TryGetValue(id, out IEntity rEntity);

        T entity = rEntity as T;

        return entity;
    }

    public static bool TryGetEntitiesByBatch(NitroxInt3 batchId, out Dictionary<NitroxId, WorldEntity> entities)
    {
        bool result = entitiesByBatchId.TryGetValue(batchId, out Dictionary<NitroxId, WorldEntity> rEntities);
        if (!result)
        {
            entities = new Dictionary<NitroxId, WorldEntity>();
            return result;
        }
        entities = new Dictionary<NitroxId, WorldEntity>(rEntities);

        return result;
    }

    public static bool TryGetEntitiesByCellId(AbsoluteEntityCell cell, out Dictionary<NitroxId, WorldEntity> entities)
    {
        bool result = entitiesByCellId.TryGetValue(cell, out Dictionary<NitroxId, WorldEntity> rEntities);
        if (!result)
        {
            entities = new Dictionary<NitroxId, WorldEntity>();
            return result;
        }
        entities = new Dictionary<NitroxId, WorldEntity>(rEntities);

        return result;
    }

    public static Dictionary<NitroxId, T> GetEntitiesOfType<T>() where T : class, IEntity
    {
        return entitiesById.Values.OfType<T>().ToDictionary(k => k.Id);
    }

    public static IEnumerable<WorldEntity> GetGlobalEntities()
    {
        return globalEntitiesById.Values;
    }

    public static void RegisterEntities(IEnumerable<IEntity> entities)
    {
        IEnumerator<IEntity> enumerator = entities.GetEnumerator();

        while (enumerator.MoveNext())
        {
            IEntity entity = enumerator.Current;

            RegisterEntity(entity);
        }
    }

    public static void RegisterEntity(IEntity entity)
    {
        if (entitiesById.ContainsKey(entity.Id))
        {
            throw new ArgumentException("Duplicate Id!");
        }

        WorldEntity wEntity = entity as WorldEntity;
        if (wEntity != null)
        {
            if (!wEntity.ExistsInGlobalRoot)
            {
                RegisterCellEntity(wEntity);
                RegisterBatchEntity(wEntity);
            }
            else
            {
                RegisterGlobalEntity(wEntity);
            }
        }

        entitiesById.Add(entity.Id, entity);
    }

    public static void AddOrUpdate(IEntity entity)
    {
        if (!entitiesById.ContainsKey(entity.Id))
        {
            RegisterEntity(entity);
            return;
        }
        UnregisterEntity(entity.Id);
        RegisterEntity(entity);
    }

    public static void UnregisterEntityFromTrackers(NitroxId entityId)
    {
        UnregisterGlobalEntity(entityId);
        UnregisterCellEntity(entityId);
    }

    public static void UnregisterEntity(NitroxId entityId)
    {
        if (entitiesById.Remove(entityId))
        {
            UnregisterGlobalEntity(entityId);
            UnregisterCellEntity(entityId);
        }
    }

    private static void RegisterGlobalEntity(WorldEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        if (!globalEntitiesById.ContainsKey(entity.Id))
        {
            globalEntitiesById.Add(entity.Id, entity);
        }
    }

    private static void UnregisterGlobalEntity(NitroxId entityId)
    {
        globalEntitiesById.Remove(entityId);
    }

    private static void RegisterBatchEntity(WorldEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        if (!entitiesByBatchId.TryGetValue(entity.AbsoluteEntityCell.BatchId, out Dictionary<NitroxId, WorldEntity> batchEntitiesById))
        {
            batchEntitiesById = new Dictionary<NitroxId, WorldEntity>();
            entitiesByBatchId.Add(entity.AbsoluteEntityCell.BatchId, batchEntitiesById);
        }

        batchEntitiesById.Add(entity.Id, entity);
    }

    private static void RegisterCellEntity(WorldEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        if (!entitiesByCellId.TryGetValue(entity.AbsoluteEntityCell, out Dictionary<NitroxId, WorldEntity> cellEntitiesById))
        {
            cellEntitiesById = new Dictionary<NitroxId, WorldEntity>();
            entitiesByCellId.Add(entity.AbsoluteEntityCell, cellEntitiesById);
        }

        cellEntitiesById.Add(entity.Id, entity);
    }

    private static void UnregisterCellEntity(NitroxId entityId)
    {
        foreach (Dictionary<NitroxId, WorldEntity> entitiesByCellId in entitiesByCellId.Values)
        {
            if (entitiesByCellId.ContainsKey(entityId))
            {
                entitiesByCellId.Remove(entityId);
                UnregisterBatchEntity(entityId, entitiesByCellId[entityId].AbsoluteEntityCell.BatchId);
            }
        }
    }

    private static void UnregisterCellEntity(NitroxId entityId, AbsoluteEntityCell cellId)
    {
        if (entitiesByCellId.TryGetValue(cellId, out Dictionary<NitroxId, WorldEntity> cellEntitiesById))
        {
            cellEntitiesById.Remove(entityId); // Remove from old cell
        }
    }

    public static void SwapEntityCell(IEntity entity, AbsoluteEntityCell oldCell)
    {
        WorldEntity wEntity = entity as WorldEntity;
        if (wEntity == null)
        {
            return;
        }

        UnregisterCellEntity(entity.Id, oldCell);
        if (wEntity.AbsoluteEntityCell.BatchId != oldCell.BatchId)
        {
            UnregisterBatchEntity(entity.Id, oldCell.BatchId);
            RegisterBatchEntity(wEntity);
        }

        RegisterCellEntity(wEntity);
    }

    private static void UnregisterBatchEntity(NitroxId entityId)
    {
        foreach (Dictionary<NitroxId, WorldEntity> batchEntitiesById in entitiesByBatchId.Values)
        {
            if (batchEntitiesById.ContainsKey(entityId))
            {
                batchEntitiesById.Remove(entityId);
            }
        }
    }

    private static void UnregisterBatchEntity(NitroxId entityId, NitroxInt3 batchId)
    {
        if (entitiesByBatchId.TryGetValue(batchId, out Dictionary<NitroxId, WorldEntity> batchEntitiesById))
        {
            batchEntitiesById.Remove(entityId); // Remove from old batch
        }
    }

    public static void SwapEntityBatch(IEntity entity, NitroxInt3 oldBatch)
    {
        WorldEntity wEntity = entity as WorldEntity;
        if (wEntity == null)
        {
            return;
        }

        UnregisterBatchEntity(entity.Id, oldBatch);
        RegisterBatchEntity(wEntity);
    }

    public static void SetParent(NitroxId parentId, NitroxId childId)
    {
        IEntity child = GetEntityById<Entity>(childId);
        if (child.ParentId != null && child.ParentId != parentId)
        {
            if (childEntitiesByParentId.TryGetValue(child.ParentId, out Dictionary<NitroxId, IEntity> oldParentChildrenById))
            {
                oldParentChildrenById.Remove(child.ParentId); // Remove from old parent
            }
        }

        if (!childEntitiesByParentId.TryGetValue(parentId, out Dictionary<NitroxId, IEntity> parentEntitiesByChild))
        {
            parentEntitiesByChild = new Dictionary<NitroxId, IEntity>();
            childEntitiesByParentId.Add(parentId, parentEntitiesByChild);
        }

        parentEntitiesByChild.Add(parentId, child);
        child.ParentId = parentId;
    }
}
