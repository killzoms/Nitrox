using System.Collections.Generic;
using AssetsTools.NET;
using NitroxServer.GameLogic.Entities;
using NitroxServer.Serialization.Resources.DataStructures;

namespace NitroxServer.Subnautica.Serialization.Resources.Parsers.MonoBehaviours
{
    public class EntitySlotParser : MonoBehaviourParser
    {
        public static Dictionary<AssetIdentifier, NitroxEntitySlot> EntitySlotsByIdentifier { get; } = new Dictionary<AssetIdentifier, NitroxEntitySlot>();

        public override void Parse(AssetIdentifier identifier, AssetIdentifier gameObjectIdentifier, AssetsFileReader reader, ResourceAssets resourceAssets)
        {
            int count = reader.ReadInt32(); // Array Count
            string[] allowedTypes = new string[count];

            for (int i = 0; i < count; i++)
            {
                allowedTypes[i] = ((EntitySlot.Type)reader.ReadInt32()).ToString();
            }

            string biomeType = ((BiomeType)reader.ReadInt32()).AsString(); // Yes

            EntitySlotsByIdentifier.Add(gameObjectIdentifier, new NitroxEntitySlot(biomeType, allowedTypes));
        }
    }
}
