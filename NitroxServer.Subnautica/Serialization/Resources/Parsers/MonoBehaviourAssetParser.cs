using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.Serialization.Resources.DataStructures;
using NitroxServer.Subnautica.Serialization.Resources.Parsers.MonoBehaviours;
using static NitroxServer.Subnautica.Serialization.Resources.Parsers.MonoScriptAssetParser;

namespace NitroxServer.Subnautica.Serialization.Resources.Parsers
{
    public class MonoBehaviourAssetParser : AssetParser
    {
        private static Dictionary<AssetIdentifier, MonobehaviourAsset> monoBehavioursByAssetId { get; } = new Dictionary<AssetIdentifier, MonobehaviourAsset>();

        private readonly Dictionary<string, MonoBehaviourParser> monoBehaviourParsersByMonoscriptName = new Dictionary<string, MonoBehaviourParser>()
        {
            { "WorldEntityData", new WorldEntityDataParser() },
            { "PrefabPlaceholder", new PrefabPlaceholderParser() },
            { "PrefabPlaceholdersGroup", new PrefabPlaceholdersGroupParser() },
            { "PrefabIdentifier", new PrefabIdentifierParser() },
            { "EntitySlot", new EntitySlotParser() }
        };

        public override void Parse(AssetIdentifier identifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            MonobehaviourAsset monoBehaviour = new MonobehaviourAsset
            {
                GameObjectIdentifier = new AssetIdentifier(reader.ReadInt32(), reader.ReadInt64()),
                Enabled = reader.ReadBoolean()
            };

            reader.Align();
            monoBehaviour.MonoscriptIdentifier = new AssetIdentifier(reader.ReadInt32(), reader.ReadInt64());
            monoBehaviour.Name = reader.ReadCountStringInt32();

            // Hack - If we have not yet loaded Monoscripts then we are currently processing unit MonoBehaviours 
            // that we do not care about.  Monoscripts should be fully loaded before we actually parse anything
            // we do care about in resource.assets.  If this becomes a problem later, we can do two passes and
            // load MonoBehaviours in the second pass.
            if (!MonoscriptsByAssetId.ContainsKey(monoBehaviour.MonoscriptIdentifier))
            {
                return;
            }

            MonoScriptAsset monoscript = MonoscriptsByAssetId[monoBehaviour.MonoscriptIdentifier];
            monoBehaviour.MonoscriptName = monoscript.Name;

            if (monoBehaviourParsersByMonoscriptName.TryGetValue(monoscript.Name, out MonoBehaviourParser monoResourceParser))
            {
                monoResourceParser.Parse(identifier, monoBehaviour.GameObjectIdentifier, reader, resourceAssets);
            }

            monoBehavioursByAssetId.Add(identifier, monoBehaviour);
        }
    }
}
