using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.MonoBehaviours;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Persistent
{
    public class PAXTerrainController_LoadAsync_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly Type targetClass = typeof(PAXTerrainController);
        private static readonly object injectionOperand = typeof(PAXTerrainController).GetMethod("LoadWorldTiles", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo largeWorldStreamerFrozenField = typeof(LargeWorldStreamer).GetField(nameof(LargeWorldStreamer.frozen), BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo paxTerrainControllerStreamerField = typeof(PAXTerrainController).GetField("streamer", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo multiplayerSubnauticaLoadingCompletedMethod = typeof(Multiplayer).GetMethod(nameof(Multiplayer.SubnauticaLoadingCompleted), BindingFlags.Public | BindingFlags.Static);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, ILGenerator ilGenerator, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);
            List<CodeInstruction> instrList = instructions.ToList();
            Label jmpLabelStartOfMethod = ilGenerator.DefineLabel();

            for (int i = 0; i < instrList.Count; i++)
            {
                CodeInstruction instruction = instrList[i];
                if (instrList[i].opcode == OpCodes.Switch)
                {
                    List<Label> labels = ((Label[])instruction.operand).ToList(); // removing unnecessary labels
                    labels.RemoveRange(3, 5);
                    yield return new CodeInstruction(instruction.opcode, labels.ToArray());
                }
                else if (instruction.opcode == OpCodes.Brtrue && instruction.operand.GetHashCode() == 10)
                {
                    yield return new CodeInstruction(OpCodes.Brtrue, jmpLabelStartOfMethod); // replace previous jump with new one
                }
                else if (instrList.Count > i + 2 &&
                         instrList[i + 1].opcode == OpCodes.Ldfld &&
                         Equals(instrList[i + 1].operand, paxTerrainControllerStreamerField) &&
                         instrList[i + 3].opcode == OpCodes.Stfld &&
                         Equals(instrList[i + 3].operand, largeWorldStreamerFrozenField))
                {
                    instruction.labels.Add(jmpLabelStartOfMethod);
                    yield return instruction; // Add a label for jumping
                }
                else if (instruction.opcode == OpCodes.Stfld &&
                         Equals(instruction.operand, largeWorldStreamerFrozenField))
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Call, multiplayerSubnauticaLoadingCompletedMethod);
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                    yield return new CodeInstruction(OpCodes.Ldc_I4_8); // Load 8 onto the stack
                    yield return new CodeInstruction(OpCodes.Stfld, GetStateField(GetLoadAsyncEnumerableMethod())); // Store last stack item into state field
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1); // return true
                    yield return new CodeInstruction(OpCodes.Ret);
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, GetMethod());
        }

        private static FieldInfo GetStateField(IReflect type)
        {
            FieldInfo stateField = null;
            foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.Name.Contains("state"))
                {
                    stateField = field;
                }
            }
            Validate.NotNull(stateField);
            return stateField;
        }

        private static Type GetLoadAsyncEnumerableMethod()
        {
            Type[] nestedTypes = targetClass.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Static);
            Type targetEnumeratorClass = null;

            foreach (Type type in nestedTypes)
            {
                if (type.FullName?.Contains("LoadAsync") == true)
                {
                    targetEnumeratorClass = type;
                }
            }

            Validate.NotNull(targetEnumeratorClass);
            return targetEnumeratorClass;
        }

        private static MethodInfo GetMethod()
        {
            MethodInfo method = GetLoadAsyncEnumerableMethod().GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            Validate.NotNull(method);

            return method;
        }
    }
}
