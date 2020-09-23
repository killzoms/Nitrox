using System.Reflection;
using Harmony;
using NitroxClient.GameLogic.Helper;
using NitroxClient.MonoBehaviours;
using static NitroxClient.GameLogic.Helper.TransientLocalObjectManager.TransientObjectType;

namespace NitroxPatcher.Patches.Dynamic
{
    /**
     * A DeconstructableBase is initialize when an base piece is fully created (unintuitive - this is the thing that tells the
     * build that this object can be deconstructed.)  When this object is destroyed (we call deconstruct) we want to store its id
     * so that it can be later transferred to the ghost object that replaces it.
     */
    public class BaseDeconstructable_Deconstructor_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(BaseDeconstructable).GetMethod(nameof(BaseDeconstructable.Deconstruct), BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(BaseDeconstructable __instance)
        {
            TransientLocalObjectManager.Add(LATEST_DECONSTRUCTED_BASE_PIECE_GUID, NitroxEntity.GetId(__instance.gameObject));
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
