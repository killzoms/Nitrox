using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Fabricator_OnCraftingBegin_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Fabricator).GetMethod("OnCraftingBegin", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(Fabricator __instance, TechType techType, float duration)
        {
            NitroxServiceLocator.LocateService<Crafting>().FabricatorCrafingStarted(__instance.gameObject, techType, duration);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
