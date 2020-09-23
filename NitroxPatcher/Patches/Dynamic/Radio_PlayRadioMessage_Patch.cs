using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxModel.Core;
using NitroxModel.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Radio_PlayRadioMessage_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Radio).GetMethod("PlayRadioMessage", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Prefix()
        {
            NitroxServiceLocator.LocateService<IPacketSender>().Send(new RadioPlayPendingMessage());
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
