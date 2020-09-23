using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsSonarButton_OnClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsSonarButton).GetMethod(nameof(CyclopsSonarButton.OnClick), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(CyclopsSonarButton __instance, ref bool ____sonarActive)
        {
            NitroxId id = NitroxEntity.GetId(__instance.subRoot.gameObject);
            NitroxServiceLocator.LocateService<Cyclops>().BroadcastChangeSonarState(id, ____sonarActive);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }

    }
}
