using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxModel.Core;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class IngameMenu_QuitGame_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(IngameMenu).GetMethod(nameof(IngameMenu.QuitGame), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix()
        {
            NitroxServiceLocator.LocateService<IMultiplayerSession>().Disconnect();
            Application.Quit();
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
