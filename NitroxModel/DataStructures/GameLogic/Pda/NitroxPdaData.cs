using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxModel.DataStructures.GameLogic.Pda
{
    public class NitroxPdaData
    {
        public List<NitroxPdaLog.EntryData> Log = new List<NitroxPdaLog.EntryData>();

        public List<NitroxPdaEncyclopedia.EntryData> Encyclopedia = new List<NitroxPdaEncyclopedia.EntryData>();

        public List<NitroxPdaScanner.EntryData> Scanner = new List<NitroxPdaScanner.EntryData>();

        public List<NitroxTechType> DefaultTech = new List<NitroxTechType>();

        public List<NitroxKnownTech.AnalysisTech> AnalysisTech = new List<NitroxKnownTech.AnalysisTech>();

        public List<NitroxKnownTech.CompoundTech> CompoundTech = new List<NitroxKnownTech.CompoundTech>();
    }
}
