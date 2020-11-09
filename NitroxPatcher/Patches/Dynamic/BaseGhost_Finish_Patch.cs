using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic.Helper;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class BaseGhost_Finish_Patch : NitroxPatch, IDynamicPatch
    {
        internal static readonly MethodInfo targetMethod = typeof(BaseGhost).GetMethod(nameof(BaseGhost.Finish), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo objectManagerAddMethod = typeof(TransientLocalObjectManager).GetMethod(nameof(TransientLocalObjectManager.Add), BindingFlags.Public | BindingFlags.Static, null, new[] { TransientLocalObjectManager.TransientObjectType.BASE_GHOST_NEWLY_CONSTRUCTED_BASE_GAMEOBJECT.GetType(), typeof(object) }, null);

        internal static readonly OpCode injectionOpCode = OpCodes.Stfld;
        internal static readonly object injectionOperand = typeof(BaseGhost).GetField("targetBase", BindingFlags.NonPublic | BindingFlags.Instance);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(injectionOpCode))
                {
                    /*
                     * TransientLocalObjectManager.Add(TransientLocalObjectManager.TransientObjectType.BASE_GHOST_NEWLY_CONSTRUCTED_BASE_GAMEOBJECT, gameObject);
                     */
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return original.Ldloc<GameObject>(1);
                    yield return new CodeInstruction(OpCodes.Call, objectManagerAddMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

