using System.Reflection;
using Harmony;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class IngameMenu_QuitGameAsync_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(IngameMenu).GetMethod(nameof(IngameMenu.QuitGameAsync), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(bool quitToDesktop)
        {
            if (!quitToDesktop)
            {
                UWE.Utils.lockCursor = false;
            }
            else
            {
                Application.Quit();
            }
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
