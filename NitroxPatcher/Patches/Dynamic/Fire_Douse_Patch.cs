using System;
using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Fire_Douse_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Fire).GetMethod(nameof(Fire.Douse), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(float) }, null);

        public static void Postfix(Fire __instance, float amount)
        {
            NitroxServiceLocator.LocateService<Fires>().OnDouse(__instance, !__instance.livemixin.IsAlive() || __instance.IsExtinguished() ? 10000 : amount);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
