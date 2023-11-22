using System.Reflection;
using NitroxClient.GameLogic;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class LargeWorldStreamer_UnloadBatch_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((LargeWorldStreamer t) => t.UnloadBatch(default(Int3)));

    public static void Prefix(Int3 index)
    {
        Resolve<Terrain>().BatchUnloaded(index);
    }
}
