using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace NitroxPatcher.Patches.Persistent
{
    public class CellManager_TryLoadCacheBatchCells_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CellManager).GetMethod(nameof(CellManager.TryLoadCacheBatchCells), BindingFlags.Public | BindingFlags.Instance);
        private static readonly object largeWorldPathPrefixMethod = typeof(LargeWorldStreamer).GetProperty("pathPrefix", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
        private static readonly object largeWorldFallbackPrefixMethod = typeof(LargeWorldStreamer).GetProperty("fallbackPrefix", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> instrList = instructions.ToList();

            Label pathPrefixJmp = generator.DefineLabel();
            Label labeledPathInstructionJmp = generator.DefineLabel();
            Label fallbackPrefixJmp = generator.DefineLabel();
            Label labeledFallbackInstructionJmp = generator.DefineLabel();

            for (int i = 0; i < instrList.Count; i++)
            {
                CodeInstruction instruction = instrList[i];
                if (instrList.Count > i + 2 && instrList[i + 2].opcode == OpCodes.Callvirt && instrList[i + 2].operand == largeWorldPathPrefixMethod)
                {
                    foreach (CodeInstruction instr in TranspilerHelper.IsMultiplayer(pathPrefixJmp, generator))
                    {
                        yield return instr;
                    }

                    yield return new CodeInstruction(OpCodes.Ldstr, ""); // Replace pathPrefix with an empty string
                    yield return new CodeInstruction(OpCodes.Br, labeledPathInstructionJmp);
                    instrList[i].labels.Add(pathPrefixJmp);
                    yield return instrList[i];
                    yield return instrList[i + 1];
                    yield return instrList[i + 2];

                    CodeInstruction labeledCodeInstruction = new CodeInstruction(instrList[i + 3].opcode, instrList[i + 3].operand);
                    labeledCodeInstruction.labels.Add(labeledPathInstructionJmp);

                    yield return labeledCodeInstruction;
                    i += 3;
                }
                else if (instrList.Count > i + 2 && instrList[i + 2].opcode == OpCodes.Callvirt && instrList[i + 2].operand == largeWorldFallbackPrefixMethod)
                {
                    foreach (CodeInstruction instr in TranspilerHelper.IsMultiplayer(fallbackPrefixJmp, generator))
                    {
                        yield return instr;
                    }

                    yield return new CodeInstruction(OpCodes.Ldstr, ""); // Replace pathPrefix with an empty string
                    yield return new CodeInstruction(OpCodes.Br, labeledFallbackInstructionJmp);
                    instrList[i].labels.Add(fallbackPrefixJmp);
                    yield return instrList[i];
                    yield return instrList[i + 1];
                    yield return instrList[i + 2];

                    CodeInstruction labeledCodeInstruction = new CodeInstruction(instrList[i + 3].opcode, instrList[i + 3].operand);
                    labeledCodeInstruction.labels.Add(labeledFallbackInstructionJmp);

                    yield return labeledCodeInstruction;
                    i += 3;
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
