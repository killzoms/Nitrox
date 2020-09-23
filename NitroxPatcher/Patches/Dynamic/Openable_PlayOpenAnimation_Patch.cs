using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Openable_PlayOpenAnimation_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Openable).GetMethod(nameof(Openable.PlayOpenAnimation), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(Openable __instance, bool openState, float duration)
        {
            if (__instance.isOpen != openState)
            {
                NitroxServiceLocator.LocateService<Interior>().OpenableStateChanged(NitroxEntity.GetId(__instance.gameObject), openState, duration);
            }
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
