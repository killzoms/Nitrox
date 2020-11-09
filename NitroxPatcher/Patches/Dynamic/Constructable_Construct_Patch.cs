﻿using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;
using NitroxModel.DataStructures;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Constructable_Construct_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Constructable).GetMethod(nameof(Constructable.Construct), BindingFlags.Public | BindingFlags.Instance);

        private static Base lastTargetBase;
        private static Int3 lastTargetBaseOffset;

        public static bool Prefix(Constructable __instance)
        {
            if (!__instance._constructed && __instance.constructedAmount < 1.0f)
            {
                NitroxServiceLocator.LocateService<Building>().ChangeConstructionAmount(__instance.gameObject, __instance.constructedAmount);
            }

            // If we are constructing a base piece then we'll want to store all of the BaseGhost information
            // as it will not be available when the construction hits 100%
            BaseGhost baseGhost = __instance.gameObject.GetComponentInChildren<BaseGhost>();

            if (baseGhost != null && baseGhost.TargetBase)
            {
                lastTargetBase = baseGhost.TargetBase.GetComponent<Base>();
                lastTargetBaseOffset = baseGhost.TargetOffset;
            }
            else
            {
                lastTargetBase = null;
                lastTargetBaseOffset = default;
            }

            return true;
        }

        public static void Postfix(Constructable __instance, bool __result)
        {
            if (__result && __instance.constructedAmount >= 1.0f)
            {
                NitroxServiceLocator.LocateService<Building>().ConstructionComplete(__instance.gameObject, Optional.OfNullable(lastTargetBase), lastTargetBaseOffset);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }
    }
}
