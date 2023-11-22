#if DEBUG
using System.Reflection;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Persistent;

internal sealed partial class StartScreen_TryToShowDisclaimer_Patch : NitroxPatch, IPersistentPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((StartScreen t) => t.TryToShowDisclaimer());

    /// <summary>
    ///     Speed up startup in development by skipping disclaimer screen.
    /// </summary>
    public static bool Prefix() => false;
}
#endif
