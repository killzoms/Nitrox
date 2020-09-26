namespace NitroxServer.GameLogic.Entities
{
    public class NitroxEntitySlot
    {
        public string[] AllowedTypes { get; }

        public string BiomeType { get; }

        public NitroxEntitySlot(string biomeType, string[] allowedTypes)
        {
            BiomeType = biomeType;
            AllowedTypes = allowedTypes;
        }
    }
}
