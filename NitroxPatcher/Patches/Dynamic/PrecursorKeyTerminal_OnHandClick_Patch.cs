using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PrecursorKeyTerminal_OnHandClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PrecursorKeyTerminal).GetMethod(nameof(PrecursorKeyTerminal.OnHandClick), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(PrecursorKeyTerminal __instance)
        {
            if (__instance.slotted)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);

                NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, new PrecursorKeyTerminalMetadata(__instance.slotted));
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
