using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel_Subnautica.Packets;
using UnityEngine;
using static Rocket;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Rocket_ElevatorControlButtonActivate_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Rocket).GetMethod(nameof(Rocket.ElevatorControlButtonActivate), BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(Rocket __instance, out RocketElevatorStates __state)
        {
            __state = __instance.elevatorState;
        }

        public static void Postfix(Rocket __instance, RocketElevatorStates __state)
        {
            if (__state != __instance.elevatorState)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);

                bool isGoingUp = __instance.elevatorState == RocketElevatorStates.Up || __instance.elevatorState == RocketElevatorStates.AtTop;
                NitroxServiceLocator.LocateService<Rockets>().CallElevator(id, RocketElevatorPanel.INTERNAL_PANEL, isGoingUp);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}

