using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PDAEncyclopedia_Add_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PDAEncyclopedia).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Prefix(string key)
        {
            NitroxServiceLocator.LocateService<PDAEncyclopediaEntry>().Add(key);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
