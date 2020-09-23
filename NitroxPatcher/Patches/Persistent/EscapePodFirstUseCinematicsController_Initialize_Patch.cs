using System.Reflection;
using Harmony;
using NitroxClient.MonoBehaviours;

namespace NitroxPatcher.Patches.Persistent
{
    public class EscapePodFirstUseCinematicsController_Initialize_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo targetMethod = typeof(EscapePodFirstUseCinematicsController).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix(EscapePodFirstUseCinematicsController __instance)
        {
            __instance.bottomCinematicTarget.gameObject.SetActive(true);
            __instance.topCinematicTarget.gameObject.SetActive(true);

            __instance.bottomFirstUseCinematicTarget.gameObject.SetActive(false);
            __instance.topFirstUseCinematicTarget.gameObject.SetActive(false);

            return !Multiplayer.Active;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
