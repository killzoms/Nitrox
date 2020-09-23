using System.Reflection;
using Harmony;
using UWE;

namespace NitroxPatcher.Patches.Dynamic
{
    public class FreezeTime_Begin_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(FreezeTime).GetMethod(nameof(FreezeTime.Begin), BindingFlags.Public | BindingFlags.Static);

        public static bool Prefix(string userId)
        {
            return userId.Equals("FeedbackPanel");
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
