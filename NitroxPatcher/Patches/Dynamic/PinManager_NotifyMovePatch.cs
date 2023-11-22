using System;
using System.Linq;
using System.Reflection;
using NitroxClient.Helpers;
using NitroxClient.MonoBehaviours;
using NitroxModel.Helper;
using NitroxModel.Packets;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class PinManager_NotifyMove_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((PinManager t) => t.NotifyMove(default, default));

    public static void Prefix()
    {
        if (!Multiplayer.Main || !Multiplayer.Main.InitialSyncCompleted)
        {
            return;
        }
        // We can set a throttle as big as we want because the local player is the only one concerned
        Func<Packet, object> dedupeMethod = (packet) =>
        {
            return string.Join("", ((PinnedRecipeMoved)packet).RecipePins); // Makes a string of all the techtypes numbers
        };
        Resolve<ThrottledPacketSender>().SendThrottled(new PinnedRecipeMoved(PinManager.main.pins.Select(techType => (int)techType).ToList()), dedupeMethod, 1f);
    }
}
