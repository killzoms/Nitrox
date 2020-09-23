using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities.Metadata;

namespace NitroxPatcher.Patches.Dynamic
{
    public class KeypadDoorConsole_AcceptNumberField_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(KeypadDoorConsole).GetMethod("AcceptNumberField", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(KeypadDoorConsole __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);
            KeypadMetadata keypadMetadata = new KeypadMetadata(__instance.unlocked);

            NitroxServiceLocator.LocateService<Entities>().BroadcastMetadataUpdate(id, keypadMetadata);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
