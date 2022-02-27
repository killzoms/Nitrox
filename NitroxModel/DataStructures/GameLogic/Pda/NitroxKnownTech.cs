using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxModel.DataStructures.GameLogic.Pda
{
    public class NitroxKnownTech
    {
        public struct AnalysisTech
        {
            public NitroxTechType TechType;
            public string UnlockMessage;
            public List<NitroxTechType> UnlockTechTypes;
        }

        public struct CompoundTech
        {
            public NitroxTechType UnlockTechType;
            public List<NitroxTechType> Dependencies;
        }
    }
}
