using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PrefabPlaceholdersGroup_Spawn_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PrefabPlaceholdersGroup).GetMethod(nameof(PrefabPlaceholdersGroup.Spawn), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix()
        {
            return false; // Disable spawning of PrefabPlaceholders(In other words large portion of objects)
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
