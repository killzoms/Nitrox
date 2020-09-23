using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;
using NitroxModel_Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ExosuitGrapplingArm_OnHit_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ExosuitGrapplingArm).GetMethod(nameof(ExosuitGrapplingArm.OnHit), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(ExosuitGrapplingArm __instance, GrapplingHook ___hook)
        {
            Exosuit componentInParent = __instance.GetComponentInParent<Exosuit>();

            return !componentInParent || componentInParent.GetPilotingMode();
        }

        public static void Postfix(ExosuitGrapplingArm __instance, GrapplingHook ___hook)
        {
            // We send the hook direction to the other player so he sees where the other player exosuit is heading
            NitroxServiceLocator.LocateService<ExosuitModuleEvent>().BroadcastArmAction(TechType.ExosuitGrapplingArmModule, __instance, ExosuitArmAction.START_USE_TOOL, ___hook.rb.velocity, null);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
