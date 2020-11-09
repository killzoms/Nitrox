using System.Collections.Generic;
using System.ComponentModel;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Subnautica.DataStructures;
using NitroxModel.Subnautica.DataStructures.GameLogic;

namespace NitroxModel.Subnautica.Helper
{
    public static class VehicleModelFactory
    {
        public static VehicleModel BuildFrom(NitroxTechType techType, NitroxId constructedItemId, NitroxVector3 position, NitroxQuaternion rotation, List<InteractiveChildObjectIdentifier> interactiveChildIdentifiers, Optional<NitroxId> dockingBayId, string name, NitroxVector3[] hsb, float health)
        {
            return techType.ToUnity() switch
            {
                TechType.Seamoth => new SeamothModel(techType, constructedItemId, position, rotation, interactiveChildIdentifiers, dockingBayId, name, hsb, health),
                TechType.Exosuit => new ExosuitModel(techType, constructedItemId, position, rotation, interactiveChildIdentifiers, dockingBayId, name, hsb, health),
                TechType.Cyclops => new CyclopsModel(techType, constructedItemId, position, rotation, interactiveChildIdentifiers, dockingBayId, name, hsb, health),
                TechType.RocketBase => new NeptuneRocketModel(techType, constructedItemId, position, rotation, interactiveChildIdentifiers, dockingBayId, name, hsb, health),
                _ => throw new InvalidEnumArgumentException($"Could not build model from: {techType}")
            };
        }
    }
}
