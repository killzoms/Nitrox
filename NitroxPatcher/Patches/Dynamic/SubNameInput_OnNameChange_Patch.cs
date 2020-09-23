using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class SubNameInput_OnNameChange_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SubNameInput).GetMethod(nameof(SubNameInput.OnNameChange), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(SubNameInput __instance)
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
                VehicleNameChange packet = new VehicleNameChange(id, subName.GetName());
                NitroxServiceLocator.LocateService<IPacketSender>().Send(packet);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
