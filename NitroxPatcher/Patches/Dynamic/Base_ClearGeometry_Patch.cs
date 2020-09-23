using System.Collections.Generic;
using System.Reflection;
using Harmony;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Base_ClearGeometry_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Base).GetMethod(nameof(Base.ClearGeometry), BindingFlags.Public | BindingFlags.Instance);

        /**
         * When new bases are constructed it will sometimes clear all of the pieces 
         * and reconnect them.  (This is primarily for visual purposes so it can change
         * out the model if required.)  When these pieces are cleared, we need to persist
         * them so that we can update the newly placed pieces with the proper id.  The new
         * pieces are added by <see cref="Base_SpawnPiece_Patch"/>
         */
        public static readonly Dictionary<string, NitroxId> NitroxIdByObjectKey = new Dictionary<string, NitroxId>();

        public static void Prefix(Base __instance, Transform[] ___cellObjects)
        {
            if (!__instance && ___cellObjects == null)
            {
                return;
            }

            foreach (Transform cellObject in ___cellObjects)
            {
                if (cellObject)
                {
                    for (int i = 0; i < cellObject.childCount; i++)
                    {
                        GameObject child = cellObject.GetChild(i).gameObject;

                        // Ensure there is already a nitrox id, we don't want to go creating one
                        // which happens if you call GetId directly and it is missing.
                        if (child && child.GetComponent<NitroxEntity>())
                        {
                            NitroxId id = NitroxEntity.GetId(child);
                            string key = child.name + child.transform.position;
                            NitroxIdByObjectKey[key] = id;

                            Log.Debug($"Clearing Base Geometry, storing id for later lookup: {key} {id}");
                        }
                    }
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
