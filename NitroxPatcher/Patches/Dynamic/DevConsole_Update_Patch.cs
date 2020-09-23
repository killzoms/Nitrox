using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    public class DevConsole_Update_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(DevConsole).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Prefix()
        {
            DevConsole.disableConsole = NitroxConsole.DisableConsole;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
