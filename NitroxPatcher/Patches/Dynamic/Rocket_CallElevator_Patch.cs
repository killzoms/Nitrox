using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Rocket_CallElevator_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Rocket).GetMethod(nameof(Rocket.CallElevator), BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(Rocket __instance, out Rocket.RocketElevatorStates __state)
        {
            __state = __instance.elevatorState;
        }

        public static void Postfix(Rocket __instance, bool up, Rocket.RocketElevatorStates __state)
        {
            if (__state != __instance.elevatorState)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);

                NitroxServiceLocator.LocateService<Rockets>().CallElevator(id, RocketElevatorPanel.EXTERNAL_PANEL, up);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
