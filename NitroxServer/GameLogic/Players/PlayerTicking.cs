using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Unity;
using NitroxModel.Helper;
using NitroxModel.Packets;
using NitroxServer.GameLogic.Entities;
using NitroxServer.GameLogic.Visibility;

namespace NitroxServer.GameLogic.Players;
public class PlayerTicking : ITickable
{
    public int TickId { get; set; }
    private Player player;
    private static readonly Lazy<IMap> map = new(() => NitroxServiceLocator.LocateService<IMap>());
    private readonly WorldEntityManager worldEntityManager;
    private readonly EntitySimulation entitySimulation;
    private ClipmapLevel[] levels;

    public PlayerTicking(Player player, WorldEntityManager worldEntityManager, EntitySimulation entitySimulation)
    {
        this.player = player;
        TickableTracker.RegisterTickable(this);

        this.worldEntityManager = worldEntityManager;
        this.entitySimulation = entitySimulation;

        levels = new ClipmapLevel[4];

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = new ClipmapLevel(i);
        }
    }

    public void Tick()
    {
        UpdateBatchVisibility();
        UpdateCellVisibility();
    }

    public void UpdateBatchVisibility()
    {
        NitroxVector3 playerPos = player.Position - new NitroxVector3(0, 8, 0);
        NitroxInt3 containingBatch = NitroxInt3.Floor(map.Value.MapTransform.InverseTransformPoint(playerPos)) / map.Value.BatchDimensions;
        NitroxInt3.Bounds effBounds = GetEffectiveBounds(containingBatch);

        if (TryGetBestBatch(playerPos, effBounds, out NitroxInt3 best))
        {
            CellManager.EnsureBatch(best);

            worldEntityManager.LoadUnspawnedEntities(best, false);
        }
    }

    private bool TryGetBestBatch(NitroxVector3 position, NitroxInt3.Bounds effBounds, out NitroxInt3 best)
    {
        best = new NitroxInt3();
        float num = float.MaxValue;
        bool result = false;

        foreach (NitroxInt3 batch in effBounds)
        {
            if (!worldEntityManager.loadedBatches.Contains(batch))
            {
                float squaredDistanceToBatch = GetSquaredDistanceToBatch(position, batch);
                if (squaredDistanceToBatch < num)
                {
                    best = batch;
                    num = squaredDistanceToBatch;
                    result = true;
                }
            }
        }

        return result;
    }

    private float GetSquaredDistanceToBatch(NitroxVector3 pos, NitroxInt3 batch)
    {
        return NitroxMath.GetPointToBoxDistanceSquared(pos, GetBatchMins(batch), GetBatchMaxs(batch));
    }

    public NitroxVector3 GetBatchMaxs(NitroxInt3 batchIndex)
    {
        NitroxVector3 position = ((batchIndex + 1) * map.Value.BatchDimensions);
        return map.Value.MapTransform.TransformPoint(position);
    }

    public NitroxVector3 GetBatchMins(NitroxInt3 batchIndex)
    {
        NitroxVector3 position = (batchIndex * map.Value.BatchDimensions);
        return map.Value.MapTransform.TransformPoint(position);
    }

    public NitroxInt3.Bounds GetEffectiveBounds(NitroxInt3 playerBatch)
    {
        return new NitroxInt3.Bounds(playerBatch - 1, playerBatch + 1);
    }

    public void UpdateCellVisibility()
    {
        NitroxInt3 streamingCenter = (NitroxInt3)map.Value.MapTransform.InverseTransformPoint(player.Position - new NitroxVector3(0, 8, 0));

        foreach (ClipmapLevel level in levels)
        {
            level.UpdateCenter(streamingCenter, out CellVisibility cellVisibility);

            if (cellVisibility != null)
            {
                player.AddCells(cellVisibility.Added);
                player.RemoveCells(cellVisibility.Removed);

                List<Entity> totalEntities = [];
                List<SimulatedEntity> totalSimulationChanges = [];

                foreach (AbsoluteEntityCell addedCell in cellVisibility.Added)
                {
                    worldEntityManager.LoadUnspawnedEntities(addedCell.BatchId, false);

                    totalSimulationChanges.AddRange(entitySimulation.GetSimulationChangesForCell(player, addedCell));
                    totalEntities.AddRange(worldEntityManager.GetEntities(addedCell));
                }

                foreach (AbsoluteEntityCell removedCell in cellVisibility.Removed)
                {
                    entitySimulation.FillWithRemovedCells(player, removedCell, totalSimulationChanges);
                }

                // Simulation update must be broadcasted before the entities are spawned
                if (totalSimulationChanges.Count > 0)
                {
                    entitySimulation.BroadcastSimulationChanges(new(totalSimulationChanges));
                }

                if (totalEntities.Count > 0)
                {
                    SpawnEntities batchEntities = new(totalEntities);
                    player.SendPacket(batchEntities);
                }
                //Log.Debug(cellVisibility);
            }
        }
    }
}
