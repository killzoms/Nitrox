
using NitroxModel.DataStructures.Unity;

namespace NitroxModel.DataStructures.GameLogic.Entities
{
    public class UweWorldEntity
    {
        public NitroxTechType TechType { get; }
        public NitroxVector3 Scale { get; }
        public string ClassId { get; }
        public string SlotType { get; }
        public int CellLevel { get; }
        public bool PrefabZUp { get; }

        public UweWorldEntity(NitroxTechType techType, NitroxVector3 scale, string classId, string slotType, int cellLevel, bool prefabZUp)
        {
            TechType = techType;
            Scale = scale;
            ClassId = classId;
            SlotType = slotType;
            CellLevel = cellLevel;
            PrefabZUp = prefabZUp;
        }
    }
}
