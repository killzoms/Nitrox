using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class SpawnConsoleCommand_OnConsoleCommand_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SpawnConsoleCommand).GetMethod("OnConsoleCommand_spawn", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo patchCallback = typeof(SpawnConsoleCommand_OnConsoleCommand_Patch).GetMethod(nameof(Callback), BindingFlags.Public | BindingFlags.Static);

        private static readonly OpCode injectionCode = OpCodes.Call;
        private static readonly object injectionOperand = typeof(Utils).GetMethod(nameof(Utils.CreatePrefab), BindingFlags.Public | BindingFlags.Static);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(injectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                /*
                 * GameObject gameObject = global::Utils.CreatePrefab(prefabForTechType, maxDist, i > 0);
                 * -> SpawnConsoleCommand_OnConsoleCommand_Patch.Callback(gameObject);
                 * LargeWorldEntity.Register(gameObject);
                 * CrafterLogic.NotifyCraftEnd(gameObject, techType);
                 * gameObject.SendMessage("StartConstruction", SendMessageOptions.DontRequireReceiver);
                 */
                if (instruction.opcode == injectionCode && instruction.operand.Equals(injectionOperand))
                {

                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, patchCallback);
                }

            }
        }

        public static void Callback(GameObject gameObject)
        {
            NitroxServiceLocator.LocateService<NitroxConsole>().Spawn(gameObject);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
