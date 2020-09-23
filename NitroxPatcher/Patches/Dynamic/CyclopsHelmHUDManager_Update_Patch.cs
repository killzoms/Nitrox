using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    class CyclopsHelmHUDManager_Update_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsHelmHUDManager).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(CyclopsHelmHUDManager __instance, ref bool ___hudActive)
        {
            // To show the Cyclops HUD every time "hudActive" have to be true. "hornObject" is a good indicator to check if the player piloting the cyclops.
            if (!__instance.hornObject.activeSelf && ___hudActive)
            {
                __instance.canvasGroup.interactable = false;
            }
            else if (!___hudActive)
            {
                ___hudActive = true;
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
