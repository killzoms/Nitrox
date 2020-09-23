using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsMotorMode_SaveEngineStateAndPowerDown_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsMotorMode).GetMethod(nameof(CyclopsMotorMode.SaveEngineStateAndPowerDown), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(CyclopsMotorMode __instance, ref bool ___engineOnOldState)
        {
            // SN disable the engine if the player leave the cyclops. So this must be avoided.
            ___engineOnOldState = __instance.engineOn;
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
