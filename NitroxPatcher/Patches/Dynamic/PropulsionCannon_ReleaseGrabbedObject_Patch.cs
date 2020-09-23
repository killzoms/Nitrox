using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PropulsionCannon_ReleaseGrabbedObject_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PropulsionCannon).GetMethod(nameof(PropulsionCannon.ReleaseGrabbedObject), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(PropulsionCannon __instance)
        {
            if (!__instance.grabbedObject)
            {
                return false;
            }

            NitroxId id = NitroxEntity.GetId(__instance.grabbedObject);

            // Request to be downgraded to a transient lock so we can still simulate the positioning.
            NitroxServiceLocator.LocateService<SimulationOwnership>().RequestSimulationLock(id, SimulationLockType.TRANSIENT, null);

            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
