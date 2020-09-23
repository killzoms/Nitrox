using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsMotorModeButton_OnClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsMotorModeButton).GetMethod(nameof(CyclopsMotorModeButton.OnClick), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(CyclopsMotorModeButton __instance, ref SubRoot ___subRoot, ref bool ___hudActive, out bool __state)
        {
            if (___subRoot != null && ___subRoot == Player.main.currentSub)
            {
                CyclopsHelmHUDManager cyclopsHUD = ___subRoot.gameObject.RequireComponentInChildren<CyclopsHelmHUDManager>();
                // To show the Cyclops HUD every time "hudActive" have to be true. "hornObject" is a good indicator to check if the player piloting the cyclops.
                if (___hudActive)
                {
                    __state = cyclopsHUD.hornObject.activeSelf;
                    return cyclopsHUD.hornObject.activeSelf;
                }
            }

            __state = false;
            return false;
        }

        public static void Postfix(CyclopsMotorModeButton __instance, bool __state, ref SubRoot ___subRoot)
        {
            if (__state)
            {
                NitroxId id = NitroxEntity.GetId(___subRoot.gameObject);
                NitroxServiceLocator.LocateService<Cyclops>().BroadcastChangeEngineMode(id, __instance.motorModeIndex);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
