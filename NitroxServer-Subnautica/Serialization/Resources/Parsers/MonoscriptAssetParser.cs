using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer_Subnautica.Serialization.Resources.Parsers
{
    public class MonoscriptAssetParser : AssetParser
    {
        public static Dictionary<AssetIdentifier, MonoscriptAsset> MonoscriptsByAssetId { get; } = new Dictionary<AssetIdentifier, MonoscriptAsset>();

        public override void Parse(AssetIdentifier identifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            MonoscriptAsset monoscriptAsset = new MonoscriptAsset { Name = reader.ReadCountStringInt32() };

            MonoscriptsByAssetId.Add(identifier, monoscriptAsset);
        }

        public class MonoscriptAsset
        {
            public string Name { get; set; }
        }
    }
}
