using System.Reflection;
using Harmony;
using NitroxClient.MonoBehaviours;

namespace NitroxPatcher.Patches.Persistent
{
    public class CellManager_GetPrefabForSlot_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CellManager).GetMethod(nameof(CellManager.GetPrefabForSlot), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(IEntitySlot slot, out EntitySlot.Filler __result)
        {
            __result = default;
            return !Multiplayer.Active;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
