using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EnergyMixin_SpawnDefault_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EnergyMixin).GetMethod(nameof(EnergyMixin.SpawnDefault), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(EnergyMixin __instance)
        {
            //Try to figure out if the default battery is spawned for a vehicle or cyclops
            if (__instance.gameObject.GetComponent<Vehicle>())
            {
                return false;
            }
            if (__instance.gameObject.GetComponentInParent<Vehicle>())
            {
                return false;
            }
            if (__instance.gameObject.GetComponentInParent<SubRoot>())
            {
                return false;
            }

            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
