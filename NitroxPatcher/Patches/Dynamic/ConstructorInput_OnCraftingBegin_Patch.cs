using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic.Helper;
using NitroxModel.Helper;
using UnityEngine;
using static NitroxClient.GameLogic.Helper.TransientLocalObjectManager;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ConstructorInput_OnCraftingBegin_Patch : NitroxPatch, IDynamicPatch
    {
        internal static readonly MethodInfo targetMethod = typeof(ConstructorInput).GetMethod("OnCraftingBegin", BindingFlags.NonPublic | BindingFlags.Instance);

        internal static readonly OpCode injectionOpCode = OpCodes.Callvirt;
        internal static readonly object injectionOperand = typeof(Constructor).GetMethod(nameof(Constructor.SendBuildBots), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(GameObject) }, null);

        private static readonly MethodInfo transientLocalObjectManagerAddMethod = typeof(TransientLocalObjectManager).GetMethod(nameof(Add), BindingFlags.Public | BindingFlags.Static, null, new Type[] { TransientObjectType.CONSTRUCTOR_INPUT_CRAFTED_GAMEOBJECT.GetType(), typeof(object) }, null);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand))
                {
                    /*
                     * TransientLocalObjectManager.Add(TransientLocalObjectManager.TransientObjectType.CONSTRUCTOR_INPUT_CRAFTED_GAMEOBJECT, gameObject);
                     */
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                    yield return original.Ldloc<GameObject>(0);
                    yield return new CodeInstruction(OpCodes.Call, transientLocalObjectManagerAddMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

