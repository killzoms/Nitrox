using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EntityCell_QueueForAwake_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EntityCell).GetMethod(nameof(EntityCell.QueueForAwake), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(EntityCell __instance)
        {
            NitroxServiceLocator.LocateService<VisibleCellManager>().CellLoaded(__instance.BatchId, __instance.CellId, __instance.Level);
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
