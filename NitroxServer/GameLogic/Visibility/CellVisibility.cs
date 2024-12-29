using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.GameLogic.Visibility
{
    public class CellVisibility
    {
        public List<AbsoluteEntityCell> Added = new List<AbsoluteEntityCell>();
        public List<AbsoluteEntityCell> Removed = new List<AbsoluteEntityCell>();

        public override string ToString()
        {
            string result = "Added:\n";
            foreach (AbsoluteEntityCell cell in Added)
            {
                result += cell.ToString() + "\n";
            }
            result += "\nRemoved:\n";
            foreach (AbsoluteEntityCell cell in Removed)
            {
                result += cell.ToString() + "\n";
            }
            return result;
        }
    }
}
