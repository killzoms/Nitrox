using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsShieldButton_OnClick_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsShieldButton).GetMethod(nameof(CyclopsShieldButton.OnClick), BindingFlags.Public | BindingFlags.Instance);
        private static readonly OpCode startCutCode = OpCodes.Ldsfld;
        private static readonly OpCode startCutCodeCall = OpCodes.Callvirt;
        private static readonly FieldInfo playerMainField = typeof(Player).GetField(nameof(Player.main), BindingFlags.Public | BindingFlags.Static);
        private static readonly OpCode endCutCode = OpCodes.Ret;

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();
            int startCut = 0;
            int endCut = instructionList.Count;
            /* Cut out
             * if (Player.main.currentSub != this.subRoot)
             * {
             * 	return;
             * }
             */
            for (int i = 1; i < instructionList.Count; i++)
            {
                if (instructionList[i - 1].opcode.Equals(startCutCode) && instructionList[i - 1].operand.Equals(playerMainField) && instructionList[i].opcode == startCutCodeCall)
                {
                    startCut = i - 1;
                }
                // Cut at the first return encountered
                if (endCut == instructionList.Count && instructionList[i].opcode.Equals(endCutCode))
                {
                    endCut = i;
                }
            }
            instructionList.RemoveRange(startCut, endCut + 1);
            if (startCut == 0)
            {
                instructionList.Insert(0, new CodeInstruction(OpCodes.Nop));
            }
            return instructionList;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
