using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Radio_OnRepair_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Radio).GetMethod(nameof(Radio.OnRepair), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(Radio __instance)
        {
            NitroxServiceLocator.LocateService<EscapePodManager>().OnRadioRepairedByMe(__instance);
            NitroxServiceLocator.LocateService<EscapePodManager>().OnRadioRepairedByMe(__instance);
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
