using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EnergyMixin_OnRemoveItem_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EnergyMixin).GetMethod("OnRemoveItem", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(EnergyMixin __instance, InventoryItem item)
        {
            // For now only broadcast, if it is a vehicle
            // Items that use batteries also have EnergyMixin components. But the items will be serialized with the battery
            // and therefore don't need to be synchronized this way
            if (item != null && (__instance.gameObject.GetComponent<Vehicle>() || __instance.gameObject.GetComponentInParent<Vehicle>() || __instance.gameObject.GetComponentInParent<SubRoot>()))
            {
                NitroxServiceLocator.LocateService<StorageSlots>().BroadcastItemRemoval(__instance.gameObject);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
