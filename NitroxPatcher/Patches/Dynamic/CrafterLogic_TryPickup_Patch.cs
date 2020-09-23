using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CrafterLogic_TryPickup_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CrafterLogic).GetMethod(nameof(CrafterLogic.TryPickup), BindingFlags.Public | BindingFlags.Instance);

        private static readonly OpCode injectionOpCode = OpCodes.Stfld;
        private static readonly object injectionOperand = typeof(CrafterLogic).GetField(nameof(CrafterLogic.numCrafted), BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo componentGetGameObjectMethod = typeof(Component).GetMethod("get_gameObject", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo craftingFabricatorItemPickedUpMethod = typeof(Crafting).GetMethod(nameof(Crafting.FabricatorItemPickedUp), BindingFlags.Public | BindingFlags.Instance);


        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand, "Operand can not be null");

            bool injected = false;

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand) && !injected)
                {
                    injected = true;

                    /*
                     * Multiplayer.Logic.Crafting.FabricatorItemPickedUp(base.gameObject, techType);
                     */
                    yield return TranspilerHelper.LocateService<Crafting>();
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, componentGetGameObjectMethod);
                    yield return original.Ldloc<TechType>();
                    yield return new CodeInstruction(OpCodes.Callvirt, craftingFabricatorItemPickedUpMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

