using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class SubNameInput_OnNameChange_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SubNameInput).GetMethod(nameof(SubNameInput.OnNameChange), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(SubNameInput __instance, SubName ___target)
        {
            if (___target)
            {
                GameObject parentVehicle;
                Vehicle vehicle = ___target.GetComponentInParent<Vehicle>();
                SubRoot subRoot = ___target.GetComponentInParent<SubRoot>();
                Rocket rocket = ___target.GetComponentInParent<Rocket>();

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
                VehicleNameChange packet = new VehicleNameChange(id, ___target.GetName());
                NitroxServiceLocator.LocateService<IPacketSender>().Send(packet);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
