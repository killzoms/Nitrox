using System.Reflection;
using Harmony;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    class CyclopsHelmHUDManager_Start_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsHelmHUDManager).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly int engineOn = Animator.StringToHash("EngineOn");
        private static readonly int engineOff = Animator.StringToHash("EngineOff");

        public static void Postfix(CyclopsHelmHUDManager __instance, ref bool ___hudActive)
        {
            ___hudActive = true;
            __instance.engineToggleAnimator.SetTrigger(__instance.motorMode.engineOn ? engineOn : engineOff);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
