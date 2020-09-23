using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class EnergyMixin_OnAddItem_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EnergyMixin).GetMethod("OnAddItem", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Postfix(EnergyMixin __instance, InventoryItem item)
        {
            //For now only broadcast, if it is a vehicle
            if (item != null && (__instance.gameObject.GetComponent<Vehicle>() || __instance.gameObject.GetComponentInParent<Vehicle>() || __instance.gameObject.GetComponentInParent<SubRoot>()))
            {
                NitroxServiceLocator.LocateService<StorageSlots>().BroadcastItemAdd(item, __instance.gameObject);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
