using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class BuilderTool_HandleInput_Patch : NitroxPatch, IDynamicPatch
    {
        internal static readonly MethodInfo targetMethod = typeof(BuilderTool).GetMethod("HandleInput", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo componentGetGameObjectMethod = typeof(Component).GetMethod("get_gameObject");
        private static readonly MethodInfo buildingDeconstructionBeginMethod = typeof(Building).GetMethod(nameof(Building.DeconstructionBegin), BindingFlags.Public | BindingFlags.Instance);

        internal static readonly OpCode injectionOpCode = OpCodes.Callvirt;
        internal static readonly object injectionOperand = typeof(Constructable).GetMethod(nameof(Constructable.SetState), BindingFlags.Public | BindingFlags.Instance);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;
                if (instruction.opcode.Equals(injectionOpCode) && instruction.operand.Equals(injectionOperand))
                {
                    /*
                     * Multiplayer.Logic.Building.DeconstructionBegin(constructable.gameObject);
                     */
                    yield return TranspilerHelper.LocateService<Building>();
                    yield return original.Ldloc<Constructable>();
                    yield return new CodeInstruction(OpCodes.Callvirt, componentGetGameObjectMethod);
                    yield return new CodeInstruction(OpCodes.Callvirt, buildingDeconstructionBeginMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
