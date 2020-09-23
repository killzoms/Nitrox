using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CyclopsSonarButton_SonarPing_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsSonarButton).GetMethod("SonarPing", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly OpCode jumpTargetCode = OpCodes.Ldsfld;
        private static readonly FieldInfo jumpTargetField = typeof(SNCameraRoot).GetField(nameof(SNCameraRoot.main), BindingFlags.Public | BindingFlags.Static);

        private static readonly FieldInfo playerMainField = typeof(Player).GetField(nameof(Player.main), BindingFlags.Public | BindingFlags.Static);
        private static readonly MethodInfo playerGetCurrentSubMethod = typeof(Player).GetMethod("get_currentSub", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo cyclopsSonarButtonSubRootField = typeof(CyclopsSonarButton).GetField(nameof(CyclopsSonarButton.subRoot), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo objectOpInequalityMethod = typeof(UnityEngine.Object).GetMethod("op_Inequality", BindingFlags.Public | BindingFlags.Static);

        // Send ping to other players        
        public static void Postfix(CyclopsSonarButton __instance)
        {
            NitroxId id = NitroxEntity.GetId(__instance.subRoot.gameObject);
            NitroxServiceLocator.LocateService<Cyclops>().BroadcastSonarPing(id);
        }


        /* As the ping would be always be executed it should be restricted to players, that are in the cyclops
        * Therefore the code generated from AssembleNewCode will be injected before the ping would be send but after energy consumption
        * End result:
        * private void SonarPing()
        * {
        * 	float num = 0f;
        * 	if (!this.subRoot.powerRelay.ConsumeEnergy(this.subRoot.sonarPowerCost, out num))
        * 	{
        * 	    this.TurnOffSonar();
        * 	    return;
        * 	}
        * 	if(Player.main.currentSub != this.subroot)
        * 	{
        * 	    return;
        * 	}
        * 	SNCameraRoot.main.SonarPing();
        * 	this.soundFX.Play();
        * }
        */
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions, ILGenerator iLGenerator)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            // Need to change the jump target for Brtrue at one point
            Label toInjectJump = iLGenerator.DefineLabel();

            // Find point to inject if player is in subroot:
            // SNCameraRoot.main.SonarPing();
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                if (instruction.opcode.Equals(jumpTargetCode) && instruction.operand.Equals(jumpTargetField))
                {
                    Label jumpLabel = instruction.labels[0];
                    IEnumerable<CodeInstruction> injectInstructions = AssembleNewCode(jumpLabel, toInjectJump);
                    foreach (CodeInstruction injectInstruction in injectInstructions)
                    {
                        yield return injectInstruction;
                    }
                }

                /* New jump target will from 
                 * 
                 * if (!this.subRoot.powerRelay.ConsumeEnergy(this.subRoot.sonarPowerCost, out num))
                 * 
                 * will be new code
                 */
                if (instruction.opcode.Equals(OpCodes.Brtrue) && instructionList[i - 1].opcode.Equals(OpCodes.Ldloc_1) && instructionList[i + 1].opcode.Equals(OpCodes.Ldarg_0))
                {
                    instruction.operand = toInjectJump;
                }
                yield return instruction;
            }
        }

        private static IEnumerable<CodeInstruction> AssembleNewCode(Label outJumpLabel, Label innerJumpLabel)
        {
            //Code to inject:
            /*
             * if (Player.main.currentSub != this.subRoot)
		     * {
			 *  return;
		     * }
             * 
             */
            List<CodeInstruction> injectInstructions = new List<CodeInstruction>();

            CodeInstruction instruction = new CodeInstruction(OpCodes.Ldsfld) { operand = playerMainField };
            instruction.labels.Add(innerJumpLabel);
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Callvirt) { operand = playerGetCurrentSubMethod };
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Ldarg_0);
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Ldfld) { operand = cyclopsSonarButtonSubRootField };
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Call) { operand = objectOpInequalityMethod };
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Brfalse) { operand = outJumpLabel };
            injectInstructions.Add(instruction);

            instruction = new CodeInstruction(OpCodes.Ret);
            injectInstructions.Add(instruction);

            return injectInstructions;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, false, true, true);
        }
    }
}
