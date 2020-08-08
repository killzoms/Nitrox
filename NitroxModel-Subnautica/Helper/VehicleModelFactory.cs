using System;
using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxModel_Subnautica.DataStructures;
using NitroxModel_Subnautica.DataStructures.GameLogic;

namespace NitroxModel_Subnautica.Helper
{
    public static class VehicleModelFactory
    {
        public static VehicleModel BuildFrom(ConstructorBeginCrafting packet)
        {
            return BuildFrom(packet.TechType, packet.ConstructedItemId, packet.Position, packet.Rotation, packet.InteractiveChildIdentifiers, packet.Name, packet.HSB, packet.Health);
        }

        public static VehicleModel BuildFrom(NitroxTechType techType, NitroxId objectId, NitroxVector3 position, NitroxQuaternion rotation,
                                             List<InteractiveChildObjectIdentifier> interactiveChildIdentifiers, string name, NitroxVector3[] hsb, float health)
        {
            NitroxObject obj = new NitroxObject(objectId);
            obj.Transform.Position = position;
            obj.Transform.Rotation = rotation;

            switch (techType.ToUnity())
            {
                case TechType.Seamoth:
                    SeamothModel seamothModel = new SeamothModel(techType, interactiveChildIdentifiers, Optional.Empty, name, hsb, health);
                    obj.AddBehavior(seamothModel);
                    return seamothModel;
                case TechType.Exosuit:
                    ExosuitModel exosuitModel = new ExosuitModel(techType, interactiveChildIdentifiers, Optional.Empty, name, hsb, health);
                    obj.AddBehavior(exosuitModel);
                    return exosuitModel;
                case TechType.Cyclops:
                    CyclopsModel cyclopsModel = new CyclopsModel(techType, interactiveChildIdentifiers, Optional.Empty, name, hsb, health);
                    obj.AddBehavior(cyclopsModel);
                    return cyclopsModel;
                case TechType.RocketBase:
                    return null;
                default:
                    throw new NotSupportedException($"Could not build model from: {techType}");
            }
        }
    }
}
