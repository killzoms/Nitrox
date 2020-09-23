using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    /*
     * Relays Cyclops FireSuppressionSystem to other players
     * This method was used instead of the OnClick to ensure, that the the suppression really started
     */
    class CyclopsFireSuppressionButton_StartCooldown_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsFireSuppressionSystemButton).GetMethod(nameof(CyclopsFireSuppressionSystemButton.StartCooldown), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(CyclopsFireSuppressionSystemButton __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.subRoot.gameObject);
            NitroxServiceLocator.LocateService<Cyclops>().BroadcastActivateFireSuppression(id);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
