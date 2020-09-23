using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Persistent
{
    public class ScreenshotManager_Initialise : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ScreenshotManager).GetMethod(nameof(ScreenshotManager.Initialize), BindingFlags.Public | BindingFlags.Static);

        public static void Prefix(ScreenshotManager __instance, ref string _savePath)
        {
            _savePath = "Nitrox Screenshots/";
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
