using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ArmsController_Start_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ArmsController).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo reconfigureMethod = typeof(ArmsController).GetMethod("Reconfigure", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(ArmsController __instance)
        {
            reconfigureMethod.Invoke(__instance, new PlayerTool[] { null });
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
