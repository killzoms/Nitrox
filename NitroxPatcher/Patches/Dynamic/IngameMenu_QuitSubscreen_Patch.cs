using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class IngameMenu_QuitSubscreen_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(IngameMenu).GetMethod(nameof(IngameMenu.QuitSubscreen), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix()
        {
            IngameMenu.main.QuitGame(false);
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
