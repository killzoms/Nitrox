using System;
using System.Reflection;
using Harmony;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Base_SpawnPiece_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Base).GetMethod("SpawnPiece", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Base).GetNestedType("Piece", BindingFlags.NonPublic | BindingFlags.Instance), typeof(Int3), typeof(Quaternion), typeof(Base.Direction?) }, null);

        /**
         * This function is called directly after the game clears all base pieces (to update
         * the view model - this is done in Base.ClearGeometry, see that patch).  The game will
         * respawn each object and we need to copy over their ids.  We reference the dictionary
         * in the <see cref="Base_ClearGeometry_Patch"/> patch so know what ids to update.
         */
        public static void Postfix(Base __instance, Transform __result)
        {
            if (!__instance || !__result)
            {
                return;
            }

            string key = __result.name + __result.position;

            if (Base_ClearGeometry_Patch.NitroxIdByObjectKey.TryGetValue(key, out NitroxId id))
            {
                Log.Debug($"When respawning geometry, found id to copy to new object: {key} {id}");
                NitroxEntity.SetNewId(__result.gameObject, id);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
