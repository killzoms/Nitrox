using System.Collections.Generic;
using System.Linq;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Unity;
using NitroxModel.DataStructures.Util;
using NitroxModel.Helper;
using NitroxServer.GameLogic.Entities.Spawning;
using NitroxModel.DataStructures.GameLogic.Entities;

namespace NitroxServer.GameLogic.Entities
{
    public class WorldEntityManager
    {
        private readonly BatchEntitySpawner batchEntitySpawner;

        public WorldEntityManager(BatchEntitySpawner batchEntitySpawner)
        {
            this.batchEntitySpawner = batchEntitySpawner;
        }

        public List<WorldEntity> GetEntities(NitroxInt3 batchId)
        {
            if (!EntityRegistry.TryGetEntitiesByBatch(batchId, out Dictionary<NitroxId, WorldEntity> entitiesById))
            {
                LoadUnspawnedEntities(batchId, false);

                if (!EntityRegistry.TryGetEntitiesByBatch(batchId, out entitiesById))
                {
                    return new List<WorldEntity>();
                }
            }
            
            return entitiesById.Values.ToList();
        }

        public List<WorldEntity> GetEntities(AbsoluteEntityCell cellId)
        {
            if (!EntityRegistry.TryGetEntitiesByCellId(cellId, out Dictionary<NitroxId, WorldEntity> entitiesById))
            {
                LoadUnspawnedEntities(cellId.BatchId, false);

                if (!EntityRegistry.TryGetEntitiesByCellId(cellId, out entitiesById))
                {
                    return new List<WorldEntity>();
                }
            }

            return entitiesById.Values.ToList();
        }

        public Optional<AbsoluteEntityCell> UpdateEntityPosition(NitroxId id, NitroxVector3 position, NitroxQuaternion rotation)
        {
            Optional<WorldEntity> opEntity = EntityRegistry.GetEntityById<WorldEntity>(id);

            if (!opEntity.HasValue)
            {
                Log.Debug("Could not update entity position because it was not found (maybe it was recently picked up)");
                return Optional.Empty;
            }

            WorldEntity entity = opEntity.Value;
            AbsoluteEntityCell oldCell = entity.AbsoluteEntityCell;

            entity.Transform.Position = position;
            entity.Transform.Rotation = rotation;

            AbsoluteEntityCell newCell = entity.AbsoluteEntityCell;
            if (oldCell != newCell)
            {
                EntitySwitchedCells(entity, oldCell, newCell);
            }

            return Optional.Of(newCell);
        }

        public void TrackEntityInTheWorld(WorldEntity entity)
        {
            EntityRegistry.AddOrUpdate(entity);
        }

        public void LoadAllUnspawnedEntities(System.Threading.CancellationToken token)
        {            
            IMap map = NitroxServiceLocator.LocateService<IMap>();

            int totalEntites = 0;

            for (int x = 0; x < map.DimensionsInBatches.X; x++)
            {
                token.ThrowIfCancellationRequested();
                for (int y = 0; y < map.DimensionsInBatches.Y; y++)
                {
                    for (int z = 0; z < map.DimensionsInBatches.Z; z++)
                    {
                        int spawned = LoadUnspawnedEntities(new(x, y, z), true);

                        Log.Debug($"Loaded {spawned} entities from batch ({x}, {y}, {z})");

                        totalEntites += spawned;
                    }
                }

                if (totalEntites > 0)
                {
                    Log.Info($"Loading: {(int)((totalEntites/ 504732.0) * 100)}%");
                }
            }
        }

        public bool IsBatchSpawned(NitroxInt3 batchId)
        {
            return batchEntitySpawner.IsBatchSpawned(batchId);
        }

        public int LoadUnspawnedEntities(NitroxInt3 batchId, bool suppressLogs)
        {
            List<Entity> spawnedEntities = batchEntitySpawner.LoadUnspawnedEntities(batchId, suppressLogs);

            List<WorldEntity> nonCellRootEntities = spawnedEntities.Where(entity => typeof(WorldEntity).IsAssignableFrom(entity.GetType()) &&
                                                                                    entity.GetType() != typeof(CellRootEntity))
                                                                   .Cast<WorldEntity>()
                                                                   .ToList();

            // UWE stores entities serialized with a handful of parent cell roots.  These only represent a small fraction of all possible cell
            // roots that could exist.  There is no reason for the server to know about these and much easier to consider top-level world entities
            // as positioned globally and not locally.  Thus, we promote cell root children to top level and throw the cell roots away. 
            foreach (CellRootEntity cellRoot in spawnedEntities.OfType<CellRootEntity>())
            {
                foreach (WorldEntity worldEntity in cellRoot.ChildEntities.Cast<WorldEntity>())
                {
                    worldEntity.ParentId = null;
                    worldEntity.Transform.SetParent(null, true);
                }

                cellRoot.ChildEntities = new List<Entity>();
            }

            EntityRegistry.RegisterEntities(nonCellRootEntities);

            foreach (WorldEntity entity in nonCellRootEntities)
            {
                List<WorldEntity> entitiesInBatch = GetEntities(entity.AbsoluteEntityCell.BatchId);
                entitiesInBatch.Add(entity);

                List<WorldEntity> entitiesInCell = GetEntities(entity.AbsoluteEntityCell);
                entitiesInCell.Add(entity);
            }

            return nonCellRootEntities.Count;
        }

        private void EntitySwitchedCells(WorldEntity entity, AbsoluteEntityCell oldCell, AbsoluteEntityCell newCell)
        {
            if (entity.ExistsInGlobalRoot)
            {
                return; // We don't care what cell a global root entity resides in.  Only phasing entities.
            }

            EntityRegistry.SwapEntityCell(entity, oldCell);
        }

        public void StopTrackingEntity(WorldEntity entity)
        {
            EntityRegistry.UnregisterEntityFromTrackers(entity.Id);
        }

        public bool TryDestroyEntity(NitroxId entityId, out Entity entity)
        {
            EntityRegistry.UnregisterEntity(entityId);
            entity = EntityRegistry.GetEntityById<Entity>(entityId);

            if (entity != null)
            {
                return false;
            }

            if (entity is WorldEntity worldEntity)
            {
                StopTrackingEntity(worldEntity);
            }

            return true;
        }
    }
}
