using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic.Helper;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class BaseDeconstructable_Deconstruct_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(BaseDeconstructable).GetMethod(nameof(BaseDeconstructable.Deconstruct), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo callbackMethod = typeof(BaseDeconstructable_Deconstruct_Patch).GetMethod(nameof(Callback), BindingFlags.Public | BindingFlags.Static);

        private static readonly OpCode injectionOpCode = OpCodes.Callvirt;
        private static readonly object injectionOperand = typeof(Constructable).GetMethod(nameof(Constructable.SetState));

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand))
                {
                    /*
                     * BaseDeconstructable_Deconstruct_Patch.Callback(gameObject);
                     */
                    yield return original.Ldloc<ConstructableBase>(0);
                    yield return new CodeInstruction(OpCodes.Call, callbackMethod);
                }
            }
        }

        public static void Callback(GameObject gameObject)
        {
            TransientLocalObjectManager.Add(TransientLocalObjectManager.TransientObjectType.LATEST_DECONSTRUCTED_BASE_PIECE, gameObject);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

