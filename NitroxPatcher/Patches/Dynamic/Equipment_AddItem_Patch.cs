using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Equipment_AddItem_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Equipment).GetMethod(nameof(Equipment.AddItem), BindingFlags.Public | BindingFlags.Instance);

        public static void Postfix(Equipment __instance, bool __result, string slot, InventoryItem newItem)
        {
            if (__result)
            {
                NitroxServiceLocator.LocateService<EquipmentSlots>().BroadcastEquip(newItem.item, __instance.owner, slot);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
