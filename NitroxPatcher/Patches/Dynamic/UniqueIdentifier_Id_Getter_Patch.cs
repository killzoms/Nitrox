using System.Reflection;
using NitroxClient.GameLogic.Helper;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

/// See <see cref="SerializationHelper.GetBytesWithoutParent"/> for more info.
public sealed partial class UniqueIdentifier_Id_Getter_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Property((UniqueIdentifier t) => t.Id).GetMethod;

    public static bool Prefix(string ___id, ref string __result)
    {
        if (___id == SerializationHelper.ID_IGNORE_KEY)
        {
            __result = null;
            return false;
        }

        return true;
    }
}
