using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Builder_TryPlace_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Builder).GetMethod(nameof(Builder.TryPlace));

        private static readonly MethodInfo baseGhostGetTargetBaseMethod = typeof(BaseGhost).GetMethod("get_TargetBase", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo gameObjectGetTransformMethod = typeof(GameObject).GetMethod("get_transform", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo transformGetPositionMethod = typeof(Transform).GetMethod("get_position", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo craftDateGetTechTypeMethod = typeof(CraftData).GetMethod(nameof(CraftData.GetTechType), BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(GameObject) }, null);
        private static readonly MethodInfo buildingPlaceBasePieceMethod = typeof(Building).GetMethod(nameof(Building.PlaceBasePiece), BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(BaseGhost), typeof(ConstructableBase), typeof(Base), typeof(TechType), typeof(Quaternion) }, null);
        private static readonly MethodInfo buildingPlaceFurnitureMethod = typeof(Building).GetMethod(nameof(Building.PlaceFurniture), BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(GameObject), typeof(TechType), typeof(Vector3), typeof(Quaternion) }, null);

        private static readonly FieldInfo builderPrefabField = typeof(Builder).GetField("prefab", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly FieldInfo builderPlaceRotationField = typeof(Builder).GetField("placeRotation", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly FieldInfo builderGhostModelField = typeof(Builder).GetField("ghostModel", BindingFlags.Static | BindingFlags.NonPublic);

        internal static readonly OpCode placeBaseInjectionOpCode = OpCodes.Callvirt;
        internal static readonly object placeBaseInjectionOperand = typeof(BaseGhost).GetMethod("Place");

        internal static readonly OpCode placeFurnitureInjectionOpCode = OpCodes.Call;
        internal static readonly object placeFurnitureInjectionOperand = typeof(SkyEnvironmentChanged).GetMethod(nameof(SkyEnvironmentChanged.Send), BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(GameObject), typeof(Component) }, null);

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Validate.NotNull(placeBaseInjectionOperand);
            Validate.NotNull(placeFurnitureInjectionOperand);

            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode.Equals(placeBaseInjectionOpCode) && instruction.operand.Equals(placeBaseInjectionOperand))
                {
                    /*
                     *  Multiplayer.Logic.Building.PlaceBasePiece(componentInParent, component.TargetBase, CraftData.GetTechType(Builder.prefab), Builder.placeRotation);
                     */
                    yield return TranspilerHelper.LocateService<Building>();
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Callvirt, baseGhostGetTargetBaseMethod);
                    yield return new CodeInstruction(OpCodes.Ldsfld, builderPrefabField);
                    yield return new CodeInstruction(OpCodes.Call, craftDateGetTechTypeMethod);
                    yield return new CodeInstruction(OpCodes.Ldsfld, builderPlaceRotationField);
                    yield return new CodeInstruction(OpCodes.Callvirt, buildingPlaceBasePieceMethod);
                }

                if (instruction.opcode.Equals(placeFurnitureInjectionOpCode) && instruction.operand.Equals(placeFurnitureInjectionOperand))
                {
                    /*
                     *  Multiplayer.Logic.Building.PlaceFurniture(gameObject, CraftData.GetTechType(Builder.prefab), Builder.ghostModel.transform.position, Builder.placeRotation);
                     */
                    yield return TranspilerHelper.LocateService<Building>();
                    yield return new CodeInstruction(OpCodes.Ldloc_2);
                    yield return new CodeInstruction(OpCodes.Ldsfld, builderPrefabField);
                    yield return new CodeInstruction(OpCodes.Call, craftDateGetTechTypeMethod);
                    yield return new CodeInstruction(OpCodes.Ldsfld, builderGhostModelField);
                    yield return new CodeInstruction(OpCodes.Callvirt, gameObjectGetTransformMethod);
                    yield return new CodeInstruction(OpCodes.Callvirt, transformGetPositionMethod);
                    yield return new CodeInstruction(OpCodes.Ldsfld, builderPlaceRotationField);
                    yield return new CodeInstruction(OpCodes.Callvirt, buildingPlaceFurnitureMethod);
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchTranspiler(harmony, targetMethod);
        }
    }
}
