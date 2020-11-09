using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Logger;

namespace NitroxPatcher.Patches.Dynamic
{
    public class DockedVehicleHandTarget_OnHandClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(DockedVehicleHandTarget).GetMethod(nameof(DockedVehicleHandTarget.OnHandClick), BindingFlags.Public | BindingFlags.Instance);

        private static DockedVehicleHandTarget dockedVehicle;
        private static VehicleDockingBay vehicleDockingBay;
        private static GUIHand guiHand;
        private static bool skipPrefix;

        private const string VEHICLE_BLOCKED_TEXT = "Another player is using this vehicle!";

        public static bool Prefix(DockedVehicleHandTarget __instance, GUIHand hand)
        {
            vehicleDockingBay = __instance.dockingBay;
            Vehicle vehicle = vehicleDockingBay.GetDockedVehicle();

            if (skipPrefix || !vehicle)
            {
                return true;
            }

            dockedVehicle = __instance;
            guiHand = hand;

            SimulationOwnership simulationOwnership = NitroxServiceLocator.LocateService<SimulationOwnership>();

            NitroxId id = NitroxEntity.GetId(vehicle.gameObject);

            if (simulationOwnership.HasExclusiveLock(id))
            {
                Log.Debug($"Already have an exclusive lock on this vehicle: {id}");
                return true;
            }

            simulationOwnership.RequestSimulationLock(id, SimulationLockType.EXCLUSIVE, ReceivedSimulationLockResponse);

            return false;
        }

        private static void ReceivedSimulationLockResponse(NitroxId id, bool lockAquired)
        {
            if (lockAquired)
            {
                NitroxServiceLocator.LocateService<Vehicles>().BroadcastVehicleUndocking(vehicleDockingBay, vehicleDockingBay.GetDockedVehicle());

                skipPrefix = true;
                targetMethod.Invoke(dockedVehicle, new[] { guiHand });
                skipPrefix = false;
            }
            else
            {
                HandReticle.main.SetInteractText(VEHICLE_BLOCKED_TEXT);
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
                dockedVehicle.isValidHandTarget = false;
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
