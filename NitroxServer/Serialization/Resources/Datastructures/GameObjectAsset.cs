using System.Collections.Generic;

namespace NitroxServer.Serialization.Resources.DataStructures
{
    public class GameObjectAsset
    {
        public AssetIdentifier Identifier { get; set; }
        public string Name { get; set; }
        public List<AssetIdentifier> Components { get; } = new List<AssetIdentifier>();
    }
}
