using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Player_SetCurrentSub_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Player).GetMethod(nameof(Player.SetCurrentSub), BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(SubRoot sub)
        {
            NitroxId subId = null;

            if (sub)
            {
                subId = NitroxEntity.GetId(sub.gameObject);
            }

            NitroxServiceLocator.LocateService<LocalPlayer>().BroadcastSubrootChange(Optional.OfNullable(subId));
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
