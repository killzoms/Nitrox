using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxServer.ConsoleCommands;

namespace NitroxServer.GameLogic.Visibility
{
    public static class CellManager
    {
        public static readonly Dictionary<NitroxInt3, NitroxBatchCells> batch2cells = new Dictionary<NitroxInt3, NitroxBatchCells>();

        private static readonly Lazy<IMap> map = new(() => NitroxServiceLocator.LocateService<IMap>());

        public static NitroxBatchCells AddBatch(NitroxInt3 batchId)
        {
            NitroxBatchCells batchCells = new NitroxBatchCells(batchId);
            batch2cells.Add(batchId, batchCells);
            return batchCells;
        }

        public static NitroxBatchCells EnsureBatch(NitroxInt3 batchId)
        {
            if (!batch2cells.TryGetValue(batchId, out NitroxBatchCells batchCells))
            {
                return AddBatch(batchId);
            }

            return batchCells;
        }

        public static List<AbsoluteEntityCell> ShowEntities(NitroxInt3.Bounds blockRange, int level)
        {
            List<AbsoluteEntityCell> visibleCells = new List<AbsoluteEntityCell>();
            if (level > 3)
            {
                return visibleCells;
            }

            foreach (NitroxInt3 int3 in NitroxInt3.Bounds.OuterCoarserBounds(blockRange, map.Value.BatchDimensions))
            {
                if (batch2cells.TryGetValue(int3, out NitroxBatchCells batch))
                {
                    NitroxInt3 @int = int3 * map.Value.BatchDimensions;
                    NitroxInt3.Bounds bsRange = (blockRange - @int).Clamp(new NitroxInt3(0, 0, 0), map.Value.BatchDimensions - 1);

                    NitroxInt3 cellSize = NitroxBatchCells.GetCellSize(level, map.Value.BatchDimensions);

                    foreach (NitroxInt3 cell in NitroxInt3.Bounds.OuterCoarserBounds(bsRange, cellSize))
                    {
                        visibleCells.Add(batch.EnsureCell(level, cell));
                    }
                }
            }

            return visibleCells;
        }
    }
}
