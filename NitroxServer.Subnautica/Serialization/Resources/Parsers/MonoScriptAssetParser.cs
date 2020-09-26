using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer.Subnautica.Serialization.Resources.Parsers
{
    public class MonoScriptAssetParser : AssetParser
    {
        public static Dictionary<AssetIdentifier, MonoScriptAsset> MonoscriptsByAssetId { get; } = new Dictionary<AssetIdentifier, MonoScriptAsset>();

        public override void Parse(AssetIdentifier identifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            MonoScriptAsset monoscriptAsset = new MonoScriptAsset { Name = reader.ReadCountStringInt32() };

            MonoscriptsByAssetId.Add(identifier, monoscriptAsset);
        }

        public class MonoScriptAsset
        {
            public string Name { get; set; }
        }
    }
}
