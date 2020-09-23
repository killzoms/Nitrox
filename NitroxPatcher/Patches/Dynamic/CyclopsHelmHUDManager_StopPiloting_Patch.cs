using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsHelmHUDManager_StopPiloting_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsHelmHUDManager).GetMethod(nameof(CyclopsHelmHUDManager.StopPiloting), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(CyclopsHelmHUDManager __instance, ref bool ___hudActive)
        {
            ___hudActive = true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
