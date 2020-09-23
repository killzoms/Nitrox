using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    class CyclopsShieldButton_StopShield_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsShieldButton).GetMethod("StopShield", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(CyclopsShieldButton __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.subRoot.gameObject);
            // Shield is activated, if activeSprite is set as sprite
            bool isActive = (__instance.activeSprite == __instance.image.sprite);
            NitroxServiceLocator.LocateService<Cyclops>().BroadcastChangeShieldState(id, isActive);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
