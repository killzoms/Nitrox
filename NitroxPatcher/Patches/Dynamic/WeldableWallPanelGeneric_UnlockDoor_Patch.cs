using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class WeldableWallPanelGeneric_UnlockDoor_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(WeldableWallPanelGeneric).GetMethod(nameof(WeldableWallPanelGeneric.UnlockDoor), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(WeldableWallPanelGeneric __instance)
        {
            if (__instance.liveMixin)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);
                WeldableWallPanelGenericMetadata weldableWallPanelGenericMetadata = new WeldableWallPanelGenericMetadata(__instance.liveMixin.health);

                NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, weldableWallPanelGenericMetadata);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
