using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxModel.Packets;

namespace NitroxServer.GameLogic.Visibility
{
    public class ClipmapLevel
    {
        public int Id { get; }
        public int CellSize { get; }
        public NitroxInt3 ArraySize { get; }

        private NitroxInt3 centerCell = new NitroxInt3(-1, -1, -1);

        private NitroxArray3<ClipmapCell> cells;

        public ClipmapLevel(int id)
        {
            Id = id;
            CellSize = 16 << id;
            ArraySize = GetArraySize(Id);
            cells = new NitroxArray3<ClipmapCell>(ArraySize.X, ArraySize.Y, ArraySize.Z);
            
            foreach (NitroxInt3 int3 in NitroxInt3.Range(ArraySize))
            {
                cells.Set(int3.X, int3.Y, int3.Z, new ClipmapCell(Id, int3));
            }
        }

        public NitroxInt3 GetArraySize(int level)
        {
            switch (level)
            {
                case 0:
                case 1:
                case 2:
                    return new NitroxInt3(7, 7, 7);
                case 3:
                    return new NitroxInt3(8, 4, 8);
                default:
                    return new NitroxInt3(1, 1, 1);
            }
        }

        public bool UpdateCenter(NitroxInt3 position, out CellVisibility visibleCells)
        {
            visibleCells = null;
            NitroxInt3 prevCenter = centerCell;
            NitroxInt3 newCenter = NitroxInt3.FloorDiv(position, CellSize);
            if (newCenter == prevCenter)
            {
                return false;
            }

            Log.Debug(centerCell);
            centerCell = newCenter;
            foreach (NitroxInt3 cell in NitroxInt3.CenterSize(centerCell, ArraySize))
            {
                NitroxInt3 modCell = NitroxInt3.PositiveModulo(cell, ArraySize);
                ClipmapCell clipmapCell = cells.Get(modCell);
                if (clipmapCell.Id == cell)
                {
                    visibleCells = new CellVisibility();
                    visibleCells.Added = clipmapCell.Load(CellSize);
                }
                else
                {
                    visibleCells = clipmapCell.Reload(cell, CellSize);
                }
            }
            if (visibleCells == null)
            {
                Log.Error("Something went wrong when Updating Center");
                return false;
            }

            return true;
        }
    }
}
