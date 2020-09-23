using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PrecursorDoorway_ToggleDoor_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PrecursorDoorway).GetMethod(nameof(PrecursorDoorway.ToggleDoor), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(PrecursorDoorway __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);
            NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, new PrecursorDoorwayMetadata(__instance.isOpen));
        }


        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
