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
    public class ExosuitModel : VehicleModel
    {
        [ProtoMember(10)]
        public NitroxId LeftArmId { get; }

        [ProtoMember(11)]
        public NitroxId RightArmId { get; }

        protected ExosuitModel()
        {
            // Constructor for serialization. Has to be "protected" for json serialization.
        }

        public ExosuitModel(NitroxTechType techType, List<InteractiveChildObjectIdentifier> interactiveChildIdentifiers, Optional<NitroxId> dockingBayId, string name, NitroxVector3[] hsb, float health) : base(techType, interactiveChildIdentifiers, dockingBayId, name, hsb, health)
        {
            LeftArmId = new NitroxId();
            RightArmId = new NitroxId();
        }

        public override string ToString()
        {
            return $"[ExosuitModel - LeftArmId: {LeftArmId} RightArmId: {RightArmId} \n{base.ToString()}]";
        }
    }
}

