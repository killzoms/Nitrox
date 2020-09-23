using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class uGUI_SignInput_OnDeselect_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(uGUI_SignInput).GetMethod(nameof(uGUI_SignInput.OnDeselect), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(uGUI_SignInput __instance)
        {
            NitroxServiceLocator.LocateService<Signs>().Changed(__instance);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
