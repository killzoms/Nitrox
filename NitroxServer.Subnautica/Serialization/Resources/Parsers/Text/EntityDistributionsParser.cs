using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer.Subnautica.Serialization.Resources.Parsers.Text
{
    public class EntityDistributionsParser : AssetParser
    {
        public override void Parse(AssetIdentifier identifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            reader.Align();
            resourceAssets.LootDistributionsJson = reader.ReadCountStringInt32().Replace("\\n", "");
        }
    }
}
