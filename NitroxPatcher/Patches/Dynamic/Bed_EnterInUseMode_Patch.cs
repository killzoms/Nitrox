using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxModel.Core;
using NitroxModel.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    class Bed_EnterInUseMode_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Bed).GetMethod("EnterInUseMode", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix()
        {
            NitroxServiceLocator.LocateService<IPacketSender>().Send(new BedEnter());
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
