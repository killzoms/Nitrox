using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using NitroxModel_Subnautica.DataStructures;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors
{
    class ItemPositionProcessor : ClientPacketProcessor<ItemPosition>
    {
        private const float ITEM_TRANSFORM_SMOOTH_PERIOD = 0.25f;

        public override void Process(ItemPosition packet)
        {
            Optional<GameObject> opItem = NitroxEntity.GetObjectFrom(packet.Id);

            if (opItem.HasValue)
            {
                MovementHelper.MoveRotateGameObject(opItem.Value, packet.Position.ToUnity(), packet.Rotation.ToUnity(), ITEM_TRANSFORM_SMOOTH_PERIOD);
            }
        }
    }
}
