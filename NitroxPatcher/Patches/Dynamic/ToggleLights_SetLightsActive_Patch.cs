﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Exceptions;
using NitroxModel.Logger;
using NitroxModel_Subnautica.DataStructures.GameLogic;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ToggleLights_SetLightsActive_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ToggleLights).GetMethod(nameof(ToggleLights.SetLightsActive), BindingFlags.Public | BindingFlags.Instance);

        private static readonly HashSet<Type> syncedParents = new HashSet<Type>()
        {
            typeof(SeaMoth),
            typeof(Seaglide),
            typeof(FlashLight),
            typeof(LEDLight)    // LEDLight uses ToggleLights, but does not provide a method to toggle them.
        };

        public static bool Prefix(ToggleLights __instance, out bool __state)
        {
            __state = __instance.lightsActive;
            return true;
        }

        public static void Postfix(ToggleLights __instance, bool __state)
        {
            if (__state != __instance.lightsActive)
            {
                // Find the right gameobject in the hierarchy to sync on:
                GameObject gameObject = null;
                Type type = null;
                foreach (Type t in syncedParents)
                {
                    if (__instance.GetComponent(t))
                    {
                        type = t;
                        gameObject = __instance.gameObject;
                        break;
                    }
                    else if (__instance.GetComponentInParent(t))
                    {
                        type = t;
                        gameObject = __instance.transform.parent.gameObject;
                        break;
                    }
                }

                if (!gameObject)
                {
                    Log.Error(new GameObjectNullException());
                    DebugUtils.PrintHierarchy(__instance.gameObject);
                }

                NitroxId id = NitroxEntity.GetId(gameObject);
                // If the floodlight belongs to a seamoth, then set the lights for the model
                if (type == typeof(SeaMoth))
                {
                    NitroxServiceLocator.LocateService<Vehicles>().GetVehicles<SeamothModel>(id).LightOn = __instance.lightsActive;
                }
                NitroxServiceLocator.LocateService<IPacketSender>().Send(new NitroxModel.Packets.ToggleLights(id, __instance.lightsActive));
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, true, true, false);
        }

        //public class LightToggleContainer
        //{
        //    public readonly Type ComponentType;
        //    public readonly bool InParent;

        //    public LightToggleContainer(Type componentType, bool inParent)
        //    {
        //        ComponentType = componentType;
        //        InParent = inParent;
        //    }

        //    public Component Get(GameObject go)
        //    {
        //        if (InParent)
        //        {
        //            return go.GetComponentInParent(ComponentType);
        //        }

        //        return go.GetComponent(ComponentType);
        //    }
        //}
    }
}
