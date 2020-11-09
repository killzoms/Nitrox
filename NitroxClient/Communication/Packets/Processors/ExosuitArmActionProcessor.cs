using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors
{
    public class ExosuitArmActionProcessor : ClientPacketProcessor<ExosuitArmActionPacket>
    {
        public override void Process(ExosuitArmActionPacket packet)
        {
            Optional<GameObject> opGameObject = NitroxEntity.GetObjectFrom(packet.ArmId);
            if (!opGameObject.HasValue)
            {
                Log.Error("Could not find exosuit arm");
                return;
            }
            GameObject gameObject = opGameObject.Value;
            switch (packet.TechType)
            {

                case TechType.ExosuitClawArmModule:
                    ExosuitModuleEvent.UseClaw(gameObject.GetComponent<ExosuitClawArm>(), packet.ArmAction);
                    break;
                case TechType.ExosuitDrillArmModule:
                    ExosuitModuleEvent.UseDrill(gameObject.GetComponent<ExosuitDrillArm>(), packet.ArmAction);
                    break;
                case TechType.ExosuitGrapplingArmModule:
                    ExosuitModuleEvent.UseGrappling(gameObject.GetComponent<ExosuitGrapplingArm>(), packet.ArmAction, packet.Vector);
                    break;
                case TechType.ExosuitTorpedoArmModule:
                    ExosuitModuleEvent.UseTorpedo(gameObject.GetComponent<ExosuitTorpedoArm>(), packet.ArmAction, packet.Vector, packet.Rotation);
                    break;
                default:
                    Log.Error($"Got an arm tech that is not handled: {packet.TechType} with action: {packet.ArmAction} for id {packet.ArmId}");
                    break;
            }

        }
    }
}
