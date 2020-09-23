using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Persistent
{
    public class SentrySdk_Start_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SentrySdk).GetMethod(nameof(SentrySdk.Start), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(SentrySdk __instance)
        {
            UnityEngine.Object.Destroy(__instance);
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
