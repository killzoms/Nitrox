using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Exosuit_ArmSpawned_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Exosuit).GetMethod("UpdateColliders", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(Exosuit __instance)
        {
            NitroxServiceLocator.LocateService<ExosuitModuleEvent>().SpawnedArm(__instance);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}

