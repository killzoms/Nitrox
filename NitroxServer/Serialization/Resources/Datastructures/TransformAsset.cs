﻿using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.Serialization.Resources.DataStructures
{
    public class TransformAsset
    {
        public AssetIdentifier Identifier { get; set; }
        public AssetIdentifier GameObjectIdentifier { get; set; }
        public AssetIdentifier ParentIdentifier { get; set; }
        public NitroxQuaternion LocalRotation { get; set; }
        public NitroxVector3 LocalPosition { get; set; } // These were misnomers
        public NitroxVector3 LocalScale { get; set; }
    }
}
