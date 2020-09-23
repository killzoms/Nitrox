using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.MonoBehaviours.Overrides;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic
{
    public class BaseAddModuleGhost_UpdatePlacement_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(BaseAddModuleGhost).GetMethod(nameof(BaseAddModuleGhost.UpdatePlacement), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo getIsPlacingMethod = typeof(MultiplayerBuilder).GetMethod("get_isPlacing", BindingFlags.Public | BindingFlags.Static);

        private static readonly OpCode injectionOpCode = OpCodes.Ldsfld;
        private static readonly object injectionOperand = typeof(Player).GetField(nameof(Player.main), BindingFlags.Public | BindingFlags.Static);

        private static readonly OpCode instructionBeforeJump = OpCodes.Ldfld;
        private static readonly object instructionBeforeJumpOperand = typeof(SubRoot).GetField(nameof(SubRoot.isBase), BindingFlags.Public | BindingFlags.Instance);
        private static readonly OpCode jumpInstructionToCopy = OpCodes.Brtrue;

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            List<CodeInstruction> instructionList = instructions.ToList();

            bool shouldInject = false;

            /**
             * When placing some modules in multiplayer it throws an exception because it tries to validate
             * that the current player is in the subroot.  We want to skip over this code if we are placing 
             * a multiplayer piece:
             * 
             * if (main == null || main.currentSub == null || !main.currentSub.isBase)
             * 
             * Injected code:
             * 
             * if (!MultiplayerBuilder.isPlacing && (main == null || main.currentSub == null || !main.currentSub.isBase))
             *    
             */
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                yield return instruction;

                if (shouldInject)
                {
                    shouldInject = false;

                    // First fetch the place we want to jump... this will be the same place as !main.currentSub.isBase
                    CodeInstruction jumpInstruction = GetJumpInstruction(instructionList, i);

                    yield return new CodeInstruction(OpCodes.Call, getIsPlacingMethod);
                    yield return new CodeInstruction(OpCodes.Brtrue_S, jumpInstruction.operand); // copy the jump location
                }

                // We want to inject just after Player main = Player.main... if this is that instruction then we'll inject after the next opcode (stfld)
                shouldInject = (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand));
            }
        }

        private static CodeInstruction GetJumpInstruction(List<CodeInstruction> instructions, int startingIndex)
        {
            for (int i = startingIndex; i < instructions.Count; i++)
            {
                CodeInstruction instruction = instructions[i];

                if (instruction.opcode == instructionBeforeJump && instruction.operand == instructionBeforeJumpOperand)
                {
                    // we located the instruction before the jump... the next instruction should be the jump
                    CodeInstruction jmpInstruction = instructions[i + 1];

                    // Validate that it is what we are looking for
                    Validate.IsTrue(jumpInstructionToCopy == jmpInstruction.opcode, "Looks like subnautica code has changed.  Update jump offset!");

                    return jmpInstruction;
                }
            }

            throw new Exception("Could not locate jump instruction to copy! Injection has failed.");
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}

