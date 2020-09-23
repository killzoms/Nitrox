using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Equipment_RemoveItem_Patch : NitroxPatch, IDynamicPatch
    {
        internal static readonly MethodInfo targetMethod = typeof(Equipment).GetMethod(nameof(Equipment.RemoveItem), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(bool), typeof(bool) }, null);
        private static readonly MethodInfo inventoryItemGetItemMethod = typeof(InventoryItem).GetMethod("get_item", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo equipmentGetOwnerMethod = typeof(Equipment).GetMethod("get_owner", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo equipmentSlotsBroadcastUnequipMethod = typeof(EquipmentSlots).GetMethod(nameof(EquipmentSlots.BroadcastUnequip), BindingFlags.Public | BindingFlags.Instance);

        internal static readonly OpCode injectionOpCode = OpCodes.Stloc_1;

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(injectionOpCode))
                {
                    /*
                     * Multiplayer.Logic.EquipmentSlots.Unequip(pickupable, this.owner, slot)
                     */
                    yield return TranspilerHelper.LocateService<EquipmentSlots>();
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Callvirt, inventoryItemGetItemMethod);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, equipmentGetOwnerMethod);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Callvirt, equipmentSlotsBroadcastUnequipMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
