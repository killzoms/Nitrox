using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.GameLogic.Visibility
{
    public class ClipmapCell
    {
        public int Level;
        public NitroxInt3 Id;

        public ClipmapCell(int level, NitroxInt3 id)
        {
            Level = level;
            Id = id;
        }

        public List<AbsoluteEntityCell> Load(int cellSize)
        {
            NitroxInt3.Bounds blockRange = NitroxInt3.Bounds.FinerBounds(Id, cellSize); // WorldStreaming.OnCellLoaded
            return CellManager.ShowEntities(blockRange, Level);
        }

        public CellVisibility Reload(NitroxInt3 newId, int cellSize)
        {
            CellVisibility visibility = new CellVisibility();
            visibility.Removed = Load(cellSize);

            Id = newId;
            visibility.Added = Load(cellSize);
            return visibility;
        }
    }
}
