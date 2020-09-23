using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PrecursorTeleporter_OnActivateTeleporter_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PrecursorTeleporter).GetMethod("OnActivateTeleporter", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(PrecursorTeleporter __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);

            NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, new PrecursorTeleporterMetadata(__instance.isOpen));
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
