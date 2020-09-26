using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using NitroxModel.Packets;
using NitroxModel.Subnautica.DataStructures;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class SubNameInput_OnColorChange_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SubNameInput).GetMethod(nameof(SubNameInput.OnColorChange), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(SubNameInput __instance, ColorChangeEventData eventData)
        {
            SubName subName = (SubName)__instance.ReflectionGet("target");
            if (subName)
            {
                GameObject parentVehicle;
                Vehicle vehicle = subName.GetComponentInParent<Vehicle>();
                SubRoot subRoot = subName.GetComponentInParent<SubRoot>();
                Rocket rocket = subName.GetComponentInParent<Rocket>();

                if (vehicle)
                {
                    parentVehicle = vehicle.gameObject;
                }
                else if (rocket)
                {
                    parentVehicle = rocket.gameObject;
                }
                else
                {
                    parentVehicle = subRoot.gameObject;
                }

                NitroxId id = NitroxEntity.GetId(parentVehicle);
                VehicleColorChange packet = new VehicleColorChange(__instance.SelectedColorIndex, id, eventData.hsb.ToDto(), eventData.color.ToDto());
                NitroxServiceLocator.LocateService<IPacketSender>().Send(packet);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
