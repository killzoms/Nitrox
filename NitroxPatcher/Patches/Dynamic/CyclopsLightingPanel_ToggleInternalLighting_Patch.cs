using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsLightingPanel_ToggleInternalLighting_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsLightingPanel).GetMethod(nameof(CyclopsLightingPanel.ToggleInternalLighting), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(CyclopsLightingPanel __instance, out bool __state)
        {
            __state = __instance.lightingOn;
            return true;
        }

        public static void Postfix(CyclopsLightingPanel __instance, bool __state)
        {
            if (__state != __instance.lightingOn)
            {
                NitroxId id = NitroxEntity.GetId(__instance.cyclopsRoot.gameObject);
                NitroxServiceLocator.LocateService<Cyclops>().BroadcastToggleInternalLight(id, __instance.lightingOn);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
