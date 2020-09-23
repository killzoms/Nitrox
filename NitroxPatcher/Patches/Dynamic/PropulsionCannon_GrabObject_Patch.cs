using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.GameLogic.HUD;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PropulsionCannon_GrabObject_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PropulsionCannon).GetMethod(nameof(PropulsionCannon.GrabObject), BindingFlags.Public | BindingFlags.Instance);

        private static PropulsionCannon cannon;
        private static GameObject grabbedObject;

        private static bool skipPrefixPatch;

        public static bool Prefix(PropulsionCannon __instance, GameObject target)
        {
            if (skipPrefixPatch)
            {
                return true;
            }

            cannon = __instance;
            grabbedObject = target;

            SimulationOwnership simulationOwnership = NitroxServiceLocator.LocateService<SimulationOwnership>();

            NitroxId id = NitroxEntity.GetId(grabbedObject);

            if (simulationOwnership.HasExclusiveLock(id))
            {
                Log.Debug($"Already have an exclusive lock on the grabbed propulsion cannon object: {id}");
                return true;
            }

            simulationOwnership.RequestSimulationLock(id, SimulationLockType.EXCLUSIVE, ReceivedSimulationLockResponse);

            return false;
        }

        private static void ReceivedSimulationLockResponse(NitroxId id, bool lockAquired)
        {
            if (lockAquired)
            {
                EntityPositionBroadcaster.WatchEntity(id, grabbedObject);

                skipPrefixPatch = true;
                cannon.GrabObject(grabbedObject);
                skipPrefixPatch = false;
            }
            else
            {
                grabbedObject.AddComponent<DenyOwnershipHand>();
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
