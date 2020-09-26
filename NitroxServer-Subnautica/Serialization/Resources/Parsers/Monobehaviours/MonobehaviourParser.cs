using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer_Subnautica.Serialization.Resources.Parsers.MonoBehaviours
{
    public abstract class MonoBehaviourParser
    {
        public abstract void Parse(AssetIdentifier identifier, AssetIdentifier gameObjectIdentifier, AssetsFileReader reader, ResourceAssets resourceAssets);
    }
}
