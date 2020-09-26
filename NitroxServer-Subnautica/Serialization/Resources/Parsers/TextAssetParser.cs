using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;
using NitroxServer_Subnautica.Serialization.Resources.Parsers.Text;

namespace NitroxServer_Subnautica.Serialization.Resources.Parsers
{
    public class TextAssetParser : AssetParser
    {
        private readonly Dictionary<string, AssetParser> textParsersByAssetName = new Dictionary<string, AssetParser>()
        {
            { "EntityDistributions", new EntityDistributionsParser() }
        };

        public override void Parse(AssetIdentifier identifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            string assetName = reader.ReadCountStringInt32();

            if (textParsersByAssetName.TryGetValue(assetName, out AssetParser textResourceParser))
            {
                textResourceParser.Parse(identifier, reader, resourceAssets);
            }
        }
    }
}
