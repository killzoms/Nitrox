using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EscapePod_Awake_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EscapePod).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix(EscapePod __instance)
        {
            return !EscapePodManager.SuppressEscapePodAwakeMethod;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
