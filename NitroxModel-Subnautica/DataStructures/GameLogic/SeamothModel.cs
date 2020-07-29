using System;
using System.Collections.Generic;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using ProtoBufNet;

namespace NitroxModel_Subnautica.DataStructures.GameLogic
{
    [Serializable]
    [ProtoContract]
    public class SeamothModel : VehicleModel
    {
        [ProtoMember(1)]
        public bool LightOn { get; set; }

        protected SeamothModel()
        {
            // Constructor for serialization. Has to be "protected" for json serialization.
        }

        public SeamothModel(NitroxModel.DataStructures.GameLogic.NitroxTechType techType, List<InteractiveChildObjectIdentifier> interactiveChildIdentifiers, Optional<NitroxId> dockingBayId, string name, NitroxVector3[] hsb, float health) : base(techType, interactiveChildIdentifiers, dockingBayId, name, hsb, health)
        {
            LightOn = true;
        }
    }
}
