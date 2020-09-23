using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;

namespace NitroxPatcher.Patches.Dynamic
{
    class CyclopsExternalDamageManager_CreatePoint_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsExternalDamageManager).GetMethod("CreatePoint", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix(CyclopsExternalDamageManager __instance, out bool __state)
        {
            // Block from creating points if they aren't the owner of the sub
            __state = NitroxServiceLocator.LocateService<SimulationOwnership>().HasAnyLockType(NitroxEntity.GetId(__instance.subRoot.gameObject));

            return __state;
        }

        public static void Postfix(CyclopsExternalDamageManager __instance, bool __state)
        {
            if (__state)
            {
                NitroxServiceLocator.LocateService<Cyclops>().OnCreateDamagePoint(__instance.subRoot);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
