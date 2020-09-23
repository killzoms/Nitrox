using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class RocketConstructor_StartRocketConstruction_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(RocketConstructor).GetMethod(nameof(RocketConstructor.StartRocketConstruction), BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo rocketConstructorRocketField = typeof(RocketConstructor).GetField(nameof(RocketConstructor.rocket), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo startRocketConstructionCallbackMethod = typeof(RocketConstructor_StartRocketConstruction_Patch).GetMethod(nameof(Callback), BindingFlags.Public | BindingFlags.Static);
        private static readonly OpCode injectionCode = OpCodes.Stloc_2;

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                /* if (this.crafterLogic.Craft(currentStageTech, craftTime)) {
			     *      GameObject toBuild = this.rocket.StartRocketConstruction();
                 *  ->  RocketConstructor_StartRocketConstruction_Patch.Callback(this.rocket, currentStageTech, this, toBuild); 
			     *      ItemGoalTracker.OnConstruct(currentStageTech);
			     *      this.SendBuildBots(toBuild);
		         * }
                 */
                if (instruction.opcode == injectionCode)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, rocketConstructorRocketField); //this.rocket
                    yield return new CodeInstruction(OpCodes.Ldarg_0); //this
                    yield return new CodeInstruction(OpCodes.Ldloc_0); //techType
                    yield return new CodeInstruction(OpCodes.Ldloc_2); //toBuild GO
                    yield return new CodeInstruction(OpCodes.Call, startRocketConstructionCallbackMethod);
                }

            }
        }

        public static void Callback(Rocket rocketInstanceAttachedToConstructor, RocketConstructor rocketConstructor, TechType currentStageTech, GameObject gameObjectToBuild)
        {
            NitroxId rocketId = NitroxEntity.GetId(rocketInstanceAttachedToConstructor.gameObject);
            NitroxId constructorId = NitroxEntity.GetId(rocketConstructor.gameObject);
            NitroxServiceLocator.LocateService<Rockets>().BroadcastRocketStateUpdate(rocketId, constructorId, currentStageTech, gameObjectToBuild);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
