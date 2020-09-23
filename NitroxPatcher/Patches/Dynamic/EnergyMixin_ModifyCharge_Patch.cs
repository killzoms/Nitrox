using System;
using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.GameLogic.Helper;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EnergyMixin_ModifyCharge_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EnergyMixin).GetMethod(nameof(EnergyMixin.ModifyCharge), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(EnergyMixin __instance, float amount)
        {
            GameObject battery = __instance.GetBattery();

            //Send package if power changed to next natural number
            if (battery && Math.Abs(Math.Floor(__instance.charge + amount) - Math.Floor(__instance.charge)) > 0.005f)
            {
                NitroxId instanceId = NitroxEntity.GetId(__instance.gameObject);
                ItemData batteryData = new ItemData(instanceId, NitroxEntity.GetId(battery), SerializationHelper.GetBytes(battery));

                NitroxServiceLocator.LocateService<StorageSlots>().EnergyMixinValueChanged(instanceId, Mathf.Floor(__instance.charge), batteryData);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
