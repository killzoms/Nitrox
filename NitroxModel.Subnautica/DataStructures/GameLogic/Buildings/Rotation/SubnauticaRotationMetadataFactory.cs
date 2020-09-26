﻿using NitroxModel.DataStructures.GameLogic.Buildings.Rotation;
using NitroxModel.DataStructures.Util;
using NitroxModel.Helper;
using NitroxModel.Subnautica.DataStructures.GameLogic.Buildings.Rotation.Metadata;

namespace NitroxModel.Subnautica.DataStructures.GameLogic.Buildings.Rotation
{
    public class SubnauticaRotationMetadataFactory : IRotationMetadataFactory
    {
        public Optional<RotationMetadata> From(object ghost)
        {
            RotationMetadata rotationMetadata = null;

            switch (ghost)
            {
                case BaseAddCorridorGhost corridorGhost:
                    {
                        int rotation = (int)corridorGhost.ReflectionGet("rotation");
                        rotationMetadata = new CorridorRotationMetadata(rotation);
                        break;
                    }
                case BaseAddMapRoomGhost baseAddMapRoomGhost:
                    {
                        BaseAddMapRoomGhost mapRoomGhost = baseAddMapRoomGhost;
                        Base.CellType cellType = (Base.CellType)mapRoomGhost.ReflectionGet("cellType");
                        int connectionMask = (int)mapRoomGhost.ReflectionGet("connectionMask");
                        rotationMetadata = new MapRoomRotationMetadata((byte)cellType, connectionMask);
                        break;
                    }
                case BaseAddModuleGhost baseAddModuleGhost:
                    {
                        BaseAddModuleGhost module = baseAddModuleGhost;

                        Int3 cell = module.anchoredFace.Value.cell;
                        int direction = (int)module.anchoredFace.Value.direction;

                        rotationMetadata = new BaseModuleRotationMetadata(cell, direction);
                        break;
                    }
            }

            return Optional.OfNullable(rotationMetadata);
        }
    }
}
