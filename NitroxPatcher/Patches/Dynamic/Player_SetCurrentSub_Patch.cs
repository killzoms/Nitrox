using System.Reflection;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class Player_SetCurrentSub_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((Player t) => t.SetCurrentSub(default, default));

    public static void Prefix(Player __instance, SubRoot sub)
    {
        // We really want to avoid unnecessary packets giving false information
        if (!Multiplayer.Main || !Multiplayer.Main.InitialSyncCompleted)
        {
            return;
        }

        // When in the water of the moonpool, it can happen that you hammer change requests
        // while the sub is not changed. This will prevent that
        if (__instance.GetCurrentSub() != sub)
        {
            Resolve<LocalPlayer>().BroadcastSubrootChange(sub.GetId());
        }
    }
}
