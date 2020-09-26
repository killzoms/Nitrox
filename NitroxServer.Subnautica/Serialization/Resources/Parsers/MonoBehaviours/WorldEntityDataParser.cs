using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;
using UWE;

namespace NitroxServer.Subnautica.Serialization.Resources.Parsers.MonoBehaviours
{
    public class WorldEntityDataParser : MonoBehaviourParser
    {
        public override void Parse(AssetIdentifier identifier, AssetIdentifier gameObjectIdentifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            reader.Align();
            uint size = reader.ReadUInt32();

            for (int i = 0; i < size; i++)
            {
                WorldEntityInfo wei = new WorldEntityInfo
                {
                    classId = reader.ReadCountStringInt32(),
                    techType = (TechType)reader.ReadInt32(),
                    slotType = (EntitySlot.Type)reader.ReadInt32(),
                    prefabZUp = reader.ReadBoolean()
                };

                reader.Align();

                wei.cellLevel = (LargeWorldEntity.CellLevel)reader.ReadInt32();
                wei.localScale = new UnityEngine.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                resourceAssets.WorldEntitiesByClassId.Add(wei.classId, wei);
            }
        }
    }
}
