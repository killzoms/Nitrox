using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.MonoBehaviours.Gui.Input;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Persistent
{
    public class GameInput_Initialize_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(GameInput).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo keyBindingManagerGetHighestKeyBindingValueMethod = typeof(KeyBindingManager).GetMethod(nameof(KeyBindingManager.GetHighestKeyBindingValue), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo mathMaxMethod = typeof(Math).GetMethod(nameof(Math.Max), BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
        private static readonly ConstructorInfo keyBindingManagerConstructor = typeof(KeyBindingManager).GetConstructors().First();

        private static readonly OpCode injectionOpCode = OpCodes.Stsfld;
        private static readonly object injectionOperand = typeof(GameInput).GetField("numButtons", BindingFlags.NonPublic | BindingFlags.Static);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand))
                {
                    /*
                     * int prev = GameInput.GetMaximumEnumValue(typeof(GameInput.Button)) + 1;
                     * //  ^ This value is already calculated by the original code, it's stored on top of the stack.
                     * KeyBindingManager keyBindingManager = new KeyBindingManager();
                     * GameButton.numButtons = Math.Max(keyBindingManager.GetHighestKeyBindingValue() + 1, prev);
                     */
                    yield return new CodeInstruction(OpCodes.Newobj, keyBindingManagerConstructor);
                    yield return new CodeInstruction(OpCodes.Callvirt, keyBindingManagerGetHighestKeyBindingValueMethod);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Add);
                    yield return new CodeInstruction(OpCodes.Call, mathMaxMethod);
                }

                yield return instruction;
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
