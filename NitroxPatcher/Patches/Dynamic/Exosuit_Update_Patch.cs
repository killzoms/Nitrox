using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Exosuit_Update_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Exosuit).GetMethod(nameof(Exosuit.Update), BindingFlags.Public | BindingFlags.Instance);

        private static readonly OpCode injectionOpCode = OpCodes.Call;
        private static readonly object injectionOperand = typeof(Exosuit).GetMethod("UpdateSounds", BindingFlags.NonPublic | BindingFlags.Instance);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            List<CodeInstruction> instructionList = instructions.ToList();

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                yield return instruction;

                /*
                 *  When syninc exo suit we always want to skip an entire if branch in the update: 
                 * 
                 *  if(!flag)
                 *  
                 *  to do this, we transform it into something that always evaluates false:
                 *  
                 *  if(!true)
                 * 
                 */
                if (instruction.opcode == injectionOpCode && instruction.operand == injectionOperand)
                {
                    i++; //increment to ldloc.2 (loading flag2 on evaluation stack)
                    CodeInstruction ldFlag2 = instructionList[i];

                    // Transform to if(!true)
                    ldFlag2.opcode = OpCodes.Ldc_I4_1;

                    yield return ldFlag2;
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

