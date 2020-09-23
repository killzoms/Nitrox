using System;
using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ArmsController_Update_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ArmsController).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);

        // Because this piece of code does a lot with reflection and executes often, prepare the reflection info instead of doing it every call (which is what ReflectionHelper does).
        // There are plans ongoing to streamline this, probably involving native code generation to get rid of reflection slowness completely. Potential solutions involve Expressions and/or IL generation.

        private static readonly FieldInfo leftAimField = typeof(ArmsController).GetField("leftAim", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo rightAimField = typeof(ArmsController).GetField("rightAim", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo reconfigureWorldTarget = typeof(ArmsController).GetField("reconfigureWorldTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo reconfigure = typeof(ArmsController).GetMethod("Reconfigure", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly Type armAiming = typeof(ArmsController).GetNestedType("ArmAiming", BindingFlags.NonPublic);
        private static readonly MethodInfo armAimingUpdate = armAiming.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo updateHandIKWeights = typeof(ArmsController).GetMethod("UpdateHandIKWeights", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix(ArmsController __instance)
        {
            if (__instance.smoothSpeedAboveWater == 0)
            {
                if ((bool)reconfigureWorldTarget.GetValue(__instance))
                {
                    reconfigure.Invoke(__instance, new PlayerTool[] { null });
                    reconfigureWorldTarget.SetValue(__instance, false);
                }

                object leftAim = leftAimField.GetValue(__instance);
                object rightAim = rightAimField.GetValue(__instance);
                object[] args = new object[] { __instance.ikToggleTime };
                armAimingUpdate.Invoke(leftAim, args);
                armAimingUpdate.Invoke(rightAim, args);

                updateHandIKWeights.Invoke(__instance, new object[] { });
                return false;
            }

            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
