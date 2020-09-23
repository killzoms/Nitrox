using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class StarshipDoor_OnDoorToggle_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(StarshipDoor).GetMethod("OnDoorToggle", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(StarshipDoor __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);
            StarshipDoorMetadata starshipDoorMetadata = new StarshipDoorMetadata(__instance.doorLocked, __instance.doorOpen);

            NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, starshipDoorMetadata);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
