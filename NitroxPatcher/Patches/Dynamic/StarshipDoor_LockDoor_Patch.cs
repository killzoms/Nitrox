using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class StarshipDoor_LockDoor_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(StarshipDoor).GetMethod(nameof(StarshipDoor.LockDoor), BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(StarshipDoor __instance)
        {
            if (!__instance.doorLocked)
            {
                NitroxId id = NitroxEntity.GetId(__instance.gameObject);
                StarshipDoorMetadata starshipDoorMetadata = new StarshipDoorMetadata(__instance.doorLocked, __instance.doorOpen);

                NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, starshipDoorMetadata);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
