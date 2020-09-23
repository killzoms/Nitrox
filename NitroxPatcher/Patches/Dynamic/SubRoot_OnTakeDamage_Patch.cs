using System;
using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    /// <summary>
    /// Hook onto <see cref="SubRoot.OnTakeDamage(DamageInfo)"/>. It'd be nice if this were the only hook needed, but both damage points and fires are created in a separate
    /// class that doesn't necessarily finish running after OnTakeDamage finishes. Since that's the case, this is used only to stop phantom damage alerts that the owner didn't register
    /// </summary>
    public class SubRoot_OnTakeDamage_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SubRoot).GetMethod(nameof(SubRoot.OnTakeDamage), BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(DamageInfo) }, null);

        public static bool Prefix(SubRoot __instance, DamageInfo damageInfo)
        {
            return NitroxServiceLocator.LocateService<SimulationOwnership>().HasAnyLockType(NitroxEntity.GetId(__instance.gameObject));
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
