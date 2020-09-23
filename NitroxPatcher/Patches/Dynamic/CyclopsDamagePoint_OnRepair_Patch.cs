﻿using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    /// <summary>
    /// <see cref="CyclopsExternalDamageManager.RepairPoint(CyclopsDamagePoint)"/> would seem like the correct method to patch, but adding to its postfix will
    /// execute before <see cref="CyclopsDamagePoint.OnRepair"/> is finished working. Both owners and non-owners will be able to repair damage points on a ship.
    /// </summary>
    class CyclopsDamagePoint_OnRepair_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsDamagePoint).GetMethod(nameof(CyclopsDamagePoint.OnRepair), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(CyclopsDamagePoint __instance)
        {
            // If the amount is high enough, it'll heal full
            NitroxServiceLocator.LocateService<Cyclops>().OnDamagePointRepaired(__instance.GetComponentInParent<SubRoot>(), __instance, 999);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
