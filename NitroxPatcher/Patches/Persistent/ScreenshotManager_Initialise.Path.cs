using System.IO;
using System.Reflection;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Persistent
{
    public partial class ScreenshotManager_Initialise : NitroxPatch, IPersistentPatch
    {
        public override MethodInfo targetMethod { get; } = Reflect.Method(() => ScreenshotManager.Initialize(default(string)));

        public static void Prefix(ScreenshotManager __instance, ref string _savePath)
        {
            _savePath = Path.GetFullPath(NitroxUser.LauncherPath ?? ".");
        }
    }
}
