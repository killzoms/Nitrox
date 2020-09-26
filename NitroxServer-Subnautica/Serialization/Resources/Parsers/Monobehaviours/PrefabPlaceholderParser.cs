using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer_Subnautica.Serialization.Resources.Parsers.MonoBehaviours
{
    public class PrefabPlaceholderParser : MonoBehaviourParser
    {
        public static Dictionary<AssetIdentifier, PrefabPlaceholderAsset> PrefabPlaceholderIdToPlaceholderAsset { get; } = new Dictionary<AssetIdentifier, PrefabPlaceholderAsset>();

        public override void Parse(AssetIdentifier identifier, AssetIdentifier gameObjectIdentifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            PrefabPlaceholderAsset prefabPlaceholderAsset = new PrefabPlaceholderAsset
            {
                Identifier = identifier,
                GameObjectIdentifier = gameObjectIdentifier,
                ClassId = reader.ReadCountStringInt32()
            };

            PrefabPlaceholderIdToPlaceholderAsset.Add(identifier, prefabPlaceholderAsset);
        }
    }
}
