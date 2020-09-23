using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class VehicleDockingBay_DockVehicle_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(VehicleDockingBay).GetMethod(nameof(VehicleDockingBay.DockVehicle), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(VehicleDockingBay __instance, Vehicle vehicle)
        {
            NitroxServiceLocator.LocateService<Vehicles>().BroadcastVehicleDocking(__instance, vehicle);
            return true;
        }

        public static void Postfix(VehicleDockingBay __instance, Vehicle vehicle)
        {
            NitroxServiceLocator.LocateService<Vehicles>().BroadcastVehicleDocking(__instance, vehicle);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}

