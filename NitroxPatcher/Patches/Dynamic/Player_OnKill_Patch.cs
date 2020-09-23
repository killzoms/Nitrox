using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Player_OnKill_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Player).GetMethod(nameof(Player.OnKill), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo skipMethod = typeof(GameModeUtils).GetMethod(nameof(GameModeUtils.IsPermadeath), BindingFlags.Public | BindingFlags.Static);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            // Skips:
            // if (GameModeUtils.IsPermadeath())
            // {
            //      SaveLoadManager.main.ClearSlotAsync(SaveLoadManager.main.GetCurrentSlot());
            //      this.EndGame();
            //      return;
            // }

            foreach (CodeInstruction instr in instructions.ToList())
            {
                if (instr.opcode == OpCodes.Call && instr.operand.Equals(skipMethod))
                {
                    CodeInstruction newInstr = new CodeInstruction(OpCodes.Ldc_I4_0)
                    {
                        labels = instr.labels
                    };
                    yield return newInstr;
                }
                else
                {
                    yield return instr;
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
