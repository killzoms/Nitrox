﻿using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ExosuitClawArm_TryUse_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ExosuitClawArm).GetMethod("TryUse", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(bool __result, ExosuitClawArm __instance, float ___cooldownTime)
        {
            if (__result)
            {
                NitroxServiceLocator.LocateService<ExosuitModuleEvent>().BroadcastClawUse(__instance, ___cooldownTime);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
