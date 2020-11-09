using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class SubRoot_OnPlayerEntered_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SubRoot).GetMethod(nameof(SubRoot.OnPlayerEntered), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo opInequalityMethod = typeof(UnityEngine.Object).GetMethod("op_Inequality");
        private static readonly FieldInfo liveMixinInvincibleField = typeof(LiveMixin).GetField(nameof(LiveMixin.invincible), BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo subRootLiveField = typeof(SubRoot).GetField("live", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly OpCode startInjectionCode = OpCodes.Ldarg_0;
        private static readonly OpCode startInjectionCodeInvincible = OpCodes.Stfld;

        /* There is a bug, where Subroot.live is not loaded when starting in a cyclops. Therefore this code-piece needs to check that and jump accordingly if not present
         * 
         * For this change
         * 
         * this.live.invincible = false
         * 
         * to
         * 
         * if (this.live != null)
         * {
         *  this.live.invincible = false
         * } 
         */
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            int injectionPoint = 0;
            Label newJumpPoint = generator.DefineLabel();
            for (int i = 3; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == startInjectionCodeInvincible &&
                    Equals(instructionList[i].operand, liveMixinInvincibleField) &&
                    instructionList[i - 3].opcode == startInjectionCode)
                {
                    instructionList[i + 1].labels.Add(newJumpPoint);
                    injectionPoint = i - 3;
                }

            }
            if (injectionPoint != 0)
            {
                List<CodeInstruction> injectedInstructions = new List<CodeInstruction> {
                                                                    new CodeInstruction(OpCodes.Ldarg_0),
                                                                    new CodeInstruction(OpCodes.Ldfld, subRootLiveField),
                                                                    new CodeInstruction(OpCodes.Ldnull),
                                                                    new CodeInstruction(OpCodes.Call, opInequalityMethod),
                                                                    new CodeInstruction(OpCodes.Brfalse, newJumpPoint)
                                                                    };
                instructionList.InsertRange(injectionPoint, injectedInstructions);
            }
            return instructionList;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
