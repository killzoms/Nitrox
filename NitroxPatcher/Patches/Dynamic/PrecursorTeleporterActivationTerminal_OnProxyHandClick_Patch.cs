using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PrecursorTeleporterActivationTerminal_OnProxyHandClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PrecursorTeleporterActivationTerminal).GetMethod(nameof(PrecursorTeleporterActivationTerminal.OnProxyHandClick), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(PrecursorTeleporterActivationTerminal __instance)
        {
            if (__instance.unlocked)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);
                PrecursorTeleporterActivationTerminalMetadata teleporterMetadata = new PrecursorTeleporterActivationTerminalMetadata(__instance.unlocked);

                NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, teleporterMetadata);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
