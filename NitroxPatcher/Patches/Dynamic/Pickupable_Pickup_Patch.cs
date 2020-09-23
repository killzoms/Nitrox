using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Pickupable_Pickup_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Pickupable).GetMethod(nameof(Pickupable.Pickup), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(Pickupable __instance)
        {
            NitroxServiceLocator.LocateService<Item>().PickedUp(__instance.gameObject, __instance.GetTechType());
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}

