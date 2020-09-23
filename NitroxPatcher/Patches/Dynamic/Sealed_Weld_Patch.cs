using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Sealed_Weld_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Sealed).GetMethod(nameof(Sealed.Weld), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(Sealed __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);

            NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, new SealedDoorMetadata(__instance._sealed, __instance.openedAmount));
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
