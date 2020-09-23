using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class uGUI_OnApplicationQuit_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(uGUI).GetMethod("OnApplicationQuit", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Prefix()
        {
            NitroxServiceLocator.LocateService<IMultiplayerSession>().Disconnect();
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
