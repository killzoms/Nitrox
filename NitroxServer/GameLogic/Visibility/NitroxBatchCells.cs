using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.GameLogic.Visibility
{
    public class NitroxBatchCells
    {
        public NitroxInt3 batchId;
        private NitroxArray3<AbsoluteEntityCell> cellsTier0;
        private NitroxArray3<AbsoluteEntityCell> cellsTier1;
        private NitroxArray3<AbsoluteEntityCell> cellsTier2;
        private NitroxArray3<AbsoluteEntityCell> cellsTier3;

        public NitroxBatchCells(NitroxInt3 batchId)
        {
            this.batchId = batchId;
            InitCellTiers();
        }

        private void InitCellTiers()
        {
            cellsTier0 = new NitroxArray3<AbsoluteEntityCell>(NitroxMath.CeilShiftRight(10, 0));
            cellsTier1 = new NitroxArray3<AbsoluteEntityCell>(NitroxMath.CeilShiftRight(10, 1));
            cellsTier2 = new NitroxArray3<AbsoluteEntityCell>(NitroxMath.CeilShiftRight(10, 1));
            cellsTier3 = new NitroxArray3<AbsoluteEntityCell>(NitroxMath.CeilShiftRight(10, 1));
        }

        public AbsoluteEntityCell Add(NitroxInt3 cellId, int level)
        {
            NitroxArray3<AbsoluteEntityCell> cells = GetCells(level);
            AbsoluteEntityCell ret = new AbsoluteEntityCell(batchId, cellId, level);
            cells.Set(cellId.X, cellId.Y, cellId.Z, ret);
            return ret;
        }

        public AbsoluteEntityCell EnsureCell(int level, NitroxInt3 cellId)
        {
            AbsoluteEntityCell cell = Get(cellId, level);
            if (cell == null)
            {
                cell = Add(cellId, level);
            }

            return cell;
        }

        public AbsoluteEntityCell Get(NitroxInt3 cellId, int level)
        {
            return GetCells(level).Get(cellId.X, cellId.Y, cellId.Z);
        }

        private NitroxArray3<AbsoluteEntityCell> GetCells(int level)
        {
            switch (level)
            {
                case 0:
                    return cellsTier0;
                case 1:
                    return cellsTier1;
                case 2:
                    return cellsTier2;
                case 3:
                    return cellsTier3;
                default:
                    return null;
            }
        }

        public static NitroxInt3 GetCellSize(int level, NitroxInt3 blocksPerBatch)
        {
            NitroxInt3 size = blocksPerBatch / 10;
            switch (level)
            {
                case 0:
                    return size;
                case 1:
                case 2:
                case 3:
                    return size << 1;
                default:
                    Log.Debug($"Unexpected cell level {level} in GetCellSize");
                    return size;
            }
        }

    }
}
