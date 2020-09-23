using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PilotingChair_OnPlayerDeath_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PilotingChair).GetMethod("OnPlayerDeath", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(PilotingChair __instance)
        {
            SubRoot subRoot = __instance.GetComponentInParent<SubRoot>();
            Validate.NotNull(subRoot, "PilotingChair cannot find it's corresponding SubRoot!");
            NitroxId id = NitroxEntity.GetId(subRoot.gameObject);

            // Request to be downgraded to a transient lock so we can still simulate the positioning.
            NitroxServiceLocator.LocateService<SimulationOwnership>().RequestSimulationLock(id, SimulationLockType.TRANSIENT, null);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
