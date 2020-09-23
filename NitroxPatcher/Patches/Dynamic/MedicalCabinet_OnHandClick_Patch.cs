using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class MedicalCabinet_OnHandClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(MedicalCabinet).GetMethod(nameof(MedicalCabinet.OnHandClick), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(MedicalCabinet __instance)
        {
            NitroxServiceLocator.LocateService<MedkitFabricator>().Clicked(__instance);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
