using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Vehicle_OnPilotModeBegin_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Vehicle).GetMethod("OnPilotModeBegin", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Prefix(Vehicle __instance)
        {
            NitroxServiceLocator.LocateService<Vehicles>().BroadcastOnPilotModeChanged(__instance, true);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
